using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dos.Common;
using Dos.ORM;
using System.Data.Common;
using Demo.Common;
using Demo.DataAccess;
using Demo.DataModel.Base;
using Demo.DataModel;
using Demo.DataCache;

namespace Demo.Business
{
    public class TStudentLogic
    {
        /// <summary>
        /// 获取数据。此数据会持续增长，所以不建议一次性缓存。建议单个Model实体缓存。
        /// </summary>
        public BaseResult GetUser(TStudentParam param)
        {
            var where = new Where<TStudent>();
            #region 模糊搜索条件
            if (!string.IsNullOrWhiteSpace(param.SearchName))
            {
                where.And(d => d.Name.Like(param.SearchName));
            }
            if (!string.IsNullOrWhiteSpace(param.SearchIDNumber))
            {
                where.And(d => d.IDNumber.Like(param.SearchIDNumber));
            }
            if (!string.IsNullOrWhiteSpace(param.SearchMobilePhone))
            {
                where.And(d => d.MobilePhone.Like(param.SearchMobilePhone));
            }
            #endregion

            #region 是否分页
            var dateCount = 0;
            if (param._PageIndex != null && param._PageSize != null)
            {
                //取总数，以计算共多少页。自行考虑将总数缓存。
                dateCount = TStudentRepository.Count(where);//.SetCacheTimeOut(10)
            }
            #endregion
            var list = TStudentRepository.Query(where, d => d.CreateTime, "desc", null, param._PageSize, param._PageIndex);
            return new BaseResult(true, list, "", dateCount);
        }
        public BaseResult GetUserModel(TStudentParam param)
        {
            if (param.Id == null)
            {
                return new BaseResult(false, null, Msg.ParamError);
            }
            //取缓存
            var model = TStudentCache.GetUserModel(param.Id.Value);
            if (model == null)
            {
                //如果缓存不存在，则从数据库获取
                model = TStudentRepository.First(d => d.Id == param.Id);
                TStudentCache.SetUserModel(model);
            }
            return new BaseResult(true, model);
        }
        /// <summary>
        /// 新增数据。必须传入姓名Name，手机号MobilePhone，身份证号IDNumber
        /// </summary>
        public BaseResult AddUser(TStudentParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Name) || string.IsNullOrWhiteSpace(param.MobilePhone)
                    || string.IsNullOrWhiteSpace(param.IDNumber))
            {
                return new BaseResult(false, null, Msg.ParamError);
            }
            var model = new TStudent
            {
                Id = Guid.NewGuid(),
                Name = param.Name,
                IDNumber = param.IDNumber,
                MobilePhone = param.MobilePhone,
                CreateTime = DateTime.Now
            };
            var count = TStudentRepository.Insert(model);
            //设置缓存
            TStudentCache.SetUserModel(model);
            return new BaseResult(count > 0, count, count > 0 ? "" : Msg.Line0);
        }
        /// <summary>
        /// 删除数据。必须传入Id
        /// </summary>
        public BaseResult DelUser(TStudentParam param)
        {
            if (param.Id == null)
            {
                return new BaseResult(false, null, Msg.ParamError);
            }
            var count = TStudentRepository.Delete(param.Id);
            //更新缓存
            TStudentCache.DelUserModel(param.Id.Value);
            return new BaseResult(count > 0, count, count > 0 ? "" : Msg.Line0);
        }
        /// <summary>
        /// 修改数据。必须传入Id
        /// </summary>
        public BaseResult UptUser(TStudentParam param)
        {
            if (param.Id == null)
            {
                return new BaseResult(false, null, Msg.ParamError);
            }
            var model = new TStudent();
            if (param.Name != null)
                model.Name = param.Name;
            if (param.IDNumber != null)
                model.IDNumber = param.IDNumber;
            if (param.MobilePhone != null)
                model.MobilePhone = param.MobilePhone;
            var count = TStudentRepository.Update(model, d => d.Id == param.Id);
            //更新缓存
            TStudentCache.DelUserModel(param.Id.Value);
            return new BaseResult(true);
        }
    }
}
