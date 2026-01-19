using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Attributes;

namespace Web.Controllers
{
    [AuthorizeSession]
    public class InvestmentControllercs : Controller
    {
        // GET: InvestmentControllercs
        public ActionResult Index()
        {
            return View();
        }

        // GET: InvestmentControllercs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InvestmentControllercs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvestmentControllercs/Create
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

        // GET: InvestmentControllercs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InvestmentControllercs/Edit/5
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

        // GET: InvestmentControllercs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InvestmentControllercs/Delete/5
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
