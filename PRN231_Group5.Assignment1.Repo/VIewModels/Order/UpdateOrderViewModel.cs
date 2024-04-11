namespace PRN231_Group5.Assignment1.Repo.VIewModels.Order;

public class UpdateOrderViewModel
{
    public int? MemberId { get; set; }
    public DateTime OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; } 

    public decimal? Freight { get; set; }
}