using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model;

namespace Service.Interfaces
{
    public interface IImageService
    {
        Task<AppImage?> GetImage(int imageId);
        Task<List<AppImage>?> GetRangeImages(params int[] imagesId);
        Task<bool> RemoveImage(AppImage image, string wwwroot_publicImage_folder_path);
        Task<bool> UpdateImage(AppImage image, string wwwroot_publicImage_folder_path);
        Task<AppImage?> SaveImage(string savePath, string fileName);
    }
}
