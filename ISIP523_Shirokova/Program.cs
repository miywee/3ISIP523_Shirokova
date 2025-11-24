using System;
using ISIP523_Shirokova;
using ISIP523_Shirokova.Context;
using ISIP523_Shirokova.Entities;


Console.WriteLine("Hello, World!"); 
List<Detail> details = Core.Context.Details.ToList();
List<Garage> garages = Core.Context.Garages.ToList();
List<DetailsGarage> detailsgarages = Core.Context.DetailsGarages.ToList();

