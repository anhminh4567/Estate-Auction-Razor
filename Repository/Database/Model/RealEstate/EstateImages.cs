using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.RealEstate
{
    public class EstateImages
    {
        public int EstateId { get; set; }
        public Estate Estate { get; set; }
        public int ImageId { get; set; }    
        public AppImage Image { get; set; }
    }
}
