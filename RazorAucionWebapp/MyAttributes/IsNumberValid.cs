using System.ComponentModel.DataAnnotations;

namespace RazorAucionWebapp.MyAttributes
{
	public class IsNumberValid : ValidationAttribute
	{
		public decimal? MinValue { get; set; }

		public IsNumberValid()
		{
		}

		public IsNumberValid(decimal minValue)
		{
			MinValue = minValue;
		}

		public IsNumberValid(string errorMessage) : base(errorMessage)
		{
		}

		public override bool IsValid(object? value)
		{
			ErrorMessage = " number must greater than 0";
			var tryParseToLong = long.TryParse(value.ToString(), out var parsedValue);
			if(tryParseToLong is false)
			{
				ErrorMessage = " not a number";
				return false;
			}
			if(parsedValue <= 0)
			{
				return false;
			}
			if(MinValue is not null) 
			{
				if( parsedValue < MinValue ) 
				{
					return false;
				}
			}
			return true;
		}
	}
}
