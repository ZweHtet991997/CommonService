using CommonServices.Models.Enums.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace CommonServices.Services.Account_Vertification_Service
{
    public class VertificationService : IVertificationService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _expiration = TimeSpan.FromMinutes(5);

        public VertificationService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public int GenerateAndStoreCode(string email)
        {
            Random random = new Random();
            var activationCode = random.Next(100000, 1000000);
            //store code in to memory cache.
            _memoryCache.Set(email, activationCode, _expiration);
            return activationCode;
        }

        public Enum ValidateCode(string email, int code)
        {
            if (_memoryCache.TryGetValue(email, out int activationCode))
            {
                if (activationCode == code)
                {
                    return EnumOTPResponse.Valid;
                }
                else if (activationCode != code)
                {
                    return EnumOTPResponse.Invalid;
                }
            }
            return EnumOTPResponse.Expired;
        }
    }
}
