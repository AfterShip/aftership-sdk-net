using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AftershipAPI.Enums;

namespace AftershipAPI
{
	/// <summary>
	/// Connection API. Connect with the API of Afthership
	/// </summary>
    public class ConnectionAPI 
    {
		String _tokenAftership;
		String _url;
		private static String URL_SERVER = "https://api.aftership.com/";

		//private static String URL_SERVER = "http://localhost:3001/";
		private static String VERSION_API = "v4";

		/// <summary>
		/// Constructor ConnectionAPI
		/// </summary>
		/// <param name="tokenAfthership"> Afthership token for the connection</param>
		/// <returns></returns>
		public ConnectionAPI( String tokenAfthership, String url = null)
        {
			_tokenAftership = tokenAfthership;
			if (url != null)
			{
				_url = url;
			}
			else
			{
				_url = URL_SERVER;
			}
        }

        /// <summary>
        /// Updates a tracking of your account
        /// </summary>
        /// <param name="tracking">  A Tracking object with the information to update
        ///                The fields trackingNumber and slug SHOULD be informed, otherwise an exception will be thrown
        ///               The fields an user can update are: smses, emails, title, customerName, orderID, orderIDPath,
        ///               customFields</param>
        /// <returns>The last Checkpoint object</returns>
        public Tracking putTracking(Tracking tracking){

            String parametersExtra="";
            if(tracking.id!=null && !(tracking.id.CompareTo("")==0)){
                parametersExtra = tracking.id;
            }else {
                String paramRequiredFields = this.replaceFirst(tracking.getQueryRequiredFields(),"&", "?");
                parametersExtra = tracking.slug +"/"+tracking.trackingNumber+paramRequiredFields;
            }


            JObject response = this.request("PUT", "/trackings/"+parametersExtra, tracking.generatePutJSON());

            return new Tracking((JObject)response["data"]["tracking"]);

        }
        /// <summary>
        /// Return the tracking information of the last checkpoint of a single tracking
        /// </summary>
        /// <param name="tracking"> A Tracking to get the last checkpoint of, it should have tracking number and slug at least</param>
        /// <returns>The last Checkpoint object</returns>
        public Checkpoint getLastCheckpoint(Tracking tracking){

            String parametersExtra="";
            if(tracking.id!=null && !(tracking.id.Equals(""))){
                parametersExtra = tracking.id;
            }else {                         
                String paramRequiredFields =   this.replaceFirst (tracking.getQueryRequiredFields (), "&", "?");  
                parametersExtra = tracking.slug+"/"+tracking.trackingNumber+paramRequiredFields;
            }

            JObject response = this.request("GET","/last_checkpoint/"+parametersExtra,null);

            JObject checkpointJSON = (JObject) response["data"]["checkpoint"];
            Checkpoint checkpoint = null;
            if(checkpointJSON.Count!=0) {
                checkpoint = new Checkpoint(checkpointJSON);
            }

            return checkpoint;
        }


        /// <summary>
        /// Return the tracking information of the last checkpoint of a single tracking
        /// </summary>
        /// <param name="tracking"> A Tracking to get the last checkpoint of, it should have tracking number and slug at least.</param>
        /// <param name="fields"> A list of fields of checkpoint wanted to be in the response</param>
        /// <param name="lang"> A String with the language desired. Support Chinese to English translation
        ///                      for china-ems and china-post only</param>
        /// <returns>The last Checkpoint object</returns>
 
