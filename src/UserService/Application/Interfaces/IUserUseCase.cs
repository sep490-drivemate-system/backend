using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;
using UserService.Application.Commons.DTOs.Users;

namespace UserService.Application.Interfaces
{
    public interface IUserUseCase
    {
        #region Users
        Task<Result<bool>> CreateDefaultUserAccount(UserCreationDTO user_information);
        Task<Result<PaginatedList<UserDetailDTO>>> GetAllUser(UserFilterDTO filter);
        Task<Result<UserDetailDTO>> GetUser(Guid userId);
        Task<Result<bool>> UpdatePersonalProfile(Guid id, UserProfileUpdateDTO user_profile);
        #endregion

        #region User contacts
        Task<Result<IEnumerable<EmergencyContactDTO>>> GetUserEmergencyContacts(Guid user_id);
        Task<Result<bool>> CreateUserEmergencyContacts(Guid user_id, EmergencyContactDTO emergency_contact);
        #endregion

        #region User Saved Addess
        Task<Result<IEnumerable<UserAddressDTO>>> GetUserSavedAddress(Guid user_id);
        Task<Result<bool>> CreateUserSavedAddress(Guid user_id, UserAddressDTO user_address);
        #endregion

        #region methods for other services
        Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithUserId(IEnumerable<Guid> user_ids);
        Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithNoviceDriverId(IEnumerable<Guid> driver_ids);
        Task<Result<IEnumerable<UserDetailDTO>>> GetUserWithInstructorId(IEnumerable<Guid> instructor_ids);
        Task<Dictionary<Guid, InstructorBasicInfoDTO>> GetBatchInstructorBasicInfo(List<Guid> instructorIds);
        Task<Dictionary<Guid, NoviceDriverBasicInfoDTO>> GetBatchNoviceDriverBasicInfo(List<Guid> noviceDriverIds);
        #endregion

        #region Statistic
        Task<Result<UserStatisticDTO>> GetUsersStatistic(UserStatisticFilterDTO filter);
        #endregion
    }
}
