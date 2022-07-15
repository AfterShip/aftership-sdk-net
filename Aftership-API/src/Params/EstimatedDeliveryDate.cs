using System;
using Newtonsoft.Json.Linq;

namespace AftershipAPI.Params
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
    
    public class EstimatedDeliveryDateParam
    {
        /// AfterShip's unique code of courier.
        /// Please refer to https://track.aftership.com/couriers/download.
        public String slug;

        /// Shipping and delivery options provided by the carrier.
        public String serviceTypeName;

        /// The location from where the package is picked up by the carrier to be delivered to the final destination.
        public Address originAddress;

        /// The final destination of the customer where the delivery will be made.
        public Address destinationAddress;

        /// AfterShip uses this object to calculate the total weight of the order.
        public Weight weight;

        /// The number of packages.
        public Int64 packageCount;

        /// The local pickup time of the package.
        /// Either `pickup_time` or `estimated_pickup` is required.
        public String pickupTime;

        /// The local pickup time of the package.
        /// Either `pickup_time` or `estimated_pickup` is required.
        public EstimatedPickup estimatedPickup;

        /// The estimated arrival date of the shipment, provided by AfterShip.
        public String estimatedDeliveryDate;

        /// The earliest estimated delivery date of the shipment, provided by AfterShip.
        public String estimatedDeliveryDateMin;

        /// The latest estimated delivery date of the shipment, provided by AfterShip.
        public String estimatedDeliveryDateMax;

        public EstimatedDeliveryDateParam(JObject jObject)
        {
            if (!jObject["slug"].IsNullOrEmpty())
            {
                slug = (String) jObject["slug"];
            }
            
            if (!jObject["serviceTypeName"].IsNullOrEmpty())
            {
                serviceTypeName = (String) jObject["serviceTypeName"];
            }
            
            if (!jObject["origin_address"].IsNullOrEmpty())
            {
                originAddress = new Address((JObject)jObject["origin_address"]);
            }
            
            if (!jObject["destination_address"].IsNullOrEmpty())
            {
                destinationAddress = new Address((JObject)jObject["destination_address"]);
            }
            
            if (!jObject["weight"].IsNullOrEmpty())
            {
                weight = new Weight((JObject)jObject["weight"]);
            }
            
            if (!jObject["package_count"].IsNullOrEmpty())
            {
                packageCount = (Int64) jObject["package_count"];
            }
            
            if (!jObject["pickup_time"].IsNullOrEmpty())
            {
                pickupTime = (String) jObject["pickup_time"];
            }
            
            if (!jObject["estimated_pickup"].IsNullOrEmpty())
            {
                estimatedPickup = new EstimatedPickup((JObject)jObject["estimated_pickup"]);
            }
            
            if (!jObject["estimated_delivery_date"].IsNullOrEmpty())
            {
                estimatedDeliveryDate = (String) jObject["estimated_delivery_date"];
            }
            
            if (!jObject["estimated_delivery_date_min"].IsNullOrEmpty())
            {
                estimatedDeliveryDateMin = (String) jObject["estimated_delivery_date_min"];
            }
            
            if (!jObject["estimated_delivery_date_max"].IsNullOrEmpty())
            {
                estimatedDeliveryDateMax = (String) jObject["estimated_delivery_date_max"];
            }
        }
    }
}
