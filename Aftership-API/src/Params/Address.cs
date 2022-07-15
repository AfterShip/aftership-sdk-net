using System;
using Newtonsoft.Json.Linq;

namespace AftershipAPI.Params
{
    public class Address
    {
        /// The country/region of the origin location from where the package is
        /// picked up by the carrier to be delivered to the final destination.
        /// Use 3 letters of ISO 3166-1 country/region code.
        public String country;

        /// State, province, or the equivalent location of the origin address.
        /// Either `origin_address.state` or `origin_address.postal_code` is required.
        public String state;

        /// City of the origin address.
        public String city;

        /// Postal code of the origin address.
        /// Either `origin_address.state` or `origin_address.postal_code` is required.
        public String postalCode;

        /// Raw location of the origin address.
        /// A raw address will help AI to identify the accurate location of the origin address.
        public String rawLocation;

        public Address(JObject jObject)
        {
            if (!jObject["country"].IsNullOrEmpty())
            {
                country = (String) jObject["country"];
            }
            
            if (!jObject["state"].IsNullOrEmpty())
            {
                state = (String) jObject["state"];
            }
            
            if (!jObject["city"].IsNullOrEmpty())
            {
                city = (String) jObject["city"];
            }
            
            if (!jObject["postal_code"].IsNullOrEmpty())
            {
                postalCode = (String) jObject["postal_code"];
            }
            
            if (!jObject["raw_location"].IsNullOrEmpty())
            {
                rawLocation = (String) jObject["raw_location"];
            }
        }
    }
}
