using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model;

namespace Repository.Interfaces.Auction
{
	public interface IAuctionRepository : ICrud<Database.Model.AuctionRelated.Auction>
	{
		Task<Database.Model.AuctionRelated.Auction?> GetFullAsync(int id);
		Task<List<Database.Model.AuctionRelated.Auction>?> GetByEstateId(int id);
	}
}
