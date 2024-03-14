using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols;
using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.Enum;
using Repository.Interfaces.DbTransaction;
using Service.Services.VnpayService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.VnpayService.VnpayUtility
{
    public class VnpayReturn
    {
        private readonly IUnitOfWork _unitOfWork;

		public VnpayReturn(IUnitOfWork unitOfWork)
		{
			this._unitOfWork = unitOfWork;
		}

		public async Task<VnpayReturnResult> OnTransactionReturn(HttpContext httpContext, Account acc, string transactionDate)
        {
            if (httpContext.Request.QueryString.Value.Length > 0)
            {
                string vnp_HashSecret = VnpayDefaultValue.Vnp_HashSecret;//ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = httpContext.Request.Query;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData.Keys)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = httpContext.Request.Query["vnp_SecureHash"];
                string TerminalID = httpContext.Request.Query["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                string bankCode = httpContext.Request.Query["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

                var returnResult = new VnpayReturnResult()
                {
                    Amount = vnp_Amount,
                    BankCode = bankCode,
                    StatusCode = vnp_ResponseCode,
                    TransactionNo = vnpayTranId,
                    TransactionStatus = vnp_TransactionStatus,
                    Vnp_TxnRef = orderId,
                };
				
				if (checkSignature)
                {
					var newTransaction = new Transaction()
					{
						AccountId = acc.AccountId,
						Status = TransactionStatus.SUCCESS,
						vnp_Amount = vnp_Amount.ToString(),
						vnp_OrderInfo = "By user id:" + acc.AccountId.ToString(),
						vnp_TransactionDate = long.Parse(transactionDate),
						vnp_TxnRef = orderId.ToString(),
						vnp_PayDate = transactionDate.ToString(),
					};
					if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        //displayMsg.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        Console.WriteLine("Success");
                        returnResult.Success = true;
                        returnResult.Message = "Success";

                        var creatResult = await _unitOfWork.Repositories.transactionRepository.CreateAsync(newTransaction);

                        //var getTransaction = (await _unitOfWork.Repositories.transactionRepository
                        //    .GetByCondition(t => t.vnp_TxnRef == orderId.ToString())).First();
                        //currentTransaction.Status = Repository.Database.Model.Enum.TransactionStatus.SUCCESS;
						//getTransaction.Status = Repository.Database.Model.Enum.TransactionStatus.SUCCESS;
                       // await _unitOfWork.Repositories.transactionRepository.UpdateAsync(currentTransaction);
                        return returnResult;
                    }
                    else if( vnp_ResponseCode == "24")
                    {
						Console.WriteLine("fail");
						returnResult.Success = false;
						returnResult.Message = "24  Giao dịch không thành công do: Khách hàng hủy giao dịch";
                        newTransaction.Status = TransactionStatus.FAIL;
                        await _unitOfWork.Repositories.transactionRepository.UpdateAsync(newTransaction);
					}
                    else
                    {
                        Console.WriteLine("fail");
                        returnResult.Success = false;
                        returnResult.Message = "Fail";
                        //var getTransaction = (await _unitOfWork.Repositories.transactionRepository
                        //   .GetByCondition(t => t.vnp_TxnRef == orderId.ToString())).First();
                        //getTransaction.Status = Repository.Database.Model.Enum.TransactionStatus.FAIL;
                        //await _unitOfWork.Repositories.transactionRepository.UpdateAsync(getTransaction);
                        //currentTransaction.Status = Repository.Database.Model.Enum.TransactionStatus.SUCCESS;
                        //await _unitOfWork.Repositories.transactionRepository.UpdateAsync(currentTransaction);
                        newTransaction.Status = TransactionStatus.FAIL;
						await _unitOfWork.Repositories.transactionRepository.UpdateAsync(newTransaction);
						return returnResult;
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        //displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId,vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    //displayAmount.InnerText = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
                else
                {
                    returnResult.Success = false;
                    returnResult.Message = "Hash Incorect, the result is tampered before";

					//currentTransaction.Status = Repository.Database.Model.Enum.TransactionStatus.SUCCESS;
					//await _unitOfWork.Repositories.transactionRepository.UpdateAsync(currentTransaction);
					return returnResult;
                    //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    //displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return null;
        }
    }
}
