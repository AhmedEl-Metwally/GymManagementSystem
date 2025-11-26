using GymManagementBLL.SettingsOfAttachment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Services.AttachmentServices
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IWebHostEnvironment _webHost;
        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if(folderName is null || file is null || file.Length == 0)
                    return null;
                if(file.Length > FileSettings.MaxFileSizeInBytes)
                    return null;

                var allowedExtensions = Path.GetExtension(file.FileName).ToLower();
                var allowedExtensionsArray = FileSettings.AllowedExtensions.Split(',');
                if(!allowedExtensionsArray.Contains(allowedExtensions))
                    return null;

                var relativeFolderPath = FileSettings.ImagesPath;
                var uploadsFolder = Path.Combine(_webHost.WebRootPath, relativeFolderPath.TrimStart('/'));
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + allowedExtensions;
                var filePath = Path.Combine(uploadsFolder, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                return Path.Combine(relativeFolderPath, fileName).Replace('\\', '/');
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return false;
                var relativeFolderPath = FileSettings.ImagesPath;
                var fullPath = Path.Combine(_webHost.WebRootPath, relativeFolderPath.TrimStart('/'), Path.GetFileName(fileName));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
