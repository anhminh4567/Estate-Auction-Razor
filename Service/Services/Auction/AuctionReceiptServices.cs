﻿using Repository.Database.Model.AuctionRelated;
using Repository.Interfaces.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auction
{
	public  class AuctionReceiptServices
	{
		private readonly IAuctionReceiptRepository _auctionReceiptRepository;

		public AuctionReceiptServices(IAuctionReceiptRepository auctionReceiptRepository)
		{
			_auctionReceiptRepository = auctionReceiptRepository;
		}

		public async Task<AuctionReceipt?> GetById(int id, string includeProperties = null)
		{
			if (includeProperties is not null)
				return await GetIncludes(id, includeProperties);
			return await _auctionReceiptRepository.GetAsync(id);
		}
		public async Task<AuctionReceipt?> GetByAuctionId(int auctionId, string includeProperties = null) 
		{
			if(auctionId == 0) 
				throw new ArgumentNullException("null argument");
			if(includeProperties == null)
				return ( await _auctionReceiptRepository.GetByCondition(a => a.AuctionId == auctionId) ).FirstOrDefault();
			return (await _auctionReceiptRepository.GetByCondition(a => a.AuctionId == auctionId,includeProperties: includeProperties)).FirstOrDefault();
		}
		public async Task<AuctionReceipt?> GetIncludes(int id, string includeProperties)
		{
			return (await _auctionReceiptRepository.GetByCondition(a => a.ReceiptId == id, includeProperties: includeProperties)).FirstOrDefault() ;
		}
		public async Task<List<AuctionReceipt>> GetWithCondition(Expression<Func<AuctionReceipt, bool>> expression = null,
            Func<IQueryable<AuctionReceipt>, IOrderedQueryable<AuctionReceipt>> orderBy = null,
            string includeProperties = "") 
		{
			return await _auctionReceiptRepository.GetByCondition(expression ,orderBy,includeProperties);
		}
		public async Task<List<AuctionReceipt>> GetAll()
		{
			return await _auctionReceiptRepository.GetAllAsync();
		}
		public async Task<AuctionReceipt> Create(AuctionReceipt auctionReceipt)
		{
			return await _auctionReceiptRepository.CreateAsync(auctionReceipt);
		}
		public async Task<bool> Update(AuctionReceipt auctionReceipt)
		{
			return await _auctionReceiptRepository.UpdateAsync(auctionReceipt);
		}
		public async Task<bool> Delete(AuctionReceipt auctionReceipt)
		{
			return await _auctionReceiptRepository.DeleteAsync(auctionReceipt);
		}
	}
}
