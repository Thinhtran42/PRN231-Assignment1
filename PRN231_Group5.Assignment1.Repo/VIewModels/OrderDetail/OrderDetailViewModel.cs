namespace PRN231_Group5.Assignment1.Repo.VIewModels.OrderDetail;

public class OrderDetailViewModel
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }
    
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double Discount { get; set; }
}