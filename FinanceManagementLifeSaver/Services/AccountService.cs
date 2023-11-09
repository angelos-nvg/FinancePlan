using AutoMapper;
using FinanceManagementLifesaver.DTO;
using FinanceManagementLifesaver.Data;
using FinanceManagementLifesaver.Interfaces;
using FinanceManagementLifesaver.Models;
using FinanceManagementLifesaver.ServiceResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManagementLifesaver.DTO.AccountDTO;

namespace FinanceManagementLifesaver.Services
{
	public class AccountService : IAccountService
	{
		private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AccountService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<Account>> CreateAccount(AccountCreateDTO account)
		{
			ServiceResponse<Account> response = new ServiceResponse<Account>();
            Account _account = _mapper.Map<AccountCreateDTO, Account>(account);
            var _user = await _context.Users.FirstOrDefaultAsync(u => u.Id == account.UserId);
            if (_user == null)
            {
                response.Success = false;
                response.Message = "User was not found";
                return response;
            }
            _account.User = _user;
            await _context.Accounts.AddAsync(_account);
			await _context.SaveChangesAsync();
            response.Data = _account;
            return response;
		}

        public async Task<ServiceResponse<AccountDTO>> GetAccountById(int accountId)
        {
			ServiceResponse<AccountDTO> response = new ServiceResponse<AccountDTO>();
            Account account = await _context.Accounts.FirstOrDefaultAsync(u => u.Id == accountId);
            if (account == null)
            {
                response.Success = false;
                return response;
            }
            response.Data = _mapper.Map<Account,AccountDTO>(account);
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<Account>>> GetAccountsByUserId(int userId)
        {
            ServiceResponse<IEnumerable<Account>> response = new ServiceResponse<IEnumerable<Account>>();
            List<Account> accounts = (List<Account>)await _context.Accounts.Where(u => u.User.Id == userId).ToListAsync();
            if (!accounts.Any())
            {
                response.Success = false;
                return response;
            }
            response.Data = accounts;
            return response;
        }

        public async Task<ServiceResponse<Account>> UpdateAccount(AccountUpdateDTO account)
        {
			ServiceResponse<Account> response = new ServiceResponse<Account>();
            Account _account = await _context.Accounts.FirstOrDefaultAsync(u => u.Id == account.Id);
            if (_account == null)
            {
                response.Success = false;
                response.Message = "Account not found";
                return response;
            }
            var _user = await _context.Users.FirstOrDefaultAsync(u => u.Id == account.UserId);
            if (_user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            _account.AccountBalance = account.AccountBalance;
            _account.AccountType = account.AccountType;
            _account.Name = account.Name;
            _account.User = _user;
            _context.Accounts.Update(_account);
            await _context.SaveChangesAsync();
            response.Data = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id);
			return response;
        }

        public async Task<ServiceResponse<Account>> DeleteAccount(int accountId)
		{
			ServiceResponse<Account> response = new ServiceResponse<Account>();
			Account account = await _context.Accounts.FirstOrDefaultAsync(u => u.Id == accountId);
            if (account == null)
            {
                response.Success = false;
                return response;
            }
            List<Transaction> transactions = await _context.Transactions.Where(t => t.Account.Id == account.Id).ToListAsync();
            if (transactions.Count > 0 )
            {
                 _context.Transactions.RemoveRange(transactions);
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            response.Data = account;
			return response;
		}
	}
}