using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols;
using Repository.Database.Model.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.VnpayService
{
    public class BuildUrl
    {
        private int _amount { get; set; } = 0;
        private string _bankCode;
        private int _transactionDuration = 5;// minute
		public BuildUrl(int amount,string bankcode)
		{
            _amount = amount;
            _bankCode = bankcode;
		}

		//protected void btnPay_Click(object sender, EventArgs e)
		public string btnPay_Click(HttpContext httpContext, Account acc)
		{
			//Get Config Info
			string vnp_Returnurl = VnpayDefaultValue.Vnp_Returnurl; //URL nhan ket qua tra ve 
            string vnp_Url = VnpayDefaultValue.Vnp_Url; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = VnpayDefaultValue.Vnp_TmnCode; //Ma website
            string vnp_HashSecret = VnpayDefaultValue.Vnp_HashSecret; //Chuoi bi mat
            
            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                //lblMessage.Text = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config";
                return string.Empty;
            }
            //Get payment input
            OrderInfo order = new OrderInfo();
            ///Save order to db
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = this._amount; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            order.OrderDesc = "desc";
            order.CreatedDate = DateTime.Now;

			DateTime vnp_expiredDate = order.CreatedDate.AddMinutes(_transactionDuration);
			string locale = "";//cboLanguage.SelectedItem.Value;
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không 
            //mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND
            //(một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY
            //là: 10000000


            if (_bankCode != null && !string.IsNullOrEmpty(_bankCode))
            {
                vnpay.AddRequestData("vnp_BankCode", _bankCode);
            }
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(httpContext));
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
			vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ 
            //thống của merchant.Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.Không được
            //trùng lặp trong ngày
            //Add Params of 2.1.0 Version
            vnpay.AddRequestData("vnp_ExpireDate", vnp_expiredDate.ToString("yyyyMMddHHmmss")); ;
                //Billing , OPTIONAL , I DONT KNOW
            //vnpay.AddRequestData("vnp_Bill_Mobile", txt_billing_mobile.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_Email", txt_billing_email.Text.Trim());
            //var fullName = txt_billing_fullname.Text.Trim();
            //if (!string.IsNullOrEmpty(fullName))
            //{
            //    var indexof = fullName.IndexOf(' ');
            //    vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
            //    vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1,
            //    fullName.Length - indexof - 1));
            //}
            //vnpay.AddRequestData("vnp_Bill_Address", txt_inv_addr1.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_City", txt_bill_city.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_Country", txt_bill_country.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_State", "");
                // Invoice, OPTIONAL TOO, BUT MIGHT BE USEFUL
            vnpay.AddRequestData("vnp_Inv_Phone", acc.Telephone);
            vnpay.AddRequestData("vnp_Inv_Email", acc.Email);
            vnpay.AddRequestData("vnp_Inv_Customer", acc.FullName);
            vnpay.AddRequestData("vnp_Inv_Address", "22 Láng Hạ, Đống Đa, Hà Nội");
            vnpay.AddRequestData("vnp_Inv_Company", "Công ty VNPAY");
            vnpay.AddRequestData("vnp_Inv_Taxcode", "20180924080900");
            vnpay.AddRequestData("vnp_Inv_Type", "I"); // I - ca nhan || O - to chuc

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //Response.Redirect(paymentUrl);
            return paymentUrl;
        }
        // vui lòng tham khảo thêm tại code demo

    }
}
