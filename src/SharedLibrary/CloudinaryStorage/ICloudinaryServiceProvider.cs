using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.CloudinaryStorage
{
    public interface ICloudinaryServiceProvider
    {
        string UploadImageStreamResourceToCloudinary(Stream stream, string file_name);

        string UploadImageFormFileResourceToCloudinary(IFormFile file, string file_name);

        bool DeleteResourceFromCloudinary(string public_id);
    }
}
