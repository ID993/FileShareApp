using AutoMapper;
using Make_a_Drop.Application.Models.Drop;
using Make_a_Drop.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Make_a_Drop.Application.Helpers;
using X.PagedList;
using Make_a_Drop.MVC.Filters;
using FluentValidation;
using Make_a_Drop.Application.Exceptions;

namespace Make_a_Drop.MVC.Controllers
{
    public class DropsController : Controller
    {
        private readonly IDropService _dropService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IDropFileService _dropFileService;
        private readonly IValidator<UploadModel> _validator;
        private readonly IMapper _mapper;
        
        public DropsController(IDropService dropService, IFirebaseStorageService firebaseStorageService, 
            IDropFileService dropFileService, IValidator<UploadModel> validator, IMapper mapper)
        {
            _dropService = dropService;
            _firebaseStorageService = firebaseStorageService;
            _dropFileService = dropFileService;
            _validator = validator;
            _mapper = mapper;               
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadModel model)
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
            //await _dropService.UploadFiles(model);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult DownloadDrop()
        {
            return View();
        }
      

        [HttpPost]
        public async Task<IActionResult> DownloadDrop(DownloadModel model)
        {
            var myDrop = await _dropService.GetByNameAsync(model.Name);
 
            if (myDrop != null && SecretKeyHash.MatchingSecretKey(myDrop.SecretKey, model.SecretKey))
            {
                return RedirectToAction("Download", new { myDrop.Id });
            }
            else
            {
                ViewBag.Message = "Drop doesn't exist or invalid secret key.";
                return View();
            }
        }

        

        [HttpGet("/Drops/Download/{guid}")]
        public async Task<IActionResult> Download(Guid guid)
        {
            var files = await _dropFileService.GetByDropIdAsync(guid);
            
            if (files.Count == 1)
            {
                var stream = await _firebaseStorageService.DownloadFileAsync(files.First().FileName);
                await _dropService.IfOwnerAnonDelete(guid);
                return File(stream, "application/octet-stream", Path.GetFileName(files.First().FileName));
            }

            var fileList = (await Task.WhenAll(files.Select(async file =>
            {
                var stream = await _firebaseStorageService.DownloadFileAsync(file.FileName);
                return new FileStreamResult(stream, "application/octet-stream")
                {
                    FileDownloadName = Path.GetFileName(file.FileName)
                };
            }))).ToList();

            await _dropService.IfOwnerAnonDelete(guid);
            
            return File(CreateZipFile.Zip(fileList), "application/zip", $"Drop_{guid}.zip");
            
        }

        [CustomAuthorize]
        [HttpGet]
        public ViewResult GetAll(string? sortOrder, string? currentFilter, string? searchString, int? page)
        {
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
                "nameDesc" => _dropService.OrderByNameDesc(),
                "Size" => _dropService.OrderBySizeAsc(),
                "sizeDesc" => _dropService.OrderBySizeDesc(),
                _ => _dropService.OrderByNameAsc()
            };

            if (!string.IsNullOrEmpty(searchString))
            {
                drops = _dropService.SearchByName(searchString);
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(drops.ToPagedList(pageNumber, pageSize));
        }

        [CustomAuthorize]
        [HttpGet("Drops/Details/{guid}")]
        public async Task<IActionResult> Details(Guid? guid)
        {
            var drop = await _dropService.FindByIdAsync(guid);
            var files = await _dropFileService.GetByDropIdAsync(guid);
            ViewBag.Files = files;
            return View(drop);
        }

        [CustomAuthorize]
        [HttpGet("Drops/Delete/{guid}")]
        public async Task<IActionResult> Delete(Guid? guid)
        {
            ViewBag.Files = await _dropFileService.GetByDropIdAsync(guid);
            return View(await _dropService.GetByIdAsync(guid));
        }

        [CustomAuthorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? guid)
        {
            var drop = await _dropService.DeleteAsync(guid);
            return RedirectToAction(nameof(GetAll));
        }

        [CustomAuthorize]
        [HttpGet("Drops/Edit/{guid}")]
        public async Task<IActionResult> Edit(Guid? guid)
        {
            var drop = await _dropService.FindByIdAsync(guid);
            ViewBag.Files = await _dropFileService.GetByDropIdAsync(guid);
            return View(drop);
        }

        [CustomAuthorize]
        [HttpPost("Drops/Edit/{guid}"), ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? guid, DropResponseModel drop)
        {
            if (guid != drop.Id)
            {
                throw new NotFoundException("Drop not found");
            }
            if (ModelState.IsValid)
            {
               await _dropService.UpdateAsync(drop);
               return RedirectToAction(nameof(Details), new { drop.Id });
            }
            return View(drop);
        }

    }
}

