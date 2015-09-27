using System;
using System.Linq;
using Interface_TheTvDB.dsSeriesTableAdapters;
using static Interface_TheTvDB.csFunctions;
using System.Data;

namespace Interface_TheTvDB
{
    class csSEA
    {
        dsSeries ds = new dsSeries();
        SEA_SeasonsTableAdapter dtSEA = new SEA_SeasonsTableAdapter();

        string sea_ID = "";
        public string SEA_ID
        {
            get { return sea_ID; }
            set { sea_ID = value; }
        }

        DateTime? sea_Created = null;
        public DateTime? SEA_Created
        {
            get { return sea_Created; }
            set { sea_Created = value; }
        }

        DateTime? sea_Changed = null;
        public DateTime? SEA_Changed
        {
            get { return sea_Changed; }
            set { sea_Changed = value; }
        }

        DateTime? sea_Deleted = null;
        public DateTime? SEA_Deleted
        {
            get { return sea_Deleted; }
            set { sea_Deleted = value; }
        }

        string sea_theTVDB_ID = "";
        public string SEA_theTVDB_ID
        {
            get { return sea_theTVDB_ID; }
            set { sea_theTVDB_ID = value; }
        }

        string sea_imdb_ID = "";
        public string SEA_imdb_ID
        {
            get { return sea_imdb_ID; }
            set { sea_imdb_ID = value; }
        }

        string sea_SER = "";
        public string SEA_SER
        {
            get { return sea_SER; }
            set { sea_SER = value; }
        }

        string sea_Name_German = "";
        public string SEA_Name_German
        {
            get { return sea_Name_German; }
            set { sea_Name_German = value; }
        }

        string sea_Name_English = "";
        public string SEA_Name_English
        {
            get { return sea_Name_English; }
            set { sea_Name_English = value; }
        }

        int sea_Number = 0;
        public int SEA_Number
        {
            get { return sea_Number; }
            set { sea_Number = value; }
        }

        int sea_OrderNumber = 0;
        public int SEA_OrderNumber
        {
            get { return sea_OrderNumber; }
            set { sea_OrderNumber = value; }
        }

        int sea_EpisodesCount = 0;
        public int SEA_EpisodesCount
        {
            get { return sea_EpisodesCount; }
            set { sea_EpisodesCount = value; }
        }

        string sea_Description_German = "";
        public string SEA_Description_German
        {
            get { return sea_Description_German; }
            set { sea_Description_German = value; }
        }

        string sea_Description_English = "";
        public string SEA_Description_English
        {
            get { return sea_Description_English; }
            set { sea_Description_English = value; }
        }

        string sea_NumberText = "";
        public string SEA_NumberText
        {
            get { return sea_NumberText; }
            set { sea_NumberText = value; }
        }

        DateTime lastChanged = new DateTime(1970, 1, 1);
        public DateTime LastChanged
        {
            get { return lastChanged; }
            set { lastChanged = value; }
        }

        public string insertSeason()
        { 
            if (string.IsNullOrEmpty(sea_ID))
                sea_ID = getSEA_IDwithTheTVDB_ID();

            if (!string.IsNullOrEmpty(sea_ID))
                return sea_ID;

            sea_ID = Guid.NewGuid().ToString();

            dtSEA.Insert(sea_ID, DateTime.Now, DateTime.Now, null, sea_theTVDB_ID, sea_imdb_ID, sea_SER,
                        sea_Name_German, sea_Name_English, sea_Number, sea_OrderNumber, sea_EpisodesCount,
                        sea_Description_German, SEA_Description_English, sea_NumberText);

            return sea_ID;

        }

        public string getSEA_IDwithTheTVDB_ID()
        {
            object returnID = "";

            if (string.IsNullOrEmpty(sea_theTVDB_ID))
                return "";

            returnID = dtSEA.getSEA_IDwithTHETVDB_ID(sea_theTVDB_ID);
            return (returnID == null) ? "" : returnID.ToString();
        }

        public void updateSeason()
        {
            sea_ID = this.getSEA_IDwithTheTVDB_ID();
            dtSEA.FillByID(ds.SEA_Seasons, sea_ID);

            if (ds.SEA_Seasons.Count > 0)
            {
                DataRow row = ds.SEA_Seasons.Rows[0];

                if (vDateTime(lastChanged) < vDateTime(row["SEA_Changed"]))
                    return;

                row = getDataRowFromClass(this, row);
                row["SEA_Changed"] = DateTime.Now;
                dtSEA.Update(row);
            }
            else
            {
                this.insertSeason();
            }
        }

        public csSEA getSeason()
        {
            csSEA sea = new csSEA();

            dtSEA.FillByID(ds.SEA_Seasons, sea_ID);

            if (ds.SEA_Seasons.Count > 0)
                sea = (csSEA)getClassFromDataRow(ds.SEA_Seasons.Rows[0], sea);

            return sea;
        }

    }

}
