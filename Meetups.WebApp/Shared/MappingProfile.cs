using AutoMapper;
using Meetups.WebApp.Shared.ViewModels;

namespace Meetups.WebApp.Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            // Add your mappings here
            // Example:
            // CreateMap<Source, Destination>();

            CreateMap<EventViewModel, Data.Entities.Event>();
            CreateMap<Data.Entities.Event, EventViewModel>();

            CreateMap<CommentViewModel, Data.Entities.Comment>();
            CreateMap<Data.Entities.Comment, CommentViewModel>();
        }
    }
}
