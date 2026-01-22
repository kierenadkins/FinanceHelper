using FinanceHelper.Application.Services.Session;
using FinanceHelper.Application.Usecases.Users.Command;
using FinanceHelper.Domain.Objects.Users;
using FinanceHelper.Web.Models;
using FinanceHelper.Web.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHelper.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ISessionManagerService _session;

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

            var result = await _mediator.Send(new LoginUserQuery { Email = model.EmailAddress, Password = model.Password });

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

            var result = await _mediator.Send(new SignupCommand
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                Password = model.Password
            });

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