using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using Microsoft.Maps.MapControl.WPF;

namespace VelibFinal
{
    public partial class Form1 : Form
    {
        m_Station m_station = new m_Station();
        m_SQLRequest m_sqlRequest = new m_SQLRequest();
        List<Station> dataGridStation = new List<Station>();
        List<Station> statusStation = new List<Station>();
        double zoomLevel = 12.0f;
        Location loc;
        bool activeActu = false;
        public int tictac { get; set; }


        public Form1()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(timer1_Tick);
            tictac = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridStation = m_station.stationDEpa;
            m_station.setStations();
            userControl12.Map.CredentialsProvider = new ApplicationIdCredentialsProvider("AlBOblZkREI8hEI2fHXkQZ-o8Sjl-u7gj-TV6mPSTdQKfGlSWMl96NaFMHRyggRO");
            Location loc = new Location(48.8574711, 2.3383117);
            userControl12.Map.SetView(loc, zoomLevel);
            label6.Text = zoomLevel.ToString();
            zoomLevel = trackBar1.Value;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.dgView.DataSource = null;
                string depa = cbDepa.SelectedItem.ToString();
                m_station.dataGrisDisplay(depa);
                this.dgView.DataSource = dataGridStation;
            }
            catch
            {
                MessageBox.Show("Une erreur est survenue lors de l'affichage des Stations.");
            }
        }

        private void dgView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    userControl12.Map.Children.Clear();
                    statusStation.Clear();
                    int ID = (int)dgView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    statusStation = m_station.stationStatus;
                    m_station.recupStationStatus(ID);
                    Station s = m_station.listingStatutStations(0);
                    lbDispo.Text = s.free.ToString();
                    lbDispoV.Text = s.avalaible.ToString();
                    lbMaxi.Text = s.total.ToString();
                    lbStatut.Text = s.open == 1 ? "Ouvert" : "Fermé";
                    lbCB.Text = s.ticket == 1 ? "Ouvert" : "Fermé";
                    loc = new Location(s.lng, s.lat);
                    userControl12.Map.SetView(loc, zoomLevel);
                    Pushpin pin = new Pushpin();
                    pin.Location = loc;
                    userControl12.Map.Children.Add(pin);
                    userControl12.UpdateLayout();
                }
            }
            catch
            {
                MessageBox.Show("Veuillez Selectionner le numero d'une station.");
            }
        }

        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Option option = new Option();
            option.Show();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            zoomLevel = trackBar1.Value;
            label6.Text = zoomLevel.ToString();
            try
            {
                userControl12.Map.SetView(loc, zoomLevel);
            }
            catch
            {
                MessageBox.Show("Veuillez d'abord séléctionner un Station.");
            }
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            credits credits = new credits();
            credits.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help help = new help();
            help.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            listBox1.Items.Add("tic tac");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            m_sqlRequest.queryUpdateSQLbase();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listBox1.Items.Add("Actualisation des données Terminées");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void démaréActualisationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activeActu == false)
            {
                Option option = new Option();
                activeActu = true;
                timer1.Start();
                timer1.Interval = 3600000;
                MessageBox.Show("Actualisation Start");
                listBox1.Items.Add("Actualisation des données en cours");
                System.Threading.Thread.Sleep(20);
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("Actualisation Stoppée");
            }
        }
    }
}
