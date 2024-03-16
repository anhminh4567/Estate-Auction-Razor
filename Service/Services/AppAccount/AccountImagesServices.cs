using Repository.Database.Model;
using Repository.Database.Model.AppAccount;
using Repository.Database.Model.RealEstate;
using Repository.Interfaces.AppAccount;
using Repository.Interfaces.DbTransaction;
using Service.Interfaces.AppAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.AppAccount
{
	public class AccountImagesServices : IAccountImagesServices
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly ImageService _imageService;

        public AccountImagesServices(IUnitOfWork unitOfWork, ImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }



        //private readonly IAccountImageRepository _accountImageRepository;
        //private readonly ImageService _imageService;

        //public AccountImagesServices(IAccountImageRepository accountImageRepository, ImageService imageService)
        //{
        //	_accountImageRepository = accountImageRepository;
        //	_imageService = imageService;
        //}
        public async Task<AppImage?> GetAccountAvatar(int accountId)
		{
			var images = await _unitOfWork.Repositories.accountImageRepository.GetAllByAccountId(accountId);
			return images.OrderByDescending(p => p.Image.Name).FirstOrDefault(p => p.Image.Name.ToLower().Contains("avatar"))?.Image;
		}
		public async Task<List<AppImage>?> GetByAccountId(int estateId)
		{
			var getImagesId = (await _unitOfWork.Repositories.accountImageRepository.GetAllByAccountId(estateId))?.Select(e => e.ImageId).ToArray();
			if (getImagesId is null)
				return null;
			return await _imageService.GetRangeImages(getImagesId);
		}
		public async Task<AppImage?> Get(int estateImageId)
		{
			return await _imageService.GetImage(estateImageId);
		}
		public async Task<(bool IsSuccess, AppImage? image, string? result)> Create(int accountId, string savePath,string fileName)
		{
            try
            {
                var trueFileName = _imageService.GenerateFilename(fileName);
                await _unitOfWork.BeginTransaction();
                var image = await _imageService.SaveImage(savePath, trueFileName);
                AccountImages accountImage = new AccountImages()
                {
                    ImageId = image.ImageId,
                    AccountId = accountId
                };
                await _unitOfWork.Repositories.accountImageRepository.CreateAsync(accountImage);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return (true, image, "Success");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return (false, null, ex.Message);
            }
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
