using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class Garage
{
    public int Id { get; set; }

    public List<string> Name { get; set; } = null!;

    public List<decimal> Balance { get; set; } = null!;
}
