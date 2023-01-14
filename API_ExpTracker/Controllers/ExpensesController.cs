using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using API_ExpTracker.Models;

namespace API_ExpTracker.Controllers
{
    public class ExpensesController : ApiController
    {
        private ExpTracker_DbEntities db = new ExpTracker_DbEntities();

        public IHttpActionResult GetExpense()
        {
            var expenses = db.Expenses.Join(db.Categories, e => e.CategoryId, c => c.CategoryId, (e, c) => new { e, c })
                .Select(ec => new ExpenseViewModel()
                {
                    ExpenseId = ec.e.ExpenseId,
                    CategoryId = ec.e.CategoryId,
                    Category = ec.c.CategoryName,
                    Title = ec.e.ExpenseTitle,
                    Description = ec.e.ExpenseDescription,
                    Date = ec.e.ExpenseDate,
                    Amount = ec.e.ExpenseAmount
                }).ToList();
            return Ok(expenses);
        }

        //GET EXPENSE BY CATEGORY: api/Expenses/ExpenseByCategory
        [Route("api/Expenses/ExpenseByCategory/{id}")]
        public IHttpActionResult GetExpenseByCategory(int id)
        {
            var expenses = db.Expenses.Join(db.Categories, e => e.CategoryId, c => c.CategoryId, (e, c) => new { e, c })
                .Where(ec => ec.e.CategoryId == id)
                .Select(ec => new ExpenseViewModel()
                {
                    ExpenseId = ec.e.ExpenseId,
                    CategoryId = ec.e.CategoryId,
                    Category = ec.c.CategoryName,
                    Title = ec.e.ExpenseTitle,
                    Description = ec.e.ExpenseDescription,
                    Date = ec.e.ExpenseDate,
                    Amount = ec.e.ExpenseAmount
                }).ToList();
            return Ok(expenses);
        }

        // GET: api/Expenses/5
        [ResponseType(typeof(Expense))]
        public IHttpActionResult GetExpense(int id)
        {
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        // PUT: api/Expenses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExpense(int id, Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            db.Entry(expense).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Expenses
        [ResponseType(typeof(Expense))]
        public IHttpActionResult PostExpense(Expense expense)
        {
            if (expense.ExpenseDescription == null)
                expense.ExpenseDescription = "NONE";
            expense.ExpenseDate = DateTime.Now;
            db.Expenses.Add(expense);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expense.ExpenseId }, expense);
        }

        // DELETE: api/Expenses/5
        [ResponseType(typeof(Expense))]
        public IHttpActionResult DeleteExpense(int id)
        {
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            db.Expenses.Remove(expense);
            db.SaveChanges();

            return Ok(expense);
        }

        //CATEGORY AMOUNT: api/Expenses/TotalCategoryAmount
        [Route("api/Expenses/TotalCategoryAmount")]
        public decimal PostTotalCategoryAmount(Expense expense)
        {
            var totCatExp = db.Expenses.Where(e => e.CategoryId == expense.CategoryId).Sum(e => e.ExpenseAmount);
            return totCatExp;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExpenseExists(int id)
        {
            return db.Expenses.Count(e => e.ExpenseId == id) > 0;
        }
    }
}