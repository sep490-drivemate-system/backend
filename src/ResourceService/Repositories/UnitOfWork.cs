
using Microsoft.EntityFrameworkCore;
using ResourceService.Repositories.Implementation;
using ResourceService.Repositories.Interfaces;
using ResourceService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IResourceRepository ResourceRepository { get; }

        int SaveChangesWithTransaction();

        Task<int> SaveChangesWithTransactionAsync();

    }

    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly ResourceDbContext _context;
        private IResourceRepository _resourceRepository;

        public UnitOfWork(ResourceDbContext context)
        {
            _context = context;
        }

        public UnitOfWork() => _context ??= new ResourceDbContext();


        public void Dispose() => _context.Dispose();

        public IResourceRepository ResourceRepository
        {
            get
            {
                return _resourceRepository ??= new ResourceRepository(_context);
            }
        }




        

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }


    }
}
