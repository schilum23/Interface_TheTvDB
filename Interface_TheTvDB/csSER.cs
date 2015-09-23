using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_TheTvDB
{
    class csSER
    {

        database_localDataSet ds = new database_localDataSet();
        dsAzure dsAzure = new dsAzure();
        database_localDataSetTableAdapters.SER_SeriesTableAdapter SER = new database_localDataSetTableAdapters.SER_SeriesTableAdapter();
        dsAzureTableAdapters.SER_SeriesTableAdapter SERAzure = new dsAzureTableAdapters.SER_SeriesTableAdapter();



        string ser_ID = "";
        public string SER_ID
        {
            get { return ser_ID; }
            set { ser_ID = value; }
        }

        DateTime ser_createdAt = DateTime.Now;
        public DateTime SER_createdAt
        {
            get { return ser_createdAt; }
            set { ser_createdAt = value; }
        }

        DateTimeOffset __updatedAt = new DateTimeOffset(DateTime.Now);
        public DateTimeOffset SER_updatedAt
        {
            get { return __updatedAt; }
            set { __updatedAt = value; }
        }

        DateTime ser_ChangeDate = DateTime.Now;
        public DateTime SER_ChangeDate
        {
            get { return ser_ChangeDate; }
            set { ser_ChangeDate = value; }
        }

        string ser_theTVDB_ID = "";
        public string SER_theTVDB_ID
        {
            get { return ser_theTVDB_ID; }
            set { ser_theTVDB_ID = value; }
        }

        string ser_Zap2It_ID = "";
        public string SER_Zap2It_ID
        {
            get { return ser_Zap2It_ID; }
            set { ser_Zap2It_ID = value; }
        }

        string ser_imdb_ID = "";
        public string SER_imdb_ID
        {
            get { return ser_imdb_ID; }
            set { ser_imdb_ID = value; }
        }

        string ser_Name_German = "";
        public string SER_Name_German
        {
            get { return ser_Name_German; }
            set { ser_Name_German = value; }
        }

        string ser_Name_English = "";
        public string SER_Name_English
        {
            get { return ser_Name_English; }
            set { ser_Name_English = value; }
        }

        string ser_DescriptionShort_German = "";
        public string SER_DescriptionShort_German
        {
            get { return ser_DescriptionShort_German; }
            set { ser_DescriptionShort_German = value; }
        }

        string ser_DescriptionShort_English = "";
        public string SER_DescriptionShort_English
        {
            get { return ser_DescriptionShort_English; }
            set { ser_DescriptionShort_English = value; }
        }

        string ser_Description_German = "";
        public string SER_Description_German
        {
            get { return ser_Description_German; }
            set { ser_Description_German = value; }
        }

        string ser_Description_English = "";
        public string SER_Description_English
        {
            get { return ser_Description_English; }
            set { ser_Description_English = value; }
        }

        DateTime ser_FirstAired_German = new DateTime(1900, 1, 1);
        public DateTime SER_FirstAired_German
        {
            get { return ser_FirstAired_German; }
            set { ser_FirstAired_German = value; }
        }

        DateTime ser_FirstAired_English = new DateTime(1900, 1, 1);
        public DateTime SER_FirstAired_English
        {
            get { return ser_FirstAired_English; }
            set { ser_FirstAired_English = value; }
        }

        double ser_Rate = 0;
        public double SER_Rate
        {
            get { return ser_Rate; }
            set { ser_Rate = value; }
        }

        int ser_RateCount = 0;
        public int SER_RateCount
        {
            get { return ser_RateCount; }
            set { ser_RateCount = value; }
        }

        double ser_imdb_Rate = 0;
        public double SER_imdb_Rate
        {
            get { return ser_imdb_Rate; }
            set { ser_imdb_Rate = value; }
        }

        int ser_imdb_RateCount = 0;
        public int SER_imdb_RateCount
        {
            get { return ser_imdb_RateCount; }
            set { ser_imdb_RateCount = value; }
        }

        int ser_RunTime = 0;
        public int SER_RunTime
        {
            get { return ser_RunTime; }
            set { ser_RunTime = value; }
        }

        string ser_State = "";
        public string SER_State
        {
            get { return ser_State; }
            set { ser_State = value; }
        }

        string ser_Soundtrack = "";
        public string SER_Soundtrack
        {
            get { return ser_Soundtrack; }
            set { ser_Soundtrack = value; }
        }

        string ser_Trailer = "";
        public string SER_Trailer
        {
            get { return ser_Trailer; }
            set { ser_Trailer = value; }
        }

        DateTime ser_ProductionDateFrom = new DateTime(1900, 1, 1);
        public DateTime SER_ProductionDateFrom
        {
            get { return ser_ProductionDateFrom; }
            set { ser_ProductionDateFrom = value; }
        }

        DateTime ser_ProductionDateTo = new DateTime(1900, 1, 1);
        public DateTime SER_ProductionDateTo
        {
            get { return ser_ProductionDateTo; }
            set { ser_ProductionDateTo = value; }
        }

        string ser_Country = "";
        public string SER_Country
        {
            get { return ser_Country; }
            set { ser_Country = value; }
        }

        string ser_Languages = "";
        public string SER_Languages
        {
            get { return ser_Languages; }
            set { ser_Languages = value; }
        }

        string ser_Awards = "";
        public string SER_Awards
        {
            get { return ser_Awards; }
            set { ser_Awards = value; }
        }

        string ser_Website = "";
        public string SER_Website
        {
            get { return ser_Website; }
            set { ser_Website = value; }
        }

        string ser_Facebook = "";
        public string SER_Facebook
        {
            get { return ser_Facebook; }
            set { ser_Facebook = value; }
        }

        string ser_Twitter = "";
        public string SER_Twitter
        {
            get { return ser_Twitter; }
            set { ser_Twitter = value; }
        }

        string ser_ImageLink = "";
        public string SER_ImageLink
        {
            get { return ser_ImageLink; }
            set { ser_ImageLink = value; }
        }

        int ser_SeasonCount = 0;
        public int SER_SeasonCount
        {
            get { return ser_SeasonCount; }
            set { ser_SeasonCount = value; }
        }

        int ser_EpisodesCount = 0;
        public int SER_EpisodesCount
        {
            get { return ser_EpisodesCount; }
            set { ser_EpisodesCount = value; }
        }

        int ser_FavouritesCount = 0;
        public int SER_FavouritesCount
        {
            get { return ser_FavouritesCount; }
            set { ser_FavouritesCount = value; }
        }

        string ser_DIR = "";
        public string SER_DIR
        {
            get { return ser_DIR; }
            set { ser_DIR = value; }
        }

        public string insertSerie()
        {
            //try
            //{
            csSER ser = new csSER();
            ser.ser_theTVDB_ID = ser_theTVDB_ID;
            ser_ID = ser.getSER_IDwithTheTVDB_ID();

            if (!string.IsNullOrEmpty(ser_ID))
                return ser_ID;

            ser_ID = Guid.NewGuid().ToString();

            SER.Insert(ser_ID, ser_createdAt, __updatedAt, DateTime.Now, ser_theTVDB_ID, ser_Zap2It_ID, ser_imdb_ID,
                ser_Name_German, ser_Name_English, ser_DescriptionShort_German, ser_DescriptionShort_English, ser_Description_German,
                ser_Description_English, ser_FirstAired_German, ser_FirstAired_English, ser_Rate, ser_RateCount, ser_imdb_Rate, ser_imdb_RateCount,
                ser_RunTime, ser_State, ser_Soundtrack, ser_Trailer, ser_ProductionDateFrom, ser_ProductionDateTo,
                ser_Country, ser_Languages, ser_Awards, ser_Website, ser_Facebook, ser_Twitter, ser_ImageLink, ser_SeasonCount,
                ser_EpisodesCount, ser_FavouritesCount, ser_DIR);


            return ser_ID;

            //}
            //catch (Exception ex)
            //{
            //    return "";
            //}

        }

        public string getSER_IDwithTheTVDB_ID()
        {

            object returnID = "";

            if (string.IsNullOrEmpty(ser_theTVDB_ID))
                return "";

            returnID = SER.getSER_IDwithTHETVDB_ID(ser_theTVDB_ID);
            return (returnID == null) ? "" : returnID.ToString();
        }

        public bool updateSerie()
        {
            //try
            //{
            SER.FillByID(ds.SER_Series, ser_ID);

            foreach (DataRow row in ds.SER_Series.Rows)
            {
                foreach (var prop in this.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(prop.Name) && !row.Table.Columns[prop.Name].ReadOnly)
                    {
                        row[prop.Name] = prop.GetValue(this, null);
                    }
                }

                row["SER_ChangeDate"] = DateTime.Now;

                SER.Update(row);
                return true;
            }

            return false;


            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

        }

        public csSER getSerie()
        {
            try
            {
                csSER ser = new csSER();

                SER.FillByID(ds.SER_Series, ser_ID);

                foreach (DataRow row in ds.SER_Series.Rows)
                {
                    foreach (var prop in ser.GetType().GetProperties())
                    {
                        if (row.Table.Columns.Contains(prop.Name))
                        {
                            if (row[prop.Name] != DBNull.Value)
                                prop.SetValue(ser, row[prop.Name], null);
                        }
                    }
                }

                ser.ser_ID = ser_ID;
                return ser;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public void syncSerieWithAzure()
        {

            SER.FillByID(ds.SER_Series, ser_ID);

            foreach (DataRow row in ds.SER_Series.Rows)
            {
                SERAzure.FillByID(dsAzure.SER_Series, row["id"].ToString());

                if (dsAzure.SER_Series.Count == 0)
                {
                    dsAzure.SER_Series.ImportRow(row);
                    dsAzure.SER_Series.Rows[0].SetAdded();
                    SERAzure.Update(dsAzure.SER_Series);
                }
                else
                {
                    foreach (DataColumn col in ds.SER_Series.Columns)
                    {
                        DataColumnCollection columns = dsAzure.SER_Series.Columns;

                        if (columns.Contains(col.ColumnName))
                        {
                            dsAzure.SER_Series.Rows[0][col.ColumnName] = row[col.ColumnName];
                        }
                    }
                    SERAzure.Update(dsAzure.SER_Series);
                }

                csSEA sea = new csSEA();
                sea.SEA_SER = ser_ID;
                sea.syncSeasonWithAzure();
            }

        }

    }
}