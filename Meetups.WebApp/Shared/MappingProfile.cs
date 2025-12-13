using AutoMapper;

namespace Meetups.WebApp.Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            // Add your mappings here
            // Example:
            // CreateMap<Source, Destination>();

            CreateMap<Features.Events.CreateEvent.EventViewModel, Data.Entities.Event>();
            CreateMap<Data.Entities.Event, Features.Events.CreateEvent.EventViewModel>();




        }
    }
}
