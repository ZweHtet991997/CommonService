using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace CommonServices.Services.Firebase_Service
{
    public class FirebaseNotiService : IFirebaseNotiService
    {
        public FirebaseNotiService()
        {
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            var credentials = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FirebaseConfig"));
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credentials
            });
        }

        public async Task<string> SendNotiAsync(string deviceToken, string title, string message)
        {
            var _message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = message
                },
                Token = deviceToken
            };
            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(_message);
                return $"Successfully sent message: {response}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending message: {ex.Message}", ex);
            }
        }
    }
}
