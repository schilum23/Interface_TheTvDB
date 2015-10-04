using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using static Interface_TheTvDB.csFunctions;
using System.Threading;


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
            string path = Path.Combine(@"C:\", "Serien.csv");
            FileStream fs = File.OpenRead(path);
            var reader = new StreamReader(fs);
            List<string> serienIDs = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                if (values.Length > 2 && values[2] == "1")
                {
                    serienIDs.Add(values[0]);
                }
            }

            txtLog.Text += Environment.NewLine + "Start: " + DateTime.Now.ToLongTimeString();
            txtLog.Text += Environment.NewLine + "Serien: " + serienIDs.Count.ToString();

            pbUpdate.Minimum = 0;
            pbUpdate.Maximum = serienIDs.Count-1;

            for (int i = 0; i < serienIDs.Count; i++)
            {
                pbUpdate.Value = i;

                //var t = new Thread(() => startManual(serienIDs[i]));
                //t.Start();
                startManual(serienIDs[i]);
            }

            // startManual("153021");
            txtLog.Text += Environment.NewLine + "Ende: " + DateTime.Now.ToLongTimeString();

        }

        private void startManual(string seriesID)
        {
            string path = inter.downloadZIP("de", seriesID) + "\\" + "de" + ".xml";
            setSeriesDataTheTVDB(path, "de");
        }

        private void setSeriesDataTheTVDB(string path, string lan)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlDocument xmlDocBanner = new XmlDocument();
            xmlDocBanner.Load(path.Replace("de.xml", "banners.xml"));

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

                oSER.SER_ImageLink = downloadImage(poster, oSER.SER_theTVDB_ID, "serie", "SER", oSER.SER_theTVDB_ID + ".jpg");
                oSER.SER_ImageLink = downloadImage(fanart, oSER.SER_theTVDB_ID, "serie", "SER_Wide", oSER.SER_theTVDB_ID + "_wide.jpg");

                setEpisodesDataTheTVDB(xmlDoc, lan, oSER, xmlDocBanner);

                txtLog.Text += Environment.NewLine + "ID: " + oSER.SER_theTVDB_ID + " Name: " + oSER.SER_Name_German + " - " + DateTime.Now.ToLongTimeString();

                Console.WriteLine("ID: " + oSER.SER_theTVDB_ID + " Name: " + oSER.SER_Name_German + " - " + DateTime.Now.ToLongTimeString());
            }
        }

        private void setEpisodesDataTheTVDB(XmlDocument xmlDoc, string lan, csSER oSER, XmlDocument xmlDocBanner)
        {

            XmlNodeList xmlList = xmlDoc.SelectNodes("//Data/Episode");
            foreach (XmlNode node in xmlList)
            {
                //XML Felder: Combined_episodenumber, Combined_season, DVD_chapter, DVD_discid, DVD_episodenumber, DVD_season, Director, EpImgFlag
                //GuestStars, ProductionCode, Writer, airsafter_season, airsbefore_episode, airsbefore_season, filename, lastupdated
                //thumb_added, thumb_height, thumb_width, Rating, RatingCount

                if (oSER == null)
                    return;

                csSEA oSEA = new csSEA();
                oSEA.SEA_theTVDB_ID = xmlNodeInnerText(node, "seasonid");
                oSEA.SEA_SER = oSER.SER_ID;
                oSEA.SEA_NumberText = xmlNodeInnerText(node, "SeasonNumber");
                oSEA.SEA_Number = vInt(xmlNodeInnerText(node, "SeasonNumber"));
                oSEA.LastChanged = vDateTimeUTC(xmlNodeInnerText(node, "lastupdated"));

                oSEA.updateSeason();

                if (!setBannersTheTVDB(xmlDocBanner, oSER.SER_theTVDB_ID, oSEA.SEA_NumberText, oSEA.SEA_theTVDB_ID, false)) {
                    setBannersTheTVDB(xmlDocBanner, oSER.SER_theTVDB_ID, oSEA.SEA_NumberText, oSEA.SEA_theTVDB_ID, true);
                }

                if (string.IsNullOrEmpty(oSEA.SEA_ID))
                    return;

                csEPI epi = new csEPI();
                epi.EPI_theTVDB_ID = xmlNodeInnerText(node, "id");
                epi.EPI_SEA = oSEA.SEA_ID;
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
            }
        }

        private bool setBannersTheTVDB(XmlDocument xmlDoc, string seriesID, string season_, string SEA_ID, bool searchAll)
        {
            // Serien Update holen
            XmlNodeList xmlList = xmlDoc.SelectNodes("//Banners/Banner");
            foreach (XmlNode node in xmlList)
            {
                string id = (node.SelectSingleNode("id") != null) ? node.SelectSingleNode("id").InnerText : "";
                string BannerPath = (node.SelectSingleNode("BannerPath") != null) ? node.SelectSingleNode("BannerPath").InnerText : "";
                string BannerType = (node.SelectSingleNode("BannerType") != null) ? node.SelectSingleNode("BannerType").InnerText : "";
                string BannerType2 = (node.SelectSingleNode("BannerType2") != null) ? node.SelectSingleNode("BannerType2").InnerText : "";
                string Colors = (node.SelectSingleNode("Colors") != null) ? node.SelectSingleNode("Colors").InnerText : "";
                string Language = (node.SelectSingleNode("Language") != null) ? node.SelectSingleNode("Language").InnerText : "";
                string Rating = (node.SelectSingleNode("Rating") != null) ? node.SelectSingleNode("Rating").InnerText : "";
                string RatingCount = (node.SelectSingleNode("RatingCount") != null) ? node.SelectSingleNode("RatingCount").InnerText : "";
                string SeriesName = (node.SelectSingleNode("SeriesName") != null) ? node.SelectSingleNode("SeriesName").InnerText : "";
                string ThumbnailPath = (node.SelectSingleNode("ThumbnailPath") != null) ? node.SelectSingleNode("ThumbnailPath").InnerText : "";
                string VignettePath = (node.SelectSingleNode("VignettePath") != null) ? node.SelectSingleNode("VignettePath").InnerText : "";
                string season = (node.SelectSingleNode("Season") != null) ? node.SelectSingleNode("Season").InnerText : "";


                if (BannerType == "season" && BannerType2 == "season" && (Language == "de" || searchAll) && season == season_)
                {
                    downloadImage(BannerPath, seriesID, "", "SEA", SEA_ID + ".jpg");
                    return true;
                }

            }
            return false;
        }
    }
}
