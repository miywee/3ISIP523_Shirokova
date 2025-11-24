using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class Detail
{
    public int Id { get; set; }

    public List<string> Name { get; set; } = null!;

    public List<decimal>? Price { get; set; }
}
