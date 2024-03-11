namespace RazorAucionWebapp.Configure
{
	public class BindAppsettings
	{
		public decimal CommissionPercentage { get; set; }
		public int RefundValidTime { get; set; }
		public Admin Admin { get; set; }
		public int? TimeToPaid { get; set; }
		public decimal ComissionFixedPrice { get; set; }
		public int RefundFixedTimeMinute { get; set; }
	}
	public class Admin
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
	}
}
