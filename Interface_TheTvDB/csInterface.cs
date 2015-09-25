using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Interface_TheTvDB
{

    class csInterface
    {
        
        string api_Key = "5B0926479125850E";
        public string API_Key
        {
            get { return api_Key; }
        }

        string path = "C:\\temp";
        public string Path
        {
            get { return path; }
        }

        string webPath = @"http://217.160.178.136:8023/217.160.178.136:8023\images";
        public string WebPath
        {
            get { return webPath; }
        }

        // ZIP Runterladen
        public string downloadZIP(string language, string seriesID)
        {

            string zipPath = path + "\\" + language + "_" + seriesID + ".zip";
            string extractPath = path + "\\" + language + "_" + seriesID;
            string link = "http://thetvdb.com/api/" + API_Key + "/series/" + seriesID + "/all/" + language + ".zip";

            // Pfad prüfen und ggfl. erstellen
            if (Directory.Exists(extractPath))
            {
                //später return entfernen
                return extractPath;
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

            return extractPath;
        }

        public static string vString(object value_)
        {
            string value = "";
            
            if (value_ != null)
            {
                value = value_.ToString();
            }

            return value;
        }

        public double vDouble(object value_)
        {
            double value = 0;
            double.TryParse(value_.ToString(), out value);
            return value;
        }

        public int vInt(object value_)
        {
            int value = 0;
            int.TryParse(value_.ToString(), out value);
            return value;
        }

        public DateTime vDateTime(object value_)
        {
            DateTime value = new DateTime(1900, 1, 1);
            DateTime.TryParse(value_.ToString(), out value);

            if (value.Year < 1900)
                value = new DateTime(1900, 1, 1);

            return value;
        }

        public string vDE(string value_, string language)
        {
            string value;
            value = (language == "de") ? value_ : "";
            return value;
        }

        public string vEN(string value_, string language)
        {
            string value;
            value = (language == "en") ? value_ : "";
            return value;
        }



    }
    
}

