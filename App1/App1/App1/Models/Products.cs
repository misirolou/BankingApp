using System.Collections.Generic;

namespace App1.Models
{
    public partial class Licenseprod
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Metaprod
    {
        public Licenseprod license { get; set; }
    }

    public class Products
    {
        public string code { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string family { get; set; }
        public string super_family { get; set; }
        public string more_info_url { get; set; }
        public Metaprod meta { get; set; }
    }

    public class productlist
    {
        public List<Products> products { get; set; }
    }
}