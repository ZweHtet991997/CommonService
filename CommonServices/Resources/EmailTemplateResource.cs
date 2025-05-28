namespace CommonServices.Resources
{
    public static class EmailTemplateResource
    {
        private static string ContactUs(string name, string phoneNo, string email, string description)
        {
            return $"Name: {name}<br>Phone Number: {phoneNo}<br>Email: {email}<br>Description: {description}";
        }

        private static string AccountActivate(string projectName, int code)
        {
            return @"<!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Account Activation</title>
  <style>
    /* General styles */
    body {
      font-family: Arial, sans-serif;
      background-color: #f4f4f4;
      margin: 0;
      padding: 0;
    }
    h4{
        font-size:22px;
    }
    .container {
      background-color: #ffffff;
      width: 80%;
      max-width: 600px;
      margin: 0 auto;
      padding: 20px;
      border-radius: 10px;
      box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    }
    .button {
      display: inline-block;
      padding: 10px 20px;
      font-size: 16px;
      color: #ffffff;
      background-color: #28a745;
      text-decoration: none;
      border-radius: 5px;
    }
    .button:hover {
      background-color: #218838;
    }
  </style>
</head>" +
$@"<body>
  <div class=""container"">
    <h2>Your Vertification Code!</h2>
    <p>Thank you for registering with us.</p>
    <p>Please enter this code to verify your account to sign in.</p>
    <p>Please do not forward this email. If you didn't request this code, you can ignore this message.</p>
    <p>The vertification code below is unique and will expire in next 5 minute.</p>
    <h4><b>{code}</b></h4>
    <p>Thank you,</p>
    <p>{projectName}</p>
  </div>
</body>
</html>
";
        }

        private static string SuccessActivate(string projectName,string userName)
        {
            return @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Account Activated</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            color: #333;
            line-height: 1.6;
            margin: 0;
            padding: 20px;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #e3e3e3;
            border-radius: 8px;
            background-color: #f9f9f9;
        }
        .button {
            display: inline-block;
            margin-top: 20px;
            padding: 10px 20px;
            background-color: #28a745;
            color: white;
            text-decoration: none;
            border-radius: 5px;
        }
        .button:hover {
            background-color: #218838;
        }
    </style>
</head>
<body>" +
    $@"<div class=""container"">
        <h2>Hello {userName}</h2>
        <p>Your account has been successfully activated!</p>
        <p>Please go to the login form.</strong></p>
        <p>Thank you,<br />{projectName}</p>
    </div>
</body>
</html>";
        }

        public static string AccountActivateTemplate(string projectName, int code)
            => AccountActivate(projectName, code);
        public static string SuccessActivateTemplate(string projectName,string userName) 
            => SuccessActivate(projectName,userName);
        public static string ContactUsTemplate(string name,
            string phoneNo,
            string email,
            string description) => ContactUs(name, phoneNo, email, description);
    }
}
