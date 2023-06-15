using Make_a_Drop.Core.Entities;

namespace Make_a_Drop.Application.Models.Drop
{
    public class DropListModel
    {
        public List<DropModel> Drops { get; set; }
    }

    public class DropListResponseModel : BaseResponseModel
    {
        public List<DropModel> Drops { get; set; }
    }
}
