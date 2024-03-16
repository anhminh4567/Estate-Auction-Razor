using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model;

namespace Service.Interfaces.RealEstate
{
    public interface IEstateImagesServices
    {
        Task<List<AppImage>?> GetByEstateId(int estateId);
        Task<AppImage?> Get(int estateImageId);
        Task<(bool IsSuccess, AppImage? image, string? result)> Create(int estateId, string savePath, string fileName);
        Task<bool> Update(AppImage image, string wwwroot_publicImage_folder_path);
        Task<bool> Delete(AppImage image, string wwwroot_publicImage_folder_path);
    }
}
