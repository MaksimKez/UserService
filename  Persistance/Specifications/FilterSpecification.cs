using Application.Dtos;
using Ardalis.Specification;
using Domain.Entities;

namespace Persistence.Specifications;

public class FilterSpecification : Specification<UserFilterEntity>
{
    public FilterSpecification(ListingDto listing)
    {
        Query.Where(f =>
            (!f.MinPrice.HasValue || listing.Price >= f.MinPrice.Value) &&
            (!f.MaxPrice.HasValue || listing.Price <= f.MaxPrice.Value) &&
            (!f.MinAreaMeterSqr.HasValue || listing.AreaMeterSqr >= f.MinAreaMeterSqr.Value) &&
            (!f.MaxAreaMeterSqr.HasValue || listing.AreaMeterSqr <= f.MaxAreaMeterSqr.Value) &&
            (!f.MinRooms.HasValue || listing.Rooms >= f.MinRooms.Value) &&
            (!f.MaxRooms.HasValue || listing.Rooms <= f.MaxRooms.Value) &&
            (!f.MinFloor.HasValue || listing.Floor >= f.MinFloor.Value) &&
            (!f.MaxFloor.HasValue || listing.Floor <= f.MaxFloor.Value) &&
            (!f.IsFurnished.HasValue || listing.IsFurnished == f.IsFurnished.Value) &&
            (!f.PetsAllowed.HasValue || listing.PetsAllowed == f.PetsAllowed.Value) &&
            (!f.HasBalcony.HasValue || listing.HasBalcony == f.HasBalcony.Value) &&
            (!f.HasAppliances.HasValue || listing.HasAppliances == f.HasAppliances.Value) &&
            (!f.NewerThanDays.HasValue || 
             listing.CreatedAt >= DateTime.UtcNow.AddDays(-f.NewerThanDays.Value))
        );
    }
}

