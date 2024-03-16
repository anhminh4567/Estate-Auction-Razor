using Org.BouncyCastle.Pqc.Crypto.Falcon;
using Repository.Database.Model.Enum;
using Repository.Database.Model.RealEstate;
using Repository.Interfaces.DbTransaction;
using Repository.Interfaces.RealEstate;
using Service.Interfaces.RealEstate;
using Service.Services.Auction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service.Services.RealEstate
{
	public class EstateServices 
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly AuctionServices _auctionServices;

		public EstateServices(IUnitOfWork unitOfWork, AuctionServices auctionServices)
		{
			_unitOfWork = unitOfWork;
			_auctionServices = auctionServices;
		}


		//private readonly IEstateRepository _estateRepository;

		//public EstateServices(IEstateRepository estateRepository)
		//{
		//	_estateRepository = estateRepository;
		//}
		public async Task<Estate?> GetById(int id, string includeProperties = null) 
		{
            if(includeProperties == null)
			    return await _unitOfWork.Repositories.estateRepository.GetAsync(id);
            return await GetWithCondition(id, includeProperties);
        }
		public async Task<Estate?> GetFullDetail(int id) 
		{
			return await _unitOfWork.Repositories.estateRepository.GetFullDetail(id);
		}
        public async Task<Estate?> GetIncludes(int id, params string[] attributeName)
        {
            return await _unitOfWork.Repositories.estateRepository.GetInclude(id,attributeName);
        }
        public async Task<List<Estate>> GetAllDetails()
        {
            return await _unitOfWork.Repositories.estateRepository.GetAllDetails();
        }
        public async Task<Estate?> GetWithCondition(int id, string properties)
        {
            return (await _unitOfWork.Repositories.estateRepository.GetByCondition(e => e.EstateId == id,includeProperties: properties)).FirstOrDefault();
        }
        public async Task<List<Estate>> GetByCompanyId(int companyId) 
		{
			return await _unitOfWork.Repositories.estateRepository.GetByCompanyId(companyId);
		}
		public async Task<List<Estate>> GetAll()
		{
			return await _unitOfWork.Repositories.estateRepository.GetAllAsync();
		}
		public async Task<(bool IsSuccess, string? message, Estate? NewEstate)> Create(Estate estate, List<string> SelectedEstateCategoriesOptions)
		{
            try
            {
                await _unitOfWork.BeginTransaction();
                var createEstate = await _unitOfWork.Repositories.estateRepository.CreateAsync(estate);
                //await _unitOfWork.RollBackAsync();
                if(createEstate is null)
                {
                    await _unitOfWork.RollBackAsync();
                    throw new Exception("cannot create, someting wrong");
                }
                foreach (var item in SelectedEstateCategoriesOptions)
                {
                    if (item == null)
                        continue;
                    var newCategories = new EstateCategories()
                    {
                        CategoryId = int.Parse(item),
                        EstateId = createEstate.EstateId,
                    };
                    var estateCategoriesResult = await _unitOfWork.Repositories.estateCategoriesRepository.CreateAsync(newCategories);
                    if (estateCategoriesResult is null)
                    {
                        await _unitOfWork.RollBackAsync();
                        return (false, "fail to create estate category, rollback",null);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return (true, "Success", createEstate);
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollBackAsync();
                return (false, ex.Message, null);
            }



            //return await _unitOfWork.Repositories.estateRepository.CreateAsync(estate);
		}
		public async Task<(bool IsSuccess, string message)> UpdateEstates(Estate estate,  List<int> SelectedCategories)
		{
			try
			{
				await _unitOfWork.BeginTransaction();
                var currentEstateCategories = await _unitOfWork.Repositories.estateCategoriesRepository.GetByCondition(c => c.EstateId == estate.EstateId);
                List<EstateCategories> returnSelectedCategories = new List<EstateCategories>();
                foreach (var sel in SelectedCategories)
                {
                    returnSelectedCategories.Add(new EstateCategories() { CategoryId = sel, EstateId = estate.EstateId });
                }
                var result = await _unitOfWork.Repositories.estateCategoriesRepository.DeleteRange(currentEstateCategories);
                foreach (var sel in returnSelectedCategories)
                {
                    var addResult = await _unitOfWork.Repositories.estateCategoriesRepository.CreateAsync(sel);
                }
                var updateResult = await _unitOfWork.Repositories.estateRepository.UpdateAsync(estate);// _estateService.Update(Estate);
                if (updateResult)
                {
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                    return (true, "success");
                }
                throw new Exception("fail to update");
				//return await _unitOfWork.Repositories.estateRepository.UpdateAsync(estate);
            }
            catch (Exception ex) 
			{
                await _unitOfWork.RollBackAsync();
				return (false,ex.Message);
			}
			
		}
		public async Task<(bool IsSuccess, string message)> DeleteEstate(Estate estate)
		{
            var isDeletable = true;
            if (estate.Status.Equals(EstateStatus.REMOVED) 
                || estate.Status.Equals(EstateStatus.BANNED) 
                || estate.Status.Equals(EstateStatus.FINISHED))
            {
                return (false, "cannot delete, estate is already " + estate.Status.ToString());
            }
            var getAuctions = await  _unitOfWork.Repositories.auctionRepository.GetByCondition(a => a.EstateId == estate.EstateId);
            try
            {
                await _unitOfWork.BeginTransaction();
                if (getAuctions is not null)
                {
                    foreach (var auction in getAuctions)
                    {
                        if (auction.Status.Equals(AuctionStatus.SUCCESS) ||
                            auction.Status.Equals(AuctionStatus.ONGOING) ||
                            auction.Status.Equals(AuctionStatus.PENDING_PAYMENT))
                        {
                            isDeletable = false;
                            //ModelState.AddModelError(string.Empty, "cannot delete, an auction is " + auction.Status.ToString());
                            break;
                        }
                    }
                }
                if (isDeletable)
                {
                    estate.Status = Repository.Database.Model.Enum.EstateStatus.REMOVED;
                    var result = await _unitOfWork.Repositories.estateRepository.UpdateAsync(estate);
                    if (result is false)
                    {
                        await _unitOfWork.RollBackAsync();
                        throw new Exception("something wrong when delete, it result in false");
                    }
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                    return (true, "Success");
                }
                throw new Exception("Fail To Update");
            }
            catch  (Exception ex) 
            {
                await _unitOfWork.RollBackAsync();
                return (false, ex.Message);                
            }
            
			
		}
		public async Task<(bool IsSuccess, string? message)> AdminBannedEstate(Estate estate) 
		{
            try
            {
                var getAuctions = await _unitOfWork.Repositories.auctionRepository.GetByEstateId(estate.EstateId);
                // lay auction cua estate cos status la ( NOT_STARTED , ONGOING, PENDING_PAYMENT )
                var getCurrentValidAuction = getAuctions.Where(
                    a => a.Status != AuctionStatus.SUCCESS &&
                    a.Status != AuctionStatus.FAILED_TO_PAY &&
                    a.Status != AuctionStatus.CANCELLED).FirstOrDefault();// this is because at one time only 1 aution valid is happening
                
                //await _unitOfWork.BeginTransaction();
                
                if(getCurrentValidAuction is not null)
                {
                    // truoc khi bann, phai cancel auction do, neu ko duoc thi ko dc bann 
					var result = await _auctionServices.AdminForceCancelAuction(getCurrentValidAuction);
                    if(result.IsSuccess == false ) 
                    {
                        throw new Exception("cannot cancel auction, so estate cannot be banned also");
                    }
                }
                // bat dau bann
				//estate.Status = EstateStatus.BANNED;
				//await _unitOfWork.Repositories.estateRepository.UpdateAsync(estate);
                //await _unitOfWork.SaveChangesAsync();
                //await _unitOfWork.CommitAsync();
                var deleteResult = await DeleteEstate(estate);
                return deleteResult;
                return (true, "Success");
            }
            catch (Exception ex) 
            {
                //await _unitOfWork.RollBackAsync();
                return (false, ex.Message);
            }
		}
	}
}
