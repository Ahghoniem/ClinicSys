using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public class AppUrlHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppUrlHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            return request == null ? string.Empty : $"{request.Scheme}://{request.Host}";
        }



    }
}
