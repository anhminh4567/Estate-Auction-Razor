using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using RazorAucionWebapp.MyAttributes;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Service.Services.AuctionService;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.AuctionMng
{
    public class CreateModel : PageModel
    {
        private readonly AuctionServices _auctionServices;
        private readonly EstateServices _estateServices;
        private readonly EstateCategoryDetailServices _estateCategoryDetailServices;
        public CreateModel(AuctionServices auctionServices, EstateServices estateServices, EstateCategoryDetailServices estateCategoryDetailServices) 
        {
            _auctionServices = auctionServices;
            _estateServices = estateServices;
            _estateCategoryDetailServices = estateCategoryDetailServices;
        }
        public IList<Estate> CompanyEstates { get; set; }
		//[BindProperty]
  //      [Required]
		//public DateTime RegistrationDate { get; set; }
		[BindProperty]
		[Required]
        [IsDateAppropriate]
		public DateTime StartDate { get; set; }
        [BindProperty]
		[Required]
        [IsDateAppropriate]
		public DateTime EndDate { get; set; }
		[BindProperty]
		[Required]
		public decimal WantedPrice { get; set; }
        [BindProperty]
		[Required]
		public decimal IncrementPrice { get; set; }
        [BindProperty]
		[Required]
		public int MaxParticipant { get; set; }
        [BindProperty]
		[NotNull]
		[Required]
		public string SelectedEstate { get; set; }
        private int _companyId { get; set; }
		public async Task<IActionResult> OnGetAsync()
        {
            await PopulateData();
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
		{
			await PopulateData();
			if (!ModelState.IsValid)
			{
				return Page();
			}
            return Page();
            //return RedirectToPage("./Index");
        }
        private async Task PopulateData()
        {
            var tryGetClaimId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var companyId);
            if (tryGetClaimId is false)
                throw new Exception("claim id is not found, means this user is not legit or not exist at all");
            _companyId = companyId;
            CompanyEstates = await _estateServices.GetByCompanyId(_companyId);
        }
    }
}
