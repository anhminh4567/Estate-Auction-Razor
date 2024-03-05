using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorAucionWebapp.MyAttributes;
using Repository.Database;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Service.Services.Auction;
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
		//public IList<Estate> CompanyEstates { get; set; }
		//[BindProperty]
		//[Required]
		//public DateTime RegistrationDate { get; set; }
		[BindProperty]
		[Required]
		[IsDateAppropriate()]
		public DateTime StartDate { get; set; }
		[BindProperty]
		[Required]
		[IsDateAppropriate()]
		public DateTime EndDate { get; set; }
        [BindProperty]
        [Required]
        [IsDateAppropriate()]
        public DateTime EndPayDate { get; set; }
        [BindProperty]
		[Required]
		public decimal WantedPrice { get; set; }
		[BindProperty]
		[Required]
		public decimal IncrementPrice { get; set; }
        [BindProperty]
        [Required]
        public decimal EntranceFee { get; set; }
        [BindProperty]
		[Required]
		public int MaxParticipant { get; set; }
		//[BindProperty]
		//[NotNull]
		//[Required]
		//public int SelectedEstate { get; set; }
		public Estate CurrentEstate { get; set; }
		[BindProperty]
		public int CurrentEstateId { get; set; }
		private int _companyId { get; set; }
		public async Task<IActionResult> OnGetAsync(int? estateId)
		{
			if(estateId is null)
			{
				return NotFound();
			}
			await PopulateData(estateId.Value);
			return Page();
		}

		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				await PopulateData(CurrentEstateId);
				if (!ModelState.IsValid)
				{
					return Page();
				}
				int comparison1 = DateTime.Compare(EndDate, StartDate);
				if (comparison1 <= 0)
				{
					ModelState.AddModelError(string.Empty, "EndDate is <= StartDate");
					return Page();
				}
				int comparison2 = DateTime.Compare(EndPayDate, EndDate);
				if (comparison2 <= 0)
				{
					ModelState.AddModelError(string.Empty, "EndPayDate is <= EndDate");
					return Page();
				}
				var newAuction = new Auction()
				{
					RegistrationDate = DateTime.Now,
					StartDate = StartDate,
					EndDate = EndDate,
					EndPayDate = EndPayDate,
					EstateId = CurrentEstate.EstateId,
					IncrementPrice = IncrementPrice,
					EntranceFee = EntranceFee,
					MaxParticipant = MaxParticipant,
					WantedPrice = WantedPrice,
					Status = AuctionStatus.NOT_STARTED,
				};
				var createResult = await _auctionServices.Create(newAuction);
				if (createResult is null)
				{
					ModelState.AddModelError(string.Empty, "someething wrong with createe ");
					return Page();
				}
				TempData["SuccessCreateMessage"] = "Successfully add a new auction";
				return RedirectToPage("./Index");
			}catch (Exception ex) 
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				Console.WriteLine(ex.Message);
				return Page();
			}
		}
		private async Task PopulateData(int estateId)
		{
			var tryGetClaimId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var companyId);
			if (tryGetClaimId is false)
				throw new Exception("claim id is not found, means this user is not legit or not exist at all");
			_companyId = companyId;
			CurrentEstateId = estateId;
			//CompanyEstates = await _estateServices.GetByCompanyId(_companyId);
			CurrentEstate = await _estateServices.GetById(CurrentEstateId);
		}
	}
}
