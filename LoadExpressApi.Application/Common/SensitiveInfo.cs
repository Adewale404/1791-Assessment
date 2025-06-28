using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoadExpressApi.Application.Common
{
    public partial class SensitiveInfo
    {
        public static string MaskSensitiveInfo(string input)
        {
            input ??= " ";
            input = MaskBVN().Replace(input, "*******");
            input = MaskDateOfBirth().Replace(input, "*******");
            input = NIN().Replace(input, "*******");
            input = MaskPassword().Replace(input, "*******");
            input = MaskUserName().Replace(input, "*******");
            return input;
        }

        private static Regex MaskBVN() => new Regex(@"(?<=""bvn"":\s*"")[^""]+", RegexOptions.IgnoreCase);
        private static Regex MaskUserName() => new Regex(@"(?<=""username"":\s*"")[^""]+", RegexOptions.IgnoreCase);
        private static Regex MaskDateOfBirth() => new Regex(@"(?<=""dateOfBirth"":\s*"")[^""]+", RegexOptions.IgnoreCase);
        private static Regex MaskPassword() => new Regex(@"(?<=""password"":\s*"")[^""]+", RegexOptions.IgnoreCase);
        private static Regex NIN() => new Regex(@"(?<=""nin"":\s*"")[^""]+", RegexOptions.IgnoreCase);

    }
}
