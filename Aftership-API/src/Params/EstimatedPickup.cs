using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AftershipAPI.Params
{
    public class EstimatedPickup
    {
        /// The local order time of the package.
        public String orderTime;

        /// Order cut off time. AfterShip will set 18:00:00 as the default value.
        public String orderCutoffTime;

        /// Operating days in a week. Number refers to the weekday.
        /// E.g., [1,2,3,4,5] means operating days are from Monday to Friday.
        /// AfterShip will set [1,2,3,4,5] as the default value.
        public List<Int64> businessDays;

        public OrderProcessingTime orderProcessingTime;

        /// The local pickup time of the package.
        public String pickupTime;

        public EstimatedPickup(JObject jObject)
        {
            if (!jObject["orderTime"].IsNullOrEmpty())
            {
                orderTime = (String) jObject["orderTime"];
            }

            if (!jObject["orderCutoffTime"].IsNullOrEmpty())
            {
                orderCutoffTime = (String) jObject["orderCutoffTime"];
            }

            if (!jObject["business_days"].IsNullOrEmpty())
            {
                businessDays = new List<Int64>();
                JArray dayArr = (JArray) jObject["business_days"];
                for (int i = 0; i < dayArr.Count; i++)
                {
                    businessDays.Add((Int64) dayArr[i]);
                }
            }

            if (!jObject["order_processing_time"].IsNullOrEmpty())
            {
                orderProcessingTime = new OrderProcessingTime((JObject)jObject["order_processing_time"]);
            }
            
            if (!jObject["pickup_time"].IsNullOrEmpty())
            {
                pickupTime = (String) jObject["pickup_time"];
            }
        }
    }
}
