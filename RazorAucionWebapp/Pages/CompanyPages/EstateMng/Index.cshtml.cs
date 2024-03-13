using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using Repository.Database;
using Repository.Database.Model;
using Repository.Database.Model.RealEstate;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CompanyPages.EstateMng
{
    public class IndexModel : PageModel
    {
        private readonly EstateServices _estateServices;
        private readonly EstateImagesServices _estateImagesServices;


        public IndexModel(EstateServices context, EstateImagesServices estateImagesServices)
        {
            _estateServices = context;
            _estateImagesServices = estateImagesServices;
        }

        public IList<Estate> Estates { get;set; } = default!;

        private int _companyId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await PopulateData();
                Estates = (await _estateServices.GetAllDetails()).Where(e => e.CompanyId.Equals(_companyId)).ToList();
                foreach(var estate in Estates)
                {
                    var appImages = await _estateImagesServices.GetByEstateId(estate.EstateId); // this will return list of image
                    foreach (var appImage in appImages)
                    {
                        var image = estate.Images.FirstOrDefault(i => i.Image.Path ==  appImage.Path);
                        if (image != null)
                        {
                            image.Image.Path = "~/PublicImages/storage/" + appImage.Path;
                        }
                    }
                }
                return Page();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Console.WriteLine(ex.Message);
                return Page();
            }
        }
        private async Task PopulateData()
        {
            var tryGetCompanyId = int.TryParse(HttpContext.User.Claims?.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var companyId);
            if (tryGetCompanyId is false)
                throw new Exception("cannot find id of company");
            _companyId = companyId;
        }
    }
}
