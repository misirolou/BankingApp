using System;
using App1.Models;
using Xamarin.Forms;

namespace App1.Cell
{
    internal class Cells : ViewCell
    {
        //Aspect of the cells that will display the information of each of the banks
        public Cells()
        {
            //Id labels identification and layout
            Label IdLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            IdLabel.SetBinding(Label.TextProperty, "id");
            //shortnames labels identification and layout
            Label nameLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            nameLabel.SetBinding(Label.TextProperty, "short_name");
            //fullnames labels identification and layout
            Label fullLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            fullLabel.SetBinding(Label.TextProperty, "full_name");
            //logo labels identification and layout
            Label logoLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            logoLabel.SetBinding(Label.TextProperty, "logo");
            //website labels identification and layout
            Label webLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            webLabel.SetBinding(Label.TextProperty, "website");

            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { IdLabel, fullLabel }
            };

            //this is the actual layout of each of the cells
            var nameLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { stack, webLabel }
            };
            View = nameLayout;
        }

        //Leaving this code here, this was supposed to be a more intuitive way of displaying information for other pages
        //but it seems not to work may need some help over here
        //This code is used in properties that may change according to the user doing something wont be used but i´ll leave the example
        // private Label idLabel, nameLabel, fullLabel, LogoLabel, webLabel;

        /*   public static readonly BindableProperty IDProperty =
       BindableProperty.Create("id", typeof(string), typeof(Cells), "");

           public string id
           {
               get { return (string)GetValue(IDProperty); }
               set { SetValue(IDProperty, value); }
           }

           public static readonly BindableProperty NameProperty =
       BindableProperty.Create("short_name", typeof(string), typeof(Cells), "");

           public string short_name
           {
               get { return (string)GetValue(NameProperty); }
               set { SetValue(NameProperty, value); }
           }

           public static readonly BindableProperty full_nameProperty =
             BindableProperty.Create("full_name", typeof(string), typeof(Cells), "");

           public string full_name
           {
               get { return (string)GetValue(full_nameProperty); }
               set { SetValue(full_nameProperty, value); }
           }

           public static readonly BindableProperty LogoProperty =
             BindableProperty.Create("logo", typeof(string), typeof(Cells), "");

           public string Logo
           {
               get { return (string)GetValue(LogoProperty); }
               set { SetValue(LogoProperty, value); }
           }

           public static readonly BindableProperty WebsiteProperty =
             BindableProperty.Create("website", typeof(string), typeof(Cells), "");

           public string website
           {
               get { return (string)GetValue(WebsiteProperty); }
               set { SetValue(WebsiteProperty, value); }
           }

           public static readonly BindableProperty MenuTitleProperty =
             BindableProperty.Create("menu", typeof(string), typeof(Cells), "");

           public string MenuTitle
           {
               get { return (string)GetValue(MenuTitleProperty); }
               set { SetValue(MenuTitleProperty, value); }
           }

           protected override void OnBindingContextChanged()
           {
               base.OnBindingContextChanged();

               if (BindingContext != null)
               {
                   if (idLabel != null) idLabel.Text = "id" + id;
                   if (nameLabel != null) nameLabel.Text = "shortname" + short_name;
                   if (fullLabel != null) fullLabel.Text = "fullname" + full_name;
                   if (LogoLabel != null) LogoLabel.Text = "logo" + Logo;
                   if (webLabel != null) webLabel.Text = "website" + website;
               }
           }*/
    }
}