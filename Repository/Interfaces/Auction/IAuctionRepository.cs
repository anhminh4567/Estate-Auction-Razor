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
	}
}
