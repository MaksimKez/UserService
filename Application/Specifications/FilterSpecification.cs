using Application.Dtos;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public class FilterSpecification : Specification<UserFilterEntity>
{
    public FilterSpecification(ListingDto listing)
    {
        Query.Where(f =>
            (!f.MinPrice.HasValue || f.MinPrice.Value <= listing.Price) &&
            (!f.MaxPrice.HasValue || f.MaxPrice.Value >= listing.Price) &&
            (!f.MinAreaMeterSqr.HasValue || f.MinAreaMeterSqr.Value <= listing.AreaMeterSqr) &&
            (!f.MaxAreaMeterSqr.HasValue || f.MaxAreaMeterSqr.Value >= listing.AreaMeterSqr) &&
            (!f.MinRooms.HasValue || f.MinRooms.Value <= listing.Rooms) &&
            (!f.MaxRooms.HasValue || f.MaxRooms.Value >= listing.Rooms) &&
            (!f.MinFloor.HasValue || f.MinFloor.Value <= listing.Floor) &&
            (!f.MaxFloor.HasValue || f.MaxFloor.Value >= listing.Floor) &&
            (!f.IsFurnished.HasValue || f.IsFurnished.Value == listing.IsFurnished) &&
            (!f.PetsAllowed.HasValue || f.PetsAllowed.Value == listing.PetsAllowed) &&
            (!f.HasBalcony.HasValue || f.HasBalcony.Value == listing.HasBalcony) &&
            (!f.HasAppliances.HasValue || f.HasAppliances.Value == listing.HasAppliances) &&
            (!f.NewerThanDays.HasValue || 
             listing.CreatedAt >= DateTime.UtcNow.AddDays(-f.NewerThanDays.Value))
        );
    }
}
