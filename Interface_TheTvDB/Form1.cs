using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface_TheTvDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnGo_Click(object sender, EventArgs e)
        {

            // getUpdates();
            //string[] ids = new string[] { "153021", "253350", "81189", "71663", "73255", "167571", "75760", "258618", "111051", "79842", 
            //                              "79842", "207621", "80379", "266189", "258744", "83661", "72227", "264492", "248861", "74845", "219341" };

            //for (int i = 0; i < ids.Length; i++)
            //{
            //    startManual(ids[i]);
            //    syncWithAzure(ids[i]);
            //}

            //startManual("153021");


            startAutomatic("20141108");

        }

        private void startAutomatic(string datum)
        {

            SER.FillByDate(ds.SER_Series, datum);

            pbUpdate.Minimum = 0;
            pbUpdate.Maximum = 499;
            pbUpdate.Step = 1;
            for (int i = 0; i < 500; i++)
            {
                pbUpdate.Value = i;
                string seriesID = ds.SER_Series[i].SER_theTVDB_ID;

                getChanges("de", seriesID);
                getChanges("en", seriesID);
            }
        }

        private void startManual(string seriesID)
        {
            getChanges("de", seriesID);
            getChanges("en", seriesID);
        }

        private void syncWithAzure(string seriesID)
        {
            csSER ser = new csSER();
            ser.SER_theTVDB_ID = seriesID;
            ser.SER_ID = ser.getSER_IDwithTheTVDB_ID();
            ser.syncSerieWithAzure();
        }

        private void getChanges(string language, string seriesID)
        {

            string path = fun.Path;
            string zipPath = path + "\\" + language + "_" + seriesID + ".zip";
            string extractPath = path + "\\" + language + "_" + seriesID;
            string link = "http://thetvdb.com/api/" + fun.API_Key + "/series/" + seriesID + "/all/" + language + ".zip";

            try
            {
                downloadZIP(zipPath, extractPath, link);
            }
            catch
            {
                return;
            }

            //setBannersTheTVDB(extractPath + "\\banners.xml", seriesID);
            setSeriesDataTheTVDB(extractPath + "\\" + language + ".xml", language);
            setEpisodesDataTheTVDB(extractPath + "\\" + language + ".xml", language);
            //setActorsTheTVDB(extractPath + "\\actors.xml", seriesID);

            // Directory.Delete(extractPath, true);

        }

        private void setEpisodesDataTheTVDB(string path, string lan)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                XmlNodeList xmlList = xmlDoc.SelectNodes("//Data/Episode");
                foreach (XmlNode node in xmlList)
                {
                    //XML Felder: Combined_episodenumber, Combined_season, DVD_chapter, DVD_discid, DVD_episodenumber, DVD_season, Director, EpImgFlag
                    //GuestStars, ProductionCode, Writer, airsafter_season, airsbefore_episode, airsbefore_season, filename, lastupdated
                    //thumb_added, thumb_height, thumb_width, Rating, RatingCount

                    csSER ser = new csSER();
                    ser.SER_theTVDB_ID = (node.SelectSingleNode("seriesid") != null) ? node.SelectSingleNode("seriesid").InnerText : "";
                    ser.SER_ID = ser.getSER_IDwithTheTVDB_ID();

                    if (string.IsNullOrEmpty(ser.SER_ID))
                        return;

                    csSEA sea = new csSEA();
                    sea.SEA_theTVDB_ID = (node.SelectSingleNode("seasonid") != null) ? node.SelectSingleNode("seasonid").InnerText : "";
                    sea.SEA_ID = sea.getSEA_IDwithTheTVDB_ID();
                    sea = sea.getSeason();
                    sea.SEA_theTVDB_ID = (node.SelectSingleNode("seasonid") != null) ? node.SelectSingleNode("seasonid").InnerText : "";

                    sea.SEA_SER = ser.SER_ID;
                    sea.SEA_NumberText = (node.SelectSingleNode("SeasonNumber") != null) ? node.SelectSingleNode("SeasonNumber").InnerText : "";
                    sea.SEA_Number = fun.vInt((node.SelectSingleNode("SeasonNumber") != null) ? node.SelectSingleNode("SeasonNumber").InnerText : "");

                    if (!string.IsNullOrEmpty(sea.SEA_ID))
                        sea.updateSeason();
                    else
                        sea.insertSeason();

                    if (string.IsNullOrEmpty(sea.SEA_ID))
                        return;

                    csEPI epi = new csEPI();
                    epi.EPI_theTVDB_ID = (node.SelectSingleNode("id") != null) ? node.SelectSingleNode("id").InnerText : "";
                    epi.EPI_ID = epi.getEPI_IDwithTheTVDB_ID();
                    epi = epi.getEpisode();
                    epi.EPI_theTVDB_ID = (node.SelectSingleNode("id") != null) ? node.SelectSingleNode("id").InnerText : "";

                    epi.EPI_SEA = sea.SEA_ID;
                    epi.EPI_Name_German = (node.SelectSingleNode("EpisodeName") != null && lan == "de") ? node.SelectSingleNode("EpisodeName").InnerText : epi.EPI_Name_German;
                    epi.EPI_Name_English = (node.SelectSingleNode("EpisodeName") != null && lan == "en") ? node.SelectSingleNode("EpisodeName").InnerText : epi.EPI_Name_English;
                    epi.EPI_NumberOfSeason = fun.vInt((node.SelectSingleNode("EpisodeNumber") != null) ? node.SelectSingleNode("EpisodeNumber").InnerText : "");
                    epi.EPI_FirstAired_English = fun.vDateTime((node.SelectSingleNode("FirstAired") != null) ? node.SelectSingleNode("FirstAired").InnerText : "");
                    epi.EPI_imdb_ID = (node.SelectSingleNode("IMDB_ID") != null) ? node.SelectSingleNode("IMDB_ID").InnerText : "";

                    string epi_Language = (node.SelectSingleNode("Language") != null) ? node.SelectSingleNode("Language").InnerText : "";
                    if (!epi.EPI_Languages.Contains(epi_Language))
                        epi.EPI_Languages += (!string.IsNullOrEmpty(epi.EPI_Languages)) ? "|" + epi_Language : epi_Language;

                    epi.EPI_DescriptionShort_German = (node.SelectSingleNode("Overview") != null && lan == "de") ? node.SelectSingleNode("Overview").InnerText : epi.EPI_Description_German;
                    epi.EPI_DescriptionShort_English = (node.SelectSingleNode("Overview") != null && lan == "en") ? node.SelectSingleNode("Overview").InnerText : epi.EPI_Description_English;

                    epi.EPI_Description_German = (node.SelectSingleNode("Overview") != null && lan == "de") ? node.SelectSingleNode("Overview").InnerText : epi.EPI_Description_German;
                    epi.EPI_Description_English = (node.SelectSingleNode("Overview") != null && lan == "en") ? node.SelectSingleNode("Overview").InnerText : epi.EPI_Description_English;
                    epi.EPI_Number = fun.vInt((node.SelectSingleNode("absolute_number") != null) ? node.SelectSingleNode("absolute_number").InnerText : "");
                    epi.EPI_NumberText = (node.SelectSingleNode("absolute_number") != null) ? node.SelectSingleNode("absolute_number").InnerText : "";

                    if (!string.IsNullOrEmpty(epi.EPI_ID))
                        epi.updateEpisode();
                    else
                        epi.insertEpisode();

                    //downloadImage(filename, seriesid, "");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void setSeriesDataTheTVDB(string path, string lan)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);


                XmlNodeList xmlList = xmlDoc.SelectNodes("//Data/Series");
                foreach (XmlNode node in xmlList)
                {
                    //Xml Felder: Actors, Airs_DayOfWeek, Airs_Time, ContentRating, Genre, Network, NetworkID, SeriesID, added, addedBy
                    //banner, tms_wanted_old

                    //string ser_Name = (node.SelectSingleNode("SeriesName") != null) ? node.SelectSingleNode("SeriesName").InnerText : "";
                    //if (ser_Name.Contains("Duplicate"))
                    //    return;

                    csSER ser = new csSER();
                    ser.SER_theTVDB_ID = (node.SelectSingleNode("id") != null) ? node.SelectSingleNode("id").InnerText : "";
                    ser.SER_ID = ser.insertSerie();
                    ser = ser.getSerie();

                    ser.SER_FirstAired_English = fun.vDateTime((node.SelectSingleNode("FirstAired") != null) ? node.SelectSingleNode("FirstAired").InnerText : "");
                    ser.SER_imdb_ID = (node.SelectSingleNode("IMDB_ID") != null) ? node.SelectSingleNode("IMDB_ID").InnerText : "";

                    string ser_Language = (node.SelectSingleNode("Language") != null) ? node.SelectSingleNode("Language").InnerText : "";
                    if (!ser.SER_Languages.Contains(ser_Language))
                        ser.SER_Languages += (!string.IsNullOrEmpty(ser.SER_Languages)) ? "|" + ser_Language : ser_Language;

                    ser.SER_DescriptionShort_German = (node.SelectSingleNode("Overview") != null && lan == "de") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_German;
                    ser.SER_DescriptionShort_English = (node.SelectSingleNode("Overview") != null && lan == "en") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_English;

                    ser.SER_Description_German = (node.SelectSingleNode("Overview") != null && lan == "de") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_German;
                    ser.SER_Description_English = (node.SelectSingleNode("Overview") != null && lan == "en") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_English;
                    ser.SER_RunTime = fun.vInt((node.SelectSingleNode("Runtime") != null) ? node.SelectSingleNode("Runtime").InnerText : "");
                    ser.SER_Name_German = (node.SelectSingleNode("SeriesName") != null && lan == "de") ? node.SelectSingleNode("SeriesName").InnerText : ser.SER_Name_German;
                    ser.SER_Name_English = (node.SelectSingleNode("SeriesName") != null && lan == "en") ? node.SelectSingleNode("SeriesName").InnerText : ser.SER_Name_English;
                    ser.SER_State = (node.SelectSingleNode("Status") != null) ? node.SelectSingleNode("Status").InnerText : "";
                    ser.SER_Zap2It_ID = (node.SelectSingleNode("zap2it_id") != null) ? node.SelectSingleNode("zap2it_id").InnerText : "";

                    txtLog.Text += Environment.NewLine + "ID: " + ser.SER_theTVDB_ID + " Name: " + ser.SER_Name_German;

                    string fanart = (node.SelectSingleNode("fanart") != null) ? node.SelectSingleNode("fanart").InnerText : "";
                    string poster = (node.SelectSingleNode("poster") != null) ? node.SelectSingleNode("poster").InnerText : "";
                    //downloadImage(fanart, ser.SER_theTVDB_ID, "");
                    ser.SER_ImageLink = downloadImage(poster, ser.SER_theTVDB_ID, "");

                    ser.updateSerie();

                }
            }
            catch
            {
                return;
            }
        }

        private void setBannersTheTVDB(string path, string seriesID)
        {
            txtLog.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine + "Banners: ";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

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

                txtLog.Text += Environment.NewLine + "ID: " + id + " BannerPath: " + BannerPath;
                //downloadImage(BannerPath, seriesID, "");    

            }
        }

        private void setActorsTheTVDB(string path, string seriesID)
        {
            txtLog.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine + "Actors: ";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            // Serien Update holen
            XmlNodeList xmlList = xmlDoc.SelectNodes("//Actors/Actor");
            foreach (XmlNode node in xmlList)
            {
                string id = (node.SelectSingleNode("id") != null) ? node.SelectSingleNode("id").InnerText : "";
                string Image = (node.SelectSingleNode("Image") != null) ? node.SelectSingleNode("Image").InnerText : "";
                string Name = (node.SelectSingleNode("Name") != null) ? node.SelectSingleNode("Name").InnerText : "";
                string Role = (node.SelectSingleNode("Role") != null) ? node.SelectSingleNode("Role").InnerText : "";
                string SortOrder = (node.SelectSingleNode("SortOrder") != null) ? node.SelectSingleNode("SortOrder").InnerText : "";

                txtLog.Text += Environment.NewLine + "ID: " + id + " Name: " + Name + " Role: " + Role;
                //downloadImage(Image, seriesID, "");

            }
        }

        // Bilder runterladen
        private string downloadImage(string imagePath, string id, string type)
        {
            try
            {
                if (String.IsNullOrEmpty(imagePath))
                    return "";

                // Pfade und Dateiname erstellen
                string link = "http://thetvdb.com/banners/" + imagePath;
                string imageType = (!String.IsNullOrEmpty(type)) ? type : imagePath.Substring(0, imagePath.IndexOf("/"));
                string path = "C:\\tempImages\\" + id + "\\" + imageType + "\\";
                string localFilename = imagePath.Substring(imagePath.LastIndexOf("/") + 1, imagePath.Length - imagePath.LastIndexOf("/") - 1);

                // Pfad prüfen und ggfl. erstellen
                if (!Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                // Prüfen ob das Bild schon existiert
                if (File.Exists(path + localFilename))
                    return path + localFilename;

                // Bild runterladen
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(link, path + localFilename);
                }

                return path + localFilename;
                //Image img = null;

                //img = Image.FromFile(path);


                //System.Drawing.Image.GetThumbnailImageAbort cb = new System.Drawing.Image.GetThumbnailImageAbort(AbortThumbnailGeneration);

                //img = img.GetThumbnailImage(80, 53, cb, IntPtr.Zero);
                //img.Save(path);
            }
            catch
            {
                return "";
            }

        }

        private bool AbortThumbnailGeneration()
        {
            return false;
        }

        // ZIP Runterladen
        private void downloadZIP(string zipPath, string extractPath, string link)
        {

            // Pfad prüfen und ggfl. erstellen
            if (Directory.Exists(extractPath))
            {
                //später return entfernen
                return;
                Directory.Delete(extractPath, true);
                System.IO.Directory.CreateDirectory(extractPath);
            }
            else
            {
                System.IO.Directory.CreateDirectory(extractPath);
            }

            // ZIP File laden
            using (var client = new WebClient())
            {
                client.DownloadFile(link, zipPath);
            }

            // ZIP entpacken und löschen
            ZipFile.ExtractToDirectory(zipPath, extractPath);
            File.Delete(zipPath);
        }

        // Geänderte/neue Serien holen
        private void getUpdates()
        {
            string date = DateTime.Today.ToString("yyyyMMdd");

            string link = "http://thetvdb.com/api/Updates.php?type=all&time=" + date;
            txtLog.Text += link;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(link);

            // Serien Update holen
            XmlNodeList xmlList = xmlDoc.SelectNodes("//Items/Series");
            foreach (XmlNode node in xmlList)
            {
                csSER ser = new csSER();
                ser.SER_theTVDB_ID = node.InnerText;
                ser.insertSerie();
            }

            // Folgen Updates holen
            xmlList = xmlDoc.SelectNodes("//Items/Episode");
            foreach (XmlNode node in xmlList)
            {
                txtLog.Text += Environment.NewLine + "Folge: " + node.InnerText;
            }


        }

    }
}
