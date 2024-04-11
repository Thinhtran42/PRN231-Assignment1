using System;
using System.Collections.Generic;

namespace PRN231_Group5.Assignment1.Repo.Models;

public partial class OrderDetail
{
    public int Id;
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double Discount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
