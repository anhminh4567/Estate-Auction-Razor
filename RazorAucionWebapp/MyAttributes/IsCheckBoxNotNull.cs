using System.ComponentModel.DataAnnotations;

namespace RazorAucionWebapp.MyAttributes
{
    public class IsCheckBoxNotNull : ValidationAttribute
    {
        public IsCheckBoxNotNull() {
            ErrorMessage = "At least one estate category must be selected!";

        }

        public override bool IsValid(object? value)
        {
            return false;
        }
    }
}
