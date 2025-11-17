using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLibrary.AI.VnptEkyc
{
    public interface IVnptEkycService
    {
        Task<VnptEkycResponse> AnalyzeDocumentAsync(
            VnptDocumentType documentType,
            Stream frontImage,
            string frontFileName,
            Stream? backImage = null,
            string? backFileName = null,
            CancellationToken cancellationToken = default);

        Task<VnptEkycResponse> MatchFacesAsync(
            Stream documentPortraitImage,
            string documentPortraitFileName,
            Stream selfieImage,
            string selfieFileName,
            CancellationToken cancellationToken = default);

        Task<VnptEkycResponse> VerifyLivenessAsync(
            Stream videoStream,
            string videoFileName,
            CancellationToken cancellationToken = default);
    }
}

