using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.AppAccount
{
    public class Company : Account
    {
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime EstablishDate { get; set; }
        public DateTime DateStartOnPlatform { get; set; }
        public IList<Estate> OwnedEstate { get; set; }
    }
}
