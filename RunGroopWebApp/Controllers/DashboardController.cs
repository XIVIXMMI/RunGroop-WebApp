using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository,
            IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
        private void MapUserEdit(AppUser user,EditUserDashboardViewModel viewModel,ImageUploadResult photoResult)
        {
            user.Id = viewModel.Id;
            user.Pace = (int)viewModel.Pace;
            user.Mileage = (int)viewModel.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = viewModel.City;
            user.State = viewModel.State;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Encountered an issue while attempting to modify the user profile.");
                return View("EditUserProfile", viewModel);
            }
            AppUser user = await _dashboardRepository.GetByIdNoTracking(viewModel.Id);

            if(user.ProfileImageUrl == " " || user.ProfileImageUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(viewModel.Image);
  
                MapUserEdit(user, viewModel, photoResult);

                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(viewModel);
                }

                var photoResult = await _photoService.AddPhotoAsync(viewModel.Image);

                MapUserEdit(user, viewModel, photoResult);

                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }
            
        }
    }
}

