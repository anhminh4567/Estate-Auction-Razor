using Repository.Database.Model;
using Repository.Interfaces.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AppAccount
{
	public class AccountImagesServices
	{
		private readonly IAccountImageRepository _accountImageRepository;
		private readonly ImageService _imageService;

		public AccountImagesServices(IAccountImageRepository accountImageRepository, ImageService imageService)
		{
			_accountImageRepository = accountImageRepository;
			_imageService = imageService;
		}
		public async Task<List<AppImage>?> GetByAccountId(int estateId)
		{
			var getImagesId = (await _accountImageRepository.GetAllByAccountId(estateId))?.Select(e => e.ImageId).ToArray();
			if (getImagesId is null)
				return null;
			return await _imageService.GetRangeImages(getImagesId);
		}
		public async Task<AppImage?> Get(int estateImageId)
		{
			return await _imageService.GetImage(estateImageId);
		}
		public async Task<AppImage?> Create(Stream imageStream, string folderType, string fileName, string folderPath = "wwwroot\\PublicImages")
		{
			return await _imageService.SaveImage(imageStream, folderType, fileName, folderPath);

		}
		public async Task<bool> Update(AppImage image, string wwwroot_publicImage_folder_path)
		{
			return await _imageService.UpdateImage(image, wwwroot_publicImage_folder_path);
		}
		public async Task<bool> Delete(AppImage image, string wwwroot_publicImage_folder_path)
		{
			return await _imageService.RemoveImage(image, wwwroot_publicImage_folder_path);
		}
	}
}
