using Repository.Database.Model;
using Repository.Database.Model.RealEstate;
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
		private readonly IEstateImagesRepository _estateImagesRepository;
		private readonly ImageService _imageService;	

		public EstateImagesServices(IEstateImagesRepository estateImagesRepository, ImageService imageService)
		{
			_estateImagesRepository = estateImagesRepository;
			_imageService = imageService;
		}
		public async Task<List<AppImage>?> GetByEstateId(int estateId) 
		{
			var getImagesId = ( await _estateImagesRepository.GetByEstateId(estateId))?.Select(e => e.ImageId).ToArray();
			if (getImagesId is null)
				return null;
			return await _imageService.GetRangeImages(getImagesId);
		}
		public async Task<AppImage?> Get(int estateImageId) 
		{
			return await _imageService.GetImage(estateImageId);
		}
		public async Task<AppImage?> Create(string folderType, string fileName,string folderPath = "wwwroot\\PublicImages") 
		{
			return await _imageService.SaveImage(folderType,fileName,folderPath);	

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
