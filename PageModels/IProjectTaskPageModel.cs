using CommunityToolkit.Mvvm.Input;
using DeteksiBanjir.Models;

namespace DeteksiBanjir.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}