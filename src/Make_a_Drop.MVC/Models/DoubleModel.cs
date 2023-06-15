using Make_a_Drop.Application.Models.Collaboration;
using Make_a_Drop.Application.Models.Drop;

namespace Make_a_Drop.MVC.Models
{
    public class DoubleModel
    {
        
        public X.PagedList.IPagedList<DropResponseModel>? Drops { get; set; }
        public CollaborationModel? Collaboration { get; set; }
    }
}
