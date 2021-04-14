using System;
using AftershipAPI.Enums;
using Newtonsoft.Json.Linq;


namespace AftershipAPI
{
    public class Checkpoint
    {
        /// Date and time of the tracking created. 
        private DateTime _createdAt;

        /// Date and time of the checkpoint, provided by courier. Value may be:
        ///Empty String,
        ///YYYY-MM-DD,
        ///YYYY-MM-DDTHH:MM:SS, or
        ///YYYY-MM-DDTHH:MM:SS+TIMEZONE 
        private String _checkpointTime;

        /// Location info (if any) 
        private String _city;

        /// Country ISO Alpha-3 (three letters) of the checkpoint 
        private ISO3Country _countryISO3;

        /// Country name of the checkpoint, may also contain other location info. 
        private String _countryName;

        /// Checkpoint message 
        private String _message;

        /// Location info (if any) 
        private String _state;

        /// Status of the checkpoint 
        private String _tag;

        /// Location info (if any) 
        private String _zip;

        /// Unique code of courier
        private String _slug;

        /// Location info provided by carrier
        private String _location;

        /// Normalized checkpoint message
        private String _subtag_message;

        public Checkpoint(JObject checkpointJSON)
        {
            // Console.WriteLibe(typeof(checkpointJSON["created_at"]));
            this.createdAt = checkpointJSON["created_at"].IsNullOrEmpty() ? DateTime.MinValue :
                (DateTime)checkpointJSON["created_at"];
            this.checkpointTime = checkpointJSON["checkpoint_time"].IsNullOrEmpty() ? null : (String)checkpointJSON["checkpoint_time"];
            this.city = checkpointJSON["city"].IsNullOrEmpty() ? null : (String)checkpointJSON["city"];
            this.countryISO3 = checkpointJSON["country_iso3"].IsNullOrEmpty() ? 0 :
                (ISO3Country)Enum.Parse(typeof(ISO3Country), (String)checkpointJSON["country_iso3"]);
            this.countryName = checkpointJSON["country_name"].IsNullOrEmpty() ? null : (String)checkpointJSON["country_name"];
            this.message = checkpointJSON["message"].IsNullOrEmpty() ? null : (String)checkpointJSON["message"];
            this.state = checkpointJSON["state"].IsNullOrEmpty() ? null : (String)checkpointJSON["state"];
            this.tag = checkpointJSON["tag"].IsNullOrEmpty() ? null : (String)checkpointJSON["tag"];
            this.zip = checkpointJSON["zip"].IsNullOrEmpty() ? null : (String)checkpointJSON["zip"];
            this.slug = checkpointJSON["slug"].IsNullOrEmpty() ? null : (String)checkpointJSON["slug"];
            this.location = checkpointJSON["location"].IsNullOrEmpty() ? null : (String)checkpointJSON["location"];
            this.subtagMessage = checkpointJSON["subtag_message"].IsNullOrEmpty() ? null : (String)checkpointJSON["subtag_message"];
        }

        public DateTime createdAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public String checkpointTime
        {
            get { return _checkpointTime; }
            set { _checkpointTime = value; }
        }

        public String city
        {
            get { return _city; }
            set { _city = value; }
        }

        public ISO3Country countryISO3
        {
            get { return _countryISO3; }
            set { _countryISO3 = value; }
        }

        public String countryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        public String message
        {
            get { return _message; }
            set { _message = value; }
        }

        public String state
        {
            get { return _state; }
            set { _state = value; }
        }

        public String tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public String zip
        {
            get { return _zip; }
            set { _zip = value; }
        }

        public String slug
        {
            get { return _slug; }
            set { _slug = value; }
        }

        public String location
        {
            get { return _location; }
            set { _location = value; }
        }

        public String subtagMessage
        {
            get { return _subtag_message; }
            set { _subtag_message = value; }
        }

    }
}

