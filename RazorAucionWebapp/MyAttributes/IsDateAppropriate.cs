using System.ComponentModel.DataAnnotations;

namespace RazorAucionWebapp.MyAttributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class IsDateAppropriate : ValidationAttribute
	{
		public DateTime? IsAfterThisDate { get; set; }

		public IsDateAppropriate(DateTime? isAfterThisDate)
		{
			IsAfterThisDate = isAfterThisDate;
		}
		public IsDateAppropriate()
		{
		}
		public override bool IsValid(object? value)
		{
			if (value is null) return false;
			var tryParseDate = DateTime.TryParse(value.ToString(),out var parsedDate);
			if (tryParseDate is false) return false;
			var compareResult = DateTime.Compare(parsedDate.Date, DateTime.Now.Date);
			switch (compareResult) 
			{
				case <= 0:
					ErrorMessage = "date is matching current date and time, it must atleast a day later";
					return false;
				default:
					break;
			}
			if(IsAfterThisDate is not null) 
			{
				var compareResult2 = DateTime.Compare(parsedDate.Date, IsAfterThisDate.Value.Date);
				switch (compareResult2)
				{
					case <= 0:
						ErrorMessage = $"date is matching the setted {IsAfterThisDate.Value.Date} date and time, it must atleast a day later";
						return false;
					default:
						break;
				}
			}
			return true;
		}
	}
}
