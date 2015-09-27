using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using static Interface_TheTvDB.csFunctions;


namespace Interface_TheTvDB
{
    public partial class Form1 : Form
    {

        csInterface inter = new csInterface();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {

            //string[] ids = new string[] { "153021", "253350", "81189", "71663", "73255", "167571", "75760", "258618", "111051", "79842",
            //                              "79842", "207621", "80379", "266189", "258744", "83661", "72227", "264492", "248861", "74845", "219341" };

            //for (int i = 0; i < ids.Length; i++)
            //{
            //    startManual(ids[i]);
            //}

            startManual("153021");

        }

        private void startManual(string seriesID)
        {
            setSeriesDataTheTVDB(inter.downloadZIP("de", seriesID) + "\\" + "de" + ".xml", "de");
        }

        private void setSeriesDataTheTVDB(string path, string lan)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            
            XmlNodeList xmlList = xmlDoc.SelectNodes("//Data/Series");
            foreach (XmlNode node in xmlList)
            {
                //Xml Felder: Actors, Airs_DayOfWeek, Airs_Time, ContentRating, Genre, Network, NetworkID, SeriesID, added, addedBy
                //banner, tms_wanted_old, lastupdated

                csSER oSER = new csSER();
                oSER.SER_theTVDB_ID = xmlNodeInnerText(node, "id");
                oSER.SER_FirstAired_English = vDateTime(xmlNodeInnerText(node, "FirstAired"));
                oSER.SER_imdb_ID = xmlNodeInnerText(node, "IMDB_ID");
                oSER.SER_Languages = xmlNodeInnerText(node, "Language"); 
                oSER.SER_DescriptionShort_German = xmlNodeInnerText(node, "Overview");
                oSER.SER_Description_German = xmlNodeInnerText(node, "Overview");
                oSER.SER_RunTime = vInt(xmlNodeInnerText(node, "Runtime"));
                oSER.SER_Name_German = xmlNodeInnerText(node, "SeriesName");
                oSER.SER_State = xmlNodeInnerText(node, "Status");
                oSER.SER_Zap2It_ID = xmlNodeInnerText(node, "zap2it_id");
                oSER.LastChanged = vDateTimeUTC(xmlNodeInnerText(node, "lastupdated"));

                string fanart = xmlNodeInnerText(node, "fanart");
                string poster = xmlNodeInnerText(node, "poster");

                oSER.updateSerie();

                oSER.SER_ImageLink = downloadImage(poster, oSER.SER_theTVDB_ID, "");

                setEpisodesDataTheTVDB(xmlDoc, lan, oSER);

                txtLog.Text += Environment.NewLine + "ID: " + oSER.SER_theTVDB_ID + " Name: " + oSER.SER_Name_German;
            }
        }

        private void setEpisodesDataTheTVDB(XmlDocument xmlDoc, string lan, csSER oSER)
        {

            XmlNodeList xmlList = xmlDoc.SelectNodes("//Data/Episode");
            foreach (XmlNode node in xmlList)
            {
                //XML Felder: Combined_episodenumber, Combined_season, DVD_chapter, DVD_discid, DVD_episodenumber, DVD_season, Director, EpImgFlag
                //GuestStars, ProductionCode, Writer, airsafter_season, airsbefore_episode, airsbefore_season, filename, lastupdated
                //thumb_added, thumb_height, thumb_width, Rating, RatingCount

                if (oSER == null)
                    return;

                csSEA sea = new csSEA();
                sea.SEA_theTVDB_ID = xmlNodeInnerText(node, "seasonid");
                sea.SEA_SER = oSER.SER_ID;
                sea.SEA_NumberText = xmlNodeInnerText(node, "SeasonNumber"); 
                sea.SEA_Number = vInt(xmlNodeInnerText(node, "SeasonNumber"));
                sea.LastChanged = vDateTimeUTC(xmlNodeInnerText(node, "lastupdated"));

                sea.updateSeason();

                if (string.IsNullOrEmpty(sea.SEA_ID))
                    return;

                csEPI epi = new csEPI();
                epi.EPI_theTVDB_ID = xmlNodeInnerText(node, "id");
                epi.EPI_SEA = sea.SEA_ID;
                epi.EPI_Name_German = xmlNodeInnerText(node, "EpisodeName");
                epi.EPI_NumberOfSeason = vInt(xmlNodeInnerText(node, "EpisodeNumber"));
                epi.EPI_FirstAired_English = vDateTime(xmlNodeInnerText(node, "FirstAired")); 
                epi.EPI_imdb_ID = xmlNodeInnerText(node, "IMDB_ID");
                epi.EPI_Languages = xmlNodeInnerText(node, "Language"); 
                epi.EPI_DescriptionShort_German = xmlNodeInnerText(node, "Overview");
                epi.EPI_Description_German = xmlNodeInnerText(node, "Overview");
                epi.EPI_Number = vInt(xmlNodeInnerText(node, "absolute_number"));
                epi.EPI_NumberText = xmlNodeInnerText(node, "absolute_number");
                epi.LastChanged = vDateTimeUTC(xmlNodeInnerText(node, "lastupdated"));

                epi.updateEpisode();

                //downloadImage(filename, seriesid, "");
            }
        }
    }
}
