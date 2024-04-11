namespace PRN231_Group5.Assignment1.Repo.VIewModels.Product;

public class CreateProductViewModel
{
    public int? CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int UnitsInStock { get; set; }
}