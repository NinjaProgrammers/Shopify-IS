using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> GetAll();
        public TEntity GetById(object Id);
        public void AddEntity(TEntity entity);

        public void Delete(object Id);

        public void Update(TEntity entity);

        public IEnumerable<TEntity> Select(TEntity entity);



    }
}
