using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Users;

namespace UserService.Application.Interfaces
{
    public interface IUserUseCase
    {
        Task<Result<bool>> CreateDefaultUserAccount(UserCreationDTO user_information);
        Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithUserId(IEnumerable<Guid> user_ids);
        Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithNoviceDriverId(IEnumerable<Guid> driver_ids);
        Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithInstructorId(IEnumerable<Guid> instructor_ids);
        Task<Result<IEnumerable<UserAddressDTO>>> GetUserSavedAddress(Guid user_id);
        Task<Result<IEnumerable<EmergencyContactDTO>>> GetUserEmergencyContacts(Guid user_id);
        Task<Result<bool>> CreateUserEmergencyContacts(Guid user_id, EmergencyContactDTO emergency_contact);
        Task<Result<bool>> CreateUserSavedAddress(Guid user_id, UserAddressDTO user_address);
        Task<Result<bool>> AddUserSavedAddress(Guid user_id, UserAddressDTO address);
        Task<Result<bool>> AddUserEmergencyContacts(Guid user_id, EmergencyContactDTO contact);
        Task<Dictionary<Guid, InstructorBasicInfoDTO>> GetBatchInstructorBasicInfo(List<Guid> instructorIds);
        Task<Dictionary<Guid, NoviceDriverBasicInfoDTO>> GetBatchNoviceDriverBasicInfo(List<Guid> noviceDriverIds);
        Task<Result<UserStatisticDTO>> GetUsersStatistic(UserStatisticFilterDTO filter);
    }
}
