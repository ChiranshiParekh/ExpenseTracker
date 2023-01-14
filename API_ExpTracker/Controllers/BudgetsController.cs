using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using API_ExpTracker.Models;

namespace API_ExpTracker.Controllers
{
    public class BudgetsController : ApiController
    {
        private ExpTracker_DbEntities db = new ExpTracker_DbEntities();

        // GET: api/Budgets
        public IQueryable<Budget> GetBudgets()
        {
            return db.Budgets;
        }

        //GET: api/Budgets/BudgetLimit
        [Route("api/Budgets/BudgetLimit")]
        public IHttpActionResult GetBudgetLimit()
        {
            var budget = db.Budgets.Select(u => u.ExpenseLimit).ToList();
            return Ok(budget);
        }

        //GET: api/Budgets/BudgetExpense
        [Route("api/Budgets/BudgetExpense")]
        public IHttpActionResult GetBudgetExpense()
        {
            var budget = db.Budgets.Select(u => u.TotalExpense).ToList();
            return Ok(budget);
        }

        //UPDATE TOTALEXPENSE: api/Budgets/UpdateBudgetTotalExpense
        [Route("api/Budgets/UpdateBudgetTotalExpense/{id}")]
        public IHttpActionResult GetUpdateBudgetTotalExpense(int id)
        {
            var budget = db.Budgets.FirstOrDefault(b => b.budgetId == id);
            budget.TotalExpense = db.Categories.Sum(c => c.CategoryExpense);
            db.SaveChanges();
            return Ok(budget.TotalExpense);
        }

        // GET: api/Budgets/5
        [ResponseType(typeof(Budget))]
        public IHttpActionResult GetBudget(int id)
        {
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return NotFound();
            }

            return Ok(budget);
        }

        //SEARCH BUDGET: api/Budgets/FindBudget
        [Route("api/Budgets/FindBudget")]
        public bool GetFindBudget()
        {
            Budget budget = db.Budgets.Find(1);
            if(budget==null)
                return false;
            else
                return true;
        }

        // PUT: api/Budgets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBudget(int id, Budget budget)
        {

            if (id != budget.budgetId)
            {
                return BadRequest();
            }

            db.Entry(budget).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
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

        // POST: api/Budgets
        [ResponseType(typeof(Budget))]
        public IHttpActionResult PostBudget(Budget budget)
        {
            budget.budgetId = 1;
            budget.TotalExpense = 0;
            db.Budgets.Add(budget);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BudgetExists(budget.budgetId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = budget.budgetId }, budget);
        }

        // DELETE: api/Budgets/5
        [ResponseType(typeof(Budget))]
        public IHttpActionResult DeleteBudget(int id)
        {
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return NotFound();
            }

            db.Budgets.Remove(budget);
            db.SaveChanges();

            return Ok(budget);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BudgetExists(int id)
        {
            return db.Budgets.Count(e => e.budgetId == id) > 0;
        }
    }
}