using SASD.Bewerbungsmanager.Application.Services;
using SASD.Bewerbungsmanager.Domain.Entities;
using SASD.Bewerbungsmanager.WinForms.Forms;
using SASD.Bewerbungsmanager.WinForms.Presentation;
using JobApplication = SASD.Bewerbungsmanager.Domain.Entities.Application;

namespace SASD.Bewerbungsmanager.WinForms.Controls;

/// <summary>Lists concrete applications and exposes the first status-history workflow.</summary>
public sealed class ApplicationsControl : UserControl
{
    private readonly ApplicationService _service;
    private readonly OpportunityService _opportunities;
    private readonly UiExceptionPresenter _errors;
    private readonly DataGridView _grid = ControlFactory.DataGrid();
    private IReadOnlyList<JobApplication> _items = [];

    /// <summary>Initializes the applications view.</summary>
    public ApplicationsControl(ApplicationService service, OpportunityService opportunities, UiExceptionPresenter errors)
    {
        _service = service;
        _opportunities = opportunities;
        _errors = errors;
        BuildLayout();
        Load += async (_, _) => await RefreshAsync();
    }

    private void BuildLayout()
    {
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 0, 0, 8) };
        toolbar.Controls.Add(ControlFactory.ToolbarButton("Neue Bewerbung", async (_, _) => await CreateAsync()));
        toolbar.Controls.Add(ControlFactory.ToolbarButton("Status ändern", async (_, _) => await ChangeStageAsync()));
        toolbar.Controls.Add(ControlFactory.ToolbarButton("Historie", (_, _) => ShowHistory()));
        toolbar.Controls.Add(ControlFactory.ToolbarButton("Aktualisieren", async (_, _) => await RefreshAsync()));
        Controls.Add(_grid);
        Controls.Add(toolbar);
    }

    private async Task RefreshAsync()
    {
        try
        {
            _items = await _service.ListAsync();
            var opportunities = await _opportunities.ListAsync();
            var titles = opportunities.ToDictionary(item => item.Id, item => item.Title);
            _grid.DataSource = _items.Select(item => new
            {
                item.Id,
                Position = titles.TryGetValue(item.OpportunityId, out var title) ? title : "(Stelle nicht gefunden)",
                Status = DisplayText.ApplicationStage(item.Stage),
                Kanal = item.Channel.ToString(),
                Gestartet = item.StartedAtUtc.LocalDateTime.ToShortDateString(),
                Versendet = item.SubmittedAtUtc?.LocalDateTime.ToShortDateString() ?? string.Empty,
                Historie = item.StatusHistory.Count,
            }).ToList();
            if (_grid.Columns["Id"] is { } idColumn)
            {
                idColumn.Visible = false;
            }
        }
        catch (Exception ex)
        {
            _errors.Show(ex, this);
        }
    }

    private async Task CreateAsync()
    {
        try
        {
            var opportunities = await _opportunities.ListAsync();
            using var dialog = new ApplicationEditForm(opportunities);
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            await _service.CreateAsync(dialog.Input);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _errors.Show(ex, this);
        }
    }

    private async Task ChangeStageAsync()
    {
        var selected = SelectedItem();
        if (selected is null)
        {
            return;
        }
        using var dialog = new ApplicationStageForm(selected.Stage);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }
        try
        {
            await _service.ChangeStageAsync(selected.Id, dialog.Stage, dialog.Note);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _errors.Show(ex, this);
        }
    }

    private void ShowHistory()
    {
        var selected = SelectedItem();
        if (selected is null)
        {
            return;
        }
        using var dialog = new ApplicationHistoryForm(selected);
        dialog.ShowDialog(this);
    }

    private JobApplication? SelectedItem()
    {
        if (_grid.CurrentRow?.Cells["Id"].Value is Guid id)
        {
            return _items.SingleOrDefault(item => item.Id == id);
        }
        return null;
    }
}
