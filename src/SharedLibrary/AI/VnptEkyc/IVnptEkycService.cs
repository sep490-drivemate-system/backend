using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibrary.AI.VnptEkyc
{
    public interface IVnptEkycService
    {
        Task<VnptEkycResponse> VerifyCitizenIdentity(
            Stream frontImage,
            Stream backImage);
    }
}

