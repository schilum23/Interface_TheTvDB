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
        public string API_Key
        {
            get { return "5B0926479125850E"; }
        }

        public string Path
        {
            get { return "C:\\temp"; }
        }

        string webPath = @"http://217.160.178.136:8023/217.160.178.136:8023\images";
        public string WebPath
        {
            get { return webPath; }
        }

        // ZIP Runterladen
        public string downloadZIP(string language, string seriesID)
        {

            string zipPath = Path + "\\" + language + "_" + seriesID + ".zip";
            string extractPath = Path + "\\" + language + "_" + seriesID;
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





    }
    
}

