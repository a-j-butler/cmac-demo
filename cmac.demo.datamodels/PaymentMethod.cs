namespace cmac.demo.datamodels;

public class PaymentMethod
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}