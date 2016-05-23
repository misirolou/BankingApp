using System.Collections.Generic;

namespace App1.Models
{
    public class Addressbranch
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string line_3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
    }

    public class Locationbranch
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Lobbybranch
    {
        public string hours { get; set; }
    }

    public class DriveUpbranch
    {
        public string hours { get; set; }
    }

    public class Licensebranch
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Metabranch
    {
        public Licensebranch license { get; set; }
    }

    public class Branch
    {
        public string id { get; set; }
        public string name { get; set; }
        public Addressbranch address { get; set; }
        public Locationbranch location { get; set; }
        public Lobbybranch lobby { get; set; }
        public DriveUpbranch drive_up { get; set; }
        public Metabranch meta { get; set; }
    }

    public class branchlist
    {
        public List<Branch> branches { get; set; }
    }
}