        public Checkpoint getLastCheckpoint(Tracking tracking,List<FieldCheckpoint> fields, String lang){

            String parameters = null;
            QueryString qs = new QueryString();
            
            if (fields!=null) qs.add("fields", string.Join(",",fields));
            if (lang!=null && !lang.Equals("")) qs.add("lang",lang);
            parameters = this.replaceFirst( qs.ToString(),"&","?");

            String parametersExtra="";
            if(tracking.id!=null && !tracking.id.Equals("")){
                parametersExtra = tracking.id+parameters;
            }else {
                String paramRequiredFields = tracking.getQueryRequiredFields();
                parametersExtra = tracking.slug+"/"+tracking.trackingNumber+parameters+paramRequiredFields;
            }

            JObject response = this.request("GET","/last_checkpoint/"+parametersExtra,null);

            JObject checkpointJSON = (JObject)response["data"]["checkpoint"];
            Checkpoint checkpoint = null;
            if(checkpointJSON.Count!=0) {
                checkpoint = new Checkpoint(checkpointJSON);
            }

            return checkpoint;
        }
        /// <summary>
        /// Retrack an expired tracking once
        /// </summary>
        /// <param name="tracking"> tracking A Tracking to reactivate, it should have tracking number and slug at least.</param>
        /// <param name="fields"> A list of fields of checkpoint wanted to be in the response</param>
        /// <param name="lang"> A String with the language desired. Support Chinese to English translation
        ///                      for china-ems and china-post only</param>
        /// <returns> A JSONObject with the response. It will contain the status code of the operation, trackingNumber,
        ///         slug and active (to true)</returns>

        public bool retrack(Tracking tracking){

            String paramRequiredFields = this.replaceFirst(tracking.getQueryRequiredFields(),"&","?");

            JObject response = this.request("POST","/trackings/"+tracking.slug+
                "/"+tracking.trackingNumber+"/retrack"+paramRequiredFields,null);

            if ( (int)response["meta"]["code"]==200) {
                if ((bool)response["data"]["tracking"]["active"]) {
                    return true;
                }else {
                    return false;
                }
            }else {
                return false;
            }

        }

        /// <summary>
        ///Get as much as 100 trackings from your account, created less than 30 days ago. If you delete right before,
        ///         you may obtain less than 100 trackings.
        /// </summary>
        /// <param name="param">page Indicated the page of 100 trackings to return, if page is 1 will return the first 100,
        ///  if is 2 -> 100-200 etc</param> 
        /// <returns> A List of Tracking Objects from your account. Max 100 trackings

        public List<Tracking> getTrackings(int page){

            List<Tracking> trackingList = null;

            JObject response = this.request("GET","/trackings?limit=100&page="+page,null);
            JArray trackingJSON = (JArray)response["data"]["trackings"];
            if(trackingJSON.Count!=0) {
                trackingList = new List<Tracking>();

                for (int i = 0; i < trackingJSON.Count; i++) {
                    trackingList.Add(new Tracking((JObject)trackingJSON[i]));
                }
            }
            return trackingList;

        }

        /// <summary>
        ///Get trackings from your account with the ParametersTracking defined in the params
        /// </summary>
        /// <param name="parameters"> ParametersTracking Object, with the information to get
        /// <returns> A List of Tracking Objects from your account.
        public List<Tracking> getTrackings(ParametersTracking parameters){
            List<Tracking> trackingList = null;
            int size =0;
            JObject response = this.request("GET","/trackings?"+parameters.generateQueryString(),null);
            JArray trackingJSON = (JArray)response["data"]["trackings"];
            if(trackingJSON.Count!=0) {
                size =  (int)response["data"]["count"];
                trackingList = new List<Tracking>();
                for (int i = 0; i < trackingJSON.Count; i++) {
                    trackingList.Add(new Tracking((JObject)trackingJSON[i]));
                }
                parameters.total = size;
            }
            return trackingList;
        }


        /// <summary>
        ///Return a list of couriers supported by AfterShip along with their names, URLs and slugs
        /// </summary>
        /// <returns>A list of Object Courier, with all the couriers supported by the API
        public List<Courier> getAllCouriers(){

            JObject response = this.request("GET","/couriers/all",null);


            JArray couriersJSON = (JArray) response["data"]["couriers"];
            List<Courier> couriers = new List<Courier>(couriersJSON.Count);

            JObject element;

            for (int i = 0; i < couriersJSON.Count; i++) {
                element = (JObject)couriersJSON[i];

                Courier newCourier = new Courier(element);
                couriers.Add(newCourier);
            }
            return couriers;
        }

