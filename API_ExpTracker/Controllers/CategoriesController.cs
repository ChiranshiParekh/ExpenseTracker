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
    public class CategoriesController : ApiController
    {
        private ExpTracker_DbEntities db = new ExpTracker_DbEntities();

        // GET: api/Categories
        public IQueryable<Category> GetCategories()
        {
            return db.Categories;
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            category.CategoryExpense = 0;
            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        //CHECK DUPLICATE CATEGORY: api/Categories/isDuplicate
        [Route("api/Categories/IsDuplicate")]
        public bool PostIsDuplicate(Category category)
        {
            var cat = db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName);
            if (cat == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //UPDATE CATEGORYEXPENSE: api/Categories/UpdateCategoryExpense
        [Route("api/Categories/UpdateCategoryExpenseForDelete")]
        public IHttpActionResult PutUpdateCategoryExpenseForDelete(string CatName)
        {
            var categoryId = (from c in db.Categories
                              where c.CategoryName == CatName
                              select c.CategoryId).FirstOrDefault();
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
            {
                var expenses = db.Expenses.Where(e => e.CategoryId == categoryId).Sum(e => e.ExpenseAmount);
                category.CategoryExpense = expenses;
                db.SaveChanges();
            }
            return Ok(category.CategoryExpense);
        }

        //UPDATE CATEGORYEXPENSE: api/Categories/UpdateCategoryExpense
        [Route("api/Categories/UpdateCategoryExpense")]
        public IHttpActionResult PutUpdateCategoryExpense(Expense expense)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == expense.CategoryId);
            if(category != null)
            {
                var expenses = db.Expenses.Where(e => e.CategoryId == expense.CategoryId).Sum(e => e.ExpenseAmount);
                category.CategoryExpense = expenses;
                db.SaveChanges();
            }
            return Ok(category.CategoryExpense);
        }

        [Route("api/Categories/TotalCategoryLimit/{id}")]
        public IHttpActionResult GetTotalCategoryLimit(int id)
        {
            var cat = db.Categories.Where(c => c.CategoryId == id).Select(u => u.CategoryLimit).ToList();
            return Ok(cat);
        }

        [Route("api/Categories/TotalCategoryExpense/{id}")]
        public IHttpActionResult GetTotalCategoryExpense(int id)
        {
            var cat = db.Categories.Where(c => c.CategoryId == id).Select(u => u.CategoryExpense).ToList();
            return Ok(cat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}