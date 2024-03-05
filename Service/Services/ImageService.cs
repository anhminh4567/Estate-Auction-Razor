using Microsoft.AspNetCore.Http;
using Repository.Database.Model;
using Repository.Interfaces;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class ImageService
	{
		private readonly IImagesRepository _imagesRepository;
		private readonly string[] _validFileExtendsion = new[] { "png", "jpeg" };
		private readonly string[] _invalidFileCharacter = new[] { "~!@#$%^&*()-=+|\\/<>," }; //allow is _ and .
		public ImageService(IImagesRepository imagesRepository)
		{
			_imagesRepository = imagesRepository;
		}

		public async Task<AppImage?> GetImage(int imageId)
		{
			return await _imagesRepository.GetAsync(imageId);
		}
		public async Task<List<AppImage>?> GetRangeImages(params int[] imagesId) 
		{
			return await _imagesRepository.GetRange(imagesId);
		}
		public async Task<bool> RemoveImage(AppImage image, string wwwroot_publicImage_folder_path) 
		{
			var imagePath = image.Path;
			if (IsFileExist(wwwroot_publicImage_folder_path, imagePath) == false)
				return false;
			var result = await _imagesRepository.DeleteAsync(image);
			if (result) 
			{
				//TODO delete image trong file wwwroot/publicimages
				return true;
			}
			return false;

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="image"></param>
		/// <param name="pathToWWWROOT"> Path nay ko the lay duoc tu day, vi no se khac nhau moi may nen 
		///	la no se duoc pass tu thang razorWebapp xuong day, va lay path nay bang cach Lat WWWRoot path, pass IWebHostEnvironment vo trong constructor cua razor page
		/// </param>
		/// <returns></returns>
		public async Task<bool> UpdateImage(AppImage image, string wwwroot_publicImage_folder_path) 
		{
			var imagePath = image.Path;
			if(IsFileExist(wwwroot_publicImage_folder_path,imagePath) == false)
				return false;
			var result = await _imagesRepository.UpdateAsync(image);
			if (result)
			{
				//TODO update image trong file wwwroot/publicimages
				return true;
			}
			return false;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="imageStream">lay tu IFormFile.OpenReadStream() , nho dispose() sau khi dung </param>
		/// <param name="folderType"> AccountImage || EstateImage, ... kieu vay </param>
		/// <param name="fileName">ten file</param>
		/// <returns></returns>
		//, string folderPath = "wwwroot\\PublicImages"
		public async Task<AppImage?> SaveImage(string rootPath, string savePath, string fileName) //folder should be local to app, webroot path or wwwroot path
		{
			/*using (var fs = new FileStream(Path.Combine(rootPath,savePath,correctFilename), FileMode.Create))
			{
				await imageStream.CopyToAsync(fs);
			}*/
			var saveImageData = new AppImage() { 
				Name = fileName, 
				Path = Path.Combine(savePath, fileName), 
				CreateDate = DateTime.Now 
			};
			var result =  await _imagesRepository.CreateAsync(saveImageData);
			if (result is not null) 
			{

				return saveImageData;
				//TODO add image vo trong file wwwroot/PublicImages
			}
			return null;
		}
		//not test yet
		public string GenerateFilename(string filename)
		{
			var fileExtendsion = filename.Split('.').Last();
			var nameOnly = filename.Split('.').First();
			var timeNowAsLong = DateTime.Now.Ticks;
			return nameOnly + "." + timeNowAsLong.ToString() + "." + fileExtendsion;
		}
		public bool IsImageValid(IFormFile imageFile)
		{
			if (IsFileExtendsionValid(imageFile.FileName) == false) return false;
			if (IsFileNameContainSpecialCharacter(imageFile.FileName)) return false;
			return true;
		}
		private bool IsFileExtendsionValid(string fileName)
		{
			var extendsion = fileName.Trim().Split('.').Last();
			var isAllowedExtendsion = _validFileExtendsion.Contains(extendsion);
			return isAllowedExtendsion;
		}
		private bool IsFileNameContainSpecialCharacter(string fileName)
		{
			foreach (char c in fileName.Trim().ToCharArray())
			{
				var isContain = _invalidFileCharacter.Contains(c.ToString());
				if (isContain)
					return true;
			}
			return false;
		}
		private bool IsFileExist(string wwwroot_publicImage_folder_path,string imagePath) 
		{
			return File.Exists(wwwroot_publicImage_folder_path + "\\" + imagePath);
		}
	}
}
