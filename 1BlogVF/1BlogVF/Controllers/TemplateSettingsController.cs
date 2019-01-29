using AutoMapper;
using BlogData;
using BlogData.Entities;
using BlogVF.SharedObjects;
using BlogVF.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace _1BlogVF.Controllers
{
    public class TemplateSettingsController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<BlogOwner> _signInManager;
        private readonly UserManager<BlogOwner> _userManager;
        private readonly IConfiguration _config;
        IEnumerable<NavBarEntity> navBarEntities;
        BaseViewModel<LoginViewModel> baseViewModel;
        BaseViewModel<TemplateViewModel> settingsViewModel;
        private readonly IBlogRepository _repository;
        private readonly IMapper _mapper;

        public TemplateSettingsController(ILogger<AccountController> logger,
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
            settingsViewModel = new BaseViewModel<TemplateViewModel>() { NavBarEntities = navBarEntities, ObjView = new TemplateViewModel() };
        }
        [HttpGet("SettingsNavBar")]
        public IActionResult SettingsNavBar()
        {
            try
            {

                return View(settingsViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while trying to get settings NavBar", ex);
            }
            return BadRequest();
        }
        [HttpPost("SettingsNavBar")]
        public IActionResult SettingsNavBar(BaseViewModel<TemplateViewModel> baseViewSettings)
        {
            try
            {
                var model = baseViewSettings.ObjView;

                if (ModelState.IsValid)
                {
                    var newNav = _mapper.Map<TemplateViewModel, NavBarEntity>(model);
                    _repository.AddEntity(newNav);
                    _repository.SaveAll();
                }
                ModelState.AddModelError("", "Failed to save nav");
                return View(settingsViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while trying to post settings NavBar", ex);
            }
            return BadRequest();
        }
    }
}