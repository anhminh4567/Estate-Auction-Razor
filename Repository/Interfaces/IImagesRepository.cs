﻿using Repository.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
	public interface IImagesRepository : ICrud<AppImage>
	{
		Task<List<AppImage>?> GetRange(params int[] imagesId);	
	}
}