        /// <summary>
        ///Return a list of user couriers enabled by user's account along their names, URLs, slugs, required fields.
        /// </summary>
        /// <returns>A list of Object Courier, with all the couriers supported by the API
        public List<Courier> getCouriers(){

            JObject response = this.request("GET","/couriers",null);


            JArray couriersJSON = (JArray) response["data"]["couriers"];
            List<Courier> couriers = new List<Courier>(couriersJSON.Count);

            JObject element;

            for (int i = 0; i < couriersJSON.Count;i++){
                element = (JObject)couriersJSON[i];

                Courier newCourier = new Courier(element);
                couriers.Add(newCourier);
            }
            return couriers;
        }

        /// <summary>
        ///Get a list of matched couriers for a tracking number based on the tracking number format 
        /// Note, only check the couriers you have defined in your account
        /// </summary>
        /// <param name="trackingNumber"> tracking number to match with couriers
        /// <returnsA List of Couriers objects that match the provided trackingNumber

        public List<Courier> detectCouriers(String trackingNumber){
            JObject body = new JObject();
            JObject tracking = new JObject();

            // trackingJSON.Add("slug",new JValue(_slug));

            if (trackingNumber == null || trackingNumber.Equals(""))
                throw  new System.ArgumentException("The tracking number should be always informed for the method detectCouriers");
            tracking.Add("tracking_number",new JValue(trackingNumber));
            body.Add("tracking",tracking);
            JObject response = this.request("POST","/couriers/detect",body.ToString());
            List<Courier> couriers = new List<Courier>();

            JArray couriersJSON = (JArray)response["data"]["couriers"];
            JObject element;

            for (int i = 0; i < couriersJSON.Count; i++) {
                element = (JObject) couriersJSON[i];

                Courier newCourier = new Courier(element);
                couriers.Add(newCourier);
            }
            return couriers;
        }


        /// <summary>
        ///Get a list of matched couriers for a tracking number based on the tracking number format Note, only check the couriers you have defined in your account
        /// Note, only check the couriers you have defined in your account
        /// </summary>
        /// <param name="trackingNumber"> Tracking number to match with couriers (mandatory)</param>
        /// <param name="trackingPostalCode"> tracking number to match with couriers
        /// <param name="trackingShipDate">sually it is refer to the posting date of the shipment, format in YYYYMMDD.
        /// Required by some couriers, such as `deutsch-post`.(optional)</param>
        /// <param name="trackingAccountNumber"> The account number for particular courier. Required by some couriers, 
        /// such as `dynamic-logistics`.(optional)</param>
        /// <param name="slugs"> The slug of couriers to detect.
        /// <returns>A List of Couriers objects that match the provided trackingNumber</returns>
 
        public List<Courier> detectCouriers(String trackingNumber,String trackingPostalCode, String trackingShipDate,
            String trackingAccountNumber, List<String> slugs){
            JObject body = new JObject();
            JObject tracking = new JObject();

            if (trackingNumber == null || trackingNumber.Equals(""))
                throw  new System.ArgumentException("Tracking number should be always informed for the method detectCouriers");
            tracking.Add("tracking_number",new JValue(trackingNumber));

            if (trackingPostalCode!= null && !trackingPostalCode.Equals(""))
                tracking.Add("tracking_postal_code", new JValue(trackingPostalCode));
            if (trackingShipDate!= null && !trackingShipDate.Equals(""))
                tracking.Add("tracking_ship_date", new JValue(trackingShipDate));
            if (trackingAccountNumber!= null && !trackingAccountNumber.Equals(""))
                tracking.Add("tracking_account_number", new JValue(trackingAccountNumber));

            if (slugs != null && slugs.Count!=0) {
                JArray slugsJSON = new JArray(slugs);
                tracking.Add("slug", slugsJSON);
            }

            body.Add("tracking",tracking);

            JObject response = this.request("POST","/couriers/detect",body.ToString());
            List<Courier> couriers = new List<Courier>();

            JArray couriersJSON = (JArray) response["data"]["couriers"];
            JObject element;

            for (int i = 0; i < couriersJSON.Count; i++) {
                element = (JObject) couriersJSON[i];

                Courier newCourier = new Courier(element);
                couriers.Add(newCourier);
            }
            return couriers;
        }

