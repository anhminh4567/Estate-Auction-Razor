using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.RealEstate;
using Service.Services.AppAccount;
using Service.Services.Auction;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
{
    public class DeleteModel : PageModel
    {
        private readonly EstateServices _estateServices;
        private readonly AuctionServices _auctionServices;
        private readonly CompanyServices _companyServices;

        public DeleteModel(EstateServices estateServices, AuctionServices auctionServices, CompanyServices companyServices)
        {
            _estateServices = estateServices;
            _auctionServices = auctionServices;
            _companyServices = companyServices;
        }

        [BindProperty]
        public Estate Estate { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estate = await _estateServices.GetById(id.Value);
            if (estate is null)
            {
                return NotFound();
            }
            else 
            {
                Estate = estate;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var estate = await _estateServices.GetById(id.Value);
            if (estate is not null)
            {
                Estate = estate;
                var result = await _estateServices.Delete(Estate);
                if(result is false)
                {
                    ModelState.AddModelError(string.Empty, "something wrong when delete");
                    return Page();
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
