using Application.Domain.Users;
using Application.Usecases.Users.Command;
using Core.Services.Session;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.Auth;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ISessionManagerService _session;

        private const string SessionKeyUser = "CurrentUser";

        public AuthController(IMediator mediator, ISessionManagerService session)
        {
            _mediator = mediator;
            _session = session;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mediator.Send(new LoginQuery { Email = model.EmailAddress, Password = model.Password });

            if (!result.Success)
            {
                model.Error = result.Errors.FirstOrDefault();
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View(new SignupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new UserAccount
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                Password = model.Password
            };

            var result = await _mediator.Send(new SignupCommand { User = user });

            if (!result.Success)
            {
                model.Errors = new ErrorViewModel(result.Errors);
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _session.ClearAll();
            return RedirectToAction("Login");
        }
    }
}