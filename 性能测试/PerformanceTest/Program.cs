using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dapper.Base;
using Dos.ORM;
using PerformanceTest.Model.EF;

namespace PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 初始化
            var time = new Stopwatch();
            var dapper = BaseDapper.Context;
            //测试查询数据量
            var scount = 10000;
            //测试插入、修改、删除数据量
            var icount = 1000;

            #endregion

            Console.WriteLine("-----测试分两类测试");
            Console.WriteLine("");
            Console.WriteLine("-----第一类：sql写法测试。由于dapper并不是真正意义上的ORM，直接写sql不能跨数据库支持，所以不能使用dapper的sql写法与Dos.ORM的面向对象写法进行对比测试。（有很多国内国外的朋友写了dapper的面向对象ORM扩展，但均不是成熟的产品，各种缺陷、未知bug甚多，一个ORM不经过3-5年在各种数据库上面的项目验证都还太年轻，所以也不对dapper的扩展做测试。）");
            Console.WriteLine("");
            Console.WriteLine("-----第二类：面向对象写法测试。这里暂时只列举了与EF的对比测试。");
            Console.WriteLine("");
            Console.WriteLine("-----因此：Dos.ORM与dapper对比sql性能；与EF对比面向对象写法性能。");
            Console.WriteLine("");
            //第一类
            for (int i = 1; i <= 2; i++)
            {
                Console.WriteLine("-----第一类，开始第【" + i + "】次测试-----");

                #region Dos.ORM查询
                time.Restart();
                var dosList = Db.Context.FromSql(" select top " 
                    + scount 
                    + " * from T_PerformanceTest ").ToList<Dapper.Model.TPerformanceTest>();
                time.Stop();
                Console.WriteLine("Dos.ORM查询" + dosList.Count + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dos.ORM插入
                //插入。每次插入均重新从连接池取连接、插入数据、归还连接。
                #region 初始化需要插入的sql
                var needInsert = new List<string>();
                var ids = new List<Guid>();
                for (int j = 0; j < icount; j++)
                {
                    ids.Add(Guid.NewGuid());
                }
                for (int j = 0; j < icount; j++)
                {
                    needInsert.Add(
                        $" insert into T_PerformanceTest values('{ids[j]}','{"IT大师 http://www.itdos.com"}'," +
                        $"{j},{j},{j},'{"IT大师text http://www.itdos.com"}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',{1},{j}) ");
                }
                #endregion
                var tcount = 0;
                time.Restart();
                foreach (var sql in needInsert)
                {
                    tcount += Db.Context.FromSql(sql).ExecuteNonQuery();
                }
                time.Stop();
                Console.WriteLine("Dos.ORM插入" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dos.ORM修改
                //修改。每次修改均重新从连接池取连接、插入数据、归还连接。
                #region 初始化需要修改的sql
                var needUpdate = new List<string>();
                for (int j = 0; j < icount; j++)
                {
                    needUpdate.Add($" update T_PerformanceTest set T1='IT大师 http://www.itdos.com2',T2={j+2},T3={j+2},T4={j+2},T5='IT大师text http://www.itdos.com2',T6='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',T7=0,T8={j+2} where Id='{ids[j]}' ");
                }
                #endregion
                tcount = 0;
                time.Restart();
                foreach (var sql in needUpdate)
                {
                    tcount += Db.Context.FromSql(sql).ExecuteNonQuery();
                }
                time.Stop();
                Console.WriteLine("Dos.ORM修改" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dos.ORM删除
                //删除。每次删除均重新从连接池取连接、插入数据、归还连接。
                #region 初始化需要删除的sql
                var needDelete = new List<string>();
                for (int j = 0; j < icount; j++)
                {
                    needDelete.Add($" delete from T_PerformanceTest where Id='{ids[j]}' ");
                }
                #endregion
                tcount = 0;
                time.Restart();
                foreach (var sql in needDelete)
                {
                    tcount += Db.Context.FromSql(sql).ExecuteNonQuery();
                }
                time.Stop();
                Console.WriteLine("Dos.ORM删除" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion

                Console.WriteLine("");

                #region Dapper查询
                time.Restart();
                var efList = BaseDapper.Context.Query<Dapper.Model.TPerformanceTest>(" select top "
                    + scount
                    + " * from T_PerformanceTest ");
                time.Stop();
                Console.WriteLine("Dapper查询" + efList.Count + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dapper插入
                //插入。每次插入均重新从连接池取连接、插入数据、归还连接。
                tcount = 0;
                time.Restart();
                foreach (var sql in needInsert)
                {
                    tcount += BaseDapper.Context.Execute(sql);
                }
                time.Stop();
                Console.WriteLine("Dapper插入" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dapper修改
                //修改。每次修改均重新从连接池取连接、插入数据、归还连接。
                tcount = 0;
                time.Restart();
                foreach (var sql in needUpdate)
                {
                    tcount += BaseDapper.Context.Execute(sql);
                }
                time.Stop();
                Console.WriteLine("Dapper修改" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dapper删除
                //删除。每次删除均重新从连接池取连接、插入数据、归还连接。
                tcount = 0;
                time.Restart();
                foreach (var sql in needDelete)
                {
                    tcount += BaseDapper.Context.Execute(sql);
                }
                time.Stop();
                Console.WriteLine("Dapper删除" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion

                Console.WriteLine("");
                scount *= 2;
                icount *= 2;
            }

            scount = 10000;
            icount = 1000;

            //第二类
            for (int i = 1; i <= 1; i++)
            {
                Console.WriteLine("-----第二类，开始第【" + i + "】次测试-----");

                #region Dos.ORM查询
                time.Restart();
                var dosList = TPerformanceTestDosRepository.Top(scount);
                time.Stop();
                Console.WriteLine("Dos.ORM查询" + dosList.Count + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dos.ORM插入
                //插入。每次插入均重新从连接池取连接、插入数据、归还连接。
                //初始化需要插入的实体
                var needInsert = new List<Dos.Model.TPerformanceTest>();
                for (int j = 0; j < icount; j++)
                {
                    needInsert.Add(new Dos.Model.TPerformanceTest()
                    {
                        Id = Guid.NewGuid(),
                        T1 = "IT大师 http://www.itdos.com",
                        T2 = j,
                        T3 = j,
                        T4 = j,
                        T5 = "IT大师text http://www.itdos.com",
                        T6 = DateTime.Now,
                        T7 = true,
                        T8 = j
                    });
                }
                var tcount = 0;
                time.Restart();
                foreach (var model in needInsert)
                {
                    tcount += TPerformanceTestDosRepository.Insert(model);
                }
                time.Stop();
                Console.WriteLine("Dos.ORM插入" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dos.ORM修改
                //修改。每次修改均重新从连接池取连接、插入数据、归还连接。
                //初始化需要修改的实体
                foreach (var model in needInsert)
                {
                    model.T1 = model.T1 + "2";
                    model.T2 = model.T2 + 2;
                    model.T3 = model.T3 + 2;
                    model.T4 = model.T4 + 2;
                    model.T5 = model.T5 + "2";
                    model.T6 = DateTime.Now;
                    model.T7 = false;
                    model.T8 = model.T8 + 2;
                }
                tcount = 0;
                time.Restart();
                foreach (var model in needInsert)
                {
                    tcount += TPerformanceTestDosRepository.Update(model);
                }
                time.Stop();
                Console.WriteLine("Dos.ORM修改" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region Dos.ORM删除
                //删除。每次删除均重新从连接池取连接、插入数据、归还连接。
                tcount = 0;
                time.Restart();
                foreach (var model in needInsert)
                {
                    tcount += TPerformanceTestDosRepository.Delete(model);
                }
                time.Stop();
                Console.WriteLine("Dos.ORM删除" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion

                Console.WriteLine("");

                #region EF查询
                time.Restart();
                var efList = T_PerformanceTestEFRepository.Top(scount);
                time.Stop();
                Console.WriteLine("EF查询" + efList.Count + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region EF插入
                //插入。每次插入均重新从连接池取连接、插入数据、归还连接。
                //初始化需要插入的实体
                var needInsert2 = new List<T_PerformanceTest>();
                for (int j = 0; j < icount; j++)
                {
                    needInsert2.Add(new T_PerformanceTest()
                    {
                        Id = Guid.NewGuid(),
                        T1 = "IT大师 http://www.itdos.com",
                        T2 = j,
                        T3 = j,
                        T4 = j,
                        T5 = "IT大师text http://www.itdos.com",
                        T6 = DateTime.Now,
                        T7 = true,
                        T8 = j
                    });
                }
                tcount = 0;
                time.Restart();
                foreach (var model in needInsert2)
                {
                    tcount += T_PerformanceTestEFRepository.Insert(model);
                }
                time.Stop();
                Console.WriteLine("EF插入" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region EF修改
                //修改。每次修改均重新从连接池取连接、插入数据、归还连接。
                //初始化需要修改的实体
                foreach (var model in needInsert2)
                {
                    model.T1 = model.T1 + "2";
                    model.T2 = model.T2 + 2;
                    model.T3 = model.T3 + 2;
                    model.T4 = model.T4 + 2;
                    model.T5 = model.T5 + "2";
                    model.T6 = DateTime.Now;
                    model.T7 = false;
                    model.T8 = model.T8 + 2;
                }
                tcount = 0;
                time.Restart();
                foreach (var model in needInsert2)
                {
                    tcount += T_PerformanceTestEFRepository.Update(model);
                }
                time.Stop();
                Console.WriteLine("EF修改" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                #region EF删除
                //删除。每次删除均重新从连接池取连接、插入数据、归还连接。
                tcount = 0;
                time.Restart();
                foreach (var model in needInsert2)
                {
                    tcount += T_PerformanceTestEFRepository.Delete(model);
                }
                time.Stop();
                Console.WriteLine("EF删除" + tcount + "条执行时间：" + time.ElapsedMilliseconds);
                #endregion
                Console.WriteLine("Ps：可能是我EF的写法有问题，才导致EF的增、删、改性能极低。暂时未找到原因。");

                Console.WriteLine("");
                scount *= 2;
                icount *= 2;
            }
        }
    }
}
