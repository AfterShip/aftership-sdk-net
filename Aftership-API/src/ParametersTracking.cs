using System;
using System.Collections.Generic;
using AftershipAPI.Enums;

namespace AftershipAPI
{
    /**
 * Keep the information for get trackings from the server, and interact with the results
 * Created by User on 13/6/14.
 */
    public class ParametersTracking
    {
        /** Page to show. (Default: 1) */
        private int _page;

        /** Number of trackings each page contain. (Default and max: 100) */
        private int _limit;

        /** Search the content of the tracking record fields: trackingNumber, title, orderId, customerName,
     * customFields, orderId, emails, smses */
        private String _keyword;

        /** Unique courier code Use comma for multiple values. (Example: dhl,ups,usps) */
        private List<String> _slugs;

        /**  Origin country of trackings. Use ISO Alpha-3 (three letters).
     * (Example: USA,HKG) */
        private List<ISO3Country> _origins;

        /** Destination country of trackings. Use ISO Alpha-3 (three letters).
     * (Example: USA,HKG) */
        private List<ISO3Country> _destinations;

        /** Current status of tracking. */
        private List<StatusTag> _tags;

        /** Start date and time of trackings created. AfterShip only stores data of 90 days.
     * (Defaults: 30 days ago, Example: 2013-03-15T16:41:56+08:00) */
        private DateTime _createdAtMin;

        /** End date and time of trackings created. (Defaults: now, Example: 2013-04-15T16:41:56+08:00) */
        private DateTime _createdAtMax;

        /** List of fields to include in the response. Fields to include: title, orderId, tag, checkpoints,
     * checkpointTime, message, countryName. (Defaults: none, Example: title,orderId) */
        private List<FieldTracking> _fields;

        /** Language, default: ''
     * Example: 'en' Support Chinese to English translation for  china-ems  and  china-post  only */
        private String _lang;

        /** Total of tracking elements from the user that match the ParametersTracking object*/
        private int _total;


        public ParametersTracking() {
            this._page = 1;
            this._limit = 100; 
        }

        public void addSlug(String slug) {
            if (_slugs == null) {
                _slugs = new List<String>();
                _slugs.Add(slug);
            }  else {
                _slugs.Add(slug);
            }
        }

        public void deleteRequireField(String slug){
            if (_slugs != null) {
                _slugs.Remove (slug);
            }
        }

        public void deleteSlugs(){
            _slugs = null;
        }

        public void addOrigin(ISO3Country origin) {
            if (_origins == null) {
                _origins = new List<ISO3Country>();
                _origins.Add(origin);
            }  else {
                _origins.Add(origin);
            }
        }

        public void deleteOrigin(ISO3Country origin){
            if (_origins != null) {
                _origins.Remove (origin);
            }
        }
            
        public void deleteOrigins(){
            _origins = null;
        }
    
        public void addDestination(ISO3Country destination) {
            if (_destinations == null) {
                _destinations = new List<ISO3Country>();
                _destinations.Add(destination);
            }  else {
                _destinations.Add(destination);
            }
        }

        public void deleteDestination(ISO3Country destination){
            if (_destinations != null) {
                _destinations.Remove (destination);
            }
        }

        public void deleteDestinations(){
            _destinations = null;
        }
     
        public void addTag(StatusTag tag) {
            if (_tags == null) {
                _tags = new List<StatusTag>();
                _tags.Add(tag);
            }  else {
                _tags.Add(tag);
            }
        }

        public void deletTag(StatusTag tag){
            if (_tags != null) {
                _tags.Remove (tag);
            }
        }

        public void deleteTags(){
            _tags = null;
        }

        public void addField(FieldTracking field) {
            if (_fields == null) {
                _fields = new List<FieldTracking>();
                _fields.Add(field);
            }  else {
                _fields.Add(field);
            }
        }

        public void deletField(FieldTracking field){
            if (_fields != null) {
                _fields.Remove (field);
            }
        }

        public void deleteFields(){
            _fields = null;
        }
      
        public int page{
            get { return _page; }
            set { _page= value; }
        }

        public int limit{
            get { return _limit; }
            set { _limit= value; }
        }

        public String keyword{
            get { return _keyword; }
            set { _keyword= value; }
        }
   
        public DateTime createdAtMin{
            get { return _createdAtMin; }
            set { _createdAtMin= value; }
        }

        public DateTime createdAtMax{
            get { return _createdAtMax; }
            set { _createdAtMax= value; }
        }

        public String lang{
            get { return _lang; }
            set { _lang= value; }
        }

        public int total{
            get { return _total; }
            set { _total= value; }
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

            if (this._slugs != null) qs.add("slug",this._slugs);

            if (this._origins != null) qs.add("origin",  string.Join(",", this._origins));

            if (this._destinations != null) qs.add("destination", string.Join(",", this._destinations));

            if (this._tags != null) qs.add("tag", string.Join(",", this._tags));

            if (this._fields != null) qs.add("fields", string.Join(",",this._fields));

            //globalJSON.put("tracking", trackingJSON);

            return qs.getQuery();
        }
    }
}

