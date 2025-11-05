using GymManagementBLL.ViewModels.AnalyticsViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface IAnalyticsService
    {
        AnalyticsViewModel GetAnalyticsData();
    }
}
