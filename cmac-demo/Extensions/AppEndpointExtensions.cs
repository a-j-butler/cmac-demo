using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;

using cmac.demo.data;
using cmac.demo.viewmodels;

using Microsoft.EntityFrameworkCore;

namespace cmac.demo.Extensions;

public static class AppEndpointExtensions
{
    public static void SetupEndpoints(this WebApplication app)
    {
        app.MapPost("/add-donation", async (DonationVm model, AppDbContext dbContext) =>
        {

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);

            if (!Validator.TryValidateObject(model, context, validationResults, true))
            {
                var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                return Results.BadRequest(new { Errors = errors });
            }

            try
            {
                int paymentMethodId = (int)model.PaymentMethod;
                var paymentMethod = dbContext.PaymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);

                var dm = new datamodels.Donation
                {
                    Amount = model.Amount ?? throw new ArgumentOutOfRangeException($"{nameof(DonationVm.Amount)}"),
                    DonationDate = model.DonationDate ?? throw new ArgumentOutOfRangeException($"{nameof(DonationVm.DonationDate)}"),
                    PaymentMethod = paymentMethod ?? throw new ArgumentException($"{nameof(DonationVm.PaymentMethod)}"),
                    OtherPaymentMethod = model.OtherPaymentMethod,
                    Notes = model.Notes
                };

                await dbContext.AddAsync(dm);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Donation added successfully");
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(statusCode: 500,
                                            title: "An error occurred attempting to add the donation",
                                            detail: ex.Message);
            }
        });

        app.MapGet("/get-donations", async (string filter, AppDbContext dbContext) =>
        {
            var query = dbContext.Donations.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(d =>
                    (d.Notes != null && d.Notes.Contains(filter, StringComparison.InvariantCultureIgnoreCase)) ||
                    (d.PaymentMethod != null && d.PaymentMethod.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
                );
            }

            var donations = await query.Select(d => new DonationListItemVm
            {
                Id = d.Id,
                Amount = d.Amount,
                DonationDate = d.DonationDate,
                PaymentMethod = new PaymentMethodVm { Id = d.PaymentMethod.Id, Name = d.PaymentMethod.Name },
                Notes = d.Notes
            }).ToListAsync();

            return new DonationListVm
            {
                TotalCount = donations.Count(),
                DonationList = donations
            };
        });
    }
}