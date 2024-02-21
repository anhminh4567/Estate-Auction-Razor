﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.AppAccount
{
    public class AccountImages
    {
        public int ImageId { get; set; } 
        public AppImage Image { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
