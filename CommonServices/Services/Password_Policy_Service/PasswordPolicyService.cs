using CommonServices.Resources;

namespace CommonServices.Services.Password_Policy_Service
{
    public class PasswordPolicyService
    {
        public static string ValidatePassword(string password)
        {
            if (password.Length < 6)
            {
                return ResponseMessageResource.MinLengthRequired;
            }

            if (password.Length >= 15)
            {
                return ResponseMessageResource.MaxLengthExceeded;
            }

            if (!password.Any(char.IsUpper))
            {
                return ResponseMessageResource.UpperCaseRequired;
            }

            if (!password.Any(char.IsDigit))
            {
                return ResponseMessageResource.DigitRequired;
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                return ResponseMessageResource.SpecialCharRequired;
            }

            return string.Empty; // Return empty string if the password is valid
        }
    }
}
