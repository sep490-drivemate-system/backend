using AutoMapper;
using BookingService.Application.Commons.Constants;
using BookingService.Application.Commons.DTOs.Car_Documents;
using BookingService.Application.Commons.DTOs.Cars.Create;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.Cars.Update;
using BookingService.Application.Commons.DTOs.Feedbacks;
using BookingService.Application.Interfaces;
using BookingService.Domain.Entities;
using Newtonsoft.Json;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using System.Linq.Expressions;

namespace BookingService.Application.UseCase
{
    public class CarUseCase(IUnitOfWork unit_of_work, ICloudinaryServiceProvider cloudinary, IHttpClientFactory http_client_factory, IMapper mapper) : ICarUseCase
    {
        private readonly IUnitOfWork _unitOfWork = unit_of_work;
        private readonly IHttpClientFactory _http_client_factory = http_client_factory;
        private readonly IMapper _mapper = mapper;
        private readonly ICloudinaryServiceProvider _cloudinary = cloudinary;

        public async Task<Result<Guid>> CreateNewCar(CarCreationDTO information)
        {
            // Checking for car brand
            var car_manufacturer = await _unitOfWork.ManufacturerRepository.GetByIdAsync(information.BrandId);

            if (car_manufacturer == null || car_manufacturer.IsDeleted)
            {
                return Result<Guid>.Failure(ServiceError.BadRequestError($"{information.BrandId}"), Messages.Commons.UNHANDLED);
            }

            // Uploading image to third party storage

            Guid temptGuid = Guid.NewGuid();

            string thumbnail_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.ThumbnailImage, $"{temptGuid}-thumbnail");
            string car_registration_front_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.RegistrationFront, $"{temptGuid}-registration_front"); ;
            string car_registration_back_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.RegistrationBack, $"{temptGuid}-registration_back"); ;
            string car_insurance_front_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.InsuranceFront, $"{temptGuid}-insurance_front"); ;
            string car_insurance_back_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.InsuranceBack, $"{temptGuid}-insurance_back"); ;
            string car_front_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarFrontImage, $"{temptGuid}-front"); ;
            string car_back_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarBackImage, $"{temptGuid}-back"); ;
            string car_left_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarLeftImage, $"{temptGuid}-left"); ;
            string car_right_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarRightImage, $"{temptGuid}-right"); ;
            string car_interior_image_url = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.InteriorImage, $"{temptGuid}-interior"); ;

            // Actual creation
            CarImage car_front = new CarImage { ImageUrl = car_front_image_url };
            CarImage car_back = new CarImage { ImageUrl = car_back_image_url };
            CarImage car_left = new CarImage { ImageUrl = car_left_image_url };
            CarImage car_right = new CarImage { ImageUrl = car_right_image_url };
            CarImage car_interiror = new CarImage { ImageUrl = car_interior_image_url };

            string documentBlob = JsonConvert.SerializeObject(new List<CarDocument>
            {
                new CarDocument {
                    FrontImageUrl = car_registration_front_image_url,
                    BackImageUrl = car_registration_back_image_url,
                    DocumentType = "registration"
                },
                new CarDocument
                {
                    FrontImageUrl = car_insurance_front_image_url,
                    BackImageUrl = car_insurance_back_image_url,
                    DocumentType = "insurance"
                }
            });

            Car car = new Car
            {
                InstructorId = information.InstructorId,
                ThumbnailUrl = thumbnail_url,
                DocumentJsonBlobString = documentBlob,
                FuelType = information.FuelType,
                Name = information.Model,
                Description = information.Description,
                //Year = information.Year,
                //Color = information.Color,
                SeatCount = information.Seats,
                LicenseTier = information.LicenseTier,
                LicensePlate = information.LicensePlate,
                CarType = information.CarType,
                CarImages = new List<CarImage> { car_front, car_back, car_left, car_right, car_interiror },
                Price = information.HourlyPrice,
                Status = Domain.Enum.CarStatus.Pending,
                ManufacturerId = information.BrandId,
                InsuranceEndTime = information.InsuranceEndTime,
            };

            try
            {
                await _unitOfWork.CarRepository.CreateAsync(car);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Commons.UNHANDLED);
            }

            // Reload and get the newly created car information
            var new_car_info = _unitOfWork.GetTrackingEntry(car);

            return Result<Guid>.Success(((Car)new_car_info.Entity).Id);
        }

        public async Task<Result<bool>> UpdateCarInformation(Guid id, CarUpdateDTO information)
        {
            var carDetail = await _unitOfWork.CarRepository.GetByIdAsync(id, "CarImages");

            if (carDetail == null || carDetail.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.UNHANDLED);
            }

            // updating simple items
            if (information.HourlyPrice.HasValue) carDetail.Price = information.HourlyPrice.Value;

            if (!string.IsNullOrEmpty(information.Model)) carDetail.Name = information.Model;

            if (!string.IsNullOrEmpty(information.LicensePlate)) carDetail.LicensePlate = information.LicensePlate;

            if (information.BrandId.HasValue) carDetail.ManufacturerId = information.BrandId.Value;

            if (information.Seats.HasValue) carDetail.SeatCount = information.Seats.Value;

            if (!string.IsNullOrEmpty(information.FuelType)) carDetail.FuelType = information.FuelType;

            if (information.InsuranceEndTime.HasValue) carDetail.InsuranceEndTime = information.InsuranceEndTime.Value;

            if (information.LicenseTier.HasValue) carDetail.LicenseTier = information.LicenseTier.Value;

            if (information.ThumbnailImage != null) carDetail.ThumbnailUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.ThumbnailImage, carDetail.ThumbnailUrl);

            // Car images
            if (information.CarFrontImage != null)
            {
                var current_image = carDetail.CarImages.FirstOrDefault(x => x.ImageUrl.Contains("front"));
                if (current_image != null)
                {
                    current_image.ImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarFrontImage, current_image.ImageUrl.Split("/").Last());
                }
            }

            if (information.CarBackImage != null)
            {
                var current_image = carDetail.CarImages.FirstOrDefault(x => x.ImageUrl.Contains("back"));

                if (current_image != null)
                {
                    current_image.ImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarFrontImage, current_image.ImageUrl.Split("/").Last());
                }
            }

            if (information.CarLeftImage != null)
            {
                var current_image = carDetail.CarImages.FirstOrDefault(x => x.ImageUrl.Contains("left"));

                if (current_image != null)
                {
                    current_image.ImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarFrontImage, current_image.ImageUrl.Split("/").Last());
                }
            }

            if (information.CarRightImage != null)
            {
                var current_image = carDetail.CarImages.FirstOrDefault(x => x.ImageUrl.Contains("right"));

                if (current_image != null)
                {
                    current_image.ImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarFrontImage, current_image.ImageUrl.Split("/").Last());
                }
            }

            if (information.CarFrontImage != null)
            {
                var current_image = carDetail.CarImages.FirstOrDefault(x => x.ImageUrl.Contains("interior"));
                if (current_image != null)
                {
                    current_image.ImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.CarFrontImage, current_image.ImageUrl.Split("/").Last());
                }
            }

            // Documents
            var docs = System.Text.Json.JsonSerializer.Deserialize<List<CarDocument>>(carDetail.DocumentJsonBlobString);
            bool docsUpdated = false;

            var insurance = docs.FirstOrDefault(x => x.DocumentType == "insurance");
            var registration = docs.FirstOrDefault(x => x.DocumentType == "registration");

            if (information.InsuranceFront != null) insurance.FrontImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.InsuranceFront, insurance.FrontImageUrl.Split("/").Last());
            if (information.InsuranceBack != null) insurance.FrontImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.InsuranceBack, insurance.BackImageUrl.Split("/").Last());

            if (information.RegistrationFront != null) registration.FrontImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.RegistrationFront, registration.FrontImageUrl.Split("/").Last());
            if (information.RegistrationBack != null) registration.BackImageUrl = _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(information.RegistrationBack, registration.BackImageUrl.Split("/").Last());

            if (docsUpdated)
            {
                carDetail.DocumentJsonBlobString = Newtonsoft.Json.JsonConvert.SerializeObject(docs);
            }

            carDetail.Status = Domain.Enum.CarStatus.ReApply;
            _unitOfWork.CarRepository.Update(carDetail);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteCar(Guid id)
        {
            var car_information = await _unitOfWork.CarRepository.GetByIdAsync(id);

            if (car_information == null)
            {
                return Result<bool>.Success(false, Messages.Commons.NOTFOUND);
            }

            try
            {
                _unitOfWork.CarRepository.Remove(car_information);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException($"{ex.Message}"), Messages.Commons.UNHANDLED);
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<CarDetailDTO>> GetCarDetail(Guid id)
        {
            var car_information = await _unitOfWork.CarRepository.GetByIdAsync(id, "CarImages,Bookings,Feedbacks,Manufacturer");

            if (car_information == null)
            {
                return Result<CarDetailDTO>.Failure(ServiceError.NotFoundError($"{id}"), Messages.Commons.NOTFOUND);
            }

            List<CarDocument> car_documents = new List<CarDocument>();

            if (!string.IsNullOrEmpty(car_information.DocumentJsonBlobString))
            {
                try
                {
                    car_documents = System.Text.Json.JsonSerializer.Deserialize<List<CarDocument>>(car_information.DocumentJsonBlobString) ?? new List<CarDocument>();
                }
                catch (System.Text.Json.JsonException)
                {
                    car_documents = new List<CarDocument>();
                }
            }

            return Result<CarDetailDTO>.Success(new CarDetailDTO
            {
                Id = car_information.Id,
                Detail = car_information.Description,
                FuelType = car_information.FuelType,
                LicensePlate = car_information.LicensePlate,
                LicenseTier = car_information.LicenseTier,
                ModelName = car_information.Name,
                SeatCounts = car_information.SeatCount,
                Images = car_information.CarImages?.Select(x => x.ImageUrl).ToList() ?? new List<string>(),
                ManufacturerName = car_information.Manufacturer?.Name ?? "Unknown",
                ManufacturerId = car_information.ManufacturerId,
                StatusEnum = car_information.Status,
                ThumbnailUrl = car_information.ThumbnailUrl,
                OwnerId = car_information.InstructorId,
                VehicleType = car_information.CarType,
                Price = car_information.Price,
                DocumentsRawString = car_information.DocumentJsonBlobString,
                Insurance = car_documents.FirstOrDefault(x => x.DocumentType == "insurance"), // Get from document blob string
                Registration = car_documents.FirstOrDefault(x => x.DocumentType == "registration"), // Get from document blob string
                BookingCount = car_information.Bookings?.Count ?? 0,
                AverageRating = car_information.Feedbacks?.Select(x => x.CarRating).DefaultIfEmpty(0).Average() ?? 0,
                Status = car_information.Status.ToString(),
            });
        }

        public async Task<Result<PaginatedList<CarDTO>>> GetCarPaginatedList(CarListFilterDTO filter)
        {
            Expression<Func<Car, bool>>? filter_expression = x => (filter.Manufacturer == null || x.Manufacturer!.Name.StartsWith(filter.Manufacturer!))
            && (filter.LicenseTier == null || x.LicenseTier >= filter.LicenseTier)
            && (filter.SeatCounts == null || x.SeatCount == filter.SeatCounts)
            && (filter.CarType == null || x.CarType == filter.CarType)
            && (filter.FuelType == null || x.FuelType.ToLower().Equals(filter.FuelType.ToLower()))
            && x.Status == Domain.Enum.CarStatus.Approved
            && !x.IsDeleted; ;
            Func<IQueryable<Car>, IOrderedQueryable<Car>>? order_expression = null;
            string included_properties = "Manufacturer,Packages,CarImages,Bookings,Feedbacks";


            if (filter.OrderBy != null)
            {
                switch (filter.OrderBy)
                {
                    case "price_asc":
                        order_expression = x => x.OrderBy(x => x.Price);
                        break;
                    case "price":
                        order_expression = x => x.OrderByDescending(x => x.Price);
                        break;
                    case "rating_asc":
                        order_expression = x => x.OrderBy(x => x.Feedbacks.Average(x => x.CarRating));
                        break;
                    case "rating":
                        order_expression = x => x.OrderByDescending(x => x.Feedbacks.Average(x => x.CarRating));
                        break;
                    case "booking_asc":
                        order_expression = x => x.OrderBy(x => x.Bookings.Count);
                        break;
                    case "booking":
                        order_expression = x => x.OrderByDescending(x => x.Bookings.Count);
                        break;
                    default:
                        break;
                }
            }

            var filtered_results = await _unitOfWork.CarRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: included_properties);

            IEnumerable<CarDTO> mapped_results = filtered_results.Select(x => new CarDTO
            {
                Id = x.Id,
                ModelName = x.Name,
                ManufacturerName = x.Manufacturer?.Name ?? "Unknown",
                ThumbnailUrl = x.ThumbnailUrl,
                SeatCounts = x.SeatCount,
                FuelType = x.FuelType,
                VehicleType = x.CarType,
                Price = x.Price,
                ManufacturerId = x.ManufacturerId,
                LicenseTier = x.LicenseTier,
                BookingCount = x.Bookings?.Count ?? 0,
                AverageRating = x.Feedbacks?.Count > 0 ? x.Feedbacks.Average(x => x.CarRating) : 0,
            });

            return Result<PaginatedList<CarDTO>>.Success(PaginatedList<CarDTO>.Create(mapped_results, filter.PageIndex, filter.PageSize));
        }

        public async Task<Result<IEnumerable<CarDetailDTO>>> GetInstructorCarList(Guid id)
        {
            Expression<Func<Car, bool>> filter_expression = x => x.InstructorId == id && !x.IsDeleted;
            Func<IQueryable<Car>, IOrderedQueryable<Car>> order_expression = x => x.OrderBy(u => u.CreatedAt);
            string included_properties = "Manufacturer,Packages,CarImages,Bookings,Feedbacks";

            var instructor_cars = await _unitOfWork.CarRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: included_properties, disable_tracking: true);

            //var carDtos = _mapper.Map<List<CarInstructorDetailDTO>>(instructor_cars);

            return Result<IEnumerable<CarDetailDTO>>.Success(instructor_cars.Select(x => new CarDetailDTO
            {
                Id = x.Id,
                ThumbnailUrl = x.ThumbnailUrl,
                LicensePlate = x.LicensePlate,
                ModelName = x.Name,
                Detail = x.Description,
                ManufacturerName = x.Manufacturer.Name,
                ManufacturerId = x.ManufacturerId,
                VehicleType = x.CarType,
                FuelType = x.FuelType,
                OwnerId = x.InstructorId,
                SeatCounts = x.SeatCount,
                Price = x.Price,
                Status = x.Status.ToString(),
                StatusEnum = x.Status,
                Images = x.CarImages.Select(x => x.ImageUrl),
                DocumentsRawString = x.DocumentJsonBlobString,
                LicenseTier = x.LicenseTier,
                Registration = System.Text.Json.JsonSerializer.Deserialize<List<CarDocument>>(x.DocumentJsonBlobString)?.FirstOrDefault(x => x.DocumentType == "registration") ?? null,
                Insurance = System.Text.Json.JsonSerializer.Deserialize<List<CarDocument>>(x.DocumentJsonBlobString)?.FirstOrDefault(x => x.DocumentType == "insurance") ?? null,
                AverageRating = x.Feedbacks.Select(x => x.CarRating).DefaultIfEmpty(0).Average(),
                BookingCount = x.Bookings.Count(),
            }), message: Messages.Commons.SUCCESS);
        }

        public async Task<Result<IEnumerable<CarInstructorDetailDTO>>> GetInstructorCarsList(Guid id)
        {
            Expression<Func<Car, bool>> filter_expression = x => x.InstructorId == id && !x.IsDeleted;
            Func<IQueryable<Car>, IOrderedQueryable<Car>> order_expression = x => x.OrderBy(u => u.CreatedAt);
            string included_properties = "CarImages";

            var instructor_cars = await _unitOfWork.CarRepository.GetAllAsync(filter: filter_expression, orderBy: order_expression, include_properties: included_properties, disable_tracking: true);


            var carDtos = _mapper.Map<List<CarInstructorDetailDTO>>(instructor_cars);
            return Result<IEnumerable<CarInstructorDetailDTO>>.Success(carDtos, message: Messages.Commons.SUCCESS);
        }

        public async Task<Result<IEnumerable<CarDTO>>> GetRecommendedCarList(int max_count = 5)
        {
            Expression<Func<Car, bool>> filterExpression = x => !x.IsDeleted && x.Feedbacks.Count() > 0;
            string includedProperties = "Manufacturer,Feedbacks,Bookings";

            var cars = await _unitOfWork.CarRepository.GetAllAsync(filter: filterExpression, include_properties: includedProperties);

            var topCars = cars.OrderByDescending(y => y.Bookings.Count()).ThenByDescending(y => y.Feedbacks.Select(x => x.CarRating).DefaultIfEmpty(0).Average()).Take(max_count);

            return Result<IEnumerable<CarDTO>>.Success(topCars.Select(x => new CarDTO
            {
                Id = x.Id,
                ModelName = x.Name,
                ThumbnailUrl = x.ThumbnailUrl,
                ManufacturerName = x.Manufacturer.Name,
                SeatCounts = x.SeatCount,
                Price = x.Price,
                VehicleType = x.CarType,
                FuelType = x.FuelType,
                LicenseTier = x.LicenseTier,
                ManufacturerId = x.ManufacturerId,
                AverageRating = x.Feedbacks.Select(x => x.CarRating).DefaultIfEmpty(0).Average(),
                BookingCount = x.Bookings.Count()
            }));
        }

        public async Task<Result<bool>> ModerateInstructorCar(Guid car_id, string action)
        {
            var car_info = await _unitOfWork.CarRepository.GetByIdAsync(car_id);

            if (car_info == null || car_info.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{car_id}"), Messages.Commons.NOTFOUND);
            }

            if (car_info.Status != Domain.Enum.CarStatus.Pending && car_info.Status != Domain.Enum.CarStatus.ReApply)
            {
                return Result<bool>.Failure(ServiceError.InvalidStateError($"{car_id}"), Messages.Commons.UNHANDLED);
            }

            switch (action)
            {
                case "approve":
                    car_info.Status = Domain.Enum.CarStatus.Approved;
                    break;
                case "decline":
                    car_info.Status = Domain.Enum.CarStatus.Rejected;
                    break;
                default:
                    return Result<bool>.Failure(ServiceError.BadRequestError($"{car_id}"), Messages.Commons.UNHANDLED);
            }

            await _unitOfWork.CommitChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<CarDTO>>> GetAllCarsForPackage(Guid packageId)
        {
            Expression<Func<Car, bool>> filterExpression = x => x.Packages.Any(x => x.Id == packageId) && !x.IsDeleted;
            string includedProperties = "Packages,Manufacturer,Bookings,Feedbacks";

            var packageCars = await _unitOfWork.CarRepository.GetAllAsync(filter: filterExpression, include_properties: includedProperties);

            return Result<IEnumerable<CarDTO>>.Success(packageCars.Select(x => new CarDTO
            {
                Id = x.Id,
                ModelName = x.Name,
                LicenseTier = x.LicenseTier,
                SeatCounts = x.SeatCount,
                ThumbnailUrl = x.ThumbnailUrl,
                Price = x.Price,
                ManufacturerName = x.Manufacturer.Name,
                VehicleType = x.CarType,
                FuelType = x.FuelType,
                AverageRating = x.Feedbacks.Select(x => x.CarRating).DefaultIfEmpty(0).Average(),
                BookingCount = x.Bookings.Count(),
                ManufacturerId = x.ManufacturerId,
            }));
        }

        public async Task<Result<IEnumerable<CarFeedbackDTO>>> GetCarFeedback(Guid car_id)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(car_id, include_properties: "Feedbacks");

            if (car == null || car.IsDeleted)
            {
                return Result<IEnumerable<CarFeedbackDTO>>.Failure(ServiceError.NotFoundError($"{car_id}"), Messages.Commons.NOTFOUND);
            }

            // Fetching users
            var userServiceClient = _http_client_factory.CreateClient("UserServiceClient");
            var responseMessage = await userServiceClient.PostAsJsonAsync("api/users/driver-ids", car.Feedbacks.Select(x => x.NoviceDriverId));
            var users = await responseMessage.Content.ReadFromJsonAsync<DefaultApiResponse<IEnumerable<UserDetailDTO>>>();

            return Result<IEnumerable<CarFeedbackDTO>>.Success(car.Feedbacks.Select(x => new CarFeedbackDTO
            {
                AvatarUrl = users.Value.FirstOrDefault(y => y.NoviceDriver.NoviceDriverId == x.NoviceDriverId)?.AvatarUrl ?? "",
                Username = users.Value.FirstOrDefault(y => y.NoviceDriver.NoviceDriverId == x.NoviceDriverId)?.FullName ?? "Anonymous",
                Rating = x.CarRating,
                Comment = x.CarFeedback,
                FeedbackDate = x.CreatedAt
            }));
        }

        public async Task<Result<IEnumerable<CarDocumentViewDTO>>> GetCarDocuments(Guid car_id)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(car_id);

            if (car == null || car.IsDeleted)
            {
                return Result<IEnumerable<CarDocumentViewDTO>>.Failure(ServiceError.NotFoundError($"{car_id}"), Messages.Commons.NOTFOUND);
            }

            var documents = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CarDocumentViewDTO>>(car.DocumentJsonBlobString);

            return Result<IEnumerable<CarDocumentViewDTO>>.Success(documents);
        }

        public async Task<Result<CarDocumentViewDTO>> GetCarDocument(Guid car_id, string type)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(car_id);

            if (car == null || car.IsDeleted)
            {
                return Result<CarDocumentViewDTO>.Failure(ServiceError.NotFoundError($"{car_id}"), Messages.Commons.NOTFOUND);
            }

            var documents = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<CarDocumentViewDTO>>(car.DocumentJsonBlobString);

            var target_document = documents.FirstOrDefault(x => x.DocumentType.Equals(type, StringComparison.InvariantCultureIgnoreCase));

            if (target_document == null)
            {
                return Result<CarDocumentViewDTO>.Failure(ServiceError.NotFoundError($"{car_id}:{type}"), Messages.Commons.NOTFOUND);
            }

            return Result<CarDocumentViewDTO>.Success(target_document);
        }

        public async Task<Result<bool>> UploadCarDocument(Guid car_id, CarDocumenDTO document)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(car_id);

            if (car == null || car.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{car_id}"), Messages.Commons.NOTFOUND);
            }

            // Deserialize into C# object for manupilation.
            var documents = System.Text.Json.JsonSerializer.Deserialize<IList<CarDocumentViewDTO>>(car.DocumentJsonBlobString) ?? [];

            var target_document = documents.FirstOrDefault(x => x.DocumentType.Equals(document.DocumentType, StringComparison.InvariantCultureIgnoreCase));
            string tempt_name = $"{Guid.NewGuid()}-{document.DocumentType}";

            // New document case
            if (target_document == null)
            {
                documents.Add(new CarDocumentViewDTO
                {
                    FrontImageUrl = document.FrontImage == null ? $"{tempt_name}-front-image" : _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(document.FrontImage, $"{tempt_name}-front-image"),
                    BackImageUrl = document.FrontImage == null ? $"{tempt_name}-back-image" : _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(document.FrontImage, $"{tempt_name}-back-image"),
                    DocumentType = document.DocumentType,
                });
            }
            // Existing document case
            else
            {
                target_document.FrontImageUrl = document.FrontImage == null ? target_document.FrontImageUrl : _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(document.FrontImage, $"{tempt_name}-front-image");
                target_document.BackImageUrl = document.FrontImage == null ? target_document.BackImageUrl : _cloudinary.UploadImageFormFileResourceToCloudinaryWithExactName(document.FrontImage, $"{tempt_name}-back-image");
            }

            // Serialize into JSON string for storing purpose
            car.DocumentJsonBlobString = System.Text.Json.JsonSerializer.Serialize(documents);

            _unitOfWork.CarRepository.Update(car);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteCarDocument(Guid car_id, string type)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(car_id);

            if (car == null || car.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{car_id}"), Messages.Commons.NOTFOUND);
            }

            // Deserialize into C# object for manupilation.
            var documents = System.Text.Json.JsonSerializer.Deserialize<IList<CarDocumentViewDTO>>(car.DocumentJsonBlobString) ?? [];

            var target_document = documents.FirstOrDefault(x => x.DocumentType.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            
            if (target_document == null)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{car_id}:{type}"), Messages.Commons.NOTFOUND);
            }

            documents.Remove(target_document);

            // Serialize into JSON string for storing purpose
            car.DocumentJsonBlobString = System.Text.Json.JsonSerializer.Serialize(documents);

            _unitOfWork.CarRepository.Update(car);
            await _unitOfWork.CommitChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
