using Application.Data;
using Application.Domain.Users;
using Application.Services.Cache;
using Core.Services.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.User
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IRepository<UserAccount> _repository;
        private readonly ICacheManagerService _cacheManager;
        private readonly LocalDbContext _context;
        private readonly ISessionManagerService _sessionManager;

        private const int CacheDurationSeconds = 3600;

        public UserAccountService(
            IRepository<UserAccount> repository,
            ICacheManagerService cacheManager,
            LocalDbContext context,
            ISessionManagerService sessionManager)
        {
            _repository = repository;
            _cacheManager = cacheManager;
            _context = context;
            _sessionManager = sessionManager;
        }

        public async Task Add(UserAccount userAccount)
        {
            userAccount.DateCreated = DateTime.UtcNow;
            userAccount.LastUpdated = DateTime.UtcNow;

            await _repository.AddAsync(userAccount);
            RemoveCache(userAccount.EmailAddress);
        }

        public async Task<UserAccount?> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email must not be empty.", nameof(email));

            var cacheKey = CacheKeys.UserByEmail(email);

            if (_cacheManager.IsSet(cacheKey))
                return _cacheManager.Get<UserAccount>(cacheKey);

            var user = await _context.UserAccounts
                .FirstOrDefaultAsync(u => u.EmailAddress == email);

            if (user != null)
                _cacheManager.Set(user, cacheKey, CacheDurationSeconds);

            return user;
        }

        public async Task<List<UserAccount>> GetAllCached()
        {
            const string cacheKey = "AllUserAccounts";

            if (_cacheManager.IsSet(cacheKey))
                return _cacheManager.Get<List<UserAccount>>(cacheKey);

            var users = await _context.UserAccounts
                .AsNoTracking()
                .ToListAsync();

            if (users.Any())
                _cacheManager.Set(users, cacheKey, CacheDurationSeconds);

            return users;
        }

        public async Task Update(UserAccount userAccount)
        {
            userAccount.LastUpdated = DateTime.UtcNow;

            await _repository.UpdateAsync(userAccount);
            RemoveCache(userAccount.EmailAddress);
        }

        public async Task Delete(int id)
        {
            var user = await _context.UserAccounts.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException($"User with Id {id} not found.");

            await _repository.DeleteAsync(user);
            RemoveCache(user.EmailAddress);
        }

        public int GetCurrent()
        {
            var userSession = _sessionManager.Get<int>("CurrentUser");
            return userSession;
        }
        private void RemoveCache(string email)
        {
            var cacheKey = CacheKeys.UserByEmail(email);
            _cacheManager.Remove(cacheKey);
            _cacheManager.Remove("AllUserAccounts");
        }
    }
}
