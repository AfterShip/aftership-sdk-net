using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Aftership.Enums;

namespace Aftership
{
	/// <summary>
	/// Connection API. Connect with the API of Afthership
	/// </summary>
    public class ConnectionAPI 
    {
	
		String _tokenAftership;
		private static String URL_SERVER = "https://api.aftership.com/";

		//private static String URL_SERVER = "http://192.168.5.100:3001/";
		private static String VERSION_API = "v4";

		/// <summary>
		/// Constructor ConnectionAPI
		/// </summary>
		/// <param name="tokenAfthership"> Afthership token for the connection</param>
		/// <returns></returns>
		public ConnectionAPI( String tokenAfthership)
        {
			_tokenAftership = tokenAfthership;
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
			string url = URL_SERVER  + VERSION_API + urlResource;
			string json_response = "";

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.Timeout = 10000;
            WebHeaderCollection header = new WebHeaderCollection();
            header.Add("aftership-api-key", _tokenAftership);
            request.Headers = header;
			request.ContentType = "application/json";
			request.Method = method;
//			Console.WriteLine(" Requesting the URL :"+ url);

			if(body!=null){

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
				StreamReader reader = new StreamReader(response.GetResponseStream());
				json_response = reader.ReadToEnd();
			
			}catch(WebException e){
				if (e.Response == null) {

					//timeout or bad internet conexion
					throw e;
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
