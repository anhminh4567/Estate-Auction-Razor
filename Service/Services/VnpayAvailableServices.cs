using Microsoft.AspNetCore.Http;
using MimeKit;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Service.Services.AppAccount;
using Service.Services.VnpayService.Model;
using Service.Services.VnpayService.VnpayUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class VnpayAvailableServices
	{
		private VnpayQuery _vnpayQuery;
		private VnpayRefund _vnpayRefund;
		private VnpayBuildUrl _vnpayBuildUrl;
		private VnpayReturn _vnpayReturn;
		private readonly TransactionServices _transactionServices;
		private readonly AccountServices _accountService;

        public VnpayAvailableServices(VnpayQuery vnpayQuery, VnpayRefund vnpayRefund, VnpayBuildUrl vnpayBuildUrl, VnpayReturn vnpayReturn, TransactionServices transactionServices, AccountServices accountService)
        {
            _vnpayQuery = vnpayQuery;
            _vnpayRefund = vnpayRefund;
            _vnpayBuildUrl = vnpayBuildUrl;
            _vnpayReturn = vnpayReturn;
            _transactionServices = transactionServices;
            _accountService = accountService;
        }

        public async Task<string?> GeneratePayUrl(HttpContext httpContext, Account account, int amount) 
		{
			return await _vnpayBuildUrl.btnPay_Click(httpContext, account, amount);
		}
		public VnpayQueryResult? QueryVnpayTransaction(HttpContext httpContext, Transaction transaction)
		{
			return _vnpayQuery.btnQuery_Click(httpContext,transaction);
		}
		public async Task< VnpayReturnResult> OnPayResult(HttpContext httpContext, Transaction transaction) 
		{
			return await _vnpayReturn.OnTransactionReturn(httpContext, transaction);
		}
		public async Task<(bool isSuccess, string message)> RefundVnpayTransaction(HttpContext httpContext, Transaction transaction, Account account, int RefundValidTimeMinute) 
		{
			var refundValidTime = DateTime.ParseExact(transaction.vnp_PayDate,"yyyyMMddHHmmss",null).AddMinutes(RefundValidTimeMinute);
			var comparisonResult = DateTime.Compare(DateTime.Now, refundValidTime);
			if(comparisonResult >= 0) 
			{
				return (false, "cannot refund, time is over only allow " + refundValidTime +" minute");
			} 
			//var balance = account.Balance;
			var tryParseTransaction = decimal.TryParse(transaction.vnp_Amount,out var transactionAmount);
			if(tryParseTransaction == false)
			{
				return (false, "transaction is not valid");
			}
			//if (balance < transactionAmount)
			//{
			//	//Console.WriteLine("cannot refund, balance in your account is not enough to paid");
			//	return (false, "cannot refund, balance in your account is not enough to paid");
			//}
			var result =  _vnpayRefund.btnRefund_Click(httpContext,transaction,account);
			if ((result?.vnp_ResponseCode?.Equals("00")).Value)
			{
				transaction.Status = TransactionStatus.CANCELLED;
				account.Balance -= transactionAmount;
				await _accountService.Update(account);
				await _transactionServices.Update(transaction);
				return (true, "yes refund success");
			}else if ((result?.vnp_ResponseCode.Equals("91")).Value)
			{
                return (false, "Không tìm thấy giao dịch yêu cầu hoàn trả");
            }else if ((result?.vnp_ResponseCode.Equals("95")).Value)
			{
                return (false, "Giao dịch này không thành công bên VNPAY. VNPAY từ chối xử lý yêu cầu, vnpay loi");
            }else if((result?.vnp_ResponseCode.Equals("97")).Value)
			{
                return (false, "97  Checksum không hợp lệ");
            }
            else if ((result?.vnp_ResponseCode.Equals("99")).Value)
            {
                return (false, "Các lỗi khác (lỗi còn lại, không có trong danh sách mã lỗi đã liệt kê, do vnpay server)");
            }
            else
			{
				return (false, "error in refunding on vnpay gate, cannot resolve this request");
            }
		}
	}
}
