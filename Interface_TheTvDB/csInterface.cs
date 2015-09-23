using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

