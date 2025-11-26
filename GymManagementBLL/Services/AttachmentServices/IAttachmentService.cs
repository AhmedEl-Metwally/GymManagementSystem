using Microsoft.AspNetCore.Http;
namespace GymManagementBLL.Services.AttachmentServices
{
    public interface IAttachmentService
    {
        string? Upload(string folderName, IFormFile file);
        bool Delete(string fileName, string folderName );
    }
}
