using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Java.Sql;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ChartsPage : ContentPage
    {
        protected SfChart chart;

        public ChartsPage()
        {
            ActivityIndicator indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            Task.WhenAll(Takingcareofbussiness());

            Title = "ChartPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");

            chart = new SfChart();

            //Initializing Primary Axis
            CategoryAxis primaryAxis = new CategoryAxis
            {
                Title = new ChartAxisTitle()
                {
                    Text = "Date"
                }
            };

            //Initializing Secondary Axis
            NumericalAxis secondaryAxis = new NumericalAxis
            {
                Title = new ChartAxisTitle()
                {
                    Text = "Transaction"
                }
            };

            //defining each of the axis used for the graphical representations.
            chart.PrimaryAxis = primaryAxis;    
            chart.SecondaryAxis = secondaryAxis;

            chart.Title = new ChartTitle()
            {
                Text = "Transaction Chart"
            };
        }

        //Taking care of bussiness, verifyifing if the information received of the transaction history is correct
        private async Task Takingcareofbussiness()
        {
            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                Debug.WriteLine("Clicked transaction button");
                var uri = String.Format(Constants.MovementUrl, AccountsPage.Bankid, AccountsPage.Accountid);

                //getting information from the online location
                await rest.GetWithToken(uri).ContinueWith(t =>
                {
                    //Problem occured a message is displayed to the user
                    if (t.IsFaulted)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Alert", "Something went wrong sorry :(", "OK");
                        });
                    }
                    //everything went fine, information should be displayed
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var result = t.Result;

                            Transactions.TransactionList jsonObject =
                                JsonConvert.DeserializeObject<Transactions.TransactionList>(result);

                            List<double> valueList = jsonObject.transactions.Select(x => double.Parse(x.details.value.amount)).ToList();
                            List<double> newBalanceList = jsonObject.transactions.Select(x => double.Parse(x.details.new_balance.amount)).ToList();
                            List<String> completedDateList = jsonObject.transactions.Select(x => x.details.completed).ToList();

                            //Collection of the value amount and the dates of the transactions made
                            ObservableCollection<ChartDataPoint> chartsvalue = new ObservableCollection<ChartDataPoint>();

                            //Collection of the users new balance amounts and the dates of the changes that may have occured
                            ObservableCollection<ChartDataPoint> chartsnewbalance = new ObservableCollection<ChartDataPoint>();

                            //adding the information to each of the charts collections
                            for (var i = 0; i < jsonObject.transactions.Count; i++)
                            {
                                chartsvalue.Add(new ChartDataPoint(completedDateList[i], valueList[i]));
                                Debug.WriteLine("Date :: {0} amount ::{1}", completedDateList[i], valueList[i]);
                                chartsnewbalance.Add(new ChartDataPoint(completedDateList[i], newBalanceList[i]));
                                Debug.WriteLine("Date :: {0} newbalance ::{1}", completedDateList[i], newBalanceList[i]);
                            }
                            Debug.WriteLine("chartsvalue::{0}", chartsvalue.Count);
                            Debug.WriteLine("chartsnewbalance :: {0}", chartsnewbalance.Count);

                            //creates a column series allowing for some animation and for the user to select and see inforamtion
                            chart.Series.Add(new ColumnSeries()
                            {
                                ItemsSource = chartsnewbalance,
                                Label = "NewBalance",
                                Color = Color.Teal,
                                EnableAnimation = true,
                                AnimationDuration = 0.8,
                                EnableTooltip = true
                        });

                            //Creates a line series allowing for some animation and for the user to select and see inforamtion
                            chart.Series.Add(new SplineSeries()
                            {
                                ItemsSource = chartsvalue,
                                Label = "Value",
                                Color = Color.Blue,
                                EnableAnimation = true,
                                AnimationDuration = 0.8,
                                EnableTooltip = true
                            });

                            //To show the items used creating a chart legend
                            chart.Legend = new ChartLegend();
                        });
                    }
                });
                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;

                Content = chart;
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "Internet problems cant receive information", "OK");
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }
    }
}