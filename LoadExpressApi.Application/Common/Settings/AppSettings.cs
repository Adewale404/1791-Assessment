using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoadExpressApi.Application.Common.Settings
{
    public class AppSettings
    {
        public const string Encryption = "Encryption";
        public string EncryptKey { get; set; } = string.Empty;
        public string EncryptIV { get; set; } = string.Empty;
        public string Env { get; set; }

    }
}
