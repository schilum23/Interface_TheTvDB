using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Interface_TheTvDB.csInterface;

namespace Interface_TheTvDB
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        public static string vTest(string value_, string language)
        {
            vString("");
            string value;
            value = (language == "en") ? value_ : "";
            return value;
        }
    }

}
