#region << 版 本 注 释 >>
/****************************************************
* 文 件 名：Sys_BaseDataLogic
* Copyright(c) 青之软件
* CLR 版本: 4.0.30319.17929
* 创 建 人：周浩
* 电子邮箱：admin@itdos.com
* 创建日期：2014/10/1 11:00:49
* 文件描述：
******************************************************
* 修 改 人：
* 修改日期：
* 备注描述：
*******************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;
using PerformanceTest;
using PerformanceTest.Model.EF;

namespace PerformanceTest
{
    public abstract class EFRepository<T>  where T : class
    {
        public static readonly DbContext Context = new DosORMSqlServerEntities();

        public static DbSet<T> Entities
        {
            get { return Context.Set<T>(); }
        }

        public static List<T> GetAll()
        {
            return Entities.ToList();
        }
        public static List<T> Query(Expression<Func<T, bool>> where)
        {
            var result = Entities.Where(where);
            if (result.Any())
            {
                return result.ToList();
            }
            return new List<T>();
        }
        public static List<T> Top(int top)
        {
            var result = Entities.Take(top);
            return result.ToList();
        }
        public static int Insert(T entity)
        {
            Entities.Add(entity);
            return Context.SaveChanges();
        }
        public static int Insert(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Entities.Add(entity);
            }
            return Context.SaveChanges();
        }
        public static int Update(T entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return Context.SaveChanges();
        }
        public static int Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
            return Context.SaveChanges();
        }
        public static int Delete(T entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            return Context.SaveChanges();
        }
        public static int Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            }
            return Context.SaveChanges();
        }
    }
}
