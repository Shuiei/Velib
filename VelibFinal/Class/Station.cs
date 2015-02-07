using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Maps.MapControl.WPF;

namespace VelibFinal
{
    class Station
    {
        public Station(int arrondissement, int numero, string adress, int open, int bonus, double lng, double lat)
        {
            this.numero = numero;
            this.adress = adress;
            this.arrondissement = arrondissement;
            this.open = open;
            this.bonus = bonus;
            this.lat = lat;
            this.lng = lng;
        }
        public Station(int avalaible, int free, int total, int ticket, int open)
        {
            this.avalaible = avalaible;
            this.free = free;
            this.total = total;
            this.ticket = ticket;
            this.open = open;
        }

        public Station(int avalaible, int free, int total, int ticket, int open, int updated)
        {
            this.avalaible = avalaible;
            this.free = free;
            this.total = total;
            this.ticket = ticket;
            this.open = open;
            this.updated = updated;
        }

        #region Acesseurs
        public int arrondissement { get; set; }
        public int numero { get; set; }
        public string adress { get; set; }
        public int open { get; set; }
        public int bonus { get; set; }
        public int ticket { get; set; }
        public int total { get; set; }
        public int free { get; set; }
        public int avalaible { get; set; }
        public int updated { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        #endregion 
    }
}