        /// <summary>
        ///Get next page of Trackings from your account with the ParametersTracking defined in the params
        /// </summary>
        /// <param name="parameters"> ParametersTracking Object, with the information to get
        /// <returns> The next page of Tracking Objects from your account


        public List<Tracking> getTrackingsNext(ParametersTracking parameters){
            parameters.page = parameters.page +1;
            return this.getTrackings(parameters);
        }

		/// <summary>
		/// A Tracking object with the information to creates
		///	The field trackingNumber SHOULD be informed, otherwise an exception will be thrown
		/// The fields an user can add are: slug, smses, emails, title, customerName, orderID, orderIDPath,
		/// customFields, destinationCountryISO3 (the others are provided by the Server)
		/// </summary>
		/// <param name="tracking"></param>
		/// <returns> A Tracking object with the fields in the same state as the server, if a field has an error,
		///          it won't be added, and won't be shown in the response (for example if the smses
		///		     phone number is not valid). This response doesn't have checkpoints informed!</returns>
		
        public Tracking createTracking(Tracking tracking)
		{
			String tracking_json  = tracking.getJSONPost();

			JObject response = this.request("POST", "/trackings", tracking_json);


			return new Tracking((JObject)response["data"]["tracking"]);
		}

		/// <summary>
		/// Delete a tracking from your account, if the tracking.id property is defined
		/// it will delete that tracking from the system, if not it will take the 
		/// tracking tracking.number and the tracking.slug for identify the tracking
		/// </summary>
		/// <param name="tracking"> A Tracking to delete</param>
		/// <returns>A boolean, true if delete correctly, and false otherwise</returns>
		public bool deleteTracking(Tracking tracking)
        {
			String parametersAll;
			if (tracking.id != null && !tracking.id.Equals ("")) {
				parametersAll = tracking.id;

			} else {
				//get the require fields if any (postal_code, tracking_account etc..)
				parametersAll = tracking.slug + "/" + tracking.trackingNumber;
			}
			JObject response =  this.request("DELETE","/trackings/"+parametersAll,null);


			if (Convert.ToInt32(response["meta"]["code"].ToString())==200)
				return true;
			else
				return false;

        }
		/// <summary>
		/// Get a specific tracking from your account. If the trackingGet.id property 
		/// is defined it will get that tracking from the system, if not it will take 
		/// the tracking tracking.number and the tracking.slug for identify the tracking
		/// </summary>
		/// <param name="trackingGet">A Tracking to get.</param></param>
		/// <returns> A Tracking object with the response</returns>
		
        public Tracking getTrackingByNumber(Tracking trackingGet){

			String parametersExtra;
			if (trackingGet.id != null && !trackingGet.id.Equals ("")) {
				 parametersExtra = trackingGet.id;

			} else {
				//get the require fields if any (postal_code, tracking_account etc..)
				String paramRequiredFields = replaceFirst (trackingGet.getQueryRequiredFields (), "&", "?");
				parametersExtra = trackingGet.slug + "/" + trackingGet.trackingNumber +
					paramRequiredFields;
			}
			JObject response = request("GET","/trackings/"+parametersExtra,null);
			JObject trackingJSON = (JObject) response["data"]["tracking"];
			Tracking tracking = null;
			if(trackingJSON.Count!=0) {
				tracking = new Tracking(trackingJSON);
			}

			return tracking;
        }

