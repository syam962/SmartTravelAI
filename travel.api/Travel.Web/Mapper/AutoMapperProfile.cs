using AutoMapper;
using Travel.Web.DTO;
using Travel.Web.Models;
using Travel.Web.Models.DTOs;

namespace Travel.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Flight to FlightDTO mapping  
            CreateMap<Flight, FlightDTO>()
                .ForMember(dest => dest.FlightCompany, opt => opt.MapFrom(src => src.FlightCompany))
                .ForMember(dest => dest.SourceCity, opt => opt.MapFrom(src => src.SourceCity))
                .ForMember(dest => dest.DestinationCity, opt => opt.MapFrom(src => src.DestinationCity))
                .ForMember(dest => dest.FlightSegments, opt => opt.MapFrom(src => src.FlightSegments));

            // FlightSegment to FlightSegmentDTO mapping  
            CreateMap<FlightSegment, FlightSegmentDTO>()
                .ForMember(dest => dest.SegmentSourceCity, opt => opt.MapFrom(src => src.SegmentSourceCity))
                .ForMember(dest => dest.SegmentDestinationCity, opt => opt.MapFrom(src => src.SegmentDestinationCity));

            // City to CityDTO mapping  
            CreateMap<City, CityDTO>();

            // FlightCompany to FlightCompanyDTO mapping  
            CreateMap<FlightCompany, FlightCompanyDTO>();

            // FlightSegmentClass to FlightSegmentClassDTO mapping
            CreateMap<FlightSegmentClass, FlightSegmentClassDTO>();

            CreateMap<Booking, BookingDTO>();


        }
    }
}
