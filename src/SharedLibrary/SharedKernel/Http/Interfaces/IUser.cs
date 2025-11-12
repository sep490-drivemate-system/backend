using SharedLibrary.SharedKernel.Http.DTOs.User;

namespace SharedLibrary.SharedKernel.Http.Interfaces
{
    public interface IUser
    {
        Task<Dictionary<Guid, InstructorBasicInfoDTO>> GetBatchInstructorInfo(List<Guid> instructorIds);
        Task<Dictionary<Guid, NoviceDriverBasicInfoDTO>> GetBatchNoviceDriverInfo(List<Guid> noviceDriverIds);
    }
}
