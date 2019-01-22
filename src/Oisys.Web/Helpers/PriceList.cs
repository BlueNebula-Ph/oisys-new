using System.ComponentModel.DataAnnotations;

namespace OisysNew.Helpers
{
    public enum PriceList
    {
        [Display(Name = "Main Price")]
        MainPrice = 1,

        [Display(Name = "Walk-In Price")]
        WalkInPrice = 2,

        [Display(Name = "N.E. Price")]
        NEPrice = 3
    }
}
