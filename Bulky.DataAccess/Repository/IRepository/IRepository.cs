using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category
        IEnumerable<T> GetAll();

        // Biểu thức chung của dòng 16 là dòng 17
        // Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        T Get(Expression<Func<T, bool>> filter);

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);


    }
}
