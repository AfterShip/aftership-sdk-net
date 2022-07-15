using System;
using Newtonsoft.Json.Linq;

namespace AftershipAPI.Params
{
    public class OrderProcessingTime
    {
        /// Processing time of an order, from being placed to being picked up.
        /// Only support day as value now. AfterShip will set day as the default value.
        public String unit;

        /// Processing time of an order, from being placed to being picked up.
        /// AfterShip will set 0 as the default value.
        public Int64 value;

        public OrderProcessingTime(JObject jObject)
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
