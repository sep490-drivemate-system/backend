using ResourceService.Application.Interfaces.Services;

namespace ResourceService.Application.Interfaces
{
    public interface IApplicationServiceProvider
    {
        IResourcesService ResourcesService { get; }
        IQuizService QuizService { get; }
        IVoucherService VoucherService { get; }
    }
}
