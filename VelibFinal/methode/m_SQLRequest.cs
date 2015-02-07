using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml.Linq;
using System.Windows.Forms;

namespace VelibFinal
{
    class m_SQLRequest
    {
        SQLiteConnection connect = new SQLiteConnection(@"Data Source=sqlvelibbbb.s3db");
        private List<Station> stationSQLold = new List<Station>();
        private List<Station> stationSQLupdate = new List<Station>();

        private void addSQLold(int numero, int available, int free)
        {
            Station s = new Station(numero, available, free);
            stationSQLold.Add(s);
        }

        private void addSQLupdate(int numero, int available, int free)
        {
            Station s = new Station(numero, available, free);
            stationSQLupdate.Add(s);
        }

        private void querySQLold()
        {
            stationSQLold.Clear();
            stationSQLupdate.Clear();
            connect.Open();
            string select = "SELECT numero,moyenneAvailable,moyenneFree FROM moyenneStation";
            SQLiteCommand command = new SQLiteCommand(select, connect);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                 addSQLold(Convert.ToInt32(reader["numero"]),Convert.ToInt32(reader["moyenneAvailable"]),Convert.ToInt32(reader["moyenneFree"]));
                 System.Threading.Thread.Sleep(10);
            }
            reader.Close();
            connect.Close();
        }

        private void querySQLupdate()
        {
            connect.Open();
            int z = 0;
            for (int i = 0; i < this.stationSQLold.Count - 1; i++)
            {
                Station s = listingSQLold(i);
                XDocument xStats = XDocument.Load(@"http://www.velib.paris.fr/service/stationdetails/paris/" + s.numero);
                var root = xStats.Descendants("station");
                foreach (XElement x in root)
                {
                    int available = int.Parse(x.Element("available").Value);
                    int free = int.Parse(x.Element("free").Value);
                    addSQLupdate(s.numero, available, free);
                    ++z;
                    if (z % 50 == 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            connect.Close();
        }

        public void queryUpdateSQLbase()
        {
            
            querySQLold();
            querySQLupdate();
            connect.Open();
            for (int i = 0; i < stationSQLold.Count-1; i++)
            {
                Station s = this.listingSQLold(i);
                Station c = this.listingSQLupdate(i);
                int available = s.avalaible + c.avalaible;
                int free = s.free + c.free;

                if (s.numero == c.numero)
                {
                    string updatestation = "UPDATE moyenneStation SET moyenneAvailable="+available+", moyenneFree="+free+", nbUtilisation=nbUtilisation+1 WHERE numero="+s.numero+";";
                    SQLiteCommand cmd = new SQLiteCommand(updatestation, connect);
                    cmd.ExecuteNonQuery();
                }
            }

            connect.Close();
        }


        public Station listingSQLold(int a)
        {
            return this.stationSQLold[a];
        }
        public Station listingSQLupdate(int a)
        {
            return this.stationSQLupdate[a];
        }
    }
}
