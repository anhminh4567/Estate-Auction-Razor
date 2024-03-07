using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.RealEstate;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.AdminPages.EstatesManage
{
    public class DeleteModel : PageModel
    {
        private readonly EstateServices _estateServices;

        public DeleteModel(EstateServices estateServices)
        {
            _estateServices = estateServices ?? throw new ArgumentNullException(nameof(estateServices));
        }

        [BindProperty]
      public Estate Estate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            try
            {
                await PopulateData(id.Value);
                return Page();
            }catch (Exception ex) 
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                await PopulateData(id.Value);
                var result = await _estateServices.AdminBannedEstate(Estate);
                if(result.IsSuccess) 
                {
                    return RedirectToPage("./Index");

                }
                else
                {
                    ModelState.AddModelError(string.Empty,result.message); 
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ex.Message);
            }

        }
        private async Task PopulateData(int estateId)
        {
            var getEstate = await _estateServices.GetById(estateId, "Auctions,Company");
            Estate = getEstate ?? throw new Exception("cannot find estate with such id");
        }
    }
}
