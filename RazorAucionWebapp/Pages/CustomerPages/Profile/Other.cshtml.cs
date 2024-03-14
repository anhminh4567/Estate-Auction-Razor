using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.RealEstate;
using Service.Services;
using Service.Services.AppAccount;
using Service.Services.RealEstate;

namespace RazorAucionWebapp.Pages.CustomerPages.Profile
{
    public class OtherModel : PageModel
    {
        private readonly AccountServices _accountServices;
        private readonly AccountImagesServices _accountImagesServices;
        public OtherModel(AccountServices accountServices, AccountImagesServices accountImagesServices)
        {
            _accountServices = accountServices;
            _accountImagesServices = accountImagesServices;
        }

        public Account Account { get; set; } = default!;
        public string Avatar { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
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
                ModelState.AddModelError(string.Empty, ex.Message);
                Console.WriteLine(ex.Message);
                return Page();
            }
        }
        private async Task PopulateData(int id)
        {
            var tryGetAccountDetail = await _accountServices.GetInclude(id, "");
            if (tryGetAccountDetail is null)
                throw new Exception("id for this account is invalid");
            Account = tryGetAccountDetail;
            var appImage = await _accountImagesServices.GetAccountAvatar(id);
            if (appImage is not null) Avatar = "~/PublicImages/storage/" + appImage.Path;
            else Avatar = "~/PublicImages/general/user_icon.png";
        }
    }
}
