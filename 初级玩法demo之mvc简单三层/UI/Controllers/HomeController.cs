using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business;
using Model.Base;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        #region Oracle、MySql、SqlServer、PostgreSql等
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public JsonResult GetUser(TStudentParam param)
        {
            var result = new TStudentLogic().GetUser(param);
            var test = Json(result);
            return test;
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        public JsonResult AddUser(TStudentParam param)
        {
            var result = new TStudentLogic().AddUser(param);
            return Json(result);
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        public JsonResult UptUser(TStudentParam param)
        {
            var result = new TStudentLogic().UptUser(param);
            return Json(result);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        public JsonResult DelUser(TStudentParam param)
        {
            var result = new TStudentLogic().DelUser(param);
            return Json(result);
        }
        #endregion
    }
}
