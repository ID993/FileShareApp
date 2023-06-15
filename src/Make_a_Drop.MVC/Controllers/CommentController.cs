using AutoMapper;
using FluentValidation;
using Make_a_Drop.Application.Models.Comment;
using Make_a_Drop.Application.Services;
using Make_a_Drop.MVC.Filters;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Make_a_Drop.MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly IDropService _dropService;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCommentModel> _validator;

        public CommentController(IDropService dropService, ICommentService commentService, IMapper mapper,
            IValidator<CreateCommentModel> validator)
        {
            _dropService = dropService;
            _commentService = commentService;
            _mapper = mapper;
            _validator = validator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(Guid guid)
        {
            var comment = await _commentService.GetByIdAsync(guid);
            return View(comment);
        }

        [CustomAuthorize]
        [HttpGet("Comment/Create/{dropId}")]
        public IActionResult Create(Guid dropId)
        {
            return View();
        }

        [CustomAuthorize]
        [HttpPost("Comment/Create/{dropId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCommentModel model, Guid dropId)
        {
           
            await _commentService.CreateAsync(model, dropId);
            return RedirectToAction(nameof(GetAll), new { dropId });
  
        }

        [CustomAuthorize]
        [HttpGet("Comment/Delete/{guid}")]
        public async Task<IActionResult> Delete(Guid? guid)
        { 
            return View(await _commentService.GetByIdAsync(guid));
        }

        [CustomAuthorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? guid)
        { 
            var comment = await _commentService.DeleteAsync(guid);
            return RedirectToAction(nameof(GetAll), new { dropId = comment.Drop.Id });
        }

        [CustomAuthorize]
        [HttpGet("Comment/GetAll/{dropId}")]
        public ViewResult GetAll(string? sortOrder, int? page, Guid dropId)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "Date" ? "dateAsc" : "Date";

            var coments = sortOrder switch
            {
                "dateAsc" => _commentService.OrderByDateAsc(dropId),
                _ => _commentService.OrderByDateDesc(dropId)
            };

            ViewBag.dropId = dropId;

            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(coments.ToPagedList(pageNumber, pageSize));
        }
    }
}
