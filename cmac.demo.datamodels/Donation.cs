using System.ComponentModel.DataAnnotations;

namespace cmac.demo.datamodels;

public class Donation
{
    public int Id { get; set; }

    public required DateTime DonationDate { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public required decimal Amount { get; set; }

    public required PaymentMethod PaymentMethod { get; set; }

    public string? OtherPaymentMethod { get; set; }

    public string? Notes { get; set; }
}