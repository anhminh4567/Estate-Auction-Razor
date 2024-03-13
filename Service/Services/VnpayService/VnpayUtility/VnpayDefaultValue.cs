using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.VnpayService.VnpayUtility
{
    public static class VnpayDefaultValue
    {
        public static string Currency = "VND";
        public static string Vnp_TmnCode = "N4NCWZW0";
        public static string Vnp_HashSecret = "NRYIEKSGJEOTAXMXENENKWDKQLMKIQCW";
        public static string Vnp_Returnurl = "https://localhost:7156/Vnpay/Return?myAccountId=";
        public static string Vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public static string Vnp_Api = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
        public static decimal Vnp_Max_Per_Transaction = 100000000;//100tr
		public static decimal Vnp_Min_Per_Transaction = 10000;//10k

	}
	public static class VnpayTransactionType
    {
        public const string HoanTraMotPhan = "03";
        public const string HoanTraToanPhan = "02";
    }
}
