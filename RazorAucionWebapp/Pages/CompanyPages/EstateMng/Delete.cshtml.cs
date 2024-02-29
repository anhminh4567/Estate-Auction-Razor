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
            try {
                var getEstate = await _estateServices.GetById(id.Value);
                var result = await _estateServices.Delete(getEstate);
                if(result is false)
                    throw new Exception("something wrong when delete, it result in false");
            }catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }            
            return RedirectToPage("./Index");
        }
    }
}
