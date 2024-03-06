using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using RazorAucionWebapp.Pages.AdminPages.EstateCategories;
using Repository.Database;
using Repository.Database.Model;
using Repository.Database.Model.RealEstate;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
{
    public class UpdateModel : PageModel
    {
        private readonly EstateServices _estateService;
        private readonly EstateCategoryDetailServices _estateCategoriesDetailServies;
        private readonly EstateCategoriesServices _estateCategoriesServices;

        public UpdateModel(EstateServices estateService,
            EstateCategoryDetailServices estateCategoryDetailServices,
            EstateCategoriesServices estateCategoriesServices)
        {
            _estateService = estateService;
            _estateCategoriesDetailServies = estateCategoryDetailServices;
            _estateCategoriesServices = estateCategoriesServices;
        }
        [BindProperty]
        [Required]
        public int EstateId { get; set; }
        [BindProperty]
        [Required]
        public int CompanyId { get; set; }
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
        [BindProperty]
        [Required]
        public List<int> SelectedCategories { get; set; }
        public List<EstateCategories> CurrentEstateCategories { get; set; }
        public List<EstateCategoryDetail> Categories { get; set; }
        public List<AppImage> EstateImages { get; set; }
        public Estate Estate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            try
            {
                await PopulateData(id.Value);
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await PopulateData(EstateId);
            if (!ModelState.IsValid || Estate is null)
            {
                return Page();
            }
            try
            {
                Estate.Name = Name;
                Estate.Description = Description;
                Estate.Width = Width;
                Estate.Length = Length;
                var result = await _estateService.Update(Estate, SelectedCategories);
                if (result.IsSuccess)
                    return RedirectToPage("./Index");
                else
                {
                    ModelState.AddModelError(string.Empty, result.message);
                    return Page();
                }
                //List<EstateCategories> selectedCategories = new List<EstateCategories>();
                //foreach(var sel in SelectedCategories) 
                //{
                //    selectedCategories.Add(new EstateCategories() { CategoryId = sel,EstateId = EstateId});
                //}
                //var result = await _estateCategoriesServices.DeleteRangeAsync(CurrentEstateCategories);
                //var addResult = await _estateCategoriesServices.CreateRangeAsync(selectedCategories);
                //var updateResult = await _estateService.Update(Estate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task PopulateData(int id)
        {
            var estate = await _estateService.GetIncludes(id, new string[] { "EstateCategory.CategoryDetail", "Images.Image" });
            if (estate is null)
                throw new NullReferenceException("estate of such id does not exist, something wrong ");
            Estate = estate;
            Categories = await _estateCategoriesDetailServies.GetAll();
            CurrentEstateCategories = new List<EstateCategories>();
            EstateImages = new List<AppImage>();
            if (estate.EstateCategory is not null)
            {
                CurrentEstateCategories = (estate.EstateCategory).ToList();
            }
            foreach (var category in estate.Images)
            {
                EstateImages.Add(category.Image);
            }
            EstateId = Estate.EstateId;
            CompanyId = Estate.CompanyId;
            Name = Estate.Name;
            Description = Estate.Description;
            Width = Estate.Width;
            Length = Estate.Length;
        }
    }
}
