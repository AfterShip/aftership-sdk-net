using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Aftership
{
    public class Courier
    {
        /** Unique code of courier */
        private String slug;
        /** Name of courier */
        private String name;
        /** Contact phone number of courier */
        private String phone;
        /** Other name of courier, if several they will be separated by commas */
        private String other_name;
        /** Website link of courier */
        private String web_url;
        /** Require fields for this courier */
        private List<String> requireFields;

        /** Default constructor with all the fields of the class */
        public Courier(String web_url, String slug, String name, String phone, String other_name) {
            this.web_url = web_url;
            this.slug = slug;
            this.name = name;
            this.phone = phone;
            this.other_name = other_name;
        }

        /**
     * Constructor, creates a Courier from a JSONObject with the information of the Courier,
     * if any field is not specified it will be ""
     *
     * @param jsonCourier   A JSONObject with information of the Courier
     * by the API.
     **/           // _trackingNumber = trackingJSON["tracking_number"]==null?null:(String)trackingJSON["tracking_number"];

        public Courier(JObject jsonCourier){
            this.web_url = jsonCourier["web_url"]==null?null:(String)jsonCourier["web_url"];
            this.slug =  jsonCourier["slug"]==null?null:(String)jsonCourier["slug"];
            this.name = jsonCourier["name"]==null?null:(String)jsonCourier["name"];
            this.phone = jsonCourier["phone"]==null?null:(String)jsonCourier["phone"];
            this.other_name = jsonCourier["other_name"]==null?null:(String)jsonCourier["other_name"];

            JArray requireFieldsArray =jsonCourier["required_fields"]==null?null:(JArray)jsonCourier["required_fields"];
            if(requireFieldsArray !=null && requireFieldsArray.Count !=0){
                this.requireFields = new List <String>();
                for (int i=0;i<requireFieldsArray.Count;i++){
                    this.requireFields.Add(requireFieldsArray[i].ToString());
                }
            }

        }

        public String TooString() {
            return "Courier{" +
                "slug='" + slug + '\'' +
                ", name='" + name + '\'' +
                ", phone='" + phone + '\'' +
                ", other_name='" + other_name + '\'' +
                ", web_url='" + web_url + '\'' +
                '}';
        }

        public String getSlug() {
            return slug;
        }

        public void setSlug(String slug) {
            this.slug = slug;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getPhone() {
            return phone;
        }

        public void setPhone(String phone) {
            this.phone = phone;
        }

        public String getOther_name() {
            return other_name;
        }

        public void setOther_name(String other_name) {
            this.other_name = other_name;
        }

        public String getWeb_url() {
            return web_url;
        }

        public void setWeb_url(String web_url) {
            this.web_url = web_url;
        }

        public List<String> getRequireFields() {
            return this.requireFields;
        }

        public void addRequierField(String requierField) {

            if (this.requireFields == null) {
                this.requireFields = new List<String>();
                this.requireFields.Add(requierField);
            } else
                this.requireFields.Add(requierField);
        }

        public void deleteRequierField(String requierField) {
            if (this.requireFields != null) {
                this.requireFields.Remove(requierField);
            }
        }

        public void deleteRequierFields() {
            this.requireFields = null;
        }
    }
}

