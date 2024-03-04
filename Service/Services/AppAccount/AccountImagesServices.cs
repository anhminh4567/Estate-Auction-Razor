using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
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
		public async Task<AppImage?> GetAccountAvatar(int accountId)
		{
			var images = await _accountImageRepository.GetAllByAccountId(accountId);
			return images.OrderByDescending(p => p.Image.Name).FirstOrDefault(p => p.Image.Name.ToLower().Contains("avatar"))?.Image;
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
		public async Task<AppImage?> Create(int accountId, string rootPath, string Savepath,string fileName)
		{
			var trueFileName = _imageService.GenerateFilename(fileName);
			var image = await _imageService.SaveImage(rootPath, Savepath, trueFileName);
			if(image is not null)
			{
                AccountImages accountImage = new AccountImages()
                {
                    ImageId = image.ImageId,
					AccountId = accountId
				};
				var aImage = await _accountImageRepository.CreateAsync(accountImage);
				return image;
            }
			else return null;
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
