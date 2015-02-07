using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Velibfinal
{
    class ClassementStation : Form1
    {
        private XDocument xDoc = XDocument.Load(@"http://www.velib.paris.fr/service/carto");
        private List<Station> mesStations;
        public Form1 otherForm = new Form1();
        public ClassementStation()
        {
            this.mesStations = new List<Station>();
        }



        private void addStation(int arrondissement, int numero, string adress, int open, int bonus)
        {
            Station s = new Station(arrondissement, numero, adress, open, bonus);
            mesStations.Add(s);
        }

        public void classementStation()
        {
            var root = xDoc.Root.Element("markers").Elements("marker");
            foreach (var x in root)
            {
                int numero = (int)x.Attribute("number");
                string adress = (string)x.Attribute("address");
                int open = (int)x.Attribute("open");
                int bonus = (int)x.Attribute("bonus");
                string fullAdress = (string)x.Attribute("fullAddress");
                int arrondissement = selectionArrondissement(fullAdress);
                addStation(arrondissement, numero, adress, open, bonus);
            }
        }

        private int selectionArrondissement(string arrondissement)
        {
            int resultatRegex = int.Parse(Regex.Match(arrondissement, @"\d{5}").Value);
            int arron = resultatRegex / 1000;
            if (arron == 75)
            {
                int result = int.Parse(resultatRegex.ToString().Substring(3, 2));
                return result;
            }
            else
            {
                int result = int.Parse(resultatRegex.ToString().Substring(1, 2));
                return result;
            }
        }

        public void listingStations(int a)
        {
            foreach (var i in this.mesStations)
            {
                if (i.arrondissement == a)
                {
                    //otherForm.Mescouilles.Items.Add(i.adress);
                }
            }
        }
    }
}
