using System.Collections.Generic;

namespace VnBusInfoW10.Model
{
    public class COL
    {
        public string DATA { get; set; }
    }

    public class ROW
    {
        public List<COL> COL { get; set; }
    }

    public class TABLE
    {
        public List<ROW> ROW { get; set; }
    }

    public class RawBusStationRootObject
    {
        public List<TABLE> TABLE { get; set; }
    }
}
