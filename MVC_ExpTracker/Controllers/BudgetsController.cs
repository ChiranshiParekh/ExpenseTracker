using MVC_ExpTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC_ExpTracker.Controllers
{
    public class BudgetsController : Controller
    {
        // GET: Budgets
        public ActionResult Index()
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Budgets/FindBudget").Result;
            var isExists = response.Content.ReadAsAsync<bool>().Result;
            if(isExists)
            {
                //redirect to dashboard
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                //show the form to add budget
                return View(new Budget());
            }
        }

        [HttpPost]
        public ActionResult Index(Budget budget)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Budgets", budget).Result;
            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult Edit() 
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Budgets/" + 1).Result;
            return View(response.Content.ReadAsAsync<Budget>().Result);
        }

        [HttpPost]
        public ActionResult Edit(Budget budget)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Budgets/" + budget.budgetId, budget).Result;
            return RedirectToAction("Dashboard", "Home");
        }
    }
}