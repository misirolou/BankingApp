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

            chart.PrimaryAxis = primaryAxis;
            chart.SecondaryAxis = secondaryAxis;

            chart.Title = new ChartTitle()
            {
                Text = "Transaction Chart"
            };

            //Content = chart;
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
                            List<String> CompletedDateList = jsonObject.transactions.Select(x => x.details.value.amount).ToList();
                            Debug.WriteLine("valuelist:: {0}",valueList.Count);
                            Debug.WriteLine("newbalancelist :: {0}", newBalanceList.Count);
                            Debug.WriteLine("dates ::{0}", CompletedDateList.Count);

                            ObservableCollection<ChartDataPoint> chartsvalue = new ObservableCollection<ChartDataPoint>();

                            for(int i = 0; i > jsonObject.transactions.Count; i++)
                            {
                                chartsvalue.Add(new ChartDataPoint(CompletedDateList[i], valueList[i]));
                                Debug.WriteLine("Date :: {0} amount ::{1}", CompletedDateList[i], valueList[i]);
                            }
                            Debug.WriteLine("chartsvalue::{0}", chartsvalue.Count);

                            ObservableCollection<ChartDataPoint> chartsnewbalance = new ObservableCollection<ChartDataPoint>();

                            for(int j = 0; j > jsonObject.transactions.Count; j++)
                            {
                                chartsnewbalance.Add(new ChartDataPoint(CompletedDateList[j], newBalanceList[j]));
                                Debug.WriteLine("Date :: {0} newbalance ::{1}", CompletedDateList[j], newBalanceList[j]);
                            }
                            Debug.WriteLine("chartsnewbalance :: {0}", chartsnewbalance);

                            //Test made to verify if the charts will work
                          /*  ObservableCollection<ChartDataPoint> hightemp = new ObservableCollection<ChartDataPoint>();

                            hightemp.Add(new ChartDataPoint("Jan", -42));
                            hightemp.Add(new ChartDataPoint("Feb", 44));
                            hightemp.Add(new ChartDataPoint("Mar", 53));
                            hightemp.Add(new ChartDataPoint("Apr", -64));
                            hightemp.Add(new ChartDataPoint("May", 75));
                            hightemp.Add(new ChartDataPoint("Jun", 83));
                            hightemp.Add(new ChartDataPoint("Jul", 87));
                            hightemp.Add(new ChartDataPoint("Aug", 84));
                            hightemp.Add(new ChartDataPoint("Sep", 78));
                            hightemp.Add(new ChartDataPoint("Oct", 67));
                            hightemp.Add(new ChartDataPoint("Nov", -55));
                            hightemp.Add(new ChartDataPoint("Dec", 45));*/

                            //creates a column series
                            chart.Series.Add(new ColumnSeries()
                            {
                                ItemsSource = chartsnewbalance,
                                Label = "NewBalance",
                                // XBindingPath = "details.completed",
                                //YBindingPath = "details.new_balance.amount", 
                                Color = Color.Red
                            });

                            //Creates a line series
                            chart.Series.Add(new SplineSeries()
                            {
                                ItemsSource = chartsvalue,
                                Label = "Value",
                                //XBindingPath = "details.completed",
                                //YBindingPath = "details.value.amount", 
                                Color = Color.Blue
                            });
                            //To show the items legend used
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