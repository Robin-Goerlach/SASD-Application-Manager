namespace Sasd.FinanceControl.Infrastructure.Persistence.Migrations;

/// <summary>
/// Central catalog of database migrations shipped with the application.
/// </summary>
public static class MigrationCatalog
{
    /// <summary>Gets all migrations in ascending version order.</summary>
    public static IReadOnlyList<DatabaseMigration> All { get; } =
    [
        new DatabaseMigration(
            1,
            "Milestone 2 master data",
            """
            CREATE TABLE IF NOT EXISTS number_sequences (
                sequence_name TEXT NOT NULL PRIMARY KEY,
                current_value INTEGER NOT NULL CHECK (current_value >= 0)
            );

            CREATE TABLE IF NOT EXISTS suppliers (
                id TEXT NOT NULL PRIMARY KEY,
                supplier_number TEXT NOT NULL COLLATE NOCASE,
                supplier_name TEXT NOT NULL,
                supplier_type TEXT NOT NULL,
                contact_person TEXT NULL,
                email TEXT NULL,
                phone TEXT NULL,
                website TEXT NULL,
                street TEXT NULL,
                postal_code TEXT NULL,
                city TEXT NULL,
                country_code TEXT NULL,
                tax_id TEXT NULL,
                customer_number TEXT NULL,
                payment_terms_days INTEGER NULL CHECK (payment_terms_days BETWEEN 0 AND 365),
                notes TEXT NULL,
                is_active INTEGER NOT NULL CHECK (is_active IN (0, 1)),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT uq_suppliers_supplier_number UNIQUE (supplier_number)
            );

            CREATE INDEX IF NOT EXISTS ix_suppliers_name
                ON suppliers (supplier_name COLLATE NOCASE);
            CREATE INDEX IF NOT EXISTS ix_suppliers_active
                ON suppliers (is_active);

            CREATE TABLE IF NOT EXISTS categories (
                id TEXT NOT NULL PRIMARY KEY,
                name TEXT NOT NULL,
                normalized_name TEXT NOT NULL,
                description TEXT NULL,
                parent_id TEXT NULL,
                is_active INTEGER NOT NULL CHECK (is_active IN (0, 1)),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT fk_categories_parent
                    FOREIGN KEY (parent_id) REFERENCES categories(id) ON DELETE RESTRICT,
                CONSTRAINT ck_categories_not_self_parent CHECK (parent_id IS NULL OR parent_id <> id)
            );

            CREATE UNIQUE INDEX IF NOT EXISTS ux_categories_parent_name
                ON categories (COALESCE(parent_id, ''), normalized_name);
            CREATE INDEX IF NOT EXISTS ix_categories_parent
                ON categories (parent_id);
            CREATE INDEX IF NOT EXISTS ix_categories_active
                ON categories (is_active);

            CREATE TABLE IF NOT EXISTS bank_accounts (
                id TEXT NOT NULL PRIMARY KEY,
                display_name TEXT NOT NULL,
                bank_name TEXT NOT NULL,
                account_holder TEXT NOT NULL,
                iban TEXT NOT NULL COLLATE NOCASE,
                bic TEXT NULL,
                currency_code TEXT NOT NULL,
                notes TEXT NULL,
                is_active INTEGER NOT NULL CHECK (is_active IN (0, 1)),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT uq_bank_accounts_iban UNIQUE (iban)
            );

            CREATE INDEX IF NOT EXISTS ix_bank_accounts_display_name
                ON bank_accounts (display_name COLLATE NOCASE);
            CREATE INDEX IF NOT EXISTS ix_bank_accounts_active
                ON bank_accounts (is_active);
            """),
        new DatabaseMigration(
            2,
            "Milestone 3 document archive",
            """
            CREATE TABLE IF NOT EXISTS documents (
                id TEXT NOT NULL PRIMARY KEY,
                document_type TEXT NOT NULL,
                original_file_name TEXT NOT NULL,
                stored_file_name TEXT NOT NULL,
                mime_type TEXT NOT NULL,
                file_size INTEGER NOT NULL CHECK (file_size > 0),
                sha256_hash TEXT NOT NULL COLLATE NOCASE,
                storage_path TEXT NOT NULL,
                document_date TEXT NULL,
                source TEXT NULL,
                notes TEXT NULL,
                is_immutable INTEGER NOT NULL DEFAULT 1 CHECK (is_immutable = 1),
                imported_at_utc TEXT NOT NULL,
                metadata_updated_at_utc TEXT NOT NULL,
                CONSTRAINT uq_documents_sha256 UNIQUE (sha256_hash),
                CONSTRAINT uq_documents_storage_path UNIQUE (storage_path)
            );

            CREATE INDEX IF NOT EXISTS ix_documents_type
                ON documents (document_type);
            CREATE INDEX IF NOT EXISTS ix_documents_document_date
                ON documents (document_date);
            CREATE INDEX IF NOT EXISTS ix_documents_imported_at
                ON documents (imported_at_utc);

            CREATE TABLE IF NOT EXISTS document_links (
                id TEXT NOT NULL PRIMARY KEY,
                document_id TEXT NOT NULL,
                linked_entity_type TEXT NOT NULL,
                linked_entity_id TEXT NOT NULL,
                created_at_utc TEXT NOT NULL,
                CONSTRAINT fk_document_links_document
                    FOREIGN KEY (document_id) REFERENCES documents(id) ON DELETE RESTRICT,
                CONSTRAINT uq_document_links_target
                    UNIQUE (document_id, linked_entity_type, linked_entity_id)
            );

            CREATE INDEX IF NOT EXISTS ix_document_links_document
                ON document_links (document_id);
            CREATE INDEX IF NOT EXISTS ix_document_links_target
                ON document_links (linked_entity_type, linked_entity_id);
            """),
        new DatabaseMigration(
            3,
            "Milestone 4 banking",
            """
            CREATE TABLE IF NOT EXISTS bank_statements (
                id TEXT NOT NULL PRIMARY KEY,
                bank_account_id TEXT NOT NULL,
                source_file_name TEXT NOT NULL,
                file_sha256 TEXT NOT NULL COLLATE NOCASE,
                import_source TEXT NOT NULL,
                original_document_id TEXT NOT NULL,
                period_from TEXT NULL,
                period_to TEXT NULL,
                statement_number TEXT NULL,
                opening_balance TEXT NULL,
                closing_balance TEXT NULL,
                currency_code TEXT NOT NULL,
                imported_at_utc TEXT NOT NULL,
                CONSTRAINT fk_bank_statements_account
                    FOREIGN KEY (bank_account_id) REFERENCES bank_accounts(id) ON DELETE RESTRICT,
                CONSTRAINT fk_bank_statements_document
                    FOREIGN KEY (original_document_id) REFERENCES documents(id) ON DELETE RESTRICT,
                CONSTRAINT uq_bank_statements_source UNIQUE (bank_account_id, file_sha256)
            );

            CREATE INDEX IF NOT EXISTS ix_bank_statements_account
                ON bank_statements (bank_account_id);
            CREATE INDEX IF NOT EXISTS ix_bank_statements_period
                ON bank_statements (period_from, period_to);
            CREATE INDEX IF NOT EXISTS ix_bank_statements_imported
                ON bank_statements (imported_at_utc);

            CREATE TABLE IF NOT EXISTS bank_transactions (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                bank_account_id TEXT NOT NULL,
                booking_date TEXT NOT NULL,
                value_date TEXT NULL,
                amount_decimal TEXT NOT NULL,
                currency_code TEXT NOT NULL,
                direction TEXT NOT NULL CHECK (direction IN ('debit', 'credit')),
                raw_description TEXT NOT NULL,
                counterparty_name TEXT NULL,
                counterparty_iban TEXT NULL,
                mandate_reference TEXT NULL,
                end_to_end_id TEXT NULL,
                bank_reference TEXT NULL,
                transaction_hash TEXT NOT NULL COLLATE NOCASE,
                is_reversal INTEGER NOT NULL CHECK (is_reversal IN (0, 1)),
                imported_at_utc TEXT NOT NULL,
                CONSTRAINT fk_bank_transactions_account
                    FOREIGN KEY (bank_account_id) REFERENCES bank_accounts(id) ON DELETE RESTRICT,
                CONSTRAINT uq_bank_transactions_hash UNIQUE (transaction_hash)
            );

            CREATE INDEX IF NOT EXISTS ix_bank_transactions_account_date
                ON bank_transactions (bank_account_id, booking_date);
            CREATE INDEX IF NOT EXISTS ix_bank_transactions_booking_date
                ON bank_transactions (booking_date);
            CREATE INDEX IF NOT EXISTS ix_bank_transactions_counterparty
                ON bank_transactions (counterparty_name COLLATE NOCASE);

            CREATE TABLE IF NOT EXISTS bank_statement_transactions (
                bank_statement_id TEXT NOT NULL,
                bank_transaction_id INTEGER NOT NULL,
                source_row_number INTEGER NOT NULL CHECK (source_row_number > 0),
                PRIMARY KEY (bank_statement_id, source_row_number),
                CONSTRAINT uq_bank_statement_transaction
                    UNIQUE (bank_statement_id, bank_transaction_id),
                CONSTRAINT fk_bank_statement_transactions_statement
                    FOREIGN KEY (bank_statement_id) REFERENCES bank_statements(id) ON DELETE RESTRICT,
                CONSTRAINT fk_bank_statement_transactions_transaction
                    FOREIGN KEY (bank_transaction_id) REFERENCES bank_transactions(id) ON DELETE RESTRICT
            );

            CREATE INDEX IF NOT EXISTS ix_bank_statement_transactions_transaction
                ON bank_statement_transactions (bank_transaction_id);

            -- Banking source data is an audit trail. Enforce immutability at the
            -- database boundary as well as in the Domain/Application layers so a
            -- future programming mistake cannot silently rewrite imported truth.
            CREATE TRIGGER IF NOT EXISTS trg_bank_statements_no_update
            BEFORE UPDATE ON bank_statements
            BEGIN
                SELECT RAISE(ABORT, 'Imported bank statements are immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_bank_statements_no_delete
            BEFORE DELETE ON bank_statements
            BEGIN
                SELECT RAISE(ABORT, 'Imported bank statements are immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_bank_transactions_no_update
            BEFORE UPDATE ON bank_transactions
            BEGIN
                SELECT RAISE(ABORT, 'Imported bank transactions are immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_bank_transactions_no_delete
            BEFORE DELETE ON bank_transactions
            BEGIN
                SELECT RAISE(ABORT, 'Imported bank transactions are immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_bank_statement_transactions_no_update
            BEFORE UPDATE ON bank_statement_transactions
            BEGIN
                SELECT RAISE(ABORT, 'Imported bank statement relations are immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_bank_statement_transactions_no_delete
            BEFORE DELETE ON bank_statement_transactions
            BEGIN
                SELECT RAISE(ABORT, 'Imported bank statement relations are immutable');
            END;
            """),
        new DatabaseMigration(
            4,
            "Milestone 5 payment assignment",
            """
            CREATE TABLE IF NOT EXISTS bank_transaction_assignments (
                bank_transaction_id INTEGER NOT NULL PRIMARY KEY,
                supplier_id TEXT NULL,
                category_id TEXT NULL,
                resolution_status TEXT NOT NULL
                    CHECK (resolution_status IN ('unresolved', 'resolved', 'ignored')),
                note TEXT NULL CHECK (note IS NULL OR length(note) <= 2000),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT fk_bank_transaction_assignments_transaction
                    FOREIGN KEY (bank_transaction_id) REFERENCES bank_transactions(id) ON DELETE RESTRICT,
                CONSTRAINT fk_bank_transaction_assignments_supplier
                    FOREIGN KEY (supplier_id) REFERENCES suppliers(id) ON DELETE RESTRICT,
                CONSTRAINT fk_bank_transaction_assignments_category
                    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE RESTRICT,
                CONSTRAINT ck_bank_transaction_assignments_resolved
                    CHECK (resolution_status <> 'resolved' OR supplier_id IS NOT NULL OR category_id IS NOT NULL),
                CONSTRAINT ck_bank_transaction_assignments_ignored
                    CHECK (resolution_status <> 'ignored' OR (supplier_id IS NULL AND category_id IS NULL))
            );

            CREATE INDEX IF NOT EXISTS ix_bank_transaction_assignments_supplier
                ON bank_transaction_assignments (supplier_id);
            CREATE INDEX IF NOT EXISTS ix_bank_transaction_assignments_category
                ON bank_transaction_assignments (category_id);
            CREATE INDEX IF NOT EXISTS ix_bank_transaction_assignments_status
                ON bank_transaction_assignments (resolution_status);

            -- Assignment rows are mutable interpretations, but they must never
            -- silently disappear. Resetting a payment therefore means updating
            -- the row back to unresolved rather than deleting history. A full
            -- change journal follows in the dedicated audit milestone.
            CREATE TRIGGER IF NOT EXISTS trg_bank_transaction_assignments_no_delete
            BEFORE DELETE ON bank_transaction_assignments
            BEGIN
                SELECT RAISE(ABORT, 'Payment assignments cannot be deleted; reset them instead');
            END;

            -- The source transaction and original creation timestamp identify
            -- the assignment record. Those identity fields are immutable even
            -- though supplier/category/status/note may legitimately change.
            CREATE TRIGGER IF NOT EXISTS trg_bank_transaction_assignments_identity
            BEFORE UPDATE ON bank_transaction_assignments
            WHEN NEW.bank_transaction_id <> OLD.bank_transaction_id
                 OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Payment assignment identity is immutable');
            END;
            """),
        new DatabaseMigration(
            5,
            "Milestone 6 contracts and subscriptions",
            """
            CREATE TABLE IF NOT EXISTS contracts (
                id TEXT NOT NULL PRIMARY KEY,
                contract_number TEXT NOT NULL COLLATE NOCASE UNIQUE,
                supplier_id TEXT NOT NULL,
                title TEXT NOT NULL CHECK (length(trim(title)) > 0),
                contract_type TEXT NOT NULL CHECK (length(trim(contract_type)) > 0),
                external_contract_number TEXT NULL,
                status TEXT NOT NULL
                    CHECK (status IN ('draft', 'active', 'notice_given', 'ended')),
                start_date TEXT NOT NULL,
                end_date TEXT NULL,
                next_cancellation_deadline TEXT NULL,
                notice_period_days INTEGER NULL
                    CHECK (notice_period_days IS NULL OR notice_period_days BETWEEN 0 AND 3650),
                auto_renewal INTEGER NOT NULL
                    CHECK (auto_renewal IN (0, 1)),
                renewal_period_months INTEGER NULL,
                expected_amount_decimal TEXT NULL,
                currency_code TEXT NOT NULL
                    CHECK (length(currency_code) = 3),
                payment_frequency TEXT NOT NULL
                    CHECK (payment_frequency IN ('none', 'one_time', 'monthly', 'quarterly', 'semi_annual', 'annual')),
                first_expected_payment_date TEXT NULL,
                notes TEXT NULL CHECK (notes IS NULL OR length(notes) <= 6000),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT fk_contracts_supplier
                    FOREIGN KEY (supplier_id) REFERENCES suppliers(id) ON DELETE RESTRICT,
                CONSTRAINT ck_contracts_dates
                    CHECK (end_date IS NULL OR end_date >= start_date),
                CONSTRAINT ck_contracts_ended_date
                    CHECK (status <> 'ended' OR end_date IS NOT NULL),
                CONSTRAINT ck_contracts_renewal
                    CHECK (
                        (auto_renewal = 0 AND renewal_period_months IS NULL) OR
                        (auto_renewal = 1 AND renewal_period_months IS NOT NULL AND renewal_period_months BETWEEN 1 AND 120)
                    ),
                CONSTRAINT ck_contracts_payment_schedule
                    CHECK (
                        (payment_frequency = 'none' AND expected_amount_decimal IS NULL AND first_expected_payment_date IS NULL) OR
                        (payment_frequency <> 'none' AND expected_amount_decimal IS NOT NULL AND first_expected_payment_date IS NOT NULL)
                    )
            );

            CREATE INDEX IF NOT EXISTS ix_contracts_supplier
                ON contracts (supplier_id);
            CREATE INDEX IF NOT EXISTS ix_contracts_status
                ON contracts (status);
            CREATE INDEX IF NOT EXISTS ix_contracts_cancellation_deadline
                ON contracts (next_cancellation_deadline);
            CREATE INDEX IF NOT EXISTS ix_contracts_start_end
                ON contracts (start_date, end_date);

            -- Contracts are editable business explanations, but they are part
            -- of the audit trail and must not silently disappear. Ending a
            -- contract is represented by its lifecycle status instead.
            CREATE TRIGGER IF NOT EXISTS trg_contracts_no_delete
            BEFORE DELETE ON contracts
            BEGIN
                SELECT RAISE(ABORT, 'Contracts cannot be deleted; end the contract instead');
            END;

            -- Stable identity fields are never rewritten after creation.
            CREATE TRIGGER IF NOT EXISTS trg_contracts_identity
            BEFORE UPDATE ON contracts
            WHEN NEW.id <> OLD.id
                 OR NEW.contract_number <> OLD.contract_number
                 OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Contract identity is immutable');
            END;
            """),
        new DatabaseMigration(
            6,
            "Milestone 7 incoming invoices",
            """
            CREATE TABLE IF NOT EXISTS invoices (
                id TEXT NOT NULL PRIMARY KEY,
                invoice_number TEXT NOT NULL COLLATE NOCASE UNIQUE,
                supplier_id TEXT NOT NULL,
                external_invoice_number TEXT NULL,
                invoice_date TEXT NOT NULL,
                due_date TEXT NULL,
                service_period_from TEXT NULL,
                service_period_to TEXT NULL,
                status TEXT NOT NULL
                    CHECK (status IN ('draft', 'open', 'disputed', 'cancelled')),
                currency_code TEXT NOT NULL
                    CHECK (length(currency_code) = 3),
                notes TEXT NULL CHECK (notes IS NULL OR length(notes) <= 6000),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT fk_invoices_supplier
                    FOREIGN KEY (supplier_id) REFERENCES suppliers(id) ON DELETE RESTRICT,
                CONSTRAINT ck_invoices_due_date
                    CHECK (due_date IS NULL OR due_date >= invoice_date),
                CONSTRAINT ck_invoices_service_period
                    CHECK (service_period_from IS NULL OR service_period_to IS NULL OR service_period_to >= service_period_from)
            );

            CREATE UNIQUE INDEX IF NOT EXISTS ux_invoices_supplier_external_number
                ON invoices (supplier_id, external_invoice_number COLLATE NOCASE)
                WHERE external_invoice_number IS NOT NULL
                  AND length(trim(external_invoice_number)) > 0;
            CREATE INDEX IF NOT EXISTS ix_invoices_supplier
                ON invoices (supplier_id);
            CREATE INDEX IF NOT EXISTS ix_invoices_status_due
                ON invoices (status, due_date);
            CREATE INDEX IF NOT EXISTS ix_invoices_invoice_date
                ON invoices (invoice_date);

            CREATE TABLE IF NOT EXISTS invoice_lines (
                id TEXT NOT NULL PRIMARY KEY,
                invoice_id TEXT NOT NULL,
                position INTEGER NOT NULL CHECK (position > 0),
                description TEXT NOT NULL CHECK (length(trim(description)) > 0),
                quantity_decimal TEXT NOT NULL,
                unit TEXT NULL,
                unit_price_net_decimal TEXT NOT NULL,
                tax_rate_percent_decimal TEXT NOT NULL,
                net_amount_decimal TEXT NOT NULL,
                tax_amount_decimal TEXT NOT NULL,
                gross_amount_decimal TEXT NOT NULL,
                CONSTRAINT fk_invoice_lines_invoice
                    FOREIGN KEY (invoice_id) REFERENCES invoices(id) ON DELETE RESTRICT,
                CONSTRAINT uq_invoice_lines_position UNIQUE (invoice_id, position)
            );

            CREATE INDEX IF NOT EXISTS ix_invoice_lines_invoice
                ON invoice_lines (invoice_id, position);

            -- Incoming invoices form part of the financial audit trail. A wrong
            -- or cancelled invoice is represented by lifecycle state rather than
            -- silently removing the record.
            CREATE TRIGGER IF NOT EXISTS trg_invoices_no_delete
            BEFORE DELETE ON invoices
            BEGIN
                SELECT RAISE(ABORT, 'Invoices cannot be deleted; cancel the invoice instead');
            END;

            -- Stable identity fields are not editable. Business attributes and
            -- line content remain editable until a later audit milestone adds a
            -- complete change journal.
            CREATE TRIGGER IF NOT EXISTS trg_invoices_identity
            BEFORE UPDATE ON invoices
            WHEN NEW.id <> OLD.id
                 OR NEW.invoice_number <> OLD.invoice_number
                 OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Invoice identity is immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_invoice_lines_identity
            BEFORE UPDATE ON invoice_lines
            WHEN NEW.id <> OLD.id OR NEW.invoice_id <> OLD.invoice_id
            BEGIN
                SELECT RAISE(ABORT, 'Invoice-line identity is immutable');
            END;
            """),
        new DatabaseMigration(
            7,
            "Milestone 8 payment reconciliation and cost allocation",
            """
            CREATE TABLE IF NOT EXISTS projects (
                id TEXT NOT NULL PRIMARY KEY,
                project_number TEXT NOT NULL COLLATE NOCASE UNIQUE,
                name TEXT NOT NULL CHECK (length(trim(name)) > 0),
                description TEXT NULL,
                start_date TEXT NULL,
                end_date TEXT NULL,
                is_active INTEGER NOT NULL CHECK (is_active IN (0, 1)),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT ck_projects_dates CHECK (start_date IS NULL OR end_date IS NULL OR end_date >= start_date)
            );

            CREATE INDEX IF NOT EXISTS ix_projects_name ON projects (name COLLATE NOCASE);
            CREATE INDEX IF NOT EXISTS ix_projects_active ON projects (is_active);

            CREATE TABLE IF NOT EXISTS cost_centers (
                id TEXT NOT NULL PRIMARY KEY,
                code TEXT NOT NULL COLLATE NOCASE UNIQUE,
                name TEXT NOT NULL CHECK (length(trim(name)) > 0),
                description TEXT NULL,
                is_active INTEGER NOT NULL CHECK (is_active IN (0, 1)),
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL
            );

            CREATE INDEX IF NOT EXISTS ix_cost_centers_name ON cost_centers (name COLLATE NOCASE);
            CREATE INDEX IF NOT EXISTS ix_cost_centers_active ON cost_centers (is_active);

            CREATE TABLE IF NOT EXISTS invoice_payment_allocations (
                id TEXT NOT NULL PRIMARY KEY,
                invoice_id TEXT NOT NULL,
                bank_transaction_id INTEGER NOT NULL,
                amount_decimal TEXT NOT NULL,
                note TEXT NULL CHECK (note IS NULL OR length(note) <= 2000),
                created_at_utc TEXT NOT NULL,
                is_voided INTEGER NOT NULL DEFAULT 0 CHECK (is_voided IN (0, 1)),
                voided_at_utc TEXT NULL,
                void_reason TEXT NULL CHECK (void_reason IS NULL OR length(void_reason) <= 1000),
                CONSTRAINT fk_invoice_payment_allocations_invoice
                    FOREIGN KEY (invoice_id) REFERENCES invoices(id) ON DELETE RESTRICT,
                CONSTRAINT fk_invoice_payment_allocations_transaction
                    FOREIGN KEY (bank_transaction_id) REFERENCES bank_transactions(id) ON DELETE RESTRICT,
                CONSTRAINT ck_invoice_payment_allocations_void
                    CHECK ((is_voided = 0 AND voided_at_utc IS NULL AND void_reason IS NULL)
                        OR (is_voided = 1 AND voided_at_utc IS NOT NULL AND void_reason IS NOT NULL))
            );

            CREATE UNIQUE INDEX IF NOT EXISTS ux_invoice_payment_allocations_active_pair
                ON invoice_payment_allocations (invoice_id, bank_transaction_id)
                WHERE is_voided = 0;
            CREATE INDEX IF NOT EXISTS ix_invoice_payment_allocations_invoice
                ON invoice_payment_allocations (invoice_id, is_voided);
            CREATE INDEX IF NOT EXISTS ix_invoice_payment_allocations_transaction
                ON invoice_payment_allocations (bank_transaction_id, is_voided);

            CREATE TABLE IF NOT EXISTS contract_payment_matches (
                id TEXT NOT NULL PRIMARY KEY,
                contract_id TEXT NOT NULL,
                bank_transaction_id INTEGER NOT NULL,
                note TEXT NULL CHECK (note IS NULL OR length(note) <= 2000),
                created_at_utc TEXT NOT NULL,
                is_voided INTEGER NOT NULL DEFAULT 0 CHECK (is_voided IN (0, 1)),
                voided_at_utc TEXT NULL,
                void_reason TEXT NULL CHECK (void_reason IS NULL OR length(void_reason) <= 1000),
                CONSTRAINT fk_contract_payment_matches_contract
                    FOREIGN KEY (contract_id) REFERENCES contracts(id) ON DELETE RESTRICT,
                CONSTRAINT fk_contract_payment_matches_transaction
                    FOREIGN KEY (bank_transaction_id) REFERENCES bank_transactions(id) ON DELETE RESTRICT,
                CONSTRAINT ck_contract_payment_matches_void
                    CHECK ((is_voided = 0 AND voided_at_utc IS NULL AND void_reason IS NULL)
                        OR (is_voided = 1 AND voided_at_utc IS NOT NULL AND void_reason IS NOT NULL))
            );

            CREATE UNIQUE INDEX IF NOT EXISTS ux_contract_payment_matches_active_pair
                ON contract_payment_matches (contract_id, bank_transaction_id)
                WHERE is_voided = 0;
            CREATE INDEX IF NOT EXISTS ix_contract_payment_matches_contract
                ON contract_payment_matches (contract_id, is_voided);
            CREATE INDEX IF NOT EXISTS ix_contract_payment_matches_transaction
                ON contract_payment_matches (bank_transaction_id, is_voided);

            CREATE TABLE IF NOT EXISTS invoice_line_cost_allocations (
                id TEXT NOT NULL PRIMARY KEY,
                invoice_line_id TEXT NOT NULL,
                project_id TEXT NULL,
                cost_center_id TEXT NULL,
                net_amount_decimal TEXT NOT NULL,
                note TEXT NULL CHECK (note IS NULL OR length(note) <= 2000),
                created_at_utc TEXT NOT NULL,
                is_voided INTEGER NOT NULL DEFAULT 0 CHECK (is_voided IN (0, 1)),
                voided_at_utc TEXT NULL,
                void_reason TEXT NULL CHECK (void_reason IS NULL OR length(void_reason) <= 1000),
                CONSTRAINT fk_invoice_line_cost_allocations_line
                    FOREIGN KEY (invoice_line_id) REFERENCES invoice_lines(id) ON DELETE RESTRICT,
                CONSTRAINT fk_invoice_line_cost_allocations_project
                    FOREIGN KEY (project_id) REFERENCES projects(id) ON DELETE RESTRICT,
                CONSTRAINT fk_invoice_line_cost_allocations_cost_center
                    FOREIGN KEY (cost_center_id) REFERENCES cost_centers(id) ON DELETE RESTRICT,
                CONSTRAINT ck_invoice_line_cost_allocations_target
                    CHECK (project_id IS NOT NULL OR cost_center_id IS NOT NULL),
                CONSTRAINT ck_invoice_line_cost_allocations_void
                    CHECK ((is_voided = 0 AND voided_at_utc IS NULL AND void_reason IS NULL)
                        OR (is_voided = 1 AND voided_at_utc IS NOT NULL AND void_reason IS NOT NULL))
            );

            CREATE INDEX IF NOT EXISTS ix_invoice_line_cost_allocations_line
                ON invoice_line_cost_allocations (invoice_line_id, is_voided);
            CREATE INDEX IF NOT EXISTS ix_invoice_line_cost_allocations_project
                ON invoice_line_cost_allocations (project_id, is_voided);
            CREATE INDEX IF NOT EXISTS ix_invoice_line_cost_allocations_cost_center
                ON invoice_line_cost_allocations (cost_center_id, is_voided);

            -- Master data is deactivated instead of deleted so historic cost
            -- allocations remain readable and referentially stable.
            CREATE TRIGGER IF NOT EXISTS trg_projects_no_delete
            BEFORE DELETE ON projects
            BEGIN
                SELECT RAISE(ABORT, 'Projects cannot be deleted; deactivate them instead');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_cost_centers_no_delete
            BEFORE DELETE ON cost_centers
            BEGIN
                SELECT RAISE(ABORT, 'Cost centres cannot be deleted; deactivate them instead');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_projects_identity
            BEFORE UPDATE ON projects
            WHEN NEW.id <> OLD.id OR NEW.project_number <> OLD.project_number OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Project identity is immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_cost_centers_identity
            BEFORE UPDATE ON cost_centers
            WHEN NEW.id <> OLD.id OR NEW.code <> OLD.code OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Cost-centre identity is immutable');
            END;

            -- Reconciliation records are append/void history. Only their void
            -- state may change after creation; source identity and amounts stay stable.
            CREATE TRIGGER IF NOT EXISTS trg_invoice_payment_allocations_no_delete
            BEFORE DELETE ON invoice_payment_allocations
            BEGIN
                SELECT RAISE(ABORT, 'Invoice payment allocations cannot be deleted; void them instead');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_invoice_payment_allocations_identity
            BEFORE UPDATE ON invoice_payment_allocations
            WHEN NEW.id <> OLD.id
                 OR NEW.invoice_id <> OLD.invoice_id
                 OR NEW.bank_transaction_id <> OLD.bank_transaction_id
                 OR NEW.amount_decimal <> OLD.amount_decimal
                 OR IFNULL(NEW.note, '') <> IFNULL(OLD.note, '')
                 OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Invoice payment allocation identity is immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_contract_payment_matches_no_delete
            BEFORE DELETE ON contract_payment_matches
            BEGIN
                SELECT RAISE(ABORT, 'Contract payment matches cannot be deleted; void them instead');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_contract_payment_matches_identity
            BEFORE UPDATE ON contract_payment_matches
            WHEN NEW.id <> OLD.id
                 OR NEW.contract_id <> OLD.contract_id
                 OR NEW.bank_transaction_id <> OLD.bank_transaction_id
                 OR IFNULL(NEW.note, '') <> IFNULL(OLD.note, '')
                 OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Contract payment match identity is immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_invoice_line_cost_allocations_no_delete
            BEFORE DELETE ON invoice_line_cost_allocations
            BEGIN
                SELECT RAISE(ABORT, 'Invoice line cost allocations cannot be deleted; void them instead');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_invoice_line_cost_allocations_identity
            BEFORE UPDATE ON invoice_line_cost_allocations
            WHEN NEW.id <> OLD.id
                 OR NEW.invoice_line_id <> OLD.invoice_line_id
                 OR IFNULL(NEW.project_id, '') <> IFNULL(OLD.project_id, '')
                 OR IFNULL(NEW.cost_center_id, '') <> IFNULL(OLD.cost_center_id, '')
                 OR NEW.net_amount_decimal <> OLD.net_amount_decimal
                 OR IFNULL(NEW.note, '') <> IFNULL(OLD.note, '')
                 OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Invoice line cost allocation identity is immutable');
            END;

            -- Once financial relations exist, changing/removing the underlying
            -- invoice line would make derived payment/cost figures ambiguous.
            -- Corrections must first void the dependent allocations.
            CREATE TRIGGER IF NOT EXISTS trg_invoice_lines_protect_allocated_update
            BEFORE UPDATE ON invoice_lines
            WHEN EXISTS (
                    SELECT 1 FROM invoice_line_cost_allocations a
                    WHERE a.invoice_line_id = OLD.id AND a.is_voided = 0)
                 OR EXISTS (
                    SELECT 1 FROM invoice_payment_allocations p
                    WHERE p.invoice_id = OLD.invoice_id AND p.is_voided = 0)
            BEGIN
                SELECT RAISE(ABORT, 'Allocated invoice lines cannot be changed until allocations are voided');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_invoice_lines_protect_allocated_delete
            BEFORE DELETE ON invoice_lines
            WHEN EXISTS (
                    SELECT 1 FROM invoice_line_cost_allocations a
                    WHERE a.invoice_line_id = OLD.id AND a.is_voided = 0)
                 OR EXISTS (
                    SELECT 1 FROM invoice_payment_allocations p
                    WHERE p.invoice_id = OLD.invoice_id AND p.is_voided = 0)
            BEGIN
                SELECT RAISE(ABORT, 'Allocated invoice lines cannot be removed until allocations are voided');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_invoices_protect_paid_financial_identity
            BEFORE UPDATE ON invoices
            WHEN EXISTS (
                    SELECT 1 FROM invoice_payment_allocations p
                    WHERE p.invoice_id = OLD.id AND p.is_voided = 0)
                 AND (
                    NEW.supplier_id <> OLD.supplier_id
                    OR NEW.currency_code <> OLD.currency_code
                    OR NEW.status = 'cancelled')
            BEGIN
                SELECT RAISE(ABORT, 'Paid invoices cannot change supplier/currency or be cancelled until payment allocations are voided');
            END;
            """),
        new DatabaseMigration(
            8,
            "Milestone 9 purchase orders",
            """
            CREATE TABLE IF NOT EXISTS purchase_orders (
                id TEXT NOT NULL PRIMARY KEY,
                order_number TEXT NOT NULL COLLATE NOCASE UNIQUE,
                supplier_id TEXT NOT NULL,
                supplier_order_number TEXT NULL,
                order_date TEXT NOT NULL,
                expected_delivery_date TEXT NULL,
                status TEXT NOT NULL CHECK (status IN ('draft','ordered','partially_received','received','cancelled','closed')),
                currency_code TEXT NOT NULL CHECK (length(currency_code) = 3),
                business_purpose TEXT NULL CHECK (business_purpose IS NULL OR length(business_purpose) <= 1000),
                notes TEXT NULL CHECK (notes IS NULL OR length(notes) <= 6000),
                total_net_decimal TEXT NOT NULL,
                total_tax_decimal TEXT NOT NULL,
                total_gross_decimal TEXT NOT NULL,
                created_at_utc TEXT NOT NULL,
                updated_at_utc TEXT NOT NULL,
                CONSTRAINT fk_purchase_orders_supplier FOREIGN KEY (supplier_id) REFERENCES suppliers(id) ON DELETE RESTRICT,
                CONSTRAINT ck_purchase_orders_delivery CHECK (expected_delivery_date IS NULL OR expected_delivery_date >= order_date)
            );

            CREATE INDEX IF NOT EXISTS ix_purchase_orders_supplier ON purchase_orders (supplier_id);
            CREATE INDEX IF NOT EXISTS ix_purchase_orders_date ON purchase_orders (order_date DESC);
            CREATE INDEX IF NOT EXISTS ix_purchase_orders_status ON purchase_orders (status);

            CREATE TABLE IF NOT EXISTS purchase_order_items (
                id TEXT NOT NULL PRIMARY KEY,
                purchase_order_id TEXT NOT NULL,
                position INTEGER NOT NULL CHECK (position > 0),
                item_name TEXT NOT NULL CHECK (length(trim(item_name)) > 0),
                description TEXT NULL CHECK (description IS NULL OR length(description) <= 2000),
                quantity_decimal TEXT NOT NULL,
                unit TEXT NULL CHECK (unit IS NULL OR length(unit) <= 40),
                unit_price_net_decimal TEXT NOT NULL,
                tax_rate_percent_decimal TEXT NOT NULL,
                category_id TEXT NULL,
                asset_candidate INTEGER NOT NULL DEFAULT 0 CHECK (asset_candidate IN (0,1)),
                inventory_candidate INTEGER NOT NULL DEFAULT 0 CHECK (inventory_candidate IN (0,1)),
                net_amount_decimal TEXT NOT NULL,
                tax_amount_decimal TEXT NOT NULL,
                gross_amount_decimal TEXT NOT NULL,
                CONSTRAINT fk_purchase_order_items_order FOREIGN KEY (purchase_order_id) REFERENCES purchase_orders(id) ON DELETE RESTRICT,
                CONSTRAINT fk_purchase_order_items_category FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE RESTRICT,
                CONSTRAINT uq_purchase_order_items_position UNIQUE (purchase_order_id, position)
            );

            CREATE INDEX IF NOT EXISTS ix_purchase_order_items_order ON purchase_order_items (purchase_order_id, position);
            CREATE INDEX IF NOT EXISTS ix_purchase_order_items_category ON purchase_order_items (category_id);

            CREATE TABLE IF NOT EXISTS purchase_order_invoice_links (
                id TEXT NOT NULL PRIMARY KEY,
                purchase_order_id TEXT NOT NULL,
                invoice_id TEXT NOT NULL,
                note TEXT NULL CHECK (note IS NULL OR length(note) <= 2000),
                created_at_utc TEXT NOT NULL,
                is_voided INTEGER NOT NULL DEFAULT 0 CHECK (is_voided IN (0,1)),
                voided_at_utc TEXT NULL,
                void_reason TEXT NULL CHECK (void_reason IS NULL OR length(void_reason) <= 1000),
                CONSTRAINT fk_purchase_order_invoice_links_order FOREIGN KEY (purchase_order_id) REFERENCES purchase_orders(id) ON DELETE RESTRICT,
                CONSTRAINT fk_purchase_order_invoice_links_invoice FOREIGN KEY (invoice_id) REFERENCES invoices(id) ON DELETE RESTRICT,
                CONSTRAINT ck_purchase_order_invoice_links_void CHECK (
                    (is_voided = 0 AND voided_at_utc IS NULL AND void_reason IS NULL)
                    OR (is_voided = 1 AND voided_at_utc IS NOT NULL AND void_reason IS NOT NULL))
            );

            CREATE UNIQUE INDEX IF NOT EXISTS ux_purchase_order_invoice_links_active_pair
                ON purchase_order_invoice_links (purchase_order_id, invoice_id) WHERE is_voided = 0;
            CREATE INDEX IF NOT EXISTS ix_purchase_order_invoice_links_order
                ON purchase_order_invoice_links (purchase_order_id, is_voided);
            CREATE INDEX IF NOT EXISTS ix_purchase_order_invoice_links_invoice
                ON purchase_order_invoice_links (invoice_id, is_voided);

            CREATE TRIGGER IF NOT EXISTS trg_purchase_orders_no_delete
            BEFORE DELETE ON purchase_orders
            BEGIN
                SELECT RAISE(ABORT, 'Purchase orders cannot be deleted; cancel or close them instead');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_purchase_orders_identity
            BEFORE UPDATE ON purchase_orders
            WHEN NEW.id <> OLD.id OR NEW.order_number <> OLD.order_number OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Purchase order identity is immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_purchase_order_items_identity
            BEFORE UPDATE ON purchase_order_items
            WHEN NEW.id <> OLD.id OR NEW.purchase_order_id <> OLD.purchase_order_id
            BEGIN
                SELECT RAISE(ABORT, 'Purchase order line identity is immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_purchase_order_invoice_links_no_delete
            BEFORE DELETE ON purchase_order_invoice_links
            BEGIN
                SELECT RAISE(ABORT, 'Order/invoice links cannot be deleted; void them instead');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_purchase_order_invoice_links_identity
            BEFORE UPDATE ON purchase_order_invoice_links
            WHEN NEW.id <> OLD.id
                 OR NEW.purchase_order_id <> OLD.purchase_order_id
                 OR NEW.invoice_id <> OLD.invoice_id
                 OR IFNULL(NEW.note, '') <> IFNULL(OLD.note, '')
                 OR NEW.created_at_utc <> OLD.created_at_utc
            BEGIN
                SELECT RAISE(ABORT, 'Order/invoice link identity is immutable');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_purchase_orders_protect_linked_financial_identity
            BEFORE UPDATE ON purchase_orders
            WHEN EXISTS (
                    SELECT 1 FROM purchase_order_invoice_links l
                    WHERE l.purchase_order_id = OLD.id AND l.is_voided = 0)
                 AND (NEW.supplier_id <> OLD.supplier_id OR NEW.currency_code <> OLD.currency_code)
            BEGIN
                SELECT RAISE(ABORT, 'Linked orders cannot change supplier or currency until invoice links are voided');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_purchase_order_items_protect_linked_update
            BEFORE UPDATE ON purchase_order_items
            WHEN EXISTS (
                    SELECT 1 FROM purchase_order_invoice_links l
                    WHERE l.purchase_order_id = OLD.purchase_order_id AND l.is_voided = 0)
            BEGIN
                SELECT RAISE(ABORT, 'Linked order lines cannot be changed until invoice links are voided');
            END;

            CREATE TRIGGER IF NOT EXISTS trg_purchase_order_items_protect_linked_delete
            BEFORE DELETE ON purchase_order_items
            WHEN EXISTS (
                    SELECT 1 FROM purchase_order_invoice_links l
                    WHERE l.purchase_order_id = OLD.purchase_order_id AND l.is_voided = 0)
            BEGIN
                SELECT RAISE(ABORT, 'Linked order lines cannot be removed until invoice links are voided');
            END;
            """),
    ];
}