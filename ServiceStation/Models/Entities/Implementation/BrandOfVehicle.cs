﻿using ServiceStation.Models.Entities.Abstraction;

namespace ServiceStation.Models.Entities.Implementation;

public class BrandOfVehicle : AbstractEntity
{
    public string Brand { get; set; }
    
    public string Model { get; set; }
}