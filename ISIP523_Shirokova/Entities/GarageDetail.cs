using System;
using System.Collections.Generic;

namespace ISIP523_Shirokova.Entities;

public partial class GarageDetail
{
    public int Id { get; set; }

    public int GarageId { get; set; }

    public int DetailsId { get; set; }

    public virtual Detail Details { get; set; } = null!;

    public virtual Garage Garage { get; set; } = null!;
}
