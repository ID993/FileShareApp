using AutoMapper;
using Make_a_Drop.Application.Models.Collaboration;
using Make_a_Drop.Core.Entities;

namespace Make_a_Drop.Application.MappingProfiles
{
    public class CollaborationProfile : Profile
    {
        public CollaborationProfile() 
        {
            CreateMap<Collaboration, CreateCollaborationModel>();
            CreateMap<CreateCollaborationModel, Collaboration>();
            CreateMap<Collaboration, CollaborationModel>();
            CreateMap<CollaborationModel, Collaboration>();
        }
    }
}
