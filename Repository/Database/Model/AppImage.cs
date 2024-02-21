using Repository.Database.Model.AppAccount;
using Repository.Database.Model.RealEstate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Database.Model
{
    public class AppImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string Path { get; set; } 
        public IList<AccountImages> AccountImages { get; set; }
        public IList<EstateImages> EstateImages { get; set; }
    }
}