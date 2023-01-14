using Microsoft.Ajax.Utilities;
using MVC_ExpTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace MVC_ExpTracker.Controllers
{
    public class ExpensesController : Controller
    {
        public decimal TotCatLimitAmt, TotCatExpAmt, TotBudgetLimitAmt, TotBudgetExpAmt;

        // GET: Expenses
        public ActionResult Index()
        {
            IEnumerable<ExpenseViewModel> expenseList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Expenses").Result;
            expenseList = response.Content.ReadAsAsync<IEnumerable<ExpenseViewModel>>().Result;
            return View(expenseList);
        }

        public ActionResult AddorEdit(int id=0)
        {
            HttpResponseMessage Catresponse = GlobalVariables.WebApiClient.GetAsync("Categories").Result;
            IEnumerable<Category> categoriesList = Catresponse.Content.ReadAsAsync<IEnumerable<Category>>().Result;
            ViewBag.category = categoriesList;

            if (id == 0)
            {
                return View(new Expense());
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Expenses/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Expense>().Result);
            }
        }

        [HttpPost]
        public ActionResult AddorEdit(Expense expense)
        {
            HttpResponseMessage Catresponse = GlobalVariables.WebApiClient.GetAsync("Categories").Result;
            IEnumerable<Category> categoriesList = Catresponse.Content.ReadAsAsync<IEnumerable<Category>>().Result;
            ViewBag.category = categoriesList;

            if (expense.ExpenseId == 0)
            {
                if (CheckExpenseCriteria(expense))
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Expenses", expense).Result;
                    TempData["successMessage"] = "Saved Successfully";

                    UpdateCategoryExpense(expense);
                    UpdateBudgetTotalExpense(1);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["failureMessage"] = "Category Limit Excessed";
                    return View();
                }
            }
            else
            {
                if (CheckExpenseCriteria(expense))
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Expenses/" + expense.ExpenseId, expense).Result;
                    UpdateCategoryExpense(expense);
                    UpdateBudgetTotalExpense(1);
                    TempData["successMessage"] = "Updated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["failureMessage"] = "Category Limit Excessed";
                    return View();
                }
            }
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Expenses/" + id.ToString()).Result;
            TempData["successMessage"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }

        

        public decimal GetTotalCategoryLimit(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Categories/TotalCategoryLimit/" + id.ToString()).Result;
            var val = response.Content.ReadAsAsync<List<decimal>>().Result;
            var valList = val.ToList();
            TotCatLimitAmt = valList.ElementAt(0);
            return TotCatLimitAmt;
        }

        public decimal GetTotalCategoryExpense(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Categories/TotalCategoryExpense/" + id.ToString()).Result;
            var val = response.Content.ReadAsAsync<List<decimal>>().Result;
            var valList = val.ToList();
            TotCatExpAmt = valList.ElementAt(0);
            return TotCatExpAmt;
        }

        public decimal GetBudgetExpense()
        {
            HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.GetAsync("Budgets/BudgetExpense").Result;
            List<decimal> budgetExp = responseMessage.Content.ReadAsAsync<List<decimal>>().Result;
            var budgetExpList = budgetExp.ToList();
            TotBudgetExpAmt = budgetExpList.ElementAt(0);
            return TotBudgetExpAmt;
        }

        public decimal GetBudgetLimit()
        {
            HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.GetAsync("Budgets/BudgetLimit").Result;
            List<decimal> budgetLimit = responseMessage.Content.ReadAsAsync<List<decimal>>().Result;
            var budgetlimitList = budgetLimit.ToList();
            TotBudgetLimitAmt = budgetlimitList.ElementAt(0);
            return TotBudgetLimitAmt;
        }

        public bool CheckExpenseCriteria(Expense expense)
        {
            if((GetTotalCategoryExpense(expense.CategoryId) + expense.ExpenseAmount) <= GetTotalCategoryLimit(expense.CategoryId))
            {
                if((GetBudgetExpense()+expense.ExpenseAmount) <= GetBudgetLimit())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void UpdateCategoryExpense(Expense expense)
        {
            HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.PutAsJsonAsync
                ("Categories/UpdateCategoryExpense", expense).Result;
            var categoryExpense = responseMessage.Content.ReadAsAsync<decimal>().Result;
        }

        public void UpdateBudgetTotalExpense(int id)
        {
            HttpResponseMessage responseMessage = GlobalVariables.WebApiClient.GetAsync
                ("Budgets/UpdateBudgetTotalExpense/"+id.ToString()).Result;
            var BudgetExpense = responseMessage.Content.ReadAsAsync<decimal>().Result;
        }

        
    }
}