using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DAL.Entities;
using WebApplication1.DAL.Data;
using WebApplication1.Models;
using Moq;
using WebApplication1.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {

        //public List <Dish> _dishes;
        //List<DishGroup> _dishGroups;
        ApplicationDbContext _context;
        int _pageSize;
        //ILogger _logger;

        public ProductController(ApplicationDbContext context/*, ILogger<ProductController> logger*/)
        {
            _pageSize = 3;
            _context = context;
           // _logger = logger;
        }

        [Route("Catalog")]
        [Route("Catalog/Page_{pageNo}")]
             public IActionResult Index(int? group, int pageNo=1)
        {
           

            // Поместить список групп во ViewData 
            ViewData["Groups"] = _context.DishGroups /*_dishGroups*/;
            // Получить id текущей группы и поместить в TempData
            ViewData["CurrentGroup"] = group ?? 0;
            
            var dishesFiltered = _context.Dishes
                 .Where(d => !group.HasValue || d.DishGroupId == group.Value);


            ///////////////////////////////////////////////////////////////////////
            ///
            var groupName = group.HasValue
                ? _context.DishGroups.Find(group.Value)?.GroupName
                : "all groups";

         
            var model = ListViewModel<Dish>.GetModel(dishesFiltered, pageNo, _pageSize);
           
            if (Request.IsAjaxRequest())
                return PartialView("_listpartial", model);
            else
                return View(model);
        }
       
    }
}