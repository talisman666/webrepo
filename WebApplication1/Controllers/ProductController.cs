using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplication1.DAL.Entities;
using WebApplication1.Models;
using WebApplication1.Extentions;
using WebApplication1.DAL.Data;
using Microsoft.AspNetCore.Http;
using Moq;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {

        //public List <Dish> _dishes;
        //List<DishGroup> _dishGroups;
        ApplicationDbContext _context;
        int _pageSize;

        public ProductController(ApplicationDbContext context)
        {
            _pageSize = 3;
            _context = context;
        }

        [Route("Catalog")]
        [Route("Catalog/Page_{pageNo}")]
             public IActionResult Index(int? group, int pageNo = 1)
        {
            ///////////////////////////////////////////////////////////////
            // Контекст контроллера
            //var controllerContext = new ControllerContext();
            //// Макет HttpContext
            //var moqHttpContext = new Mock<HttpContext>();
            //moqHttpContext.Setup(c => c.Request.Headers)
            //.Returns(new HeaderDictionary());
            //controllerContext.HttpContext = moqHttpContext.Object;
            //var controller = new ProductController(_context)
            //{ ControllerContext = controllerContext };

            // Поместить список групп во ViewData 
            ViewData["Groups"] = _context.DishGroups /*_dishGroups*/;
            // Получить id текущей группы и поместить в TempData
            ViewData["CurrentGroup"] = group ?? 0;
            //////////////////////////////////////////////////////////////////

            // Поместить список групп во ViewData
            //ViewData["Groups"] = _dishGroups;
            //// Получить id текущей группы и поместить в TempData
            //ViewData["CurrentGroup"] = group ?? 0;
            //var dishesFiltered = _dishes
            //                    .Where(d => !group.HasValue || d.DishGroupId == group.Value);
            var dishesFiltered = _context.Dishes
                 .Where(d => !group.HasValue || d.DishGroupId == group.Value);


            ///////////////////////////////////////////////////////////////////////
            ///
            var groupName = group.HasValue
                ? _context.DishGroups.Find(group.Value)?.GroupName
                : "all groups";

            ////////////////////////////////////////////////////////
            // Поместить список групп во ViewData
  //          ViewData["Groups"] = _context.DishGroups;

            //return View(ListViewModel<Dish>.GetModel(dishesFiltered, pageNo, _pageSize));
            var model = ListViewModel<Dish>.GetModel(dishesFiltered, pageNo, _pageSize);
            ////if (Request.Headers["x-requested-with"]
            //.ToString().ToLower().Equals("xmlhttprequest"))
            //    return PartialView("_listpartial", model);
            //else
            //    return View(model);
            if (Request.IsAjaxRequest())
                return PartialView("_listpartial", model);
            else
                return View(model);
        }
        /// Инициализация списков
        /// </summary>
 //       private void SetupData()
 //       {
 //           _dishGroups = new List<DishGroup>
 //{
 //                new DishGroup {DishGroupId=1, GroupName="Стартеры"},
 //                new DishGroup {DishGroupId=2, GroupName="Салаты"},
 //                new DishGroup {DishGroupId=3, GroupName="Супы"},
 //                new DishGroup {DishGroupId=4, GroupName="Основные блюда"},
 //                new DishGroup {DishGroupId=5, GroupName="Напитки"},
 //                new DishGroup {DishGroupId=6, GroupName="Десерты"}
 //           };
 //           _dishes = new List<Dish>
 //                {
 //               new Dish {DishId = 1, DishName="Картофель отварной",
 //               Description="Национальное белорусское блюдо",
 //               Calories =200, DishGroupId=4, Image="potato.png" },
 //               new Dish { DishId = 2, DishName="Чизкейк",
 //               Description="Нежный и вкусный",
 //               Calories =330, DishGroupId=6, Image="cheesecake.png" },
 //               new Dish { DishId = 3, DishName="Шоколадный торт",
 //               Description="Просто объедение",
 //               Calories =635, DishGroupId=6, Image="cake.png" },
 //               new Dish { DishId = 4, DishName="Запеченый лосось",
 //               Description="Изысканное блюдо",
 //               Calories =524, DishGroupId=4, Image="fish.png" },
 //               new Dish { DishId = 5, DishName="Лазанья",
 //               Description="Для тех, кто скучает по Италии",
 //               Calories =280, DishGroupId=4, Image="lasagna.png" }
 //                };
 //       }
    }
}