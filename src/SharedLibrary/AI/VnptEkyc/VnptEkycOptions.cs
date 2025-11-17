using System;
using System.Collections.Generic;

namespace SharedLibrary.AI.VnptEkyc
{
    public class VnptEkycOptions
    {
        public const string SectionName = "VnptEkyc";

        public string BaseUrl { get; set; } = "https://api.vnpt.vn";
        public string ApiKey { get; set; } = string.Empty;
        public string ApiSecret { get; set; } = string.Empty;
        public string ApiKeyHeaderName { get; set; } = "x-api-key";
        public string ApiSecretHeaderName { get; set; } = "x-api-secret";

        public string DocumentEndpoint { get; set; } = "/ekyc/v1/ocr";
        public string FaceMatchEndpoint { get; set; } = "/ekyc/v1/face/match";
        public string LivenessEndpoint { get; set; } = "/ekyc/v1/face/liveness";

        public string DocumentFrontFieldName { get; set; } = "front_image";
        public string DocumentBackFieldName { get; set; } = "back_image";
        public string DocumentTypeFieldName { get; set; } = "document_type";

        public string FaceDocumentFieldName { get; set; } = "document_face";
        public string FaceSelfieFieldName { get; set; } = "selfie_face";

        public string LivenessFieldName { get; set; } = "video";

        public int HttpTimeoutSeconds { get; set; } = 60;

        public Dictionary<string, string> DocumentTypeMapping { get; set; } = new(StringComparer.OrdinalIgnoreCase)
        {
            ["CitizenIdentification"] = "cccd_chip",
            ["CitizenIdentificationCard"] = "cccd",
            ["DriverLicense"] = "gplx",
            ["Passport"] = "hc"
        };
    }
}

