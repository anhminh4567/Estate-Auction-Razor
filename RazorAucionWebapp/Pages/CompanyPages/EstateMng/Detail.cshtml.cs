using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model;
using Repository.Database.Model.RealEstate;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
{
    public class DetailModel : PageModel
    {
        private readonly EstateServices _estateServices;
        private readonly AuctionServices _auctionServices;
        private readonly EstateCategoriesServices _estateCategoriesServices;
        private readonly EstateCategoryDetailServices _estateCategoryDetailServices;
        private readonly EstateImagesServices _estateImagesServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DetailModel(
            EstateServices estateServices, 
            AuctionServices auctionServices, 
            EstateCategoriesServices estateCategoriesServices, 
            EstateCategoryDetailServices estateCategoryDetailServices, 
            EstateCategoryDetailServices estateCategoryDetailServices1,
            EstateImagesServices estateImagesServices,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _estateServices = estateServices;
            _auctionServices = auctionServices;
            _estateCategoriesServices = estateCategoriesServices;
            _estateCategoryDetailServices = estateCategoryDetailServices;
            _estateImagesServices = estateImagesServices;
            _webHostEnvironment = webHostEnvironment;
        }
        public List<string> Images { get; set; } = new List<string>();
        public Estate Estate { get; set; } = default!; 
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var estate = await _estateServices.GetFullDetail(id.Value);
                Estate = estate;
                await GetImages();
                return Page();
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                Console.WriteLine(ex.Message);
                return Page();
            }
            
        }

        private async Task GetImages()
        {
            var appImages = await _estateImagesServices.GetByEstateId(Estate.EstateId);
            foreach(var appImage in appImages)
            {
                Images.Add("~/PublicImages/storage/" + appImage.Path);
            }
        }
    }
}
