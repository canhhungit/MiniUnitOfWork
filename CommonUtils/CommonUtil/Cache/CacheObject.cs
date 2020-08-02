using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtil.Cache
{
    public interface ICacheEntity
    {
        //string SortKey { get; }
        bool HasAndMapByKey(ICacheEntity obj);

        bool HasKey(ICacheEntity obj);

        void Map(ICacheEntity obj);
        object JsonClone();
    }
    public class CacheObject<T>
    {
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string KeyUpdate { get; set; }

        public T ObjVal { get; set; }
        public CacheObject(T objVal)
        {
            LastUpdate = CreateDate = DateTime.Now;
            ObjVal = objVal;
            KeyUpdate = string.Empty;
        }

        public CacheObject(DateTime creteDate, T objVal)
        {
            LastUpdate = CreateDate = creteDate;
            ObjVal = objVal;
        }
    }

    public class CacheListObject<T> where T : ICacheEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string KeyUpdate { get; set; }

        public List<T> LstVal { get; set; }
        public CacheListObject(List<T> lstVal)
        {
            LastUpdate = CreateDate = DateTime.Now;
            LstVal = lstVal;
            KeyUpdate = string.Empty;
        }

        public CacheListObject(DateTime creteDate, List<T> lstVal)
        {
            LastUpdate = CreateDate = creteDate;
            LstVal = lstVal;
        }

        public void Update(List<T> lstVal, DateTime updateTime)
        {
            LastUpdate = updateTime;
            foreach (var item in lstVal)
            {
                if (!LstVal.Any(a => a.HasAndMapByKey(item)))
                {
                    LstVal.Add(item);
                }
            }
        }
    }

    public class DFCahce<T> where T : ICacheEntity
    {
        private readonly ICacheService cacheService;
        private readonly ICacheService cacheRedisService;
        public string Key;
        private readonly Func<string, List<T>> Create;
        private readonly Func<string, DateTime, DateTime, List<T>> Update;
        private readonly Func<string, string> GetUpdateKey;
        public DFCahce(Func<string, List<T>> _Create, Func<string, DateTime, DateTime, List<T>> _Update, Func<string, string> _GetUpdateKey)
        {
            Create = _Create;
            Update = _Update;
            GetUpdateKey = _GetUpdateKey;
        }

        public List<T> GetVal()
        {
            var cacheObj = cacheService.GetOrAdd(Key, () => { return GetRedisVal(); });
            var key = GetUpdateKey(Key);
            if (!cacheObj.KeyUpdate.Equals(key))
            {
                var DateOld = cacheObj.LastUpdate;
                var DateNow = DateTime.Now;
                var lstUP = Update(key, DateOld, DateNow);
                cacheObj.Update(lstUP, DateNow);
                cacheObj.KeyUpdate = key;
            }
            return cacheObj.LstVal;
        }

        private CacheListObject<T> GetRedisVal()
        {
            var DateNow = DateTime.Now;
            var lstObj = Create(Key);
            var result = new CacheListObject<T>(DateNow, lstObj)
            {
                KeyUpdate = GetUpdateKey(Key)
            };
            return result;
        }
    }
}