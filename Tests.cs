using System;
using Newtonsoft.Json;
using Aftership.Enums;
using System.Collections.Generic;

namespace Aftership{

	public class Tests{

		static void Main(string[] args)
		{
   
//			//Create an instance of ConnectionAPI using the token of the user
            ConnectionAPI connection = new ConnectionAPI("????-?????-?????-???");
            //ConnectionAPI connection = new ConnectionAPI("93ac55c6-b86c-46dd-a8f4-5dfacd84a295");

//
//			//create a new tracking to add to our account
//			Tracking newTracking = new Tracking ("7126900292");
//			newTracking.slug = "dhl";
//			newTracking.title = "this is a test";
//			newTracking.addEmails ("emailprueba@gmail.com");
//			newTracking.addEmails ("another@gmail.com");
//			newTracking.addSmses("+85295340110");
//			newTracking.addSmses("+85295349999");
//			newTracking.customerName = "Mr Smith";
//			newTracking.destinationCountryISO3 = ISO3Country.HKG;
//			newTracking.orderID = "10000";
//			newTracking.orderIDPath = "wwww.aaaaa.com";
//			newTracking.trackingAccountNumber = "1234567";
//			newTracking.trackingPostalCode = "28046";
//			newTracking.trackingShipDate = "today";
//			newTracking.addCustomFields("price","1000");
//			newTracking.addCustomFields("product","iphone 5");
//
//			//before adding it, lets try to delete (otherwise maybe it would fail if already exist)
//			try{
//				//is important to catch the exceptions and read the messages,
//				//it will give you information why the transactions went wrong
//				if(connection.deleteTracking(newTracking))
//					Console.Write("Tracking deleted!!");
//			}catch(Exception e){
//				Console.Write (e.Message);
//			}
//
//			//now lets add the tracking
//			Tracking trackingAdded = connection.createTracking(newTracking);
//
//			//the tracking is added, it return a Tracking with the information
//			//that the system have of the tracking (probably it won't have the
//			//Checkpoints, cause the system didn't have time to retrieve them from
//			//the couriers, so we can retrieve the Tracking
//			Tracking trackingGet = connection.getTrackingByNumber (trackingAdded);
//
//			//trackingGet will have all the information we want (including checkpoints)
//			// there is 2 ways of get the tracking:
//			//	* by id
//			//	* by slug, tracking number and optinally required params
//
//			//example id:
////			Tracking trackingGet1 = new Tracking("");//you dont care about tracking number
////			trackingGet1.id = "53be255bfdacaaae7b17834b";
////			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);
////
//			//example slug, tracking:
//			Tracking trackingGet2 = new Tracking("RC328021065CN");
//			trackingGet2.slug = "canada-post";
//			connection.getTrackingByNumber (trackingGet2);
//
//			//example slug,tracking + required fields
//			Tracking trackingGet3 = new Tracking("RT406182863DE");
//			trackingGet3.slug = "deutsch-post";
//			trackingGet3.trackingShipDate = "20140627";
//			connection.getTrackingByNumber (trackingGet3);
//
//            Tracking trackingGet4 = new Tracking ("9405510897700003230737");
//            trackingGet4.slug = "usps";
////            Tracking trackingAdded1 = connection.createTracking(trackingGet4);
////            Console.WriteLine (">>>>"+trackingAdded1.ToString ());
//
//            Tracking test_usps = connection.getTrackingByNumber (trackingGet4);
//            Console.WriteLine ("");

//            int i;
//            List<Tracking> listTrackings = connection.getTrackings (1);
//            Console.WriteLine ("Number of trackings-> "+listTrackings.Count);
//
//            for (i = 0; i < listTrackings.Count; i++) {
//                Console.WriteLine (listTrackings [i].ToString ());
//                ;
//            }
//            DateTime newDateTime = DateMethods.getDate ("2014-10-20T03:24:18-00:00");
//
//            Console.WriteLine (DateMethods.ToString(newDateTime));

//            List<FieldCheckpoint> fields = new List<FieldCheckpoint>();
//            fields.Add(FieldCheckpoint.message);
//            Tracking trackingGet1 = new Tracking("whatever");
//            trackingGet1.id = "53d1e35405e166704ea8adb9";
//
//          
//            fields.Add(FieldCheckpoint.created_at);
//            //            System.out.println("list:"+fields.toString());
//            Checkpoint newCheckpoint2 = connection.getLastCheckpoint(trackingGet1,fields,"");
////            Assert.AreEqual( "Network movement commenced", newCheckpoint2.message);
//            Console.Write (DateMethods.ToString(newCheckpoint2.createdAt));
//          
//
            ParametersTracking parameters = new ParametersTracking();
            parameters.addSlug("dhl");
            DateTime date = DateTime.Today.AddMonths(-1);


            parameters.setCreatedAtMin (date);
            try{
            List<Tracking> totalDHL = connection.getTrackings(parameters);
            }catch(Exception e){
                Console.WriteLine (e);
            }
//            ParametersTracking param1 = new ParametersTracking();
//            param1.addDestination(ISO3Country.ESP);
//            param1.setLimit(20);
//            List<Tracking> totalSpain =connection.getTrackings(param1);
//           
//            List<Tracking> totalSpain2 =connection.getTrackingsNext(param1);
//
//            connection.getTrackings (1);
//
		}

	}
}
;
