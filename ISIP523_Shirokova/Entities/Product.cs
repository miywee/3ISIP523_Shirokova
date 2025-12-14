using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();
}
