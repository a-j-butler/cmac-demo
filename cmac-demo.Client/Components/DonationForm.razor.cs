using System.Net.Http.Json;

using cmac.demo.viewmodels;

using Microsoft.AspNetCore.Components;

using Radzen;

namespace cmac.demo.Client.Components;

public partial class DonationForm : ComponentBase
{
    [Inject]
    public NotificationService? NotificationService { get; set; }

    [Inject]
    public HttpClient? Http { get; set; }

    [Parameter]
    public EventCallback OnDonationAdded { get; set; }

    protected DonationVm Donation { get; set; } = new();

    protected IEnumerable<Enum> paymentMethods = Enum.GetValues(typeof(PaymentMethods)).Cast<Enum>();

    public async void HandleSubmit()
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

        var response = await Http.PostAsJsonAsync("/add-donation", Donation);

        if (response.IsSuccessStatusCode)
        {
            NotificationService?.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Success",
                Detail = "Donation was added successfully",
                Duration = 4000
            });

            await OnDonationAdded.InvokeAsync();

            ClearForm();
        }
        else
        {
            NotificationService?.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = "Donation was not added due to an error",
                Duration = 4000
            });
        }
    }

    public void ClearForm()
    {
        Donation = new();

        StateHasChanged();
    }
}