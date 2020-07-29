using System;
using System.Collections.Generic;
using System.Linq;

namespace VrLifeAPI.Provider.Core.Services.AppService
{
    /// <summary>
    /// Interface aplikační databázové služby na práci s daty aplikací
    /// </summary>
    public interface IAppDataService
    {
        /// <summary>
        /// Výpis všech hodnot daného názvu pole.
        /// </summary>
        /// <param name="field">Název pole.</param>
        /// <returns>Seznam datových hodnot.</returns>
        List<DataValue> List(string field);

        /// <summary>
        /// Získání hodnoty podle názvu pole a ID záznamu.
        /// </summary>
        /// <param name="field">Název pole.</param>
        /// <param name="id">ID záznamu.</param>
        /// <returns>Datová hodnota.</returns>
        DataValue Get(string field, ulong id);

        /// <summary>
        /// Aktualizace existujícího záznamu, případně vytvoření nového.
        /// </summary>
        /// <param name="id">ID záznamu.</param>
        /// <param name="val">Hodnota záznamu, kdy název pole je uvnitř hodnoty.</param>
        void UpdateOrInsert(ulong id, DataValue val);

        /// <summary>
        /// Smazání záznamu podle názvu a jeho ID.
        /// </summary>
        /// <param name="field">Název pole.</param>
        /// <param name="id">ID záznamu.</param>
        void Delete(string field, ulong id);

        /// <summary>
        /// Smazání všech záznamu z dané aplikace do DB.
        /// </summary>
        void Clear();

        /// <summary>
        /// Vložení více hodnot do databáze.
        /// </summary>
        /// <param name="id">ID záznamu.</param>
        /// <param name="data">Seznam hodnot k vložení s názvy polí v jejích objektech.</param>
        void InsertObj(ulong id, List<DataValue> data);

        /// <summary>
        /// Vložení jedné hodnoty.
        /// </summary>
        /// <param name="id">ID záznamu.</param>
        /// <param name="val">Hodnota k vložení s názvem pole v tomto objektu.</param>
        void Insert(ulong id, DataValue val);
    }

    /// <summary>
    /// Typ datové hodnoty v DataValue objektu
    /// </summary>
    public enum DataType
    {
        DT_EMPTY,
        DT_NUMERIC,
        DT_STRING,
        DT_DECIMAL,
        DT_MULTIPLE
    }

    /// <summary>
    /// Objekt obsahující všechny hodnoty v záznamu Aplikační tabulky DB
    /// </summary>
    public struct DataValue
    {
        /// <summary>
        /// typ datové hodnoty.
        /// </summary>
        public DataType Type { get; private set; }

        /// <summary>
        /// Celočíselná hodnota.
        /// </summary>
        public long? LongVal { get; private set; }

        /// <summary>
        /// Reálná hodnota.
        /// </summary>
        public double? DoubleVal { get; private set; }

        /// <summary>
        /// Textová hodnota.
        /// </summary>
        public string StringVal { get; private set; }

        /// <summary>
        /// Název pole v tabulce.
        /// </summary>
        public string Field { get; private set; }

        /// <summary>
        /// Flag, zda je lib. hodnota přítomna.
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Konsturktor pole s prázdnými hodnotami.
        /// </summary>
        /// <param name="field">Název pole.</param>
        public DataValue(string field)
        {
            Field = field;
            HasValue = false;
            LongVal = null;
            DoubleVal = null;
            StringVal = null;
            Type = DataType.DT_EMPTY;
        }

        /// <summary>
        /// Konsturktor pole se všemi hodnotami.
        /// </summary>
        /// <param name="field">Název pole.</param>
        /// <param name="strVal">Textová hodnota.</param>
        /// <param name="longVal">Celočíselná hodnota.</param>
        /// <param name="doubleVal">Reálná hodnota.</param>
        public DataValue(string field, string strVal, long longVal, double doubleVal)
        {
            Field = field;
            HasValue = true;
            StringVal = strVal;
            LongVal = longVal;
            DoubleVal = doubleVal;
            Type = DataType.DT_MULTIPLE;
        }

        /// <summary>
        /// Konstruktor pole s celočíselnou hodnotu.
        /// </summary>
        /// <param name="field">Název pole.</param>
        /// <param name="val">Celočíselná hodnota.</param>
        public DataValue(string field, long val)
        {
            Field = field;
            HasValue = true;
            LongVal = val;
            Type = DataType.DT_NUMERIC;
            DoubleVal = null;
            StringVal = null;
        }

        /// <summary>
        /// Konstruktor pole s reálnou hodnotu.
        /// </summary>
        /// <param name="field">Název pole.</param>
        /// <param name="val">Reálná hodnota.</param>
        public DataValue(string field, double val)
        {
            Field = field;
            HasValue = true;
            DoubleVal = val;
            Type = DataType.DT_DECIMAL;
            LongVal = null;
            StringVal = null;
        }

        /// <summary>
        /// Konstruktor pole s textovou hodnotu.
        /// </summary>
        /// <param name="field">Název pole.</param>
        /// <param name="val">Textová hodnota.</param>
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
