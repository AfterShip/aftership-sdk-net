using System;
using System.Collections.Generic;
using Aftership.Enums;

namespace Aftership{
    /**
 * Keep the information for get trackings from the server, and interact with the results
 * Created by User on 13/6/14.
 */
    public class ParametersTracking
    {
        /** Page to show. (Default: 1) */
        private int page;

        /** Number of trackings each page contain. (Default and max: 100) */
        private int limit;

        /** Search the content of the tracking record fields: trackingNumber, title, orderId, customerName,
     * customFields, orderId, emails, smses */
        private String keyword;

        /** Unique courier code Use comma for multiple values. (Example: dhl,ups,usps) */
        private List<String> slugs;

        /**  Origin country of trackings. Use ISO Alpha-3 (three letters).
     * (Example: USA,HKG) */
        private List<ISO3Country> origin;

        /** Destination country of trackings. Use ISO Alpha-3 (three letters).
     * (Example: USA,HKG) */
        private List<ISO3Country> destination;

        /** Current status of tracking. */
        private List<StatusTag> tags;

        /** Start date and time of trackings created. AfterShip only stores data of 90 days.
     * (Defaults: 30 days ago, Example: 2013-03-15T16:41:56+08:00) */
        private DateTime createdAtMin;

        /** End date and time of trackings created. (Defaults: now, Example: 2013-04-15T16:41:56+08:00) */
        private DateTime createdAtMax;

        /** List of fields to include in the response. Fields to include: title, orderId, tag, checkpoints,
     * checkpointTime, message, countryName. (Defaults: none, Example: title,orderId) */
        private List<FieldTracking> fields;

        /** Language, default: ''
     * Example: 'en' Support Chinese to English translation for  china-ems  and  china-post  only */
        private String lang;

        /** Total of tracking elements from the user that match the ParametersTracking object*/
        private int total;


        public ParametersTracking() {
            this.page = 1;
            this.limit = 100; 
        }

        public void addSlug(String slug) {

            if (this.slugs == null) {
                this.slugs = new List<String>();
                this.slugs.Add(slug);
            } else {
                this.slugs.Add(slug);
            }
        }

        public void deleteSlug(String slug) {
            if (this.slugs != null) {
                this.slugs.Remove(slug);
            }
        }

        public void deleteSlug() {
            this.slugs = null;
        }

        public void addOrigin(ISO3Country origin) {

            if (this.origin == null) {
                this.origin = new List<ISO3Country>();
                this.origin.Add(origin);
            } else {
                this.origin.Add(origin);
            }
        }

        public void deleteOrigin(ISO3Country origin) {
            if (this.origin != null) {
                this.origin.Remove(origin);
            }
        }

        public void deleteOrigin() {
            this.origin = null;
        }

        public void addDestination(ISO3Country destination) {

            if (this.destination == null) {
                this.destination = new List<ISO3Country>();
                this.destination.Add(destination);
            } else {
                this.destination.Add(destination);
            }
        }

        public void deleteDestination(ISO3Country destination) {
            if (this.destination != null) {
                this.destination.Remove(destination);
            }
        }

        public void deleteDestination() {
            this.destination = null;
        }

        public void addTag(StatusTag tag) {

            if (this.tags == null) {
                this.tags = new List<StatusTag>();
                this.tags.Add(tag);
            } else {
                this.tags.Add(tag);
            }
        }

        public void deleteTag(StatusTag tag) {
            if (this.tags != null) {
                this.tags.Remove(tag);
            }
        }

        public void deleteTags() {
            this.tags = null;
        }

        public void addField(FieldTracking field) {

            if (this.fields == null) {
                this.fields = new List<FieldTracking>();
                this.fields.Add(field);
            } else {
                this.fields.Add(field);
            }
        }

        public void deleteField(FieldTracking field) {
            if (this.fields != null) {
                this.fields.Remove(field);
            }
        }

        public void deleteFields() {
            this.fields = null;
        }

        public int getPage() {
            return page;
        }

        public void setPage(int page) {
            this.page = page;
        }

        public int getLimit() {
            return limit;
        }

        public void setLimit(int limit) {
            this.limit = limit;
        }

        public String getKeyword() {
            return keyword;
        }

        public void setKeyword(String keyword) {
            this.keyword = keyword;
        }

        public DateTime? getCreatedAtMin() {
            return createdAtMin;
        }

        public void setCreatedAtMin(DateTime createdAtMin) {
            this.createdAtMin = createdAtMin;
        }

        public DateTime? getCreatedAtMax() {
            return createdAtMax;
        }

        public void setCreatedAtMax(DateTime createdAtMax) {
            this.createdAtMax = createdAtMax;
        }

        public String getLang() {
            return lang;
        }

        public void setLang(String lang) {
            this.lang = lang;
        }

        public int getTotal() {
            return total;
        }

        public void setTotal(int total) {
            this.total = total;
        }

        /**
    * Create a QueryString with all the fields of this class different of Null
    *
    * @return the String with the param codified in the QueryString
    */
        public String generateQueryString() {

            QueryString qs = new QueryString("page", this.page.ToString());
            qs.add("limit", this.limit.ToString());

            if (this.keyword != null) qs.add("keyword", this.keyword);
            if (this.createdAtMin !=  default(DateTime)) qs.add("created_at_min", DateMethods.ToString(this.createdAtMin));
            if (this.createdAtMax !=  default(DateTime)) qs.add("created_at_max", DateMethods.ToString(this.createdAtMax));
            if (this.lang != null)qs.add("lang", this.lang);

            if (this.slugs != null) qs.add("slug",this.slugs);

            if (this.origin != null) qs.add("origin",  string.Join(",", this.origin));

            if (this.destination != null) qs.add("destination", string.Join(",", this.destination));

            if (this.tags != null) qs.add("tag", string.Join(",", this.tags));

            if (this.fields != null) qs.add("fields", string.Join(",",this.fields));

            //globalJSON.put("tracking", trackingJSON);

            return qs.getQuery();
        }
    }
}

