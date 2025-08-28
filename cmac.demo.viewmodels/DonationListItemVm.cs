namespace cmac.demo.viewmodels;

public class DonationListItemVm
{
    public int Id { get; set; }

    public DateTime DonationDate { get; set; }

    public decimal Amount { get; set; }

    public required PaymentMethodVm PaymentMethod { get; set; }

    public string? Notes { get; set; }
}