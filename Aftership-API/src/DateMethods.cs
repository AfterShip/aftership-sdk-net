using System;

namespace AftershipAPI
{
    public class DateMethods
    {
        private static String ISO8601Long = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";
        private static String DateFormat = "MM'/'dd'/'yyyy HH:mm:ss";
        public static DateTime getDate( String date){
//            SimpleDateFormat dateFormat;
//            StringBuilder sb = new StringBuilder(date);
//            DateTime newDate = null;
//            if (sb.length() == 25) {
//                dateFormat = new SimpleDateFormat(ISO8601Long);
//                dateFormat.setTimeZone(TimeZone.getTimeZone("UTC"));
//                sb.deleteCharAt(22);
//                newDate = dateFormat.parse(sb.toString());
//                return newDate;
//            } else{
//                throw new AftershipAPIException("The date receive is not properly formatted yyyy-MM-dd'T'HH:mm:ssZ and is: "
//                    +date);
            //            },
//            Console.WriteLine (date.Length +"date >>> " + date);
            if (date.Length == 25) {
                    return DateTime.ParseExact(date, ISO8601Long,
                        System.Globalization.CultureInfo.InvariantCulture);  
            }
            DateTime responseDateTime = DateTime.MinValue;
            try
            {
                responseDateTime = DateTime.ParseExact(date, DateFormat,
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                DateTime.TryParse(date, out responseDateTime);
            }

            return responseDateTime;
        }

        public static String ToString(DateTime date){
//
//            SimpleDateFormat dateFormat;
//            StringBuilder sb;
//            dateFormat = new SimpleDateFormat(ISO8601Long);
//            dateFormat.setTimeZone(TimeZone.getTimeZone("UTC"));
//
//            sb = new StringBuilder(dateFormat.format(date));
//            sb.insert(22,':');
//
//            return sb.toString();
            return date.ToUniversalTime().ToString (ISO8601Long);

        }
    }
}

