using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VrLifeServer.Database;
using VrLifeServer.Database.DbModels;

namespace VrLifeServer.Core.Services.AppService
{
    class AppDataService
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

    public enum DataType
    {
        DT_EMPTY,
        DT_NUMERIC,
        DT_STRING,
        DT_DECIMAL,
        DT_MULTIPLE
    }

    public struct DataValue
    {
        public DataType Type { get; private set; }
        public long? LongVal { get; private set; }
        public double? DoubleVal { get; private set; }
        public string StringVal { get; private set; }
        public string Field { get; private set; }
        public bool HasValue { get; private set; }

        public DataValue(string field)
        {
            Field = field;
            HasValue = false;
            LongVal = null;
            DoubleVal = null;
            StringVal = null;
            Type = DataType.DT_EMPTY;
        }

        public DataValue(string field, string strVal, long longVal, double doubleVal)
        {
            Field = field;
            HasValue = true;
            StringVal = strVal;
            LongVal = longVal;
            DoubleVal = doubleVal;
            Type = DataType.DT_MULTIPLE;
        }

        public DataValue(string field, long val)
        {
            Field = field;
            HasValue = true;
            LongVal = val;
            Type = DataType.DT_NUMERIC;
            DoubleVal = null;
            StringVal = null;
        }

        public DataValue(string field, double val)
        {
            Field = field;
            HasValue = true;
            DoubleVal = val;
            Type = DataType.DT_DECIMAL;
            LongVal = null;
            StringVal = null;
        }

        public DataValue(string field, string val)
        {
            Field = field;
            HasValue = true;
            StringVal = val;
            Type = DataType.DT_STRING;
            LongVal = null;
            DoubleVal = null;
        }
    }
}
