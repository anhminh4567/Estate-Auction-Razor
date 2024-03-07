using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorAucionWebapp.BackgroundServices;
using Repository.Database.Model;
using Service.Services.AppAccount;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class UpdateModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AccountImagesServices _accountImagesServices;
        private readonly AccountServices _accountServices;
        [BindProperty]
        public int Id {  get; set; }
        [BindProperty]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [BindProperty]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [BindProperty]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number is not in correct format")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Not valid phone number")]
        public string Tel { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Pass { get; set; }
        [BindProperty]
        public AppImage Image { get; set; }
        [BindProperty]
        public string Avatar { get; set; }
        public IFormFile ImageFile { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "NewPass is required")]
        public string NewPass { get; set; }
        public UpdateModel(IWebHostEnvironment webHostEnvironment, AccountImagesServices accountImagesServices, AccountServices accountServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _accountServices = accountServices;
            _accountImagesServices = accountImagesServices;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var flag = await PopulateData();
            if (flag is false)
            {
                return RedirectToPage("/Registration/Login");
            }
            await GetAvatar();
            return Page();
        }
        public async Task<IActionResult> OnPostImageUpdateAsync()
        {
            if (ImageFile is not null)
            {
                var (directory, path) = GetAvatarDirectory();
                var result = await _accountImagesServices.Create(Id, path, "avatar.png");
                if (result.IsSuccess)
                {
                    ImageHandler.SaveImage(ImageFile, Path.Combine(directory, path), result.image.Name);
                    UpdateClaim("~/PublicImages/storage/" + result.image.Path);
                    TempData["Success"] = "Avatar updated";
                    return RedirectToPage("./Detail");
                }
                else TempData["Failed"] = "Unknown error has occured";
            }
            else TempData["Failed"] = "No image uploaded";
            await GetAvatar();
            return Page();
        }
        public async Task<IActionResult> OnPostUpdateAsync()
        {

            var flag = ModelState["Name"].Errors.Any() || ModelState["Email"].Errors.Any() || ModelState["Tel"].Errors.Any() || ModelState["Pass"].Errors.Any();
            if (!flag)
            {
                var account = await _accountServices.GetById(Id);
                if (account.Password.Equals(Pass))
                {
                    account.FullName = Name;
                    account.Email = Email;
                    account.Dob = Dob;
                    account.Telephone = Tel;
                    var result = await _accountServices.Update(account);
                    if (result)
                    {
                        TempData["Success"] = "Profile updated";
                        return RedirectToPage("./Detail");
                    }
                    else TempData["Failed"] = "Unknown error has occured";
                }
                else
                {
                    TempData["Pass"] = "Wrong password";
                }
            }
            else{
                TempData["Name"] = ModelState["Name"].Errors.FirstOrDefault()?.ErrorMessage;
                TempData["Email"] = ModelState["Email"].Errors.FirstOrDefault()?.ErrorMessage;
                TempData["Tel"] = ModelState["Tel"].Errors.FirstOrDefault()?.ErrorMessage;
                TempData["Dob"] = ModelState["Dob"].Errors.FirstOrDefault()?.ErrorMessage;
                TempData["Pass"] = ModelState["Pass"].Errors.FirstOrDefault()?.ErrorMessage;
            }
            await GetAvatar();
            return Page();
        }
        public async Task<IActionResult> OnPostChangeAsync()
        {
            var flag = ModelState["Pass"].Errors.Any() || ModelState["NewPass"].Errors.Any();
            if (!flag)
            {
                var account = await _accountServices.GetById(Id);
                if (account.Password.Equals(Pass))
                {
                    if (Pass != NewPass)
                    {
                        account.Password = NewPass;
                        var result = await _accountServices.Update(account);
                        if (result)
                        {
							TempData["Success"] = "Password changed";
                            return RedirectToPage("./Detail");
						}
                        else TempData["Failed"] = "Unknown error has occured";
                    }
                    else
                    {
                        TempData["NewPass"] = "New password can't be your old password.";
                    }
                }
                else
                {
                    TempData["Pass"] = "Wrong password";
                }
            }
            else
            {
                TempData["OldPass"] = ModelState["Pass"].Errors.FirstOrDefault().ErrorMessage;
                TempData["NewPass"] = ModelState["NewPass"].Errors.FirstOrDefault().ErrorMessage;
			}
            await GetAvatar();
            return RedirectToPage();
        }
        private async Task<bool> PopulateData()
        {
            var id = HttpContext.User.FindFirst("Id")?.Value;
            var result = int.TryParse(id, out int accountId);
            if (result)
            {
                var account = await _accountServices.GetById(accountId);
                Id = account.AccountId;
                Name = account.FullName;
                Email = account.Email;
                Tel = account.Telephone;
                Dob = account.Dob;
                return true;
            }
            return false;
        }
        private async Task<bool> GetAvatar()
        {
            Avatar = HttpContext.User.FindFirst("Avatar").Value;
            if (Avatar != "")
            {
                return true;
            }
            return false;
        }
        private (string,string) GetAvatarDirectory()
        {
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (email is not null)
            {
                email = ImageHandler.RemoveWhiteSpace(email);
                var directory = _webHostEnvironment.ContentRootPath;
                return (Path.Combine(directory.Replace("//","\\"), "wwwroot","PublicImages","storage"),Path.Combine("user", email));
            }
            else return (null, null);
        }

        private void UpdateClaim(string path)
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            Identity.RemoveClaim(Identity.FindFirst("Avatar"));
            Identity.AddClaim(new Claim("Avatar", path));
        }
    }
}
