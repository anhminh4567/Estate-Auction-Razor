using Repository.Database.Model;
using Repository.Database.Model.RealEstate;
using Repository.Interfaces.DbTransaction;
using Repository.Interfaces.RealEstate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.RealEstate
{
	public class EstateImagesServices
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly ImageService _imageService;

        public EstateImagesServices(IUnitOfWork unitOfWork, ImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }


        //private readonly IEstateImagesRepository _estateImagesRepository;
        //private readonly ImageService _imageService;	

        //public EstateImagesServices(IEstateImagesRepository estateImagesRepository, ImageService imageService)
        //{
        //	_estateImagesRepository = estateImagesRepository;
        //	_imageService = imageService;
        //}
        public async Task<List<AppImage>?> GetByEstateId(int estateId) 
		{
			var getImagesId = ( await _unitOfWork.Repositories.estateImagesRepository.GetByEstateId(estateId))?.Select(e => e.ImageId).ToArray();
			if (getImagesId is null)
				return null;
			return await _imageService.GetRangeImages(getImagesId);
		}
		public async Task<AppImage?> Get(int estateImageId) 
		{
			return await _imageService.GetImage(estateImageId);
		}
		public async Task<(bool IsSuccess, AppImage? image, string? result)> Create(int estateId, string savePath, string fileName) 
		{
			try
			{
                var trueFileName = _imageService.GenerateFilename(fileName);
				await _unitOfWork.BeginTransaction();
				var image = await _imageService.SaveImage(savePath, trueFileName);
                EstateImages estateImages = new EstateImages()
                {
                    ImageId = image.ImageId,
                    EstateId = estateId,
                };
                await _unitOfWork.Repositories.estateImagesRepository.CreateAsync(estateImages);
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
			return await _imageService.UpdateImage(image,wwwroot_publicImage_folder_path);
		}
		public async Task<bool> Delete(AppImage image, string wwwroot_publicImage_folder_path) 
		{
			return await _imageService.RemoveImage(image, wwwroot_publicImage_folder_path);
		}

	}

}
