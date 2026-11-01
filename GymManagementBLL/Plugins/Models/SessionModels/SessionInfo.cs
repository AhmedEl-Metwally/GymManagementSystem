namespace GymManagementBLL.Plugins.Models.SessionModels
{
    public record SessionInfo(string Category, string Trainer, string Date, string Start, string End, string Status, int Capacity, int AvailableSlots);

}
