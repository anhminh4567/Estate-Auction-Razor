using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model;

namespace Service.Interfaces.AppAccount
{
    public interface IAccountImagesServices
    {
        Task<AppImage?> GetAccountAvatar(int accountId);
        Task<List<AppImage>?> GetByAccountId(int estateId);
        Task<AppImage?> Get(int estateImageId);
        Task<(bool IsSuccess, AppImage? image, string? result)> Create(int accountId, string savePath, string fileName);
        Task<bool> Update(AppImage image, string wwwroot_publicImage_folder_path);
        Task<bool> Delete(AppImage image, string wwwroot_publicImage_folder_path);

    }
}
