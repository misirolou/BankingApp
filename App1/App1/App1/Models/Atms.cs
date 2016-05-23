using System.Collections.Generic;

namespace App1.Models
{
    public class Atm
    {
        public string id { get; set; }
        public string name { get; set; }
        public Addressatm address { get; set; }
        public Locationatm location { get; set; }
        public Metaatm meta { get; set; }
    }

    public class Addressatm
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string line_3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
    }

    public class Locationatm
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Licenseatm
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Metaatm
    {
        public Licenseatm license { get; set; }
    }

    public class atmlist
    {
        public List<Atm> atms { get; set; }
    }
}