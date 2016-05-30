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

		public Checkpoint (JObject checkpointJSON){
           // Console.WriteLibe(typeof(checkpointJSON["created_at"]));
            this.createdAt = checkpointJSON["created_at"]== null? DateTime.MinValue:
                (DateTime)checkpointJSON["created_at"];
			this.checkpointTime = checkpointJSON["checkpoint_time"]==null?null:(String)checkpointJSON["checkpoint_time"];
			this.city = checkpointJSON["city"]==null?null:(String)checkpointJSON["city"];
			this.countryISO3 = checkpointJSON["country_iso3"]==null?0:
				(ISO3Country)Enum.Parse(typeof(ISO3Country), (String)checkpointJSON["country_iso3"]);
			this.countryName = checkpointJSON["country_name"]==null?null:(String)checkpointJSON["country_name"];
			this.message = checkpointJSON["message"]==null?null:(String)checkpointJSON["message"];
			this.state = checkpointJSON["state"]==null?null:(String)checkpointJSON["state"];
			this.tag = checkpointJSON["tag"]==null?null:(String)checkpointJSON["tag"];
			this.zip = checkpointJSON["zip"]==null?null:(String)checkpointJSON["zip"];
		}

        public DateTime createdAt{
			get { return _createdAt; }
			set { _createdAt = value; }
		}

		public String checkpointTime{
			get { return _checkpointTime; }
			set { _checkpointTime = value; }
		}

		public String city{
			get { return _city; }
			set { _city = value; }
		}

		public ISO3Country countryISO3{
			get { return _countryISO3; }
			set { _countryISO3 = value; }
		}

		public String countryName{
			get { return _countryName; }
			set { _countryName = value; }
		}

		public String message{
			get { return _message; }
			set { _message = value; }
		}

		public String state{
			get { return _state; }
			set { _state = value; }
		}

		public String tag{
			get { return _tag; }
			set { _tag = value; }
		}

		public String zip{
			get { return _zip; }
			set { _zip = value; }
		}



	}
}

