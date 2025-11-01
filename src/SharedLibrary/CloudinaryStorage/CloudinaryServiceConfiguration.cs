using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.CloudinaryStorage
{
    public class CloudinaryServiceConfiguration
    {
        [ConfigurationKeyName("cloud_name")]
        public string CloudName { get; set; }

        [ConfigurationKeyName("api_key")]
        public string ApiKey { get; set; }

        [ConfigurationKeyName("api_secret")]
        public string ApiSecret { get; set; }
    }
}
