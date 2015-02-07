using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VelibFinal
{
    public partial class Option : Form
    {
        Form1 form = new Form1();

        public Option()
        {
            InitializeComponent();
        }

        private void tBoxActu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                form.tictac = int.Parse(tBoxActu.Text);
                form.timer1.Interval = (form.tictac * 60000);
                MessageBox.Show("Timer set à" + form.timer1.Interval);
            }
        }

        private void Option_Load(object sender, EventArgs e)
        {
            tBoxActu.Text = form.tictac.ToString();
        }
    }
}
