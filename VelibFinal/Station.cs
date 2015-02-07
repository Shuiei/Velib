using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velibfinal
{
    class Station
    {
        public Station(int arrondissement, int numero, string adress, int open, int bonus)
        {
            this.numero = numero;
            this.adress = adress;
            this.arrondissement = arrondissement;
            this.open = open;
            this.bonus = bonus;
        }

        public int arrondissement { get; private set; }
        public int numero { get; private set; }
        public string adress { get; private set; }
        public int open { get; private set; }
        public int bonus { get; set; }
    }
}
