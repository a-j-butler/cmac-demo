using System.ComponentModel.DataAnnotations;

namespace cmac.demo.viewmodels;

public class DonationListVm
{
    public int TotalCount { get; set; }

    public IEnumerable<DonationListItemVm>? DonationList { get; set; }
}