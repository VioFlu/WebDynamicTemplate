using AutoMapper;
using BlogData;
using BlogData.Entities;
using BlogVF.SharedObjects;
using BlogVF.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace _1BlogVF.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<BlogOwner> _signInManager;
        private readonly UserManager<BlogOwner> _userManager;
        private readonly IConfiguration _config;
        IEnumerable<NavBarEntity> navBarEntities;
        BaseViewModel<LoginViewModel> baseViewModel;
        //BaseViewModel<SettingsViewModel> settingsViewModel;
        private readonly IBlogRepository _repository;
        private readonly IMapper _mapper;

        public AccountController(ILogger<AccountController> logger,
                                SignInManager<BlogOwner> signInManager,
                                UserManager<BlogOwner> userManager,
                                IConfiguration config,
                                IBlogRepository repository,
                                IMapper mapper)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _repository = repository;
            _mapper = mapper;
            navBarEntities = _repository.GetAllNavBarEntities();
            baseViewModel = new BaseViewModel<LoginViewModel>() { NavBarEntities = navBarEntities, ObjView = new LoginViewModel() };
            //settingsViewModel = new BaseViewModel<SettingsViewModel>() { NavBarEntities = navBarEntities, ObjView = new SettingsViewModel() };
        }
        //[HttpGet("Settings")]
        //public IActionResult Settings()
        //{
        //    return View(settingsViewModel);
        //}
        //[HttpPost("settings")]
        //public IActionResult Settings(BaseViewModel<SettingsViewModel> baseViewSettings)
        //{
        //    var model = baseViewSettings.ObjView;

        //    if (ModelState.IsValid)
        //    {
        //        var newNav = _mapper.Map<SettingsViewModel, NavBarEntity>(model);
        //        _repository.AddEntity(newNav);
        //        _repository.SaveAll();
        //    }
        //    ModelState.AddModelError("", "Failed to save nav");
        //    return View(settingsViewModel);
        //}

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }
            return View(baseViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(BaseViewModel<LoginViewModel> baseView)
        {
            try
            {
                var model = baseView.ObjView;
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.UserName,
                                                                    model.Password,
                                                                    model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            Redirect(Request.Query["ReturnUrl"].First());
                        }
                        else
                        {
                            return RedirectToAction("Index", "App");
                        }
                    }
                }
                ModelState.AddModelError("", "Failed to login");
                return View(baseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while logging in", ex);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user != null)
                    {
                        var result = _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                        if (result.IsCompletedSuccessfully)
                        {
                            //create the token
                            var claims = new[]
                            {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                            //new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                _config["Tokens:Issuer"],
                                _config["Tokens:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(30),
                                signingCredentials: creds
                                );
                            var results = new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo
                            };
                            return Created("", results);
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred while issuing the Token", ex);
            }
            return BadRequest();
        }
    }
}