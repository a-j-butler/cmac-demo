using System.Net.Http.Json;

using cmac.demo.viewmodels;

using Microsoft.AspNetCore.Components;

using Radzen;
using Radzen.Blazor;

namespace cmac.demo.Client.Components;

public partial class DonationList : ComponentBase
{
    [Inject]
    public NotificationService? NotificationService { get; set; }

    [Inject]
    public HttpClient? Http { get; set; }

    protected RadzenDataGrid<DonationListItemVm>? Grid;

    protected IEnumerable<DonationListItemVm>? Donations;

    protected string searchTerm = "";

    protected int RecordCount = 0;

    protected bool IsLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await OnSearchAsync();
    }

    protected async Task OnSearchAsync()
    {
        if (Http == null)
        {
            NotificationService?.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = "Unable to communicate with the server",
                Duration = 4000
            });

            return;
        }

        if (IsLoading) return;

        IsLoading = true;

        StateHasChanged();

        try
        {
            var response = await Http.GetFromJsonAsync<DonationListVm>($"/get-donations?filter={searchTerm}");

            Donations = response?.DonationList ?? new List<DonationListItemVm>();

            RecordCount = response?.TotalCount ?? 0;
        }
        catch (Exception ex)
        {
            NotificationService?.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = "Unable to communicate with the server",
                Duration = 4000
            });
        }

        await Grid?.Reload();

        IsLoading = false;

        StateHasChanged();
    }

    public async Task RefreshDonationsAsync()
    {
        Console.WriteLine("Refresh Invoked");
        await OnSearchAsync();
    }
}