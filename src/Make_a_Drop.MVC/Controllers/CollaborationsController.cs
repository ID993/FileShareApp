using Make_a_Drop.Application.Services;
using Make_a_Drop.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Make_a_Drop.Application.Models.Collaboration;
using Make_a_Drop.MVC.Filters;
using Make_a_Drop.Application.Models.Drop;
using AutoMapper;
using Make_a_Drop.Core.Entities.Identity;
using X.PagedList;
using FluentValidation;

namespace Make_a_Drop.MVC.Controllers
{
    public class CollaborationsController : Controller
    {
        private readonly ICollaborationService _collaborationService;
        private readonly IDropService _dropService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCollaborationModel> _validator;

        public CollaborationsController(ICollaborationService collaborationService, 
            IDropService dropService, IUserService userService, IMapper mapper, IValidator<CreateCollaborationModel> validator)
        {
            _collaborationService = collaborationService;
            _dropService = dropService;
            _userService = userService;
            _mapper = mapper;
            _validator = validator;
        }

        [CustomAuthorize]
        [HttpGet]
        public async Task<ViewResult> GetAll(string? sortOrder, string? currentFilter, string? searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";
           
            if (searchString != null || page < 1)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var collabs = sortOrder switch
            {
                "nameDesc" => _collaborationService.OrderByNameDesc(),
                _ => _collaborationService.OrderByNameAsc()
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                collabs = _collaborationService.SearchByName(searchString);
            }
            foreach (var collaboration in collabs)
            {
                var user = await _userService.GetByIdAsync(collaboration.OwnerId);
                collaboration.Owner = _mapper.Map<ApplicationUser>(user);
            }

            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(collabs.ToPagedList(pageNumber, pageSize));
        }

        

        [HttpGet]
        public async Task<ViewResult> GetAllParts(string? sortOrder, string? currentFilter, string? searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";

            if (searchString != null || page < 1)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var collabs = sortOrder switch
            {
                "nameDesc" => _collaborationService.OrderByNameDescParts(),
                _ => _collaborationService.OrderByNameAscParts()
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                collabs = _collaborationService.SearchByNameParts(searchString);
            }
            foreach (var collaboration in collabs)
            {
                var user = await _userService.GetByIdAsync(collaboration.OwnerId);
                collaboration.Owner = _mapper.Map<ApplicationUser>(user);
            }

            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(collabs.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewCollaborationModel newCollaborationModel)
        {
            await _collaborationService.CreateAsync(new CreateCollaborationModel { Name = newCollaborationModel.Name, Users = newCollaborationModel.Users });
            return RedirectToAction("GetAll");
        }


        [CustomAuthorize]
        [HttpGet("Collaborations/Details/{id}")]
        public async Task<ViewResult> Details(string? sortOrder, string? currentFilter, 
            string? searchString, int? page, Guid id)
        {
            var collaboration = await _collaborationService.GetByIdAsync(id);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";
            ViewBag.SizeSortParm = sortOrder == "Size" ? "sizeDesc" : "Size";

            if (searchString != null || page < 1)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var drops = sortOrder switch
            {
                "nameDesc" => _dropService.OrderByNameDescCol(id),
                "Size" => _dropService.OrderBySizeAscCol(id),
                "sizeDesc" => _dropService.OrderBySizeDescCol(id),
                _ => _dropService.OrderByNameAscCol(id)
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                drops = _dropService.SearchByNameCol(id, searchString);
            }

            int pageSize = 1;
            int pageNumber = (page ?? 1);
            var model = new DoubleModel 
            { 
                Drops = drops.ToPagedList(pageNumber, pageSize), 
                Collaboration = collaboration 
            };
            return View(model);

        }


        [CustomAuthorize]
        [HttpGet("Collaborations/Drop/{id}")]
        public IActionResult Drop(Guid id)
        {
            ViewBag.CollaborationId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Drop(UploadModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else if (await _dropService.UploadFiles(model) == 0)
            {
                ViewBag.Message = "Drop name already exist. Chose another name.";
                return View();
            }
            return RedirectToAction(nameof(Details), new { id = model.CollaborationId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(Guid collaborationId, string userId)
        {
            var collaboration = await _collaborationService.GetByIdAsync(collaborationId);
            var user = collaboration.Users.Single(u => u.Id == userId);
            collaboration.Users.Remove(user);
            var updatedCollaboration = await _collaborationService.UpdateAsync(collaboration);
            return RedirectToAction(nameof(Details), new { id = collaborationId });
        }

        [CustomAuthorize]
        [HttpGet("Collaborations/AddUsers/{id}")]
        public IActionResult AddUsers(Guid id)
        {
            var collaboration = new CollaborationAddUsersModel{ CollaborationId = id };
            return View(collaboration);
        }


        [HttpPost]
        public async Task<IActionResult> AddUsers(Guid collaborationId, string users)
        {
            await _collaborationService.AddUsers(new CreateCollaborationModel { Id = collaborationId, Users = users });
            return RedirectToAction("GetAll");
        }

        [CustomAuthorize]
        [HttpGet("Collaborations/Delete/{guid}")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            return View(await _collaborationService.GetByIdAsync(guid));
        }

        [CustomAuthorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid guid)
        {
            await _collaborationService.DeleteAsync(guid);
            return RedirectToAction(nameof(GetAll));
        }
    }   
}
