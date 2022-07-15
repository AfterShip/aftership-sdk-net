using System;
using Newtonsoft.Json.Linq;

namespace AftershipAPI.Params
{
    public class Weight
    {
        /// The weight unit of the package.
        public String unit;

        /// The weight of the shipment.
        public Int64 value;

        public Weight(JObject jObject)
        {
            if (!jObject["unit"].IsNullOrEmpty())
            {
                unit = (String) jObject["unit"];
            }
            
            if (!jObject["value"].IsNullOrEmpty())
            {
                value = (Int64) jObject["value"];
            }
        }
    }
}
