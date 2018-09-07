using Microsoft.AspNetCore.Identity;
using TimeSheet.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeSheet.Models.Repos.Base
{
    public interface IRepo<T> where T : EntityBase
    {
        int Count { get; }
        bool HasChanges { get; }
        T Find(Guid id);
        T GetFirst();
        IEnumerable<T> GetAll();
        IEnumerable<T> GetRange(int skip,int take);
        int Add(T entity, bool persist = true);
        int AddRange(IEnumerable<T> entities, bool persist = true);
        int Update(T entity, bool persist = true);
        int UpdateRange(IEnumerable<T> entities, bool persist = true);
        int Delete(T entity, bool persist = true);
        int DeleteRange(IEnumerable<T> entities, bool persist = true);
        int Delete(Guid id, byte[] timeStamp, bool persist = true);
        int SaveChanges();
        Task<T> FindById(Guid id);
    }
}
