using Microsoft.AspNetCore.Http;
using Repository.Database.Model;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class ImageService
	{
		private readonly IImagesRepository _imagesRepository;
		private readonly string[] _validFileExtendsion = new []{ "png","jpeg"};
		private readonly string[] _invalidFileCharacter = new[] { "~!@#$%^&*()-=+|\\/<>," }; //allow is _ and .
		public ImageService(IImagesRepository imagesRepository)
		{
			_imagesRepository = imagesRepository;
		}
		
		public async Task<AppImage> GetImage(int imageId) 
		{
			return await _imagesRepository.GetAsync(imageId);
		}
		public async Task<bool> DeleteImage(int imageId)
		{
			return await _imagesRepository.DeleteAsync(await GetImage(imageId));
		}
		public async Task<AppImage> SaveImage(IFormFile formBodyImage, string folderPath ) //folder should be local to app, webroot path or wwwroot path
		{
			var correctFilename = GenerateFilename(formBodyImage.FileName);
			var savePath = folderPath + "\\" + correctFilename;
			using (var s = formBodyImage.OpenReadStream()) 
			{
				using(var fs = new FileStream(savePath, FileMode.Create)) 
				{
					await s.CopyToAsync(fs);
				}
			}
			var saveImageData = new AppImage() { Name = correctFilename, Path = savePath, CreateDate = DateTime.Now };
			return await _imagesRepository.CreateAsync(saveImageData);
		}
		//not test yet
		public string GenerateFilename(string filename) 
		{
			var fileExtendsion = filename.Split('.').Last() ;
			var nameOnly = filename.Substring(0,filename.Length - 1 - ".".Length - fileExtendsion.Length ) ;
			var timeNowAsLong = DateTime.Now.Ticks;
			return nameOnly + "." + timeNowAsLong.ToString() + "." + fileExtendsion; 
		}
		public bool IsImageValid(IFormFile imageFile)
		{
			if (IsFileExtendsionValid(imageFile.FileName) == false) return false;
			if (IsFileNameContainSpecialCharacter(imageFile.FileName)) return false;
			return true;
		}
		private bool IsFileExtendsionValid (string fileName) 
		{
			var extendsion = fileName.Trim().Split('.').Last();
			var isAllowedExtendsion = _validFileExtendsion.Contains(extendsion);
			return isAllowedExtendsion;
		}
		private bool IsFileNameContainSpecialCharacter(string fileName) 
		{
			foreach(char c in fileName.Trim().ToCharArray()) 
			{
				var isContain = _invalidFileCharacter.Contains(c.ToString());
				if (isContain)
					return true;
			}
			return false;
		}

	}
}
