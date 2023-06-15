using AutoMapper;
using Make_a_Drop.Application.Models.Drop;
using Make_a_Drop.Core.Entities;

namespace Make_a_Drop.Application.MappingProfiles
{
    public class DropProfile : Profile
    {

        public DropProfile()
        {
            CreateMap<Drop, CreateDropModel>();
            CreateMap<CreateDropModel, Drop>();
            CreateMap<Drop, DropResponseModel>();
            CreateMap<DropResponseModel, Drop>();
            CreateMap<Drop, DropModel>();
            CreateMap<DropModel, Drop>();
            CreateMap<DropModel, UploadModel>();
            CreateMap<UploadModel, DropModel>();
            CreateMap<BaseUploadModel, DropModel>();
            CreateMap<DropModel, BaseUploadModel > ();

        }


    }
}
