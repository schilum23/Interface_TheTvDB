using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_TheTvDB
{
    class csSEA
    {
        database_localDataSet ds = new database_localDataSet();
        dsAzure dsAzure = new dsAzure();
        database_localDataSetTableAdapters.SEA_SeasonsTableAdapter SEA = new database_localDataSetTableAdapters.SEA_SeasonsTableAdapter();
        dsAzureTableAdapters.SEA_SeasonsTableAdapter SEAAzure = new dsAzureTableAdapters.SEA_SeasonsTableAdapter();

        string sea_ID = "";
        public string SEA_ID
        {
            get { return sea_ID; }
            set { sea_ID = value; }
        }

        DateTime sea_createdAt = DateTime.Now;
        public DateTime SEA_createdAt
        {
            get { return sea_createdAt; }
            set { sea_createdAt = value; }
        }

        DateTimeOffset __updatedAt = new DateTimeOffset(DateTime.Now);
        public DateTimeOffset SER_updatedAt
        {
            get { return __updatedAt; }
            set { __updatedAt = value; }
        }

        DateTime sea_ChangeDate = DateTime.Now;
        public DateTime SEA_ChangeDate
        {
            get { return sea_ChangeDate; }
            set { sea_ChangeDate = value; }
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

        public string insertSeason()
        {
            //try
            //{

            if (string.IsNullOrEmpty(sea_ID))
                sea_ID = getSEA_IDwithTheTVDB_ID();

            if (!string.IsNullOrEmpty(sea_ID))
                return sea_ID;

            sea_ID = Guid.NewGuid().ToString();

            SEA.Insert(sea_ID, sea_createdAt, __updatedAt, DateTime.Now, sea_theTVDB_ID, sea_imdb_ID, sea_SER,
                        sea_Name_German, sea_Name_English, sea_Number, sea_OrderNumber, sea_EpisodesCount,
                        sea_Description_German, SEA_Description_English, sea_NumberText);

            return sea_ID;

            //}
            //catch (Exception ex)
            //{
            //    return "";
            //}

        }

        public string getSEA_IDwithTheTVDB_ID()
        {
            object returnID = "";

            if (string.IsNullOrEmpty(sea_theTVDB_ID))
                return "";

            returnID = SEA.getSEA_IDwithTHETVDB_ID(sea_theTVDB_ID);
            return (returnID == null) ? "" : returnID.ToString();
        }

        public bool updateSeason()
        {
            //try
            //{
            SEA.FillByID(ds.SEA_Seasons, sea_ID);

            foreach (DataRow row in ds.SEA_Seasons.Rows)
            {
                foreach (var prop in this.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(prop.Name) && !row.Table.Columns[prop.Name].ReadOnly)
                    {
                        row[prop.Name] = prop.GetValue(this, null);
                    }
                }

                row["SEA_ChangeDate"] = DateTime.Now;
                SEA.Update(row);
                return true;
            }

            return false;


            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

        }

        public csSEA getSeason()
        {
            try
            {
                csSEA sea = new csSEA();

                SEA.FillByID(ds.SEA_Seasons, sea_ID);

                foreach (DataRow row in ds.SEA_Seasons.Rows)
                {
                    foreach (var prop in sea.GetType().GetProperties())
                    {
                        if (row.Table.Columns.Contains(prop.Name))
                        {
                            if (row[prop.Name] != DBNull.Value)
                                prop.SetValue(sea, row[prop.Name], null);
                        }
                    }
                }

                sea.sea_ID = sea_ID;
                return sea;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public void syncSeasonWithAzure()
        {

            SEA.FillBySER(ds.SEA_Seasons, sea_SER);

            foreach (DataRow row in ds.SEA_Seasons.Rows)
            {
                SEAAzure.FillByID(dsAzure.SEA_Seasons, row["id"].ToString());

                if (dsAzure.SEA_Seasons.Count == 0)
                {
                    dsAzure.SEA_Seasons.ImportRow(row);
                    dsAzure.SEA_Seasons.Rows[0].SetAdded();
                    SEAAzure.Update(dsAzure.SEA_Seasons);
                }
                else
                {
                    foreach (DataColumn col in ds.SEA_Seasons.Columns)
                    {
                        dsAzure.SEA_Seasons.Rows[0][col.ColumnName] = row[col.ColumnName];
                    }

                    SEAAzure.Update(dsAzure.SEA_Seasons);
                }

                csEPI epi = new csEPI();
                epi.EPI_SEA = row["id"].ToString();
                epi.syncEpisodeWithAzure();
            }

        }

    }

}
