using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Database;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.AdminPages.Companies
{
    public class CreateModel : PageModel
    {
        private readonly CompanyServices _companyServices;
        private readonly AccountServices _accountServices;
        public CreateModel(CompanyServices companyServices, AccountServices accountServices)
        {
            _companyServices = companyServices;
            _accountServices = accountServices;
        }
        [BindProperty]
        [Required(ErrorMessage = "Owner is required.")]
        public string OwnerId { get; set; }
        [BindProperty]
        public Company Company { get; set; } = new Company(initBase: true);
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (String.IsNullOrEmpty(OwnerId))
            {
                TempData["Owner"] = "No owner selected";
            }
            else
            {
                if (await _accountServices.IsEmailExisted(Company.Email))
                {
                    ModelState.AddModelError(string.Empty, "email exist");
                }
                else
                {
                    var Account = await _accountServices.GetById(int.Parse(OwnerId));
                    //_accountServices.Create();
                    var companyResult = await _companyServices.Create(new Company(Account)
                    {
                        CompanyName = Company.CompanyName,
                        Description = Company.Description,
                        Address = Company.Address,
                        EstablishDate = Company.EstablishDate
                    });
                    if(companyResult is not null) return RedirectToPage("./Index");
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnGetCustomersAsync()
        {
            var list = await _accountServices.GetActiveCustomers();
            return new JsonResult(list);
        }
    }
}

