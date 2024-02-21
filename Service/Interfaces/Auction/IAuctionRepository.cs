using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Database.Model;
namespace Service.Interfaces.Auction
{
	public interface IAuctionRepository : ICrud<Repository.Database.Model.AuctionRelated.Auction>
	{
	}
}
