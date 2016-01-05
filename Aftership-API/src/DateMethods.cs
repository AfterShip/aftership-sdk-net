using System;
// changes to support Russian culture, check this http://stackoverflow.com/questions/2193012/string-was-not-recognized-as-a-valid-datetime-format-dd-mm-yyyy

namespace AftershipAPI
{
    public class DateMethods
    {
        private static String ISO8601Short = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";
		

        public static String ToString(DateTime date){
			// since we pass it to UniversalTime we can add the +00:00 manually
            return date.ToString(ISO8601Short);

        }
    }
}

