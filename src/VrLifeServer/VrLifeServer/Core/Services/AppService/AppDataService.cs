using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeServer.Database;
using VrLifeServer.Database.DbModels;

namespace VrLifeServer.Core.Services.AppService
{
    class AppDataService : IAppDataService
    {
        public ulong _appId;
        public AppDataService(ulong appId)
        {
            _appId = appId;
        }

        public List<DataValue> List(string field)
        {
            using (var db = new VrLifeDbContext())
            {
                return db.AppData.Where(x => x.AppId == _appId && x.FieldName == field)
                    .Select(x => new DataValue(field, x.StringValue, x.NumericValue, x.DecimalValue)).ToList();
            }
        }

        public DataValue Get(string field, ulong id)
        {
            using (var db = new VrLifeDbContext())
            {
                AppData data = db.AppData.SingleOrDefault(x => x.AppId == _appId && x.FieldName == field && x.FieldId == id);
                if (data == null)
                {
                    return new DataValue(field);
                }
                return new DataValue(field, data.StringValue, data.NumericValue, data.DecimalValue);
            }
        }

        public void UpdateOrInsert(ulong id, DataValue val)
        {
            using (var db = new VrLifeDbContext())
            {
                AppData data = db.AppData.SingleOrDefault(x => x.AppId == _appId && x.FieldName == val.Field && x.FieldId == id);
                if (data != null)
                {
                    SetValues(data, val);
                    db.SaveChanges();
                    return;
                }
            }
            Insert(id, val);
        }

        public void Delete(string field, ulong id)
        {
            using (var db = new VrLifeDbContext())
            {
                var toRemove = db.AppData.Where(x => x.AppId == _appId && x.FieldName == field && x.FieldId == id);
                db.AppData.RemoveRange(toRemove);
                db.SaveChanges();
            }
        }

        public void Clear()
        {
            using (var db = new VrLifeDbContext())
            {
                db.AppData.RemoveRange(db.AppData.Where(x => x.AppId == _appId));
                db.SaveChanges();
            }
        }

        public void InsertObj(ulong id, List<DataValue> data)
        {
            foreach(DataValue val in data)
            {
                Insert(id, val);
            }
        }

        public void Insert(ulong id, DataValue val)
        {
            using (var db = new VrLifeDbContext())
            {
                AppData data = new AppData();
                data.AppId = _appId;
                data.FieldName = val.Field;
                data.FieldId = id;
                SetValues(data, val);
                db.AppData.Add(data);
                db.SaveChanges();
            }
        }

        private void SetValues(AppData appData, DataValue val)
        {
            if (val.StringVal != null)
            {
                appData.StringValue = val.StringVal;
            }
            if (val.DoubleVal.HasValue)
            {
                appData.DecimalValue = val.DoubleVal.Value;
            }
            if (val.LongVal.HasValue)
            {
                appData.NumericValue = val.LongVal.Value;
            }
        }
    }
}
