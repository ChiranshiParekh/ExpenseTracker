using MVC_ExpTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC_ExpTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Dashboard(int id = 0)
        {
            HttpResponseMessage CatResponse = GlobalVariables.WebApiClient.GetAsync("Categories").Result;
            IEnumerable<Category> categoryList = CatResponse.Content.ReadAsAsync<IEnumerable<Category>>().Result;
            if (id == 0)
            {
                //all expense
                HttpResponseMessage responseMessage1 = GlobalVariables.WebApiClient.GetAsync("Budgets/BudgetLimit").Result;
                List<decimal> budgetLimit = responseMessage1.Content.ReadAsAsync<List<decimal>>().Result;
                var budgetlimitList = budgetLimit.ToList();

                HttpResponseMessage responseMessage2 = GlobalVariables.WebApiClient.GetAsync("Budgets/BudgetExpense").Result;
                List<decimal> budgetExpense = responseMessage2.Content.ReadAsAsync<List<decimal>>().Result;
                var budgetExpenseList = budgetExpense.ToList();   

                HttpResponseMessage ExpResponse = GlobalVariables.WebApiClient.GetAsync("Expenses").Result;
                IEnumerable<ExpenseViewModel> expList = ExpResponse.Content.ReadAsAsync<IEnumerable<ExpenseViewModel>>().Result;

                DashboardViewModel viewModel = new DashboardViewModel();
                viewModel.Categories = categoryList;
                viewModel.Expenses = expList;
                viewModel.Limit = budgetlimitList.ElementAt(0);
                viewModel.ExpenseAmt = budgetExpenseList.ElementAt(0);

                return View(viewModel);
            }
            else
            {
                DashboardViewModel viewModel = new DashboardViewModel();
                
                //specific category expenses
                HttpResponseMessage response1 = GlobalVariables.WebApiClient.GetAsync("Categories/TotalCategoryLimit/" + id.ToString()).Result;
                var val1 = response1.Content.ReadAsAsync<List<decimal>>().Result;
                var valList1 = val1.ToList();
                viewModel.Limit = valList1.ElementAt(0);

                HttpResponseMessage response2 = GlobalVariables.WebApiClient.GetAsync("Categories/TotalCategoryExpense/" + id.ToString()).Result;
                var val2 = response2.Content.ReadAsAsync<List<decimal>>().Result;
                var valList2 = val2.ToList();
                viewModel.ExpenseAmt = valList2.ElementAt(0);

                HttpResponseMessage ExpResponse = GlobalVariables.WebApiClient.GetAsync("Expenses/ExpenseByCategory/" + id.ToString()).Result;
                IEnumerable<ExpenseViewModel> expList = ExpResponse.Content.ReadAsAsync<IEnumerable<ExpenseViewModel>>().Result;
                viewModel.Expenses = expList;
                viewModel.Categories = categoryList;

                return View(viewModel);
            }
            
        }
    }
}