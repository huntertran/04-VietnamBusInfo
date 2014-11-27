using System.Collections.ObjectModel;

namespace GetData.Model
{
    public class BusTextData
    {
        public string go { get; set; }
        public string back { get; set; }
        public string timeInfo { get; set; }
    }

    public class BusName
    {
        public string number { get; set; }

        public string name { get; set; }

        public BusTextData busTextData { get; set; }
    }

    public class BusNameList
    {
        public ObservableCollection<BusName> busNameCollection { get; set; } 
    }
}
