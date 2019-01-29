using BlogData;
using BlogData.Entities;
using BlogVF.SharedObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _1BlogVF.Controllers
{
    public class AppController : Controller
    {
        private readonly IBlogRepository _repository;
        private IEnumerable<NavBarEntity> navBarEntities;
        private readonly ILogger<AppController> _logger;
        private readonly UserManager<BlogOwner> _userManager;

        public AppController(IBlogRepository repository,
                            ILogger<AppController> logger,
                            UserManager<BlogOwner> userManager)
        {
            _repository = repository;
            navBarEntities = _repository.GetAllNavBarEntities();
            _logger = logger;
            _userManager = userManager;
        }

        //the name of the action should match the name of the view we earlier created Index.chtml 
        public async Task<IActionResult> Index()
        {
            try
            {
                //View() helper methpod is used to return the ViewResult, the Index.chtml page is rendereds and shown
                BlogOwner blogOwner = new BlogOwner();
                if (User.Identity.IsAuthenticated)
                {
                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    blogOwner.FirstName = currentUser.FirstName;
                    blogOwner.LastName = currentUser.LastName;
                }
                BaseViewModel<BlogOwner> baseViewModel = new BaseViewModel<BlogOwner>()
                {
                    NavBarEntities = navBarEntities,
                    ObjView = blogOwner
                };
                return View(baseViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while loading Index page in app controller", ex);
            }
            return BadRequest();
        }
    }
}