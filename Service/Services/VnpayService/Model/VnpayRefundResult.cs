﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.VnpayService.Model
{
	public class VnpayRefundResult
	{
		public string? vnp_ResponseId { get; set; }
		public string? vnp_Command { get; set; }
		public string? vnp_ResponseCode { get; set; }
		public string? vnp_Message { get; set; }
		public string? vnp_TxnRef { get; set; }
		public string? vnp_Amount { get; set; }
		public string? vnp_OrderInfo { get; set; }
		public string? vnp_BankCode { get; set; }
		public string? vnp_PayDate { get; set; }
		public string? vnp_TransactionNo { get; set; }
		public string? vnp_TransactionType { get; set; }
		public string? vnp_TransactionStatus { get; set; }
	}
}
