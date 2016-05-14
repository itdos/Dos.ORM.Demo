#region << 版 本 注 释 >>
/****************************************************
* 文 件 名：
* Copyright(c) 质见软件
* CLR 版本: 4.0.30319.17929
* 创 建 人：周浩
* 电子邮箱：zhouhao@itdos.com
* 创建日期：2016/3/1 10:00:11
* 文件描述：
******************************************************
* 修 改 人：
* 修改日期：
* 备注描述：
*******************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dos.ORM;

namespace PerformanceTest
{
    public class Db
    {
        public static readonly DbSession Context = new DbSession(DatabaseType.SqlServer9, ConfigurationManager.ConnectionStrings["SqlConn"].ToString());
    }
}
