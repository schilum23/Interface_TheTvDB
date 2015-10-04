using System;
using System.Data;
using System.IO;
using System.Net;
using System.Xml;

namespace Interface_TheTvDB
{
    class csFunctions
    {
        // String
        public static string vString(object value_)
        {
            string value = "";

            if (value_ != null)
            {
                value = value_.ToString();
            }

            return value;
        }

        // Double
        public static double vDouble(object value_)
        {
            double value = 0;
            double.TryParse(value_.ToString(), out value);
            return value;
        }

        // Integer
        public static int vInt(object value_)
        {
            int value = 0;
            int.TryParse(value_.ToString(), out value);
            return value;
        }

        // Date
        public static DateTime vDateTime(object value_)
        {
            DateTime value = new DateTime(1900, 1, 1);
            DateTime.TryParse(value_.ToString(), out value);

            if (value.Year < 1900)
                value = new DateTime(1900, 1, 1);

            return value;
        }

        // Date UTC
        public static DateTime vDateTimeUTC(object value_)
        {
            long mil = 0;

            if (!long.TryParse(value_.ToString(), out mil))
            {
                return new DateTime(1900, 1, 1);
            }

            DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            value = value.AddSeconds(mil);

            if (value.Year < 1900)
                value = new DateTime(1900, 1, 1);

            return value;
        }

        // Bilder runterladen
        public static string downloadImage(string imagePath, string id, string type, string folder, string newFilename)
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
            if (File.Exists(path + newFilename))
                return path + newFilename;

            // Bild runterladen
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(link, path + newFilename);

                NetworkCredential cred = new NetworkCredential("olro", "123user!");


                WebClient ftp = new WebClient();
                ftp.Credentials = cred;
                ftp.UploadFile("ftp://217.160.178.136/Images/" + folder + "/" + newFilename, path + newFilename);
            }

            return path + localFilename;
        }

        // 2 Datenzeilen vergleichen
        public static bool rowsAreEuqal(DataRow row1, DataRow row2)
        {
            foreach (DataColumn col in row1.Table.Columns)
            {
                if (row2.Table.Columns.Contains(col.ColumnName)) {
                    if (!row1[col.ColumnName].Equals(row2[col.ColumnName]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Get Text from XML Node
        public static string xmlNodeInnerText(XmlNode node, string name)
        {
            if (node.SelectSingleNode(name) != null)
            {
                return node.SelectSingleNode(name).InnerText;
            }
            return "";
        }

        public static DataRow getDataRowFromClass(object cs, DataRow dr)
        {
            if (dr == null || cs == null)
                return dr;

            foreach (var prop in cs.GetType().GetProperties())
            {
                if (dr.Table.Columns.Contains(prop.Name) && !dr.Table.Columns[prop.Name].ReadOnly)
                {
                    object value = prop.GetValue(cs, null);
                    if (value != null)
                    {
                        dr[prop.Name] = value;
                    }
                }
            }
            return dr;
        }

        public static object getClassFromDataRow(DataRow dr, object cs)
        {
            if (dr == null || cs == null)
                return cs;

            foreach (var prop in cs.GetType().GetProperties())
            {
                if (dr.Table.Columns.Contains(prop.Name))
                {
                    if (dr[prop.Name] != DBNull.Value)
                        prop.SetValue(cs, dr[prop.Name], null);
                }
            }
            return cs;
        }

        public static string vDE(string value_, string language)
        {
            string value;
            value = (language == "de") ? value_ : "";
            return value;
        }

        public static string vEN(string value_, string language)
        {
            string value;
            value = (language == "en") ? value_ : "";
            return value;
        }
    }
}
