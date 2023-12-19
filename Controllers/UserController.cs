using GoodsStore.Interfaces;
using GoodsStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GoodsStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsersAsync();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var userDetailViewModel = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            return View(userDetailViewModel);
        }
    }
}
