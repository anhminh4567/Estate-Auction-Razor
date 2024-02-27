using System.ComponentModel.DataAnnotations;

namespace RazorAucionWebapp.MyAttributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class IsDateAppropriate : ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			if (value is null) return false;
			var tryParseDate = DateTime.TryParse(value.ToString(),out var parsedDate);
			if (tryParseDate is false) return false;
			var compareResult = DateTime.Compare(parsedDate, DateTime.Now);
			switch (compareResult) 
			{
				case <= 0:
					ErrorMessage = "date is matching current date and time, it must atleast a day later";
					return false;
				default:
					return true;
			}
			
		}
	}
}
