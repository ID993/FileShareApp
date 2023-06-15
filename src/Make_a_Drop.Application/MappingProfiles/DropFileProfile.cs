using AutoMapper;
using Make_a_Drop.Application.Models.DropFile;
using Make_a_Drop.Core.Entities;

namespace Make_a_Drop.Application.MappingProfiles
{
    public class DropFileProfile : Profile
    {
        public DropFileProfile()
        {
            CreateMap<DropFile, CreateDropFileModel>();
            CreateMap<CreateDropFileModel, DropFile>();
            CreateMap<DropFileModel, DropFile>();
            CreateMap<DropFile, DropFileModel>();
            
        }
    }
}
