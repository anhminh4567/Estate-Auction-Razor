using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Database.Model.RealEstate
{
    public class EstateCategories
    {
        public int EstateId { get; set; }
        public Estate Estate { get; set; }
        public int CategoryId { get; set; }
        public EstateCategoryDetail CategoryDetail { get; set; }
       
    }
}