using Microsoft.AspNetCore.Http;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
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

		public VnpayAvailableServices(VnpayQuery vnpayQuery, VnpayRefund vnpayRefund, VnpayBuildUrl vnpayBuildUrl, VnpayReturn vnpayReturn)
		{
			_vnpayQuery = vnpayQuery;
			_vnpayRefund = vnpayRefund;
			_vnpayBuildUrl = vnpayBuildUrl;
			_vnpayReturn = vnpayReturn;

		}
		public string? GeneratePayUrl(HttpContext httpContext, Account account, int amount) 
		{
			return _vnpayBuildUrl.btnPay_Click(httpContext, account, amount);
		}
		public VnpayQueryResult? QueryVnpayTransaction(HttpContext httpContext, Transaction transaction)
		{
			return _vnpayQuery.btnQuery_Click(httpContext,transaction);
		}
		public VnpayReturnResult PayResult(HttpContext httpContext) 
		{
			return _vnpayReturn.OnTransactionReturn(httpContext);
		}
	}
}
