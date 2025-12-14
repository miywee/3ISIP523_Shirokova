using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class OrdersProduct
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
