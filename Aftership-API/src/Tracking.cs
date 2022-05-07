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

        /// Date and time of the tracking created.
        private DateTime _createdAt;

        /// Date and time of the tracking last updated.
        private DateTime _updatedAt;

        /// Date and time the tracking was last updated
        private DateTime _lastUpdatedAt;

        /// Tracking number of a shipment. Duplicate tracking numbers, or tracking number with invalid tracking
        /// number format will not be accepted.
        private String _trackingNumber;

        /// Unique code of each courier. If you do not specify a slug, Aftership will automatically detect
        /// the courier based on the tracking number format and your selected couriers
        private String _slug;
        
        /// fields informed by Aftership API
        /// Whether or not AfterShip will continue tracking the shipments.
        /// Value is `false` when status is `Delivered` or `Expired`.
        private bool _active;

        /// Google cloud message registration IDs to receive the push notifications.
        /// Accept either array or comma separated as input.
        private List<String> _android;

        /// Custom fields that accept any TEXT STRING
        private Dictionary<String, String> _customFields;

        /// Customer name of the tracking.
        private String _customerName;

        /// Total delivery time in days
        private int _deliveryTime;

        /// ISO Alpha-3(three letters)to specify the destination of the shipment.
        /// If you use postal service to send international shipments, AfterShip will automatically
        /// get tracking results at destination courier as well (e.g. USPS for USA).
        private ISO3Country _destinationCountryISO3;

        /// Destination country of the tracking detected from the courier. ISO Alpha-3 (three letters).
        /// Value will be null if the courier doesn't provide the destination country.
        private ISO3Country _courierDestinationCountryISO3;

        /// Email address(es) to receive email notifications. Use comma for multiple emails.
        private List<String> _emails;

        /// Expected delivery date (if any).
        private String _expectedDelivery;

        /// Apple iOS device IDs to receive the push notifications.
        /// Accept either array or comma separated as input.
        private List<String> _ios;

        /// Text field for the note
        private String _note;

        /// Text field for order ID
        private String _orderID;

        /// Text field for order path
        private String _orderIDPath;

        /// Date and time of the order created
        private String _orderDate;

        /// Origin country of the tracking. ISO Alpha-3
        private ISO3Country _originCountryISO3;

        /// Number	Number of packages under the tracking.
        private int _shipmentPackageCount;

        /// Date and time the tracking was picked up
        private String _shipmentPickupDate;

        /// Date and time the tracking was delivered
        private String _shipmentDeliveryDate;

        /// Shipment type provided by carrier (if any).
        private String _shipmentType;

        /// Shipment weight provied by carrier
        private Nullable<float> _shipmentWeight;

        /// Weight unit provied by carrier, either in kg or lb
        private String _shipmentWeightUnit;

        /// Signed by information for delivered shipment (if any).
        private String _signedBy;

        /// Phone number(s) to receive sms notifications. Use comma for multiple emails.
        /// Enter + area code before phone number.
        private List<String> _smses;

        ///  Source of how this tracking is added.
        private String _source;

        /// Current status of tracking.
        private StatusTag _tag;

        /// Current subtag of tracking
        private String _subtag;

        /// Normalized tracking message
        private String _subtagMessage;

        /// Title of the tracking. Default value as trackingNumber
        private String _title;

        ///  Number of attempts AfterShip tracks at courier's system.
        private int _trackedCount;

        /// Indicates if the shipment is trackable till the final destination
        private Boolean _lastMileTrackingSupported;

        /// Store, customer, or order language of the tracking. ISO 639-1 Language Code
        private String _language;

        /// Unique Token
        private String _uniqueToken;

        /// Array of Hash describes the checkpoint information.
        List<Checkpoint> _checkpoints;

        /// Phone number(s) subscribed to receive sms notifications. Comma separated for multiple values
        private List<String> _subscribedSmses;

        /// Email address(es) subscribed to receive email notifications. Comma separated for multiple values
        private List<String> _subscribedEmails;

        /// Whether or not the shipment is returned to sender.
        /// Value is true when any of its checkpoints has subtag Exception_010 (returning to sender) or
        /// Exception_011 (returned to sender). Otherwis value is false
        private Boolean _returnToSender;

        /// Promised delivery date of an order in YYYY-MM-DD format
        private String _orderPromisedDeliveryDate;

        /// Shipment delivery type
        private String _deliveryType;

        /// Shipment pickup location for receiver
        private String _pickupLocation;

        /// Shipment pickup note for receiver
        private String _pickupNote;

        /// Official tracking URL of the courier
        private String _courierTrackingLink;

        /// Date and time of the first attempt by the carrier to deliver the package to the addressee
        private String _firstAttemptedAt;
        
        /// Delivery instructions (delivery date or address) can be modified by visiting the link if supported by a carrier
        private String _courierRedirectLink;

        /// Tracking Account number tracking_account_number
        private String _trackingAccountNumber;

        /// Tracking origin country tracking_origin_country
        private String _trackingOriginCountry;

        /// Tracking destination country tracking_destination_country
        private String _trackingDestinationCountry;

        /// Tracking key tracking_key
        private String _trackingKey;

        /// Tracking postal code tracking_postal_code
        private String _trackingPostalCode;

        /// Tracking ship date tracking_ship_date
        private String _trackingShipDate;

        /// Tracking state tracking_state
        private String _trackingState;

        /// Estimated delivery time of the shipment provided by AfterShip, indicate when the shipment should arrive.
        private EstimatedDeliveryDate _estimatedDeliveryDate;
        
        /// The unique numeric identifier for the order for use by shop owner and customer. 
        private String _orderNumber;

        public Tracking(String trackingNumber)
        {
            _trackingNumber = trackingNumber;
            _title = trackingNumber;
        }

        public Tracking(JObject trackingJSON)
        {
            String destination_country_iso3;
            String origin_country_iso3;
            String courier_destination_country_iso3;

            this.id = trackingJSON["id"] == null ? null : (String) trackingJSON["id"];

            //fields that can be updated by the user
            _trackingNumber = trackingJSON["tracking_number"] == null ? null : (String) trackingJSON["tracking_number"];
            _slug = trackingJSON["slug"] == null ? null : (String) trackingJSON["slug"];
            _title = trackingJSON["title"] == null ? null : (String) trackingJSON["title"];
            _customerName = trackingJSON["customer_name"] == null ? null : (String) trackingJSON["customer_name"];
            destination_country_iso3 = (String) trackingJSON["destination_country_iso3"];

            if (destination_country_iso3 != null && destination_country_iso3 != String.Empty)
            {
                _destinationCountryISO3 = (ISO3Country) Enum.Parse(typeof(ISO3Country), destination_country_iso3);
            }

            _orderID = trackingJSON["order_id"] == null ? null : (String) trackingJSON["order_id"];
            _orderIDPath = trackingJSON["order_id_path"] == null ? null : (String) trackingJSON["order_id_path"];
            _orderNumber = trackingJSON["order_number"] == null ? null : (String)trackingJSON["order_number"];
            _trackingAccountNumber = trackingJSON["tracking_account_number"] == null
                ? null
                : (String) trackingJSON["tracking_account_number"];
            _trackingPostalCode = trackingJSON["tracking_postal_code"] == null
                ? null
                : (String) trackingJSON["tracking_postal_code"];
            _trackingShipDate = trackingJSON["tracking_ship_date"] == null
                ? null
                : (String) trackingJSON["tracking_ship_date"];
            _note = trackingJSON["note"] == null ? null : (String) trackingJSON["note"];
            _language = trackingJSON["language"] == null ? null : (String) trackingJSON["language"];
            _orderPromisedDeliveryDate = trackingJSON["order_promised_delivery_date"] == null
                ? null
                : (String) trackingJSON["order_promised_delivery_date"];
            _deliveryType = trackingJSON["delivery_type"] == null ? null : (String) trackingJSON["delivery_type"];
            _pickupLocation = trackingJSON["pickup_location"] == null ? null : (String) trackingJSON["pickup_location"];
            _pickupNote = trackingJSON["pickup_note"] == null ? null : (String) trackingJSON["pickup_note"];
            _trackingOriginCountry = trackingJSON["tracking_origin_country"] == null
                ? null
                : (String) trackingJSON["tracking_origin_country"];
            _trackingDestinationCountry = trackingJSON["tracking_destination_country"] == null
                ? null
                : (String) trackingJSON["tracking_destination_country"];
            _trackingKey = trackingJSON["tracking_key"] == null ? null : (String) trackingJSON["tracking_key"];
            _trackingState = trackingJSON["tracking_state"] == null ? null : (String) trackingJSON["tracking_state"];

            JArray smsesArray = trackingJSON["smses"] == null ? null : (JArray) trackingJSON["smses"];
            if (smsesArray != null && smsesArray.Count != 0)
            {
                _smses = new List<String>();
                for (int i = 0; i < smsesArray.Count; i++)
                {
                    _smses.Add((String) smsesArray[i]);
                }
            }

            JArray emailsArray = trackingJSON["emails"] == null ? null : (JArray) trackingJSON["emails"];
            if (emailsArray != null && emailsArray.Count != 0)
            {
                _emails = new List<String>();
                for (int i = 0; i < emailsArray.Count; i++)
                {
                    _emails.Add((String) emailsArray[i]);
                }
            }

            JArray subscribedSmsesArray = trackingJSON["subscribed_smses"] == null
                ? null
                : (JArray) trackingJSON["subscribed_smses"];
            if (subscribedSmsesArray != null && subscribedSmsesArray.Count != 0)
            {
                _subscribedSmses = new List<String>();
                for (int i = 0; i < subscribedSmsesArray.Count; i++)
                {
                    _subscribedSmses.Add((String) subscribedSmsesArray[i]);
                }
            }

            JArray subscribedEmailsArray = trackingJSON["subscribed_emails"] == null
                ? null
                : (JArray) trackingJSON["subscribed_emails"];
            if (subscribedEmailsArray != null && subscribedEmailsArray.Count != 0)
            {
                _subscribedEmails = new List<String>();
                for (int i = 0; i < subscribedEmailsArray.Count; i++)
                {
                    _subscribedEmails.Add((String) subscribedEmailsArray[i]);
                }
            }

            JObject customFieldsJSON = trackingJSON["custom_fields"] == null || !trackingJSON["custom_fields"].HasValues
                ? null
                : (JObject) trackingJSON["custom_fields"];

            if (customFieldsJSON != null)
            {
                _customFields = new Dictionary<String, String>();
                IEnumerable<JProperty> keys = customFieldsJSON.Properties();
                foreach (var item in keys)
                {
                    _customFields.Add(item.Name, (String) customFieldsJSON[item.Name]);
                }
            }

            //fields that can't be updated by the user, only retrieve
            _createdAt = trackingJSON["created_at"].IsNullOrEmpty()
                ? DateTime.MinValue
                : (DateTime) trackingJSON["created_at"];
            _updatedAt = trackingJSON["updated_at"].IsNullOrEmpty()
                ? DateTime.MinValue
                : (DateTime) trackingJSON["updated_at"];
            _expectedDelivery = trackingJSON["expected_delivery"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["expected_delivery"];

            _active = trackingJSON["active"].IsNullOrEmpty() ? false : (bool) trackingJSON["active"];

            origin_country_iso3 = (String) trackingJSON["origin_country_iso3"];

            if (origin_country_iso3 != null && origin_country_iso3 != String.Empty)
            {
                _originCountryISO3 = (ISO3Country) Enum.Parse(typeof(ISO3Country), origin_country_iso3);
            }

            _shipmentPackageCount = trackingJSON["shipment_package_count"].IsNullOrEmpty()
                ? 0
                : (int) trackingJSON["shipment_package_count"];
            _shipmentType = trackingJSON["shipment_type"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["shipment_type"];
            _signedBy = trackingJSON["signed_by"].IsNullOrEmpty() ? null : (String) trackingJSON["signed_by"];
            _source = trackingJSON["source"].IsNullOrEmpty() ? null : (String) trackingJSON["source"];
            _tag = (String) trackingJSON["tag"] == null
                ? 0
                : (StatusTag) Enum.Parse(typeof(StatusTag), (String) trackingJSON["tag"]);

            _trackedCount = trackingJSON["tracked_count"].IsNullOrEmpty() ? 0 : (int) trackingJSON["tracked_count"];
            _uniqueToken = trackingJSON["unique_token"].IsNullOrEmpty() ? null : (String) trackingJSON["unique_token"];

            _subtag = trackingJSON["subtag"].IsNullOrEmpty() ? null : (string) trackingJSON["subtag"];
            _subtagMessage = trackingJSON["subtag_message"].IsNullOrEmpty()
                ? null
                : (string) trackingJSON["subtag_message"];
            _courierTrackingLink = trackingJSON["courier_tracking_link"].IsNullOrEmpty()
                ? null
                : (string) trackingJSON["courier_tracking_link"];
            _android = new List<String>();
            _ios = new List<String>();
            _deliveryTime = trackingJSON["delivery_time"].IsNullOrEmpty() ? 0 : (int) trackingJSON["tracked_count"];
            _lastUpdatedAt = trackingJSON["last_updated_at"].IsNullOrEmpty()
                ? DateTime.MinValue
                : (DateTime) trackingJSON["last_updated_at"];
            _orderDate = trackingJSON["order_date"].IsNullOrEmpty() ? null : (String) trackingJSON["order_date"];
            _shipmentPickupDate = trackingJSON["shipment_pickup_date"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["shipment_pickup_date"];
            _shipmentDeliveryDate = trackingJSON["shipment_delivery_date"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["shipment_delivery_date"];
            _shipmentWeight = trackingJSON["shipment_weight"].IsNullOrEmpty()
                ? null
                : (float) trackingJSON["shipment_weight"];
            _shipmentWeightUnit = trackingJSON["shipment_weight_unit"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["shipment_weight_unit"];
            _lastMileTrackingSupported = trackingJSON["last_mile_tracking_supported"].IsNullOrEmpty()
                ? false
                : (Boolean) trackingJSON["last_mile_tracking_supported"];
            _language = trackingJSON["language"].IsNullOrEmpty() ? null : (String) trackingJSON["language"];
            _returnToSender = trackingJSON["return_to_sender"].IsNullOrEmpty()
                ? false
                : (Boolean) trackingJSON["return_to_sender"];
            _orderPromisedDeliveryDate = trackingJSON["order_promised_delivery_date"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["order_promised_delivery_date"];
            _deliveryType = trackingJSON["delivery_type"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["delivery_type"];
            _pickupLocation = trackingJSON["pickup_location"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["pickup_location"];
            _pickupNote = trackingJSON["pickup_note"].IsNullOrEmpty() ? null : (String) trackingJSON["pickup_note"];
            _firstAttemptedAt = trackingJSON["first_attempted_at"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["first_attempted_at"];
            _courierRedirectLink = trackingJSON["courier_redirect_link"].IsNullOrEmpty()
                ? null
                : (String) trackingJSON["courier_redirect_link"];
            _estimatedDeliveryDate = trackingJSON["aftership_estimated_delivery_date"].IsNullOrEmpty()
                ? null
                : new EstimatedDeliveryDate((JObject) trackingJSON["aftership_estimated_delivery_date"]);

            courier_destination_country_iso3 = (String) trackingJSON["courier_destination_country_iso3"];

            if (courier_destination_country_iso3 != null && courier_destination_country_iso3 != String.Empty)
            {
                _courierDestinationCountryISO3 =
                    (ISO3Country) Enum.Parse(typeof(ISO3Country), courier_destination_country_iso3);
            }

            // checkpoints
            JArray checkpointsArray = trackingJSON["checkpoints"].IsNullOrEmpty()
                ? null
                : (JArray) trackingJSON["checkpoints"];
            if (checkpointsArray != null && checkpointsArray.Count != 0)
            {
                _checkpoints = new List<Checkpoint>();
                for (int i = 0; i < checkpointsArray.Count; i++)
                {
                    _checkpoints.Add(new Checkpoint((JObject) checkpointsArray[i]));
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

        public List<String> subscribedSmses
        {
            get { return _subscribedSmses; }
            set { _subscribedSmses = value; }
        }

        public void addSubscribedSmses(String smses)
        {
            if (_subscribedSmses == null)
            {
                _subscribedSmses = new List<String>();
                _subscribedSmses.Add(smses);
            }
            else
            {
                _subscribedSmses.Add(smses);
            }
        }

        public void deleteSubscribedSmes(String smses)
        {
            if (_subscribedSmses != null)
            {
                _subscribedSmses.Remove(smses);
            }
        }

        public List<String> subscribedEmails
        {
            get { return _subscribedEmails; }
            set { _subscribedEmails = value; }
        }

        public void addSubscribedEmails(String emails)
        {
            if (_subscribedEmails == null)
            {
                _subscribedEmails = new List<String>();
                _subscribedEmails.Add(emails);
            }
            else
            {
                _subscribedEmails.Add(emails);
            }
        }

        public void deleteSubscribedEmails(String emails)
        {
            if (_subscribedEmails != null)
            {
                _subscribedEmails.Remove(emails);
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

        public String trackingOriginCountry
        {
            get { return _trackingOriginCountry; }
            set { _trackingOriginCountry = value; }
        }

        public String trackingDestinationCountry
        {
            get { return _trackingDestinationCountry; }
            set { _trackingDestinationCountry = value; }
        }

        public String trackingKey
        {
            get { return _trackingKey; }
            set { _trackingKey = value; }
        }

        public String trackingState
        {
            get { return _trackingState; }
            set { _trackingState = value; }
        }

        public String note
        {
            get { return _note; }
            set { _note = value; }
        }

        public DateTime lastUpdatedAt
        {
            get { return _lastUpdatedAt; }
            set { _lastUpdatedAt = value; }
        }

        public int deliveryTime
        {
            get { return _deliveryTime; }
            set { _deliveryTime = value; }
        }

        public List<string> android
        {
            get { return _android; }
            set { _android = value; }
        }

        public List<string> ios
        {
            get { return _ios; }
            set { _ios = value; }
        }

        public ISO3Country courierDestinationCountryISO3
        {
            get { return _courierDestinationCountryISO3; }
            set { _courierDestinationCountryISO3 = value; }
        }

        public String orderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        public String shipmentPickupDate
        {
            get { return _shipmentPickupDate; }
            set { _shipmentPickupDate = value; }
        }

        public String shipmentDeliveryDate
        {
            get { return _shipmentDeliveryDate; }
            set { _shipmentDeliveryDate = value; }
        }

        public float? shipmentWeight
        {
            get { return (float) _shipmentWeight; }
            set { _shipmentWeight = value; }
        }

        public String shipmentWeightUnit
        {
            get { return _shipmentWeightUnit; }
            set { _shipmentWeightUnit = value; }
        }

        public bool lastMileTrackingSupported
        {
            get { return _lastMileTrackingSupported; }
            set { _lastMileTrackingSupported = value; }
        }

        public String language
        {
            get { return _language; }
            set { _language = value; }
        }

        public bool returnToSender
        {
            get { return _returnToSender; }
            set { _returnToSender = value; }
        }

        public String orderPromisedDeliveryDate
        {
            get { return _orderPromisedDeliveryDate; }
            set { _orderPromisedDeliveryDate = value; }
        }

        public String deliveryType
        {
            get { return _deliveryType; }
            set { _deliveryType = value; }
        }

        public String pickupLocation
        {
            get { return _pickupLocation; }
            set { _pickupLocation = value; }
        }

        public String pickupNote
        {
            get { return _pickupNote; }
            set { _pickupNote = value; }
        }

        public String firstAttemptedAt
        {
            get { return _firstAttemptedAt; }
            set { _firstAttemptedAt = value; }
        }

        public String courierRedirectLink
        {
            get { return _courierRedirectLink; }
            set { _courierRedirectLink = value; }
        }

        public EstimatedDeliveryDate estimatedDeliveryDate
        {
            get { return _estimatedDeliveryDate; }
            set { _estimatedDeliveryDate = value; }
        }

        public String getJSONPost()
        {
            JObject globalJSON = new JObject();
            JObject trackingJSON = new JObject();
            //	trackingJSON.Add("hola",
            trackingJSON.Add("tracking_number", new JValue(_trackingNumber));
            if (_slug != null) trackingJSON.Add("slug", new JValue(_slug));
            if (_title != null) trackingJSON.Add("title", new JValue(_title));
            if (_note != null) trackingJSON.Add("note", new JValue(_note));
            if (_orderDate != null) trackingJSON.Add("order_date", new JValue(_orderDate));
            if (_language != null) trackingJSON.Add("language", new JValue(_language));
            if (_orderPromisedDeliveryDate != null)
                trackingJSON.Add("order_promised_delivery_date", new JValue(_orderPromisedDeliveryDate));
            if (_deliveryType != null) trackingJSON.Add("delivery_type", new JValue(_deliveryType));
            if (_pickupLocation != null) trackingJSON.Add("pickup_location", new JValue(_pickupLocation));
            if (_pickupNote != null) trackingJSON.Add("pickup_note", new JValue(_pickupNote));
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
            if (_originCountryISO3 != 0)
                trackingJSON.Add("origin_country_iso3", new JValue(_originCountryISO3.ToString()));
            if (_orderID != null) trackingJSON.Add("order_id", new JValue(_orderID));
            if (_orderIDPath != null) trackingJSON.Add("order_id_path", new JValue(_orderIDPath));
            if (_orderNumber != null) trackingJSON.Add("order_number", new JValue(_orderNumber));

            if (_trackingAccountNumber != null)
                trackingJSON.Add("tracking_account_number", new JValue(_trackingAccountNumber));
            if (_trackingPostalCode != null) trackingJSON.Add("tracking_postal_code", new JValue(trackingPostalCode));
            if (_trackingShipDate != null) trackingJSON.Add("tracking_ship_date", new JValue(trackingShipDate));
            if (_trackingOriginCountry != null)
                trackingJSON.Add("tracking_origin_country", new JValue(_trackingOriginCountry));
            if (_trackingDestinationCountry != null)
                trackingJSON.Add("tracking_destination_country", new JValue(_trackingDestinationCountry));
            if (_trackingKey != null) trackingJSON.Add("tracking_key", new JValue(_trackingKey));
            if (_trackingState != null) trackingJSON.Add("tracking_state", new JValue(_trackingState));

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
            if (_note != null) trackingJSON.Add("note", new JValue(_note));
            if (_language != null) trackingJSON.Add("language", new JValue(_language));
            if (_orderPromisedDeliveryDate != null)
                trackingJSON.Add("order_promised_delivery_date", new JValue(_orderPromisedDeliveryDate));
            if (_deliveryType != null) trackingJSON.Add("delivery_type", new JValue(_deliveryType));
            if (_pickupLocation != null) trackingJSON.Add("pickup_location", new JValue(_pickupLocation));
            if (_pickupNote != null) trackingJSON.Add("pickup_note", new JValue(_pickupNote));
            if (_slug != null) trackingJSON.Add("slug", new JValue(_slug));

            if (_trackingAccountNumber != null)
                trackingJSON.Add("tracking_account_number", new JValue(_trackingAccountNumber));
            if (_trackingPostalCode != null) trackingJSON.Add("tracking_postal_code", new JValue(trackingPostalCode));
            if (_trackingShipDate != null) trackingJSON.Add("tracking_ship_date", new JValue(trackingShipDate));
            if (_trackingOriginCountry != null)
                trackingJSON.Add("tracking_origin_country", new JValue(_trackingOriginCountry));
            if (_trackingDestinationCountry != null)
                trackingJSON.Add("tracking_destination_country", new JValue(_trackingDestinationCountry));
            if (_trackingKey != null) trackingJSON.Add("tracking_key", new JValue(_trackingKey));
            if (_trackingState != null) trackingJSON.Add("tracking_state", new JValue(_trackingState));

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
            if (_orderNumber != null) trackingJSON.Add("order_number", new JValue(_orderNumber));
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

            if (this.trackingOriginCountry != null)
            {
                qs.add("tracking_origin_country", this.trackingOriginCountry);
                containsInfo = true;
            }

            if (this.trackingDestinationCountry != null)
            {
                qs.add("tracking_destination_country", this.trackingDestinationCountry);
                containsInfo = true;
            }

            if (this.trackingKey != null)
            {
                qs.add("tracking_key", this.trackingKey);
                containsInfo = true;
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

            if (this.trackingState != null)
            {
                qs.add("tracking_state", this.trackingState);
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
