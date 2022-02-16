using System;
using Newtonsoft.Json.Linq;


namespace AftershipAPI
{
    public class EstimatedDeliveryDate
    {
        /// The estimated arrival date of the shipment.
        private String _estimatedDeliveryDate;

        /// The reliability of the estimated delivery date based on the trend of the transit time
        /// for the similar delivery route and the carrier's delivery performance
        /// range from 0.0 to 1.0 (Beta feature).
        private Nullable<float> _confidenceScore;

        /// Earliest estimated delivery date of the shipment.
        private String _estimatedDeliveryDateMin;

        /// Latest estimated delivery date of the shipment.
        private String _estimatedDeliveryDateMax;

        public EstimatedDeliveryDate(JObject estimatedDeliveryDateJSON)
        {
            this.estimatedDeliveryDate = estimatedDeliveryDateJSON["estimated_delivery_date"].IsNullOrEmpty()
                ? null
                : (String) estimatedDeliveryDateJSON["estimated_delivery_date"];
            this.confidenceScore = estimatedDeliveryDateJSON["confidence_score"].IsNullOrEmpty()
                ? null
                : (float) estimatedDeliveryDateJSON["confidence_score"];
            this.estimatedDeliveryDateMin = estimatedDeliveryDateJSON["estimated_delivery_date_min"].IsNullOrEmpty()
                ? null
                : (String) estimatedDeliveryDateJSON["estimated_delivery_date_min"];
            this.estimatedDeliveryDateMax = estimatedDeliveryDateJSON["estimated_delivery_date_max"].IsNullOrEmpty()
                ? null
                : (String) estimatedDeliveryDateJSON["estimated_delivery_date_max"];
        }

        public String estimatedDeliveryDate
        {
            get { return _estimatedDeliveryDate; }
            set { _estimatedDeliveryDate = value; }
        }

        public float? confidenceScore
        {
            get { return (float) _confidenceScore; }
            set { _confidenceScore = value; }
        }

        public String estimatedDeliveryDateMin
        {
            get { return _estimatedDeliveryDateMin; }
            set { _estimatedDeliveryDateMin = value; }
        }

        public String estimatedDeliveryDateMax
        {
            get { return _estimatedDeliveryDateMax; }
            set { _estimatedDeliveryDateMax = value; }
        }
    }
}
