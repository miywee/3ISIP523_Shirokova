using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class Detail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }
    
    

    public virtual ICollection<GarageDetail> GarageDetails { get; set; } = new List<GarageDetail>();
}
