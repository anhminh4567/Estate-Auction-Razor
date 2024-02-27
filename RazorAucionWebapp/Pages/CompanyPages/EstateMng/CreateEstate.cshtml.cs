using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model.RealEstate;
using Service.Services.AuctionService;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
{
    public class CreateModel : PageModel
    {
        private readonly EstateServices _estateServices;
        private readonly EstateCategoriesServices _estateCategoriesServices;
        private readonly EstateCategoryDetailServices _estateCategoryDetailServices;
        public CreateModel(
            EstateServices estateServices,
            EstateCategoryDetailServices estateCategoryDetailServices,
            EstateCategoriesServices estateCategoriesServices)
        {
            _estateServices = estateServices;
            _estateCategoryDetailServices = estateCategoryDetailServices;
            _estateCategoriesServices = estateCategoriesServices;
        }

        public async Task<IActionResult> OnGet()
        {
            //ViewData["CompanyId"] = new SelectList(_context.Companys, "AccountId", "CMND");
            await PopulateData();
            return Page();
        }

        [BindProperty]
        [Required]
        public string Name { get; set; }
        [BindProperty]
        [Required]
        public string Description { get; set; }
        [BindProperty]
        [Required]
        public float Width { get; set; }
        [BindProperty]
        [Required]
        public float Length { get; set; }
        // added property
        [BindProperty]
        [Required(ErrorMessage = "At least one estate category must be selected!")]
        public string CheckedCategoriesBoxes { get; set; }
        //[BindProperty]
        //[Required]
        public List<string> SeletedEstateCategoriesOptions { get; set; }
        public List<SelectListItem> EstateCategoriesOptions { get; set; }
        [BindProperty]
        [Required]
        public string ImageUrl { get; set; }
        private int CompanyId { get; set; }
        private Estate Estate { get; set; } = new Estate();

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            await PopulateData();
            SeletedEstateCategoriesOptions = new List<string>(CheckedCategoriesBoxes.Split(','));
            if (!ModelState.IsValid || Estate == null)
            {
                return Page();
            }
            Estate.Width = Width;
            Estate.Length = Length;
            Estate.Description = Description;
            Estate.Name = Name;
            Estate.CompanyId = CompanyId;
            var estateResult = await _estateServices.Create(Estate);
            if (estateResult == null)
            {
                ModelState.AddModelError(string.Empty, "something wrong when create");
                return Page();
            }
            foreach (var item in SeletedEstateCategoriesOptions)
            {
                var newCategories = new EstateCategories()
                {
                    CategoryId = int.Parse(item),
                    EstateId = estateResult.EstateId,
                };
                var estateCategoriesResult = await _estateCategoriesServices.CreateEstateCategories(newCategories);
                if (estateCategoriesResult is null)
                {
                    ModelState.AddModelError(string.Empty, "something wrong when adding categories");
                    return Page();
                }
            }
            return RedirectToPage("./Index");
        }
        private async Task PopulateData()
        {
            var getCategoryList = await _estateCategoryDetailServices.GetAll();
            if (getCategoryList.Count > 0)
            {
                EstateCategoriesOptions = new List<SelectListItem>();
                foreach (var category in getCategoryList)
                {
                    EstateCategoriesOptions.Add(new SelectListItem(category.CategoryName, category.CategoryId.ToString()));
                }
            }
            var result = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var companyId);
            if (result is false)
                throw new Exception("this user is not company id, create an account first to create this, very simple, go to admin page and do so");
            CompanyId = companyId;
            
        }
    }
}
