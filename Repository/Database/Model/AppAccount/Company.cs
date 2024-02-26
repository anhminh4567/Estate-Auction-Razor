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
            Status = acc.Status;
            IsVerified = acc.IsVerified;
            Role = acc.Role;
            CMND = acc.CMND;
            Balance = acc.Balance;
        }
        public Company(bool initBase)
        {
            if (initBase)
            {
                base.Role = Enum.Role.COMPANY;
                base.Balance = 0;
                base.IsVerified = 1;
                base.Status = Enum.AccountStatus.PENDING;
            }
        }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime EstablishDate { get; set; }
        public DateTime DateStartOnPlatform { get; set; }
        public IList<Estate> OwnedEstate { get; set; }
    }
}
