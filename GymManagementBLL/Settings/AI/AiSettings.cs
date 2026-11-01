namespace GymManagementBLL.Settings.AI
{
    public class AiSettings
    {
        public const string SectionName = "AI";
        public string EndPoint { get; set; } = string.Empty;
        public string ModelId { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
