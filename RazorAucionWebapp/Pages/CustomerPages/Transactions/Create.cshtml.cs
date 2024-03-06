using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit.Cryptography;
using Repository.Database;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Service.Services;
using Service.Services.AppAccount;

namespace RazorAucionWebapp.Pages.CustomerPages.Transactions
{
    public class CreateModel : PageModel
    {
        private readonly TransactionServices _transactionServices;
        private readonly VnpayAvailableServices _vnpayAvailableServices;
        private readonly AccountServices _accountService;
        public CreateModel(TransactionServices transactionServices, VnpayAvailableServices vnpayAvailableServices, AccountServices accountService)
        {
            _transactionServices = transactionServices;
            _vnpayAvailableServices = vnpayAvailableServices;
            _accountService = accountService;
        }
        [BindProperty]
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
        //public int TransactionId { get; set; }
        //public int? AccountId { get; set; }
        //public TransactionStatus Status { get; set; }
        //public string vnp_Amount { get; set; }
        //public long vnp_TransactionDate { get; set; }
        //public string vnp_OrderInfo { get; set; }
        //public string vnp_PayDate { get; set; }

        //private Transaction Transaction { get; set; } = default!;
        private int _userId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            PopulateData();
            return Page();
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            PopulateData();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var checkResult = IsAmountValid(Amount);
                if (checkResult.isValid == false)
                {
                    ModelState.AddModelError(string.Empty, checkResult.Message);
                    return Page();
                }
                var getUser = await _accountService.GetById(_userId);
                var urlRedirect = GenerateUrlRedirect(HttpContext, getUser, Amount);
                if (urlRedirect is null)
                {
                    ModelState.AddModelError(string.Empty, "something wrong with transactin, return later");
                    return Page();
                }
                var newTransaction = new Transaction()
                {
                    AccountId = _userId,
                    Status = TransactionStatus.PENDING,
                    vnp_Amount = Amount.ToString(),
                    vnp_OrderInfo = "TEST ORDER, MUST NOT USE TO REFUND, CANNOT REFUND",
                    vnp_PayDate = DateTime.Now.ToString(),
                    vnp_TransactionDate = DateTime.Now.Ticks,
                    vnp_TxnRef = string.Empty,
                };
                var saveTransaction = await _transactionServices.Create(newTransaction);
                if (saveTransaction is null)
                {
                    ModelState.AddModelError(string.Empty, "something wrong with transaction, return later");
                    return Page();
                }
                urlRedirect = GenerateUrlRedirect(HttpContext, getUser, Amount, saveTransaction.TransactionId);
                return Redirect(url: urlRedirect);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            return RedirectToPage("./Index");
        }
        private string? GenerateUrlRedirect(HttpContext httpContext, Account account, int amount, int transactionId = 0)
        {
            if (true)
            {
                return $"https://localhost:7156/Vnpay/Return?handler=TestTransactionHandler&amount={amount}&transactionId={transactionId}";
            }
            else
                return _vnpayAvailableServices.GeneratePayUrl(httpContext, account, amount);
        }
        private (bool isValid, string Message) IsAmountValid(int amount)
        {
            //do some logic here, not yet
            if (true)
                return (true, $"{Amount} is valid.");
            else
                return (false, "amount is not valid, try enter different amount");
        }
        private void PopulateData()
        {
            var tryGetUserId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var userId);
            if (tryGetUserId is false)
                throw new UnauthorizedAccessException("User Unauthorized");
            _userId = userId;
        }
    }
}
