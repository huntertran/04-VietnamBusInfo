using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using HtmlAgilityPack;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model;
using VnBusInfoW10.Utilities;

namespace VnBusInfoW10.ViewModel.SettingGroup
{
    public class UpdateDatabaseViewModel : INotifyPropertyChanged
    {
        Stopwatch sw = new Stopwatch();

        private string _textNotification;

        private double _updateProgress;
        private int _notiCount;

        private string _runTime;

        public string TextNotification
        {
            get { return _textNotification; }
            set
            {
                if (value == _textNotification) return;
                _textNotification = value;
                OnPropertyChanged();
            }
        }

        public double UpdateProgress
        {
            get { return _updateProgress; }
            set
            {
                if (value.Equals(_updateProgress)) return;
                _updateProgress = value;
                OnPropertyChanged();
            }
        }

        public int NotiCount
        {
            get { return _notiCount; }
            set
            {
                if (value == _notiCount) return;
                _notiCount = value;
                OnPropertyChanged();
            }
        }

        public string RunTime
        {
            get { return _runTime; }
            set
            {
                if (value == _runTime) return;
                _runTime = value;
                OnPropertyChanged();
            }
        }

        public async Task GetData()
        {
            NotiCount = 0;
            await Task.Run(() => GetBusInfo());
        }

        private async Task GetBusInfo()
        {
            string html = await NetwordMethod.GetHttpAsString("http://buyttphcm.com.vn/TTLT.aspx");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes =
                doc.DocumentNode
                    .Descendants("select")
                    .First(d => d.Attributes.Contains("id") &&
                                d.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_DropDownList1")
                    .Descendants("option")
                    .ToList();

            StaticData.MapVm.AllBus = new ObservableCollection<BusTotal>();

            for (int index = 1; index < nodes.Count; index++)
            {
                HtmlNode htmlNode = nodes[index];
                BusTotal b = new BusTotal();
                b.Id = index;
                b.TextInfo = new BusTextInfo();
                b.TextInfo.RouteNumber = htmlNode.Attributes["value"].Value;

                string[] temp = WebUtility.HtmlDecode(htmlNode.NextSibling.InnerText).Split('-');
                string r = "";
                foreach (string s in temp)
                {
                    try
                    {
                        Convert.ToInt32(s.Trim());
                    }
                    catch (Exception)
                    {
                        r = r + "-" + s;
                    }
                }

                b.TextInfo.RouteName = r.TrimStart(' ','-');
                StaticData.MapVm.AllBus.Add(b);

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    Notify("New bus info added: " + b.TextInfo.RouteNumber + " " + b.TextInfo.RouteName);
                });

                
            }
        }

        private void Notify(string s)
        {
            NotiCount++;
            TextNotification = s;
        }

        private void Start()
        {
            sw = Stopwatch.StartNew();
        }

        private void Stop()
        {
            sw.Stop();

            var elapsedMs = sw.ElapsedMilliseconds;
            RunTime = elapsedMs.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
