namespace CommonServices.Resources
{
    public static class ResponseMessageResource
    {
        public const string Success = "Success";
        public const string AccountActivated = "Account Activated";
        public const string OTPExpired = "OTP Expired";
        public const string InvalidOTP = "Invalid OTP Code";

        #region PasswordPolicy
        public const string MinLengthRequired = "Password must be at least 6 characters long.";
        public const string MaxLengthExceeded = "Password too long.Your password must be at most 15 characters long.";
        public const string UpperCaseRequired = "Password must include at least one uppercase letter.";
        public const string DigitRequired = "Password must include at least one digit.";
        public const string SpecialCharRequired = "Password must include at least one special character.";
        #endregion

        #region FTP
        public const string CreateFailed = "Failed to Create Directory";
        #endregion
    }

}
