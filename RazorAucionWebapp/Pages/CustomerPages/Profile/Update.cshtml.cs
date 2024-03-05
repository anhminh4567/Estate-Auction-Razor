using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Service.Services.AppAccount;
using System.Drawing;
using System.Security.Claims;

namespace RazorAucionWebapp.Pages.CustomerPages
{
    public class UpdateModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AccountImagesServices _accountImagesServices;
        private readonly AccountServices _accountServices;
        [BindProperty]
        public Account Account { get; set; }
        [BindProperty]
        public AppImage Image { get; set; }
        [BindProperty]
        public string Avatar { get; set; }
        public IFormFile ImageFile { get; set; }
        [BindProperty]
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
            GetAvatar();
            return Page();
        }
        public async Task<IActionResult> OnPostImageUpdateAsync()
        {
            byte[] bytes = null;
            if (ImageFile != null)
            {
                using (Stream fs = ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        await PopulateData();
                        var stream = br.ReadBytes((Int32)fs.Length);
                        var (directory, path, filename) = GetAvatarDirectory();
                        var result = await _accountImagesServices.Create(Account.AccountId, directory, path, filename);
                        if(result is not null)
                        {
                            SaveImage(stream, Path.Combine(directory, result.Path));
                            var identity = HttpContext.User.Identity as ClaimsIdentity;
                            identity.RemoveClaim(identity.FindFirst("Avatar"));
                            identity.AddClaim(new Claim("Avatar", "~/PublicImages/storage/"+result.Path));
                            TempData["Success"] = "Avatar updated";
                        }
                        else TempData["Failed"] = "Unknown error has occured";
                    }
                }
            }
            GetAvatar();
            return Page();
        }
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            var flag = ModelState.IsValid;
            if (flag)
            {
                var account = await _accountServices.GetById(Account.AccountId);
                if (account.Password.Equals(Account.Password))
                {
                    account.FullName = Account.FullName;
                    account.Email = Account.Email;
                    account.Dob = Account.Dob;
                    account.Telephone = Account.Telephone;
                    var result = await _accountServices.Update(account);
                    if (result) TempData["Success"] = "Profile updated";
                    else TempData["Failed"] = "Unknown error has occured";
                }
                else
                {
                    ModelState.AddModelError("IncorrectPassword", "Your password doesn't match");
                }
            }
            else
            {
                ModelState.AddModelError("IncorrectForm", "Please fill in your form");
            }
            GetAvatar();
            return Page();
        }
        public async Task<IActionResult> OnPostChangeAsync()
        {
            var flag = ModelState.IsValid;
            if (flag)
            {
                var account = await _accountServices.GetById(Account.AccountId);
                if (account.Password.Equals(Account.Password))
                {
                    if (Account.Password != NewPass)
                    {
                        account.Password = NewPass;
                        var result = await _accountServices.Update(account);
                        if (result) TempData["Success"] = "Password changed";
                        else TempData["Failed"] = "Unknown error has occured";
                    }
                    else
                    {
                        ModelState.AddModelError("DuplicatePassword", "New password can't be your old password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("IncorrectPassword", "Your password doesn't match");
                }
            }
            else
            {
                ModelState.AddModelError("IncorrectForm", "Please fill in your form");
            }
            GetAvatar();
            return RedirectToPage();
        }
        private void SaveImage(Byte[] stream, string path)
        {
            using (var ms = new MemoryStream(stream))
            {
                using (var img = System.Drawing.Image.FromStream(ms))
                {
                    int maxWidth = 800;
                    int maxHeight = 800;

                    int newWidth, newHeight;

                    if (img.Width > maxWidth || img.Height > maxHeight)
                    {
                        float aspectRatio = (float)img.Width / (float)img.Height;

                        if (img.Width > maxWidth)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)(newWidth / aspectRatio);
                        }
                        else
                        {
                            newHeight = maxHeight;
                            newWidth = (int)(newHeight / aspectRatio);
                        }
                    }
                    else
                    {
                        newWidth = img.Width;
                        newHeight = img.Height;
                    }
                    using (Bitmap resizedMap = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics graphics = Graphics.FromImage(resizedMap))
                        {
                            graphics.DrawImage(img, 0, 0, newWidth, newHeight);
                        }
                    }
                    img.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }
        private async Task<bool> PopulateData()
        {
            var id = HttpContext.User.FindFirst("Id")?.Value;
            var result = int.TryParse(id, out int accountId);
            if (result)
            {
                Account = await _accountServices.GetById(accountId);
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
        private (string,string,string) GetAvatarDirectory()
        {
            var directory = _webHostEnvironment.ContentRootPath;
            var result = (Path.Combine(directory.Replace("//","\\"), "wwwroot","PublicImages","storage"),Path.Combine("user", Account.Email), "avatar.png");
            return result;
        }
    }
}
