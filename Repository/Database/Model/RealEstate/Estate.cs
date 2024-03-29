﻿using Repository.Database.Model.AppAccount;
using Repository.Database.Model.AuctionRelated;
using Repository.Database.Model.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Repository.Database.Model.RealEstate
{
    public class Estate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EstateId { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public IList<EstateCategories>? EstateCategory { get; set; }
        public IList<Auction>? Auctions { get; set; }
        public IList<EstateImages>? Images { get; set; }
        public EstateStatus Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Area { get; set; }
        public int Floor { get; set; }
        public string Location { get; set; }
        public string Coordinate { get; set; }
        
    }
}
