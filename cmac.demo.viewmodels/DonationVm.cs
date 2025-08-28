using System.ComponentModel.DataAnnotations;

namespace cmac.demo.viewmodels;

public class DonationVm : IValidatableObject
{
    public int? Id { get; set; }

    [Required]
    public DateTime? DonationDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal? Amount { get; set; }

    [Required]
    public PaymentMethods? PaymentMethod { get; set; }

    public string? OtherPaymentMethod { get; set; }

    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var donation = validationContext.ObjectInstance as DonationVm;

        if (donation != null)
        {
            // Verify Date is not in the future
            if (donation.DonationDate > DateTime.Now.Date)
                yield return new ValidationResult("Donation Date must not be in the future.", [nameof(DonationDate)]);

            // Verify Other Payment Method field is filled out if 'Other' option has been selected
            if(donation.PaymentMethod == PaymentMethods.Other && string.IsNullOrEmpty(donation.OtherPaymentMethod?.Trim()))
                yield return new ValidationResult("Other Payment Method field is required.", [nameof(OtherPaymentMethod)]);
        }
    }
}