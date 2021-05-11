using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstanzuelaEats.Api.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Common.Modelos;
    using Domain.Modelos;

    [Authorize]
    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        public IQueryable<Category> GetCategories()
        {
            return db.Categories.OrderBy(c => c.Description);
        }
    }
}