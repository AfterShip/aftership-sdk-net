using System;
using System.Collections.Generic;
using AftershipAPI.Enums;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace AftershipAPI
{
    internal static class JSONExtensions
    {
        internal static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }

    /// <summary>
    /// Tracking. Keep instances of trackings
    /// </summary>
    public class Tracking
    {
        ///Tracking ID in the Afthership system 
        private String _id;

        ///Tracking number of a shipment. Duplicate tracking numbers, or tracking number with invalid tracking
        ///number format will not be accepted. 
        private String _trackingNumber;

        ///Unique code of each courier. If you do not specify a slug, Aftership will automatically detect
        ///the courier based on the tracking number format and your selected couriers
        private String _slug;

        /// Email address(es) to receive email notifications. Use comma for multiple emails. 
        private List<String> _emails;

        /// Phone number(s) to receive sms notifications. Use comma for multiple emails.
        ///Enter + area code before phone number. 
        private List<String> _smses;

        /// Title of the tracking. Default value as trackingNumber 
        private String _title;

        /// Customer name of the tracking. 
        private String _customerName;

        /// ISO Alpha-3(three letters)to specify the destination of the shipment.
        /// If you use postal service to send international shipments, AfterShip will automatically
        /// get tracking results at destination courier as well (e.g. USPS for USA). 
        private ISO3Country _destinationCountryISO3;

        ///  Origin country of the tracking. ISO Alpha-3 
        private ISO3Country _originCountryISO3;

        /// Text field for order ID 
        private String _orderID;

        /// Text field for order path 
        private String _orderIDPath;

        /// Custom fields that accept any TEXT STRING
        private Dictionary<String, String> _customFields;

        /// fields informed by Aftership API

        ///  Date and time of the tracking created. 
        private DateTime _createdAt;

        /// Date and time of the tracking last updated. 
        private DateTime _updatedAt;

        /// Whether or not AfterShip will continue tracking the shipments.
        ///Value is `false` when status is `Delivered` or `Expired`. 
        private bool _active;

        /// Expected delivery date (if any). 
        private String _expectedDelivery;

        ///  Number	Number of packages under the tracking. 
        private int _shipmentPackageCount;

        /// Shipment type provided by carrier (if any). 
        private String _shipmentType;

        /// Signed by information for delivered shipment (if any). 
        private String _signedBy;

        ///  Source of how this tracking is added.  
        private String _source;

        /// Current status of tracking. 
        private StatusTag _tag;

        ///  Number of attempts AfterShip tracks at courier's system. 
        private int _trackedCount;

        /// Array of Hash describes the checkpoint information. 
        List<Checkpoint> _checkpoints;

        ///Unique Token
        private String _uniqueToken;

        ///Tracking Account number tracking_account_number
        private String _trackingAccountNumber;

        ///Tracking postal code tracking_postal_code
        private String _trackingPostalCode;

        ///Tracking ship date tracking_ship_date
        private String _trackingShipDate;

        /// Current subtag of tracking
        private String _subtag;

        /// Normalized tracking message 
        private String _subtagMessage;

        /// Official tracking URL of the courier
        private String _courierTrackingLink;

        public Tracking(String trackingNumber)
        {
            _trackingNumber = trackingNumber;
            _title = trackingNumber;
        }

        public Tracking(JObject trackingJSON)
        {
            String destination_country_iso3;
            String origin_country_iso3;

            this.id = trackingJSON["id"] == null ? null : (String)trackingJSON["id"];

            //fields that can be updated by the user
            _trackingNumber = trackingJSON["tracking_number"] == null ? null : (String)trackingJSON["tracking_number"];
            _slug = trackingJSON["slug"] == null ? null : (String)trackingJSON["slug"];
            _title = trackingJSON["title"] == null ? null : (String)trackingJSON["title"];
            _customerName = trackingJSON["customer_name"] == null ? null : (String)trackingJSON["customer_name"];
            destination_country_iso3 = (String)trackingJSON["destination_country_iso3"];

            if (destination_country_iso3 != null && destination_country_iso3 != String.Empty)
            {
                _destinationCountryISO3 = (ISO3Country)Enum.Parse(typeof(ISO3Country), destination_country_iso3);
            }
            _orderID = trackingJSON["order_id"] == null ? null : (String)trackingJSON["order_id"];
            _orderIDPath = trackingJSON["order_id_path"] == null ? null : (String)trackingJSON["order_id_path"];
            _trackingAccountNumber = trackingJSON["tracking_account_number"] == null ? null :
                (String)trackingJSON["tracking_account_number"];
            _trackingPostalCode = trackingJSON["tracking_postal_code"] == null ? null :
                (String)trackingJSON["tracking_postal_code"];
            _trackingShipDate = trackingJSON["tracking_ship_date"] == null ? null :
                (String)trackingJSON["tracking_ship_date"];

            JArray smsesArray = trackingJSON["smses"] == null ? null : (JArray)trackingJSON["smses"];
            if (smsesArray != null && smsesArray.Count != 0)
            {
                _smses = new List<String>();
                for (int i = 0; i < smsesArray.Count; i++)
                {
                    _smses.Add((String)smsesArray[i]);
                }
            }

            JArray emailsArray = trackingJSON["emails"] == null ? null : (JArray)trackingJSON["emails"];
            if (emailsArray != null && emailsArray.Count != 0)
            {
                _emails = new List<String>();
                for (int i = 0; i < emailsArray.Count; i++)
                {
                    _emails.Add((String)emailsArray[i]);
                }
            }

            JObject customFieldsJSON = trackingJSON["custom_fields"] == null || !trackingJSON["custom_fields"].HasValues ? null :
                (JObject)trackingJSON["custom_fields"];

            if (customFieldsJSON != null)
            {
                _customFields = new Dictionary<String, String>();
                IEnumerable<JProperty> keys = customFieldsJSON.Properties();
                foreach (var item in keys)
                {
                    _customFields.Add(item.Name, (String)customFieldsJSON[item.Name]);
                }
            }

            //fields that can't be updated by the user, only retrieve
            _createdAt = trackingJSON["created_at"].IsNullOrEmpty() ? DateTime.MinValue : (DateTime)trackingJSON["created_at"];
            _updatedAt = trackingJSON["updated_at"].IsNullOrEmpty() ? DateTime.MinValue : (DateTime)trackingJSON["updated_at"];
            _expectedDelivery = trackingJSON["expected_delivery"].IsNullOrEmpty() ? null : (String)trackingJSON["expected_delivery"];

            _active = trackingJSON["active"].IsNullOrEmpty() ? false : (bool)trackingJSON["active"];

            origin_country_iso3 = (String)trackingJSON["origin_country_iso3"];

            if (origin_country_iso3 != null && origin_country_iso3 != String.Empty)
            {
                _originCountryISO3 = (ISO3Country)Enum.Parse(typeof(ISO3Country), origin_country_iso3);
            }
            _shipmentPackageCount = trackingJSON["shipment_package_count"].IsNullOrEmpty() ? 0 :
                (int)trackingJSON["shipment_package_count"];
            _shipmentType = trackingJSON["shipment_type"].IsNullOrEmpty() ? null : (String)trackingJSON["shipment_type"];
            _signedBy = trackingJSON["signed_by"].IsNullOrEmpty() ? null : (String)trackingJSON["signed_by"];
            _source = trackingJSON["source"].IsNullOrEmpty() ? null : (String)trackingJSON["source"];
            _tag = (String)trackingJSON["tag"] == null ? 0 :
                (StatusTag)Enum.Parse(typeof(StatusTag), (String)trackingJSON["tag"]);

            _trackedCount = trackingJSON["tracked_count"].IsNullOrEmpty() ? 0 : (int)trackingJSON["tracked_count"];
            _uniqueToken = trackingJSON["unique_token"].IsNullOrEmpty() ? null : (String)trackingJSON["unique_token"];

            _subtag = trackingJSON["subtag"].IsNullOrEmpty() ? null: (string)trackingJSON["subtag"];
            _subtagMessage = trackingJSON["subtag_message"].IsNullOrEmpty() ? null : (string)trackingJSON["subtag_message"];
            _courierTrackingLink = trackingJSON["courier_tracking_link"].IsNullOrEmpty() ? null : (string)trackingJSON["courier_tracking_link"];

            // checkpoints
            JArray checkpointsArray = trackingJSON["checkpoints"].IsNullOrEmpty() ? null :
                (JArray)trackingJSON["checkpoints"];
            if (checkpointsArray != null && checkpointsArray.Count != 0)
            {
                _checkpoints = new List<Checkpoint>();
                for (int i = 0; i < checkpointsArray.Count; i++)
                {
                    _checkpoints.Add(new Checkpoint((JObject)checkpointsArray[i]));
                }
            }
        }


        public String id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String trackingNumber
        {
            get { return _trackingNumber; }
            set { _trackingNumber = value; }
        }

        public String slug
        {
            get { return _slug; }
            set { _slug = value; }
        }

        public List<String> emails
        {
            get { return _emails; }
            set { _emails = value; }
        }

        public void addEmails(String emails)
        {
            if (_emails == null)
            {
                _emails = new List<String>();
                _emails.Add(emails);
            }
            else
            {
                _emails.Add(emails);
            }
        }

        public void deleteEmail(String email)
        {
            if (_emails != null)
            {
                _emails.Remove(email);
            }
        }
        public List<String> smses
        {
            get { return _smses; }
            set { _smses = value; }
        }

        public void addSmses(String smses)
        {
            if (_smses == null)
            {
                _smses = new List<String>();
                _smses.Add(smses);
            }
            else
            {
                _smses.Add(smses);
            }
        }

        public void deleteSmes(String smses)
        {
            if (_smses != null)
            {
                _smses.Remove(smses);
            }
        }

        public String title
        {
            get { return _title; }
            set { _title = value; }
        }

        public String customerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        public ISO3Country destinationCountryISO3
        {
            get { return _destinationCountryISO3; }
            set { _destinationCountryISO3 = value; }
        }

        public String orderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        public String orderIDPath
        {
            get { return _orderIDPath; }
            set { _orderIDPath = value; }
        }

        public Dictionary<String, String> customFields
        {
            get { return _customFields; }
            set { _customFields = value; }
        }

        public void addCustomFields(String field, String value)
        {

            if (_customFields == null)
            {
                _customFields = new Dictionary<String, String>();
            }
            customFields.Add(field, value);
        }

        public void deleteCustomFields(String field)
        {
            if (this.customFields != null)
            {
                this.customFields.Remove(field);
            }
        }

        public DateTime createdAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public DateTime updatedAt
        {
            get { return _updatedAt; }
            set { _updatedAt = value; }
        }

        public bool active
        {
            get { return _active; }
            set { _active = value; }
        }

        public String expectedDelivery
        {
            get { return _expectedDelivery; }
            set { _expectedDelivery = value; }
        }

        public ISO3Country originCountryISO3
        {
            get { return _originCountryISO3; }
            set { _originCountryISO3 = value; }
        }

        public int shipmentPackageCount
        {
            get { return _shipmentPackageCount; }
            set { _shipmentPackageCount = value; }
        }

        public int trackedCount
        {
            get { return _trackedCount; }
            set { _trackedCount = value; }
        }

        public String shipmentType
        {
            get { return _shipmentType; }
            set { _shipmentType = value; }
        }

        public String signedBy
        {
            get { return _signedBy; }
            set { _signedBy = value; }
        }

        public String source
        {
            get { return _source; }
            set { _source = value; }
        }

        public StatusTag tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public String uniqueToken
        {
            get { return _uniqueToken; }
            set { _uniqueToken = value; }
        }

        public String trackingAccountNumber
        {
            get { return _trackingAccountNumber; }
            set { _trackingAccountNumber = value; }
        }

        public String trackingPostalCode
        {
            get { return _trackingPostalCode; }
            set { _trackingPostalCode = value; }
        }

        public String trackingShipDate
        {
            get { return _trackingShipDate; }
            set { _trackingShipDate = value; }
        }

        public String subtag
        {
            get { return _subtag; }
            set { _subtag = value; }
        }

        public String subtagMessage
        {
            get { return _subtagMessage; }
            set { _subtagMessage = value; }
        }

        public String courierTrackingLink
        {
            get { return _courierTrackingLink; }
            set { _courierTrackingLink = value; }
        }

        public List<Checkpoint> checkpoints
        {
            get { return _checkpoints; }
            set { _checkpoints = value; }
        }

        public String getJSONPost()
        {
            JObject globalJSON = new JObject();
            JObject trackingJSON = new JObject();
            //	trackingJSON.Add("hola",
            trackingJSON.Add("tracking_number", new JValue(_trackingNumber));
            if (_slug != null) trackingJSON.Add("slug", new JValue(_slug));
            if (_title != null) trackingJSON.Add("title", new JValue(_title));
            if (_emails != null)
            {
                JArray emailsJSON = new JArray(_emails);
                trackingJSON["emails"] = emailsJSON;
            }
            if (_smses != null)
            {
                JArray smsesJSON = new JArray(_smses);
                trackingJSON["smses"] = smsesJSON;
            }
            if (_customerName != null) trackingJSON.Add("customer_name", new JValue(_customerName));
            if (_destinationCountryISO3 != 0)
                trackingJSON.Add("destination_country_iso3", new JValue(_destinationCountryISO3.ToString()));
            if (_orderID != null) trackingJSON.Add("order_id", new JValue(_orderID));
            if (_orderIDPath != null) trackingJSON.Add("order_id_path", new JValue(_orderIDPath));

            if (_trackingAccountNumber != null) trackingJSON.Add("tracking_account_number", new JValue(_trackingAccountNumber));
            if (_trackingPostalCode != null) trackingJSON.Add("tracking_postal_code", new JValue(trackingPostalCode));
            if (_trackingShipDate != null) trackingJSON.Add("tracking_ship_date", new JValue(trackingShipDate));

            if (_customFields != null)
            {
                JObject customFieldsJSON = new JObject();
                foreach (KeyValuePair<String, String> pair in _customFields)
                {
                    customFieldsJSON.Add(pair.Key, new JValue(pair.Value));
                }

                trackingJSON["custom_fields"] = customFieldsJSON;
            }


            globalJSON["tracking"] = trackingJSON;

            return globalJSON.ToString();
        }


        public String generatePutJSON()
        {
            JObject globalJSON = new JObject();
            JObject trackingJSON = new JObject();
            JObject customFieldsJSON;

            if (_title != null) trackingJSON.Add("title", new JValue(_title));
            if (_emails != null)
            {
                JArray emailsJSON = new JArray(_emails);
                trackingJSON["emails"] = emailsJSON;
            }
            if (this.smses != null)
            {
                JArray smsesJSON = new JArray(_smses);
                trackingJSON["smses"] = smsesJSON;
            }
            if (_customerName != null) trackingJSON.Add("customer_name", new JValue(_customerName));
            if (_orderID != null) trackingJSON.Add("order_id", new JValue(_orderID));
            if (_orderIDPath != null) trackingJSON.Add("order_id_path", new JValue(_orderIDPath));
            if (_customFields != null)
            {
                customFieldsJSON = new JObject();

                foreach (KeyValuePair<String, String> pair in _customFields)
                {
                    customFieldsJSON.Add(pair.Key, new JValue(pair.Value));
                }
                trackingJSON["custom_fields"] = customFieldsJSON;
            }
            globalJSON["tracking"] = trackingJSON;

            return globalJSON.ToString();
        }

        public String getQueryRequiredFields()
        {
            bool containsInfo = false;
            QueryString qs = new QueryString();
            if (this.trackingAccountNumber != null)
            {
                containsInfo = true;
                qs.add("tracking_account_number", this.trackingAccountNumber);
            }
            if (this.trackingPostalCode != null)
            {
                qs.add("tracking_postal_code", this.trackingPostalCode);
                containsInfo = true;
            }
            if (this.trackingShipDate != null)
            {
                qs.add("tracking_ship_date", this.trackingShipDate);
                containsInfo = true;
            }
            if (containsInfo)
            {
                return qs.ToString();
            }
            return "";

        }

        public override string ToString()
        {

            return "_id: " + _id + "\n_trackingNumber: " + _trackingNumber + "\n_slug:" + _slug;

        }
    }


}



