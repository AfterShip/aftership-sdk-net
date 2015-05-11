using System;
using System.Collections.Generic;
using AftershipAPI.Enums;

namespace AftershipAPI
{

	/// <summary>
	/// Creates a url friendly String
	/// </summary>
	public class QueryString
	{
		private String query = "";

		//careful, this constructor creates the first element with &
		public QueryString(){}

		public QueryString(String name, String value) {
			encode(name, value);
		}
			

		public void add(String name, List<String> list) {
			query += "&";

			String value =String.Join(",",list.ToArray());
			encode(name, value);
		}

		public void add(String name, String value) {
			query += "&";
			encode(name, value);
		}

		private void encode(String name, String value) {
            query +=  System.Uri.EscapeDataString(name);
			query += "=";
            query +=  System.Uri.EscapeDataString(value);
		}

		public String getQuery() {
			return query;
		}

		public override String  ToString() {
			return getQuery();
		}
	}
}

