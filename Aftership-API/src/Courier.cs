using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AftershipAPI
{
    public class Courier
    {
        /**
         * Unique code of courier
         */
        private String _slug;

        /**
         * Name of courier
         */
        private String _name;

        /**
         * Contact phone number of courier
         */
        private String _phone;

        /**
         * Other name of courier, if several they will be separated by commas
         */
        private String _other_name;

        /**
         * Website link of courier
         */
        private String _web_url;

        /**
         * Require fields for this courier
         */
        private List<String> _requireFields;

        /**
         * Require fields for this courier
         */
        private List<String> _optionalFields;

        /**
         * Default language of tracking results
         */
        private String _defaultLanguage;

        /**
         * Other supported languages
         */
        private List<String> _supportLanguages;

        /**
         * Country code (ISO Alpha-3) where the courier provides service
         */
        private List<String> _serviceFromCountryIso3;

        /**
         * Default constructor with all the fields of the class
         */
        public Courier(String web_url, String slug, String name, String phone, String other_name)
        {
            this._web_url = web_url;
            this._slug = slug;
            this._name = name;
            this._phone = phone;
            this._other_name = other_name;
        }

        /**
         * Constructor, creates a Courier from a JSONObject with the information of the Courier,
         * if any field is not specified it will be ""
         *
         * @param jsonCourier   A JSONObject with information of the Courier
         * by the API.
         */
        // _trackingNumber = trackingJSON["tracking_number"]==null?null:(String)trackingJSON["tracking_number"];
        public Courier(JObject jsonCourier)
        {
            this._web_url = jsonCourier["web_url"] == null ? null : (String) jsonCourier["web_url"];
            this._slug = jsonCourier["slug"] == null ? null : (String) jsonCourier["slug"];
            this._name = jsonCourier["name"] == null ? null : (String) jsonCourier["name"];
            this._phone = jsonCourier["phone"] == null ? null : (String) jsonCourier["phone"];
            this._other_name = jsonCourier["other_name"] == null ? null : (String) jsonCourier["other_name"];

            JArray requireFieldsArray =
                jsonCourier["required_fields"] == null ? null : (JArray) jsonCourier["required_fields"];
            if (requireFieldsArray != null && requireFieldsArray.Count != 0)
            {
                this._requireFields = new List<String>();
                for (int i = 0; i < requireFieldsArray.Count; i++)
                {
                    this._requireFields.Add(requireFieldsArray[i].ToString());
                }
            }

            JArray optionalFieldsArray =
                jsonCourier["optional_fields"] == null ? null : (JArray) jsonCourier["optional_fields"];
            if (optionalFieldsArray != null && optionalFieldsArray.Count != 0)
            {
                this._optionalFields = new List<String>();
                for (int i = 0; i < optionalFieldsArray.Count; i++)
                {
                    this._optionalFields.Add(optionalFieldsArray[i].ToString());
                }
            }
            
            this._defaultLanguage = jsonCourier["default_language"] == null ? null : (String) jsonCourier["default_language"];
            
            JArray supportLanguagesArray =
                jsonCourier["support_languages"] == null ? null : (JArray) jsonCourier["support_languages"];
            if (supportLanguagesArray != null && supportLanguagesArray.Count != 0)
            {
                this._supportLanguages = new List<String>();
                for (int i = 0; i < supportLanguagesArray.Count; i++)
                {
                    this._supportLanguages.Add(supportLanguagesArray[i].ToString());
                }
            }
            
            JArray serviceFromCountryIso3Array =
                jsonCourier["service_from_country_iso3"] == null ? null : (JArray) jsonCourier["service_from_country_iso3"];
            if (serviceFromCountryIso3Array != null && serviceFromCountryIso3Array.Count != 0)
            {
                this._serviceFromCountryIso3 = new List<String>();
                for (int i = 0; i < serviceFromCountryIso3Array.Count; i++)
                {
                    this._serviceFromCountryIso3.Add(serviceFromCountryIso3Array[i].ToString());
                }
            }
        }

        public String TooString()
        {
            return "Courier{" +
                   "slug='" + _slug + '\'' +
                   ", name='" + _name + '\'' +
                   ", phone='" + _phone + '\'' +
                   ", other_name='" + _other_name + '\'' +
                   ", web_url='" + _web_url + '\'' +
                   '}';
        }

        public String slug
        {
            get { return _slug; }
            set { _slug = value; }
        }

        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public String other_name
        {
            get { return _other_name; }
            set { _other_name = value; }
        }

        public String web_url
        {
            get { return _web_url; }
            set { _web_url = value; }
        }
        
        public String defaultLanguage
        {
            get { return _defaultLanguage; }
            set { _defaultLanguage = value; }
        }

        public List<String> requireFields
        {
            get { return _requireFields; }
            set { _requireFields = value; }
        }

        public void addRequireField(String requireField)
        {
            if (_requireFields == null)
            {
                _requireFields = new List<String>();
                _requireFields.Add(requireField);
            }
            else
            {
                _requireFields.Add(requireField);
            }
        }

        public void deleteRequireField(String requireField)
        {
            if (_requireFields != null)
            {
                _requireFields.Remove(requireField);
            }
        }

        public void deleteRequireFields()
        {
            _requireFields = null;
        }


        public List<String> optionalFields
        {
            get { return _optionalFields; }
            set { _optionalFields = value; }
        }

        public void addOptionalField(String optionalField)
        {
            if (_optionalFields == null)
            {
                _optionalFields = new List<String>();
                _optionalFields.Add(optionalField);
            }
            else
            {
                _optionalFields.Add(optionalField);
            }
        }

        public void deleteOptionalField(String optionalField)
        {
            if (_optionalFields != null)
            {
                _optionalFields.Remove(optionalField);
            }
        }

        public void deleteOptionalFields()
        {
            _optionalFields = null;
        }
        
        public List<String> supportLanguages
        {
            get { return _supportLanguages; }
            set { _supportLanguages = value; }
        }

        public void addSupportLanguages(String supportLanguage)
        {
            if (_supportLanguages == null)
            {
                _supportLanguages = new List<String>();
                _supportLanguages.Add(supportLanguage);
            }
            else
            {
                _supportLanguages.Add(supportLanguage);
            }
        }

        public void deleteSupportLanguages(String supportLanguage)
        {
            if (_supportLanguages != null)
            {
                _supportLanguages.Remove(supportLanguage);
            }
        }

        public void deleteSupportLanguagess()
        {
            _supportLanguages = null;
        }
        
        public List<String> serviceFromCountryIso3
        {
            get { return _serviceFromCountryIso3; }
            set { _serviceFromCountryIso3 = value; }
        }

        public void addServiceFromCountryIso3(String serviceFromCountryIso3)
        {
            if (_serviceFromCountryIso3 == null)
            {
                _serviceFromCountryIso3 = new List<String>();
                _serviceFromCountryIso3.Add(serviceFromCountryIso3);
            }
            else
            {
                _serviceFromCountryIso3.Add(serviceFromCountryIso3);
            }
        }

        public void deleteServiceFromCountryIso3(String serviceFromCountryIso3)
        {
            if (_serviceFromCountryIso3 != null)
            {
                _serviceFromCountryIso3.Remove(serviceFromCountryIso3);
            }
        }

        public void deleteServiceFromCountryIso3s()
        {
            _serviceFromCountryIso3 = null;
        }
    }
}
