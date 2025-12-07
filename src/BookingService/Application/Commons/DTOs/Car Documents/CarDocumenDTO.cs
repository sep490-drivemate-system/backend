namespace BookingService.Application.Commons.DTOs.Car_Documents
{
    public class CarDocumenDTO
    {
        public IFormFile? FrontImage { get; set; }
        public IFormFile? BackImage { get; set; }
        public string DocumentType { get; set; } // Insurance, Car Registrations, ...
    }

    public class CarDocumentBatchDTO
    {
        public IList<CarDocumenDTO> Documents { get; set; }
    }

    public class CarDocumentViewDTO
    {
        public string FrontImageUrl { get; set; }
        public string BackImageUrl { get; set; }
        public string DocumentType { get; set; }
    }
}
