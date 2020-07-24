using System;
using System.Collections.Generic;
using System.Linq;

namespace VrLifeAPI.Provider.Core.Services.AppService
{
    public interface IAppDataService
    {
        List<DataValue> List(string field);

        DataValue Get(string field, ulong id);

        void UpdateOrInsert(ulong id, DataValue val);

        void Delete(string field, ulong id);

        void Clear();

        void InsertObj(ulong id, List<DataValue> data);

        void Insert(ulong id, DataValue val);
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
