using GoodsStore.Data;
using GoodsStore.Interfaces;
using GoodsStore.Models;
using GoodsStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IOrderItemsRepository _orderItemsRepository;

        public AccountApiController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IOrderItemsRepository orderItemsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderItemsRepository = orderItemsRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Ok("Login successful");
                    }
                }
                return BadRequest("Wrong credentials. Please try again");
            }
            return BadRequest("Wrong email. Please try again");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                return BadRequest("This email address is already in use");
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return Ok("User registered successfully");
            }

            return BadRequest("Failed to register user");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout successful");
        }

        [HttpGet("user-orders")]
        public async Task<IActionResult> UserOrders()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var orders = _orderItemsRepository.GetOrdersByUser(user);

            return Ok(orders);
        }
    }
}
