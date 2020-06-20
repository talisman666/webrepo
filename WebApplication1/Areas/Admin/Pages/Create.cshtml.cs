﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.DAL.Data;
using WebApplication1.DAL.Entities;

namespace WebApplication1
{
    public class CreateModel : PageModel
    {
        private readonly WebApplication1.DAL.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
      //  private IWebHostEnvironment env;

        public CreateModel(WebApplication1.DAL.Data.ApplicationDbContext context,     IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        public IActionResult OnGet()
        {
            // ViewData["DishGroupId"] = new SelectList(_context.DishGroups, "DishGroupId", "DishGroupId");
            ViewData["DishGroupId"] = new SelectList(_context.DishGroups, "DishGroupId", "GroupName");
            return Page();
        }

        [BindProperty]
        public IFormFile Image { get; set; }
        [BindProperty]
        public Dish Dish { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Dishes.Add(Dish);
            await _context.SaveChangesAsync();
            if (Image != null)
            {
                var fileName = $"{Dish.DishId}" +  Path.GetExtension(Image.FileName);
                Dish.Image = fileName;
                var path = Path.Combine(_environment.WebRootPath, "Images", fileName);
                using (var fStream = new FileStream(path, FileMode.Create))
                {
                    await Image.CopyToAsync(fStream);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}