		/// <summary>
		///  Get a specific tracking from your account. If the trackingGet.id property 
		/// is defined it will get that tracking from the system, if not it will take 
		/// the tracking tracking.number and the tracking.slug for identify the tracking
		/// </summary>
		/// <param name="trackingGet">A Tracking to get</param>
		/// <param name="fields">A list of fields wanted to be in the response</param>
		/// <param name="lang">A String with the language desired. Support Chinese to 
		/// English translation for china-ems and china-post only</param>
		/// <returns></returns>
		public Tracking getTrackingByNumber(Tracking trackingGet,List<FieldTracking> fields,String lang){
		
			String parametersAll;

			//encode the fields required
			String params_query;
			QueryString qs = new QueryString();
			if (fields != null) {
				qs.add ("fields", parseListFieldTracking(fields));
			}

			if (lang!=null && !lang.Equals("")) qs.add("lang",lang);

			params_query = replaceFirst(qs.ToString(),"&","?");

			if (trackingGet.id != null && !trackingGet.id.Equals ("")) {
				parametersAll = trackingGet.id+params_query;

			} else {
				//get the require fields if any (postal_code, tracking_account etc..)
				String paramRequiredFields = trackingGet.getQueryRequiredFields();
				parametersAll = trackingGet.slug +
				"/" + trackingGet.trackingNumber + params_query + paramRequiredFields;
			}

			JObject response = this.request("GET","/trackings/"+parametersAll,null);
			JObject trackingJSON = (JObject) response["data"]["tracking"];
			Tracking tracking = null;
			if(trackingJSON.Count!=0) {
				tracking = new Tracking(trackingJSON);
			}

			return tracking;
		}

		/// <summary>
    	/// Make a request to the HTTP API of Aftership
		/// </summary>
		///<param name="method">String with the method of the request: GET, POST, PUT, DELETE</param> 
		///<param name="urlResource">String with the URL of the request</param> 
		///<param name="body">String JSON with the body of the request, 
		/// if the request doesn't need body "GET/DELETE", the bodywould be null</param> 
		/// <returns>A String JSON with the response of the request</returns>
		/// 
		public JObject request(String method, String urlResource, String body)
        {
           // Console.WriteLine ("Start Request "+DateTime.Now);
			string url = _url  + VERSION_API + urlResource;
			string json_response = "";

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.Timeout = 150000;
            WebHeaderCollection header = new WebHeaderCollection();
            header.Add("aftership-api-key", _tokenAftership);
            request.Headers = header;
			request.ContentType = "application/json";
			request.Method = method;
			// Console.WriteLine(method+" Requesting the URL :"+ url);

			if(body!=null){
               // Console.WriteLine ("body: " + body);
				//is a POST or PUT  
				using (var streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(body);
					streamWriter.Flush();
					streamWriter.Close();
				}
			}

			try{
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//                if(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created){
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    json_response = reader.ReadToEnd();
//                }else if (response.StatusCode == HttpStatusCode.RequestTimeout){
//                    throw new TimeoutException();
//                }else{
//                    throw new WebException(response.StatusCode.ToString());
//                }
			}catch(WebException e){
				if (e.Response == null) {
                    throw e;
					//timeout or bad internet conexion
				} else {
					//probably Aftership will give more information of the problem, so we read the response
					HttpWebResponse response = e.Response as HttpWebResponse;				
					using (Stream stream = response.GetResponseStream ())
					using (var reader = new StreamReader (stream)) {
						String text = reader.ReadToEnd ();
						throw new WebException (text, e);
					}
				}

			}catch(Exception e){
                throw e;
            }
//            }finally{
//                Console.WriteLine ("Finish Request "+DateTime.Now);
//            }
            Console.WriteLine ("Response request: "+json_response+"*");
			return JObject.Parse(json_response);;
        }


		/// Parse a List<FieldTracking> to List<String>
		public List<String> parseListFieldTracking(List<FieldTracking> list){

			List<String> listString = new List<String> ();

			foreach (FieldTracking element in list) // Loop through List with foreach
			{
				listString.Add (element.ToString());
			}

			return listString;

		}

		/// Replace first ocurrence from a String
		public String replaceFirst(string text, string search, string replace)
		{
			int pos = text.IndexOf(search);
			if (pos < 0)
			{
				return text;
			}
			return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}

	}


    
}
