using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Forms;

namespace VelibFinal
{
    class m_Station
    {
        private XDocument xDoc = XDocument.Load(@"http://www.velib.paris.fr/service/carto");
        private List<Station> stationAll = new List<Station>();
        public List<Station> stationDEpa = new List<Station>();
        public List<Station> stationStatus = new List<Station>();


        private void addStation(int arrondissement, int numero, string adress, int open, int bonus, double lat, double lng)
        {
            Station s = new Station(arrondissement, numero, adress, open, bonus, lat, lng);
            stationAll.Add(s);
        }

        private void addStationUpdate(int available, int free, int total, int ticket, int open, int updated, double lat,double lng)
        {
            Station s = new Station(available, free, total, ticket, open, updated, lat, lng);
            stationStatus.Add(s);
        }

        public void setStations()
        {
                var root = xDoc.Root.Element("markers").Elements("marker");
                foreach (var x in root)
                {
                    string adress = (string)x.Attribute("fullAddress");
                    int arrondissement = selectionArrondissement(adress);
                    int numero = (int)x.Attribute("number");
                    int open = (int)x.Attribute("open");
                    int bonus = (int)x.Attribute("bonus");
                    double lat = (double)x.Attribute("lat");
                    double lng = (double)x.Attribute("lng");
                    addStation(arrondissement, numero, adress, open, bonus, lat, lng);
                }
        }
        public void recupStationStatus(int a)
        {
            XDocument xStats = XDocument.Load(@"http://www.velib.paris.fr/service/stationdetails/paris/" + a);
            var root = xStats.Descendants("station");
            foreach (XElement x in root)
            {
                int available = int.Parse(x.Element("available").Value);
                int free = int.Parse(x.Element("free").Value);
                int total = int.Parse(x.Element("total").Value);
                int ticket = int.Parse(x.Element("ticket").Value);
                int open = int.Parse(x.Element("open").Value);
                int updated = int.Parse(x.Element("updated").Value);
                for (int i = 0; i < this.stationAll.Count-1; i++)
                {
                    Station s = this.listingStations(i);
                    if(s.numero == a)
                    {
                        double lat = s.lat;
                        double lng = s.lng;
                        addStationUpdate(available, free, total, ticket, open, updated, lat, lng);
                    }
                }
            }
        }

        private int selectionArrondissement(string a)
        {
            try
            {
                int resultatRegex = int.Parse(Regex.Match(a, @"\d{5}").Value);
                int arron = resultatRegex / 1000;
                if (arron == 75)
                {
                    int result = int.Parse(resultatRegex.ToString().Substring(3, 2));
                    return result;
                }
                else
                {
                    int result = int.Parse(resultatRegex.ToString().Substring(0, 2));
                    return result;
                }
            }
            catch
            {
                return 0;
            }
        }

        public Station listingStations(int a)
        {
            return this.stationAll[a];
        }

        public Station listingStatutStations(int a)
        {
                return this.stationStatus[a];
        }

        public void dataGrisDisplay(string n)
        {
            this.stationDEpa.Clear();
            for (int i = 0; i < this.stationAll.Count; i++)
            {
                Station s = this.listingStations(i);
                if (s.arrondissement == int.Parse(n))
                    this.stationDEpa.Add(s);
            }
        }
    }
}
