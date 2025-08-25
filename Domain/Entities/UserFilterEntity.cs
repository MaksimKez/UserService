using System.Runtime.InteropServices.JavaScript;

namespace Domain.Entities;

public class UserFilterEntity
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public UserProfileEntity Profile { get; set; }
    
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public decimal? MinAreaMeterSqr { get; set; }
    public decimal? MaxAreaMeterSqr { get; set; }

    public int? MinRooms { get; set; }
    public int? MaxRooms { get; set; }

    public int? MinFloor { get; set; }
    public int? MaxFloor { get; set; }

    public bool? IsFurnished { get; set; }
    public bool? PetsAllowed { get; set; }
    public bool? HasBalcony { get; set; }
    
    public int? NewerThanDays { get; set; }
    
    public bool? HasAppliances { get; set; }

    public static UserFilterEntity Default(Guid profileId)
    {
        return new UserFilterEntity
        {
            Id = Guid.NewGuid(),
            ProfileId = profileId,
        
            MinPrice = 0,
            MaxPrice = 10000000,

            MinAreaMeterSqr = 0,
            MaxAreaMeterSqr = 10000000,

            MinRooms = 0,
            MaxRooms = 100,

            MinFloor = 0,
            MaxFloor = 100,

            IsFurnished = true,
            PetsAllowed = true,
            HasBalcony = true,
        
            NewerThanDays = 7,
            HasAppliances = true
        };
    }
}