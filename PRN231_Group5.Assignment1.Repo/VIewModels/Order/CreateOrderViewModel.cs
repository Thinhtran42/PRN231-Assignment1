namespace PRN231_Group5.Assignment1.Repo.VIewModels.Order;

public class CreateOrderViewModel
{
    public int OrderId { get; set; }

    public int? MemberId { get; set; }

    public decimal? Freight { get; set; }
}