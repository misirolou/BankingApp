using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.Models;
using Xamarin.Forms;

namespace App1.Cell.ListViews
{
    class AtmListViews : ListView
    {
        public static string Latitude { get; private set; }
        public static string Longitude { get; private set; }

        public AtmListViews()
        {
            var locations = new branchlist();

            var cell = new DataTemplate(typeof(TextCell));

            cell.SetBinding(TextCell.TextProperty, "latitude");
            cell.SetBinding(TextCell.DetailProperty, "longitude");

            ItemTemplate = cell;

            ItemsSource = locations.branches;

            ItemSelected += (s, e) => {
                if (SelectedItem == null)
                    return;
                var selected = (Branch)e.SelectedItem;
                SelectedItem = null;
            };
        }


        public void FilterLocations(string filter)
        {
            this.BeginRefresh();

            /* if (string.IsNullOrWhiteSpace(filter))
             {
                 this.ItemsSource = branchlist;
             }
             else
             {
                 this.ItemsSource = locations
                         .Where(x => x.Title.ToLower()
                            .Contains(filter.ToLower()));
             }
             */
            this.EndRefresh();
        }

    }
}
