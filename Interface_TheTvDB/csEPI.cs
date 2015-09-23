using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface_TheTvDB.dsSeriesTableAdapters;
using System.Data;

namespace Interface_TheTvDB
{
    class csEPI
    {
        dsSeries ds = new dsSeries();
        dsSeriesTableAdapters.EPI_EpisodesTableAdapter dtEPI = new dsSeriesTableAdapters.EPI_EpisodesTableAdapter();

        string epi_ID = "";
        public string EPI_ID
        {
            get { return epi_ID; }
            set { epi_ID = value; }
        }

        DateTime epi_Created = DateTime.Now;
        public DateTime EPI_Created
        {
            get { return epi_Created; }
            set { epi_Created = value; }
        }

        DateTime epi_Changed = DateTime.Now;
        public DateTime EPI_Changed
        {
            get { return epi_Changed; }
            set { epi_Changed = value; }
        }

        DateTime epi_Deleted;
        public DateTime EPI_Deleted
        {
            get { return epi_Deleted; }
            set { epi_Deleted = value; }
        }

        string epi_theTVDB_ID = "";
        public string EPI_theTVDB_ID
        {
            get { return epi_theTVDB_ID; }
            set { epi_theTVDB_ID = value; }
        }

        string epi_imdb_ID = "";
        public string EPI_imdb_ID
        {
            get { return epi_imdb_ID; }
            set { epi_imdb_ID = value; }
        }

        string epi_SEA = "";
        public string EPI_SEA
        {
            get { return epi_SEA; }
            set { epi_SEA = value; }
        }

        string epi_Name_German = "";
        public string EPI_Name_German
        {
            get { return epi_Name_German; }
            set { epi_Name_German = value; }
        }

        string epi_Name_English = "";
        public string EPI_Name_English
        {
            get { return epi_Name_English; }
            set { epi_Name_English = value; }
        }

        string epi_DescriptionShort_German = "";
        public string EPI_DescriptionShort_German
        {
            get { return epi_DescriptionShort_German; }
            set { epi_DescriptionShort_German = value; }
        }

        string epi_DescriptionShort_English = "";
        public string EPI_DescriptionShort_English
        {
            get { return epi_DescriptionShort_English; }
            set { epi_DescriptionShort_English = value; }
        }

        string epi_Description_German = "";
        public string EPI_Description_German
        {
            get { return epi_Description_German; }
            set { epi_Description_German = value; }
        }

        string epi_Description_English = "";
        public string EPI_Description_English
        {
            get { return epi_Description_English; }
            set { epi_Description_English = value; }
        }

        DateTime epi_FirstAired_German = new DateTime(1900, 1, 1);
        public DateTime EPI_FirstAired_German
        {
            get { return epi_FirstAired_German; }
            set { epi_FirstAired_German = value; }
        }

        DateTime epi_FirstAired_English = new DateTime(1900, 1, 1);
        public DateTime EPI_FirstAired_English
        {
            get { return epi_FirstAired_English; }
            set { epi_FirstAired_English = value; }
        }

        double epi_Rate = 0;
        public double EPI_Rate
        {
            get { return epi_Rate; }
            set { epi_Rate = value; }
        }

        int epi_RateCount = 0;
        public int EPI_RateCount
        {
            get { return epi_RateCount; }
            set { epi_RateCount = value; }
        }

        double epi_imdb_Rate = 0;
        public double EPI_imdb_Rate
        {
            get { return epi_imdb_Rate; }
            set { epi_imdb_Rate = value; }
        }

        int epi_imdb_RateCount = 0;
        public int EPI_imdb_RateCount
        {
            get { return epi_imdb_RateCount; }
            set { epi_imdb_RateCount = value; }
        }

        int epi_WatchedCount = 0;
        public int EPI_WatchedCount
        {
            get { return epi_WatchedCount; }
            set { epi_WatchedCount = value; }
        }

        int epi_Number = 0;
        public int EPI_Number
        {
            get { return epi_Number; }
            set { epi_Number = value; }
        }

        int epi_NumberOfSeason = 0;
        public int EPI_NumberOfSeason
        {
            get { return epi_NumberOfSeason; }
            set { epi_NumberOfSeason = value; }
        }
 
        string epi_NumberText = "";
        public string EPI_NumberText
        {
            get { return epi_NumberText; }
            set { epi_NumberText = value; }
        }

        string epi_Languages = "";
        public string EPI_Languages
        {
            get { return epi_Languages; }
            set { epi_Languages = value; }
        }

        //public string insertEpisode()
        //{

        //    if (string.IsNullOrEmpty(epi_ID))
        //        epi_ID = getEPI_IDwithTheTVDB_ID();

        //    if (!string.IsNullOrEmpty(epi_ID))
        //        return epi_ID;

        //    epi_ID = Guid.NewGuid().ToString();

        //    dtEPI.Insert(epi_ID, epi_Created, epi_Changed, epi_Deleted, epi_theTVDB_ID, epi_imdb_ID, epi_SEA,
        //        epi_Name_German, epi_Name_English, epi_DescriptionShort_German, epi_DescriptionShort_English,
        //        epi_Description_German, epi_Description_English, epi_FirstAired_German, epi_FirstAired_English, epi_Rate,
        //        epi_RateCount, epi_imdb_Rate, epi_imdb_RateCount, epi_WatchedCount, epi_Number, epi_NumberOfSeason, epi_NumberText, epi_Languages);

        //    return epi_ID;

        //}

        //public string getEPI_IDwithTheTVDB_ID()
        //{
        //    object returnID = "";

        //    if (string.IsNullOrEmpty(epi_theTVDB_ID))
        //        return "";

        //    returnID = dtEPI.getEPI_IDwithTHETVDB_ID(epi_theTVDB_ID);
        //    return (returnID == null) ? "" : returnID.ToString();
        //}

        //public bool updateEpisode()
        //{
  
        //    dtEPI.FillByID(ds.EPI_Episodes, epi_ID);

        //    foreach (DataRow row in ds.EPI_Episodes.Rows)
        //    {
        //        foreach (var prop in this.GetType().GetProperties())
        //        {
        //            if (row.Table.Columns.Contains(prop.Name) && !row.Table.Columns[prop.Name].ReadOnly)
        //            {
        //                row[prop.Name] = prop.GetValue(this, null);
        //            }
        //        }

        //        row["EPI_ChangeDate"] = DateTime.Now;
        //        dtEPI.Update(row);
        //        return true;
        //    }

        //    return false;

        //}

        //public csEPI getEpisode()
        //{

        //    csEPI epi = new csEPI();

        //    dtEPI.FillByID(ds.EPI_Episodes, epi_ID);

        //    foreach (DataRow row in ds.EPI_Episodes.Rows)
        //    {
        //        foreach (var prop in epi.GetType().GetProperties())
        //        {
        //            if (row.Table.Columns.Contains(prop.Name))
        //            {
        //                if (row[prop.Name] != DBNull.Value)
        //                    prop.SetValue(epi, row[prop.Name], null);
        //            }
        //        }
        //    }

        //    epi.epi_ID = epi_ID;
        //    return epi;

        //}

    }
}

