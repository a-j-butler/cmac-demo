using cmac.demo.data;
using cmac.demo.datamodels;
using cmac.demo.viewmodels;

namespace cmac.demo.Extensions;
public static class AppDataExensions
{
    public static void SetupSeedData(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var paymentMethods = new List<PaymentMethod>()
            {
                new PaymentMethod { Id = 1, Name = PaymentMethods.Card.ToString() },
                new PaymentMethod { Id = 2, Name = PaymentMethods.Cash.ToString() },
                new PaymentMethod { Id = 3, Name = PaymentMethods.Cheque.ToString() },
                new PaymentMethod { Id = 4, Name = PaymentMethods.Online.ToString() },
                new PaymentMethod { Id = 5, Name = PaymentMethods.Other.ToString() }
            };

            db.PaymentMethods.AddRange(paymentMethods);
            db.SaveChanges();

            db.Donations.AddRange(
                new Donation { Amount = 5.0m, DonationDate = DateTime.Now.AddDays(-1), PaymentMethod = paymentMethods[3], Notes = null },
                new Donation { Amount = 14.50m, DonationDate = DateTime.Now.AddDays(-1), PaymentMethod = paymentMethods[1], Notes = "Test notes for Cash donation" },
                new Donation { Amount = 7.0m, DonationDate = DateTime.Now.AddDays(-2), PaymentMethod = paymentMethods[0], Notes = "Test notes for Cash donation" },
                new Donation { Amount = 3.0m, DonationDate = DateTime.Now.AddDays(-3), PaymentMethod = paymentMethods[3], Notes = "Test notes for Online donation" },
                new Donation { Amount = 8.0m, DonationDate = DateTime.Now.AddDays(-3), PaymentMethod = paymentMethods[2], Notes = "Test Notes for Cheque donation" },
                new Donation { Amount = 12.0m, DonationDate = DateTime.Now.AddDays(-3), PaymentMethod = paymentMethods[0], Notes = "Test Notes for Card donation" },
                new Donation { Amount = 21.70m, DonationDate = DateTime.Now.AddDays(-5), PaymentMethod = paymentMethods[0], Notes = "Test Notes for Card donation" },
                new Donation { Amount = 19.0m, DonationDate = DateTime.Now.AddDays(-5), PaymentMethod = paymentMethods[0], Notes = null },
                new Donation { Amount = 5.0m, DonationDate = DateTime.Now.AddDays(-6), PaymentMethod = paymentMethods[4], OtherPaymentMethod = "BitCoin", Notes = "Converted from BitCoin to GBP" },
                new Donation { Amount = 4.25m, DonationDate = DateTime.Now.AddDays(-7), PaymentMethod = paymentMethods[3], Notes = "Test notes for Online donation" },
                new Donation { Amount = 1.0m, DonationDate = DateTime.Now.AddDays(-8), PaymentMethod = paymentMethods[0], Notes = "Test Notes for Card donation" },
                new Donation { Amount = 2.0m, DonationDate = DateTime.Now.AddDays(-8), PaymentMethod = paymentMethods[1], Notes = null }
            );
            db.SaveChanges();
        }
    }
}