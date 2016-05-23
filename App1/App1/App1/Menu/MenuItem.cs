using System;

namespace App1.Menu
{
    public class MenuItem
    {
        //Title that will be presented in the menu
        public string Title { get; set; }

        //to indicate the page type that it will change to
        public Type TargetType { get; set; }
    }
}