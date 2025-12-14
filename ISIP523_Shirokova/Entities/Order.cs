using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public double? Amount { get; set; }

    public int PointId { get; set; }

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual Point Point { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
