using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AftershipAPI.Params
{
    public class EstimatedDeliveryDateBatchPredictParam
    {
        public List<EstimatedDeliveryDateParam> estimatedDeliveryDates;

        public EstimatedDeliveryDateBatchPredictParam(List<EstimatedDeliveryDateParam> estimatedDeliveryDates)
        {
            this.estimatedDeliveryDates = estimatedDeliveryDates;
        }

        public String getJSONPost()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(this, serializerSettings);
        }
    }
}
