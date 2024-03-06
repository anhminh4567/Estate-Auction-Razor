using Repository.Database.Model.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Database.Model.AppAccount
{
    public class Company : Account
    {
        public Company() : base() 
        {
        }
        public Company(Account acc) : base() 
        {
            Email = acc.Email;
            Password = acc.Password;
            FullName = acc.FullName;
            Dob = acc.Dob;
            Telephone = acc.Telephone;
            Status = Enum.AccountStatus.ACTIVED;
            Role = Enum.Role.COMPANY;
            CMND = acc.CMND;
            Balance = 0;
        }
        public Company(bool initBase)
        {
            if (initBase)
            {
                base.CMND = "0";
                base.Role = Enum.Role.COMPANY;
                base.Balance = 0;
                base.Status = Enum.AccountStatus.ACTIVED;
            }
        }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime EstablishDate { get; set; }
        public DateTime DateStartOnPlatform { get; set; } = DateTime.Now.Date;
		public IList<Estate> OwnedEstate { get; set; }
    }
}
