using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers
{
    public class FileBuilderBuildHelper
    {
        private readonly AppUrlHelper _appUrlHelper;
        public FileBuilderBuildHelper(AppUrlHelper appUrlHelper)
        {
            _appUrlHelper = appUrlHelper;
        }

        public string BuildImageUrl(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            return $"{_appUrlHelper.GetBaseUrl()}/{relativePath.TrimStart('/').Replace("\\", "/")}";
        }
    }
}
