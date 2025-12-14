using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductCart> ProductCarts { get; set; } = new List<ProductCart>();
}
