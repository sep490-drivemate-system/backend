using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.CloudinaryStorage
{
    public class CloudinaryServiceProvider: ICloudinaryServiceProvider
    {
        private readonly CloudinaryServiceConfiguration _config;

        private readonly Cloudinary CloudinaryProvider;

        public CloudinaryServiceProvider(IConfiguration config) 
        {
            _config = config.GetSection("Cloudinary").Get<CloudinaryServiceConfiguration>();

            if (_config == null)
            {
                throw new InvalidOperationException("Can not initialize the cloudinary third party system (no configuration found)");
            }

            CloudinaryProvider = new Cloudinary(new Account(_config.CloudName, _config.ApiKey, _config.ApiSecret));
            CloudinaryProvider.Api.Secure = true;
        }

        public string UploadImageStreamResourceToCloudinary(Stream stream, string file_name)
        {
            ImageUploadParams image_param = new ImageUploadParams()
            {
                File = new FileDescription(file_name, stream),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
            };

            ImageUploadResult result = CloudinaryProvider.Upload(image_param);

            Console.WriteLine(result.JsonObj);

            return result.SecureUrl.ToString();
        }

        public string UploadImageFormFileResourceToCloudinary(IFormFile file, string file_name)
        {
            return UploadImageStreamResourceToCloudinary(file.OpenReadStream(), $"{file_name}-{file.FileName}");
        }

        public string UploadImageFormFileResourceToCloudinaryWithExactName(IFormFile file, string file_name)
        {
            return UploadImageStreamResourceToCloudinary(file.OpenReadStream(), $"{file_name}");
        }

        public bool DeleteResourceFromCloudinary(string public_id)
        {
            DeletionParams delete_oaranm = new DeletionParams(public_id);
            DeletionResult delete_result = CloudinaryProvider.Destroy(delete_oaranm);

            return delete_result.Result == "ok";
        }
    }
}
