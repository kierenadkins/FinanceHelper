using FinanceHelper.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHelper.Web.Controllers
{
    [AuthorizeSession]
    public class SavingsController : Controller
    {
        // GET: SavingsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SavingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SavingsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SavingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SavingsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SavingsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SavingsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SavingsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
