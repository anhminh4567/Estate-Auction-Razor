using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Service.Services;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.CustomerPages.Profile
{
    public class DetailModel : PageModel
    {
        private readonly AccountServices _accountServices;

		public DetailModel(AccountServices accountServices)
		{
			_accountServices = accountServices;
		}

		public Account Account { get; set; } = default!;
        private int _userId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await PopulateData();
            }catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();
            }
            return Page();
        }
        private async Task PopulateData() 
        {
            var tryGetId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var userId);
            if (tryGetId is false)
                throw new Exception("unauthorized user");
            _userId = userId;
            var tryGetAccountDetail = await _accountServices.GetInclude(_userId,"");
            if (tryGetAccountDetail is null)
                throw new Exception("id for this account is invalid");
            Account = tryGetAccountDetail;
        }
    }
}
