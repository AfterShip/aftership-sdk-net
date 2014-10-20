using System;

namespace Aftership
{
    public class DateMethods
    {
        private static String ISO8601Long = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";



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
            Console.WriteLine (date.Length +"date >>> " + date);
            if (date.Length == 25) {
                    return DateTime.ParseExact(date,ISO8601Long, System.Globalization.CultureInfo.InvariantCulture);  
            }

            return  Convert.ToDateTime(date);  ;
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

