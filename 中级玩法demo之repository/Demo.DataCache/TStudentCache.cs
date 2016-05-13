using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DataModel;
using Demo.DataCache.Base;


namespace Demo.DataCache
{
    public class TStudentCache : CacheBase
    {
        public static TStudent GetUserModel(Guid userId)
        {
            var result = Get<TStudent>("GetUser" + userId);
            return result;
        }
        public static bool SetUserModel(TStudent model)
        {
            return Set("GetUser" + model.Id, model);
        }
        public static bool DelUserModel(Guid userId)
        {
            return Remove("GetUser" + userId);
        }
    }
}
