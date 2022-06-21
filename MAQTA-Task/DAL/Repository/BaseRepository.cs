using MAQTA.DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MAQTA.DAL.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class

    {

        internal MAQTADbContext context;

        internal DbSet<TEntity> dbSet;


        public BaseRepository(MAQTADbContext context)

        {

            this.context = context;

            this.dbSet = context.Set<TEntity>();

        }


        public virtual IEnumerable<TEntity> GetWithRawSql(string query,

            params object[] parameters)

        {

            return dbSet.FromSqlRaw(query, parameters).ToList();

        }


        public virtual IEnumerable<TEntity> Get(

            Expression<Func<TEntity, bool>> filter = null,

            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            string[] includeProperties = null)

        {

            IQueryable<TEntity> query = dbSet;


            if (filter != null)

            {

                query = query.Where(filter);

            }


            if (includeProperties != null)

            {

                foreach (var includeProperty in includeProperties)

                {

                    query = query.Include(includeProperty);

                }

            }




            if (orderBy != null)

            {

                return orderBy(query).ToList();

            }

            else

            {

                return query.ToList();

            }

        }


        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)

        {

            IQueryable<TEntity> query = dbSet;


            if (filter != null)

            {

                query = query.Where(filter);

            }




            return query.Count();



        }





        public virtual TEntity GetByID(object id)

        {

            return dbSet.Find(id);

        }


        public virtual void Insert(TEntity entity)

        {

            dbSet.Add(entity);

        }


        public virtual void Delete(object id)

        {

            TEntity entityToDelete = dbSet.Find(id);

            Delete(entityToDelete);

        }


        public virtual void Delete(TEntity entityToDelete)

        {

            if (context.Entry(entityToDelete).State == EntityState.Detached)

            {

                dbSet.Attach(entityToDelete);

            }

            dbSet.Remove(entityToDelete);

        }


        public virtual void Update(TEntity entityToUpdate)

        {

            dbSet.Attach(entityToUpdate);

            context.Entry(entityToUpdate).State = EntityState.Modified;

        }

      
    }
}
