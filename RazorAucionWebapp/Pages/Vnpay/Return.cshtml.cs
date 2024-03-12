using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Database.Model;
using Service.Services;
using Service.Services.AppAccount;
using Service.Services.VnpayService.Model;
using Service.Services.VnpayService.VnpayUtility;

namespace RazorAucionWebapp.Pages.Vnpay
{
	public class ReturnModel : PageModel
	{
		private readonly VnpayAvailableServices _vnpayAvailableServices;
		private readonly TransactionServices _transactionServices;
		private readonly AccountServices _accountServices;
		public ReturnModel(VnpayAvailableServices vnpayAvailableServices, TransactionServices transactionServices,AccountServices accountServices)
		{
			_vnpayAvailableServices = vnpayAvailableServices;
			_transactionServices = transactionServices;
			_accountServices = accountServices;
		}
		[BindProperty]
		public VnpayReturnResult? VnpayResult { get; set; }
		private int _userId { get; set; }
		private Transaction _currentTransaction { get; set; }
		public async Task<IActionResult> OnGet(int? myTransactionId)
		{
			if (myTransactionId == null)
				return BadRequest();
			try
			{
				await PopulateData(myTransactionId.Value);
				var returnResult = await _vnpayAvailableServices.OnPayResult(HttpContext, _currentTransaction);
				if (returnResult is null || returnResult.Success == false)
				{
					ModelState.AddModelError(string.Empty, "something wrong, why null, why empty, check code implementation");
					return Page();
				}
				// if sucess, add that to user balance
				VnpayResult = returnResult;
				var getUser = await _accountServices.GetById(_userId);
				getUser.Balance += VnpayResult.Amount;
				await _accountServices.Update(getUser);
				return Page();
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.Message);
			}
			
		}
		public async Task<IActionResult> OnGetTestTransactionHandler(int amount, int transactionId)
		{
			await PopulateData(transactionId);
			_currentTransaction.Status = Repository.Database.Model.Enum.TransactionStatus.SUCCESS;
			var result = await _transactionServices.Update(_currentTransaction);
			if(result is false)
			{
				ModelState.AddModelError(string.Empty, "something worng with updateing transaction, contatct admin now");
				return Page();
			}
			var getUser = await _accountServices.GetById(_userId);
			getUser.Balance += decimal.Parse(_currentTransaction.vnp_Amount);
			await _accountServices.Update(getUser);
			ViewData["SuccessTransactionTest"] = $"yes you have successfully add {amount}";
			return Page();
		}
		private async Task PopulateData(int transactionId)
		{
			var tryGetUserId = int.TryParse(HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("Id"))?.Value, out var userId);
			if (tryGetUserId is false)
				throw new UnauthorizedAccessException("User Unauthorized");
			_userId = userId;
			var transaction =  await _transactionServices.GetById(transactionId);
			if (transaction is null)
				throw new Exception("not found transaction");
			_currentTransaction = transaction;
		}
	}
}
