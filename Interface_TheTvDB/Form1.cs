using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using System.Xml;


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
            //getChanges("en", seriesID);
        }

        private void setSeriesDataTheTVDB(string path, string lan)
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
                ser.SER_theTVDB_ID = (inter.vString(node.SelectSingleNode("idddd")) != null) ? node.SelectSingleNode("id").InnerText : "";
                ser.SER_ID = ser.insertSerie();
                ser = ser.getSerie();
                ser.SER_FirstAired_English = inter.vDateTime((node.SelectSingleNode("FirstAired") != null) ? node.SelectSingleNode("FirstAired").InnerText : "");
                ser.SER_imdb_ID = (node.SelectSingleNode("IMDB_ID") != null) ? node.SelectSingleNode("IMDB_ID").InnerText : "";

                string ser_Language = (node.SelectSingleNode("Language") != null) ? node.SelectSingleNode("Language").InnerText : "";
                if (!ser.SER_Languages.Contains(ser_Language))
                    ser.SER_Languages += (!string.IsNullOrEmpty(ser.SER_Languages)) ? "|" + ser_Language : ser_Language;

                ser.SER_DescriptionShort_German = (node.SelectSingleNode("Overview") != null && lan == "de") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_German;
                ser.SER_DescriptionShort_English = (node.SelectSingleNode("Overview") != null && lan == "en") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_English;

                ser.SER_Description_German = (node.SelectSingleNode("Overview") != null && lan == "de") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_German;
                ser.SER_Description_English = (node.SelectSingleNode("Overview") != null && lan == "en") ? node.SelectSingleNode("Overview").InnerText : ser.SER_Description_English;
                ser.SER_RunTime = inter.vInt((node.SelectSingleNode("Runtime") != null) ? node.SelectSingleNode("Runtime").InnerText : "");
                ser.SER_Name_German = (node.SelectSingleNode("SeriesName") != null && lan == "de") ? node.SelectSingleNode("SeriesName").InnerText : ser.SER_Name_German;
                ser.SER_Name_English = (node.SelectSingleNode("SeriesName") != null && lan == "en") ? node.SelectSingleNode("SeriesName").InnerText : ser.SER_Name_English;
                ser.SER_State = (node.SelectSingleNode("Status") != null) ? node.SelectSingleNode("Status").InnerText : "";
                ser.SER_Zap2It_ID = (node.SelectSingleNode("zap2it_id") != null) ? node.SelectSingleNode("zap2it_id").InnerText : "";

                txtLog.Text += Environment.NewLine + "ID: " + ser.SER_theTVDB_ID + " Name: " + ser.SER_Name_German;

                string fanart = (node.SelectSingleNode("fanart") != null) ? node.SelectSingleNode("fanart").InnerText : "";
                string poster = (node.SelectSingleNode("poster") != null) ? node.SelectSingleNode("poster").InnerText : "";
                vTest();

                ser.updateSerie();
                
            }
        }
    }
}
