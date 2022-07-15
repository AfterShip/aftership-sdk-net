using System;
using Newtonsoft.Json.Linq;


namespace AftershipAPI
{
    public class LatestEstimatedDelivery
    {
        /// <summary>
        /// The format of the EDD. Either a single date or a date range.
        /// </summary>
        private String _type;

        /// <summary>
        /// The source of the EDD. Either the carrier, AfterShip AI, or based on your custom EDD settings.
        /// </summary>
        private String _source;

        /// <summary>
        /// The latest EDD time.
        /// </summary>
        private String _datetime;

        /// <summary>
        /// For a date range EDD format, the date and time for the lower end of the range.
        /// </summary>
        private String _datetimeMin;

        /// <summary>
        /// For a date range EDD format, the date and time for the upper end of the range.
        /// </summary>
        private String _datetimeMax;

        public LatestEstimatedDelivery(JObject json)
        {
            this.type = json["type"].IsNullOrEmpty() ? null : (String) json["type"];
            this.source = json["source"].IsNullOrEmpty() ? null : (String) json["source"];
            this.datetime = json["datetime"].IsNullOrEmpty() ? null : (String) json["datetime"];
            this.datetimeMin = json["datetime_min"].IsNullOrEmpty() ? null : (String) json["datetime_min"];
            this._datetimeMax = json["datetime_max"].IsNullOrEmpty() ? null : (String) json["datetime_max"];
        }

        public String type
        {
            get { return _type; }
            set { _type = value; }
        }

        public String source
        {
            get { return _source; }
            set { _source = value; }
        }

        public String datetime
        {
            get { return _datetime; }
            set { _datetime = value; }
        }

        public String datetimeMin
        {
            get { return _datetimeMin; }
            set { _datetimeMin = value; }
        }

        public String datetimeMax
        {
            get { return _datetimeMax; }
            set { _datetimeMax = value; }
        }
    }
}
