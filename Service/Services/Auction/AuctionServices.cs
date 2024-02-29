using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using Repository.Interfaces.RealEstate;
using Service.Services.AppAccount;
using Service.Services.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auction
{
    public class AuctionServices
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly EstateServices _estateServices;
        private readonly EstateCategoriesServices _estateCategoriesServices;
        private readonly JoinedAuctionServices _joinedAuctionServices;
        public AuctionServices(
            IAuctionRepository auctionRepository,
            EstateServices estateService,
            EstateCategoriesServices estateCategoriesServices,
            JoinedAuctionServices joinedAuctionServices
            )
        {
            _auctionRepository = auctionRepository;
            _estateServices = estateService;
            _estateCategoriesServices = estateCategoriesServices;
            _joinedAuctionServices = joinedAuctionServices;
        }
        public async Task<bool> CheckForJoinedAuction(int accountId, int auctionId)
        {
            return await _joinedAuctionServices.IsJoined(accountId, auctionId);
        }
        public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetActiveAuctions()
        {
            return await _auctionRepository.GetActiveAuctions();
        }

        public async Task<Repository.Database.Model.AuctionRelated.Auction> GetById(int id)
        {
            return await _auctionRepository.GetAsync(id);
        }
        public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetByCompanyId(int companyId)
        {
            var getEstates = (await _estateServices.GetByCompanyId(companyId)).Select(e => e.EstateId).ToArray();
            if (getEstates is null)
                throw new Exception("something wrong with get estate, it should never be null, only empty list or so");
            var result = new List<Repository.Database.Model.AuctionRelated.Auction>();
            foreach (var estate in getEstates)
            {
                var tryGetAuctions = await _auctionRepository.GetByEstateId(estate);
                if (tryGetAuctions is not null)
                    result = result.Concat(tryGetAuctions).ToList();
            }
            return result;
        }
        public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetAll()
        {
            return await _auctionRepository.GetAllAsync();
        }
        public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetRange(int start, int amount)
        {
            if (start < 0 || amount <= 0)
                throw new ArgumentException("start, amount must > 0");
            return await _auctionRepository.GetRange(start, amount);
        }
        public async Task<Repository.Database.Model.AuctionRelated.Auction?> GetInclude(int auctionId, string includes)
        {
            return (await _auctionRepository.GetByCondition(a => a.AuctionId == auctionId, includeProperties: includes)).FirstOrDefault();
        }
        public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetRangeInclude_Estate_Company(int start, int amount)
        {
            if (start < 0 || amount <= 0)
                throw new ArgumentException("start, amount must > 0");
            var result = await _auctionRepository.GetRange_IncludeEstate_Company(start, amount);
            foreach (var auction in result)
            {
                var estateId = auction.EstateId;
                auction.Estate.EstateCategory = await _estateCategoriesServices.GetEstateCategoriesByEstateId(estateId);
            }
            return result;
        }
        public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetByEstateId(int estateId)
        {
            return await _auctionRepository.GetByEstateId(estateId);
        }
        public async Task<List<Repository.Database.Model.AuctionRelated.Auction>> GetWithCondition(
            Expression<Func<Repository.Database.Model.AuctionRelated.Auction, bool>> expression = null,
            Func<IQueryable<Repository.Database.Model.AuctionRelated.Auction>,IOrderedQueryable<Repository.Database.Model.AuctionRelated.Auction>> orderBy = null,
            string includeProperties = "")
        {
            return await _auctionRepository.GetByCondition(expression,orderBy,includeProperties);
        }
        public async Task<Repository.Database.Model.AuctionRelated.Auction?> Create(Repository.Database.Model.AuctionRelated.Auction auction)
        {
            var getEstate = await _estateServices.GetById(auction.EstateId);
            if (getEstate is not null)
            {
                var tryGetEstateAuctionStatus = await _auctionRepository.GetByEstateId(getEstate.EstateId);
                foreach (var auc in tryGetEstateAuctionStatus)
                {
                    if (auction.Status != Repository.Database.Model.Enum.AuctionStatus.CANCELLED) // nghia la auction cho mieng dat nay chua bi huy, neu v thi ko duoc tao auction moi cho no
                        throw new Exception("error in creeate auction, this is because the estate you select has already been in auction");
                }
            }
            return await _auctionRepository.CreateAsync(auction);
        }
        public async Task<bool> Delete(Repository.Database.Model.AuctionRelated.Auction auction)
        {
            auction.Status = Repository.Database.Model.Enum.AuctionStatus.CANCELLED;
            return await _auctionRepository.UpdateAsync(auction);
        }
        public async Task<bool> Update(Repository.Database.Model.AuctionRelated.Auction auction)
        {
            return await _auctionRepository.UpdateAsync(auction);
        }
    }
}

