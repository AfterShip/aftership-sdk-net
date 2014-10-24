using System;
using NUnit.Framework;
using Aftership;
using Aftership.Enums;
using System.Collections.Generic;

namespace MonoTests.TestConnectionAPI
{



	[TestFixture]
	public class TestConnectionAPI
	{
		//remember to change your API Key
        ConnectionAPI connection  = new ConnectionAPI("???????-????-????????");
        static int TOTAL_COURIERS_API = 222;

        String [] couriersDetected={"dpd","fedex"};

		//post tracking number
		String trackingNumberPost ="05167019264110";
		String slugPost = "dpd";
		String orderIDPathPost ="www.whatever.com";
		String orderIDPost ="ID 1234";
		String customerNamePost="Mr Smith";
		String titlePost = "this title";
		ISO3Country countryDestinationPost=ISO3Country.USA;
		String email1Post ="email@yourdomain.com";
		String email2Post ="another_email@yourdomain.com";
		String sms1Post ="+85292345678";
		String sms2Post = "+85292345679";
		String customProductNamePost ="iPhone Case";
		String customProductPricePost ="USD19.99";

		//tracking numbers to detect
		String trackingNumberToDetect ="09445246482536";
		String trackingNumberToDetectError = "asdq";

		//Tracking to Delete
		String trackingNumberDelete = "596454081704";
		String slugDelete ="fedex";


		//tracking to DeleteBad
		String trackingNumberDelete2 = "798865638020";

		static bool firstTime = true;

        Dictionary <string, string> firstCourier= new  Dictionary<string, string>();
        Dictionary <string, string> firstCourierAccount= new  Dictionary<string, string>();
       

		public TestConnectionAPI ()
		{
		}

		[SetUp]		
		public void setUp(){
			if(firstTime) {
                Console.WriteLine ("****************SET-UP BEGIN**************");
				firstTime=false;
				//delete the tracking we are going to post (in case it exist)
				Tracking tracking = new Tracking("05167019264110");
				tracking.slug = "dpd";

                //first courier
                firstCourier.Add("slug","india-post-int");
                firstCourier.Add("name","India Post International");
                firstCourier.Add("phone","+91 1800 11 2011");
                firstCourier.Add("other_name","भारतीय डाक, Speed Post & eMO, EMS, IPS Web");
                firstCourier.Add("web_url","http://www.indiapost.gov.in/");

                //first courier in your account
                firstCourierAccount.Add("slug","usps");
                firstCourierAccount.Add("name","USPS");
                firstCourierAccount.Add("phone","+1 800-275-8777");
                firstCourierAccount.Add("other_name","United States Postal Service");
                firstCourierAccount.Add("web_url","https://www.usps.com");

				try {connection.deleteTracking(tracking); } catch (Exception e) {
					Console.WriteLine("**1"+e.Message);}
				Tracking tracking1 = new Tracking(trackingNumberToDetect);
				tracking1.slug = "dpd";
				try{connection.deleteTracking(tracking1);} catch (Exception e) {
					Console.WriteLine("**2"+e.Message);}
				try{
					Tracking newTracking = new Tracking(trackingNumberDelete);
					newTracking.slug = slugDelete;
					connection.createTracking(newTracking);}catch (Exception e) {
					Console.WriteLine("**3"+e.Message);
				}
                try{
                    Tracking newTracking1 = new Tracking("9400110897700003231250");
                    newTracking1.slug = "usps";
                    connection.createTracking(newTracking1);}catch (Exception e) {
                        Console.WriteLine("**4"+e.Message);

                }
                Console.WriteLine ("****************SET-UP FINISH**************");


			}

		}

        [Test]
        public void testDetectCouriers(){

            //get trackings of this number.
            List<Courier> couriers = connection.detectCouriers(trackingNumberToDetect);
            Assert.AreEqual( 2, couriers.Count);
            //the couriers should be dpd or fedex
            Assert.IsTrue(Equals(couriers[0].slug,couriersDetected[0])
                || Equals(couriers[1].slug,couriersDetected[0]));
            Assert.IsTrue(Equals(couriers[0].slug,couriersDetected[1])
                || Equals(couriersDetected[1],couriers[1].slug));

            //if the trackingNumber doesn't match any courier defined, should give an error.

            try {
                List<Courier> couriers1 = connection.detectCouriers(trackingNumberToDetectError);
                Assert.AreEqual (0, couriers1.Count);
            }catch (Exception e){
                Assert.AreEqual("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\"}}}", e.Message);
            }

            List<String> slugs = new List<String>();
            slugs.Add("dtdc");
            slugs.Add("ukrposhta");
            slugs.Add("usps");
            //   slugs.add("asdfasdfasdfasd");
            slugs.Add("dpd");
            List<Courier> couriers2 = connection.detectCouriers(trackingNumberToDetect,"28046","",null,slugs);
            Assert.AreEqual(1, couriers2.Count);
        }

		[Test]
		public void TestCreateTracking() {
			Tracking tracking1 = new Tracking(trackingNumberPost);
			tracking1.slug = slugPost;
			tracking1.orderIDPath = orderIDPathPost;
			tracking1.customerName = customerNamePost;
			tracking1.orderID = orderIDPost;
			tracking1.title = titlePost ;
			tracking1.destinationCountryISO3 = countryDestinationPost;
			tracking1.addEmails(email1Post);
			tracking1.addEmails(email2Post);
			tracking1.addCustomFields("product_name",customProductNamePost);
			tracking1.addCustomFields("product_price",customProductPricePost);
			tracking1.addSmses(sms1Post);
			tracking1.addSmses(sms2Post);
			Tracking trackingPosted = connection.createTracking(tracking1);

			Assert.AreEqual(trackingNumberPost, trackingPosted.trackingNumber,"#A01");
			Assert.AreEqual(slugPost, trackingPosted.slug,"#A02");
			Assert.AreEqual(orderIDPathPost, trackingPosted.orderIDPath,"#A03");
			Assert.AreEqual(orderIDPost, trackingPosted.orderID,"#A04");
			Assert.AreEqual(countryDestinationPost,
				trackingPosted.destinationCountryISO3,"#A05");

			Assert.IsTrue(trackingPosted.emails.Contains(email1Post),"#A06");
			Assert.IsTrue(trackingPosted.emails.Contains(email2Post),"#A07");
			Assert.AreEqual( 2, trackingPosted.emails.Count,"#A08");

			Assert.IsTrue(trackingPosted.smses.Contains(sms1Post),"#A09");
			Assert.IsTrue(trackingPosted.smses.Contains(sms2Post),"#A10");
			Assert.AreEqual( 2, trackingPosted.smses.Count,"#A11");

			Assert.AreEqual(customProductNamePost,
				trackingPosted.customFields["product_name"],"#A12");
			Assert.AreEqual(customProductPricePost,
				trackingPosted.customFields["product_price"],"#A13");
		}

		[Test]
		public void TestCreateTrackingEmptySlug() {
			//test post only informing trackingNumber (the slug can be dpd and fedex)
			Tracking tracking2 = new Tracking(trackingNumberToDetect);
			Tracking trackingPosted2 = connection.createTracking(tracking2);
			Assert.AreEqual( trackingNumberToDetect, trackingPosted2.trackingNumber,"#A14");
			Assert.AreEqual("dpd", trackingPosted2.slug,"#A15");//the system assign dpd (it exist)


		}

		//test post tracking number doesn't exist
		[Test]
		public void TestCreateTrackingError() {
			Tracking tracking3 = new Tracking (trackingNumberToDetectError);

			try {
				connection.createTracking (tracking3);
				//always should give an exception before this
				Assert.IsTrue (false, "#A16");
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				Assert.AreEqual ("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\",\"title\":\"asdq\"}}}",
					e.Message, "#A17");
			}
		}


		[Test]
		public void testDeleteTracking(){

			//delete a tracking number (posted in the setup)
			Tracking deleteTracking = new Tracking(trackingNumberDelete);
			deleteTracking.slug = slugDelete;
			Assert.IsTrue(connection.deleteTracking(deleteTracking), "#A18");
	
		}

		[Test]
		public void testDeleteTracking1(){
			//if the slug is bad informed
			try{
				Tracking deleteTracking2 = new Tracking(trackingNumberDelete2);
				Assert.IsTrue( connection.deleteTracking(deleteTracking2), "#A19");
				//always should give an exception before this
				Assert.IsTrue(false);
			}catch (Exception e){
				Assert.AreEqual( "{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//798865638020\"}}",
					e.Message, "#A20");
			}
		
		}

		[Test]
		public void testDeleteTracking2(){
		
			//if the trackingNumber is bad informed
			try{
				Tracking deleteTracking3 = new Tracking("adfa");
				deleteTracking3.slug="fedex";
				Assert.IsTrue(connection.deleteTracking(deleteTracking3),"#A20");
				//always should give an exception before this
				Assert.IsTrue(false,"#A21");
			}catch (Exception e){
                Assert.AreEqual("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{}}",
					e.Message,"#A22");
			}
		}

		[Test]
		public void testGetTrackingByNumber(){
            String trackingNumber = "RR814001189ES";
			String slug = "spain-correos-es";

			Tracking trackingGet1 = new Tracking(trackingNumber);
			trackingGet1.slug = slug;

			Tracking tracking = connection.getTrackingByNumber(trackingGet1);
			Assert.AreEqual(trackingNumber, tracking.trackingNumber,"#A23");
			Assert.AreEqual( slug, tracking.slug,"#A24");
			Assert.AreEqual( null, tracking.shipmentType,"#A25");

			List <Checkpoint> checkpoints = tracking.checkpoints;
			Checkpoint lastCheckpoint = checkpoints [checkpoints.Count - 1];
			Assert.IsTrue (checkpoints!=null,"A25-1");
			Assert.AreEqual (6 , checkpoints.Count,"A25-2");

            Assert.AreEqual ("Salida de la Oficina Internacional de origen" ,lastCheckpoint.message,"A25-3");
            Assert.AreEqual ("Spain" ,lastCheckpoint.countryName,"A25-4");


		}

		[Test]
		public void testGetTrackingByNumber2(){

			//slug is bad informed
			try{
				Tracking trackingGet2 = new Tracking("RC328021065CN");

				connection.getTrackingByNumber(trackingGet2);
				//always should give an exception before this
				Assert.IsTrue(false,"#A26");
			}catch (Exception e){
				Assert.AreEqual("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//RC328021065CN\"}}",
					e.Message,"#A27");
			}
		}

		[Test]
		public void testGetTrackingByNumber3(){

			//if the trackingNumber is bad informed
			try{
				Tracking trackingGet3 = new Tracking("adf");
				trackingGet3.slug = "fedex";
				connection.getTrackingByNumber(trackingGet3);
				//always should give an exception before this
				Assert.IsTrue(false,"#A28");
			}catch (Exception e){
				Console.Write (e.Message);
				Assert.AreEqual("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{}}",
					e.Message,"#A29");
			}
		}

		[Test]
		public void testGetTrackingByNumber4(){
			List<FieldTracking> fields = new List<FieldTracking>();
			fields.Add(FieldTracking.tracking_number);
			fields.Add(FieldTracking.active);
            Tracking trackingGet1 = new Tracking("RR814001189ES");
			trackingGet1.slug = "spain-correos-es";
			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1,fields,"");
            Assert.AreEqual( "RR814001189ES", tracking3.trackingNumber,"#A30");
			Assert.AreEqual( null, tracking3.title,"#A31");
			Assert.AreEqual( null, tracking3.slug,"#A32");
			Assert.AreEqual( null, tracking3.checkpoints,"#A33");
		}

		[Test]
		public void testGetTrackingByNumber5(){
			//courier require postalCode
			Tracking trackingGet1 = new Tracking("8W9JM0014847A094");
			trackingGet1.slug = "arrowxl";
			trackingGet1.trackingPostalCode = "BB102PN";
			List<FieldTracking> fields = new List<FieldTracking>();
			fields.Add(FieldTracking.id);

			fields.Add(FieldTracking.tracking_number);
			fields.Add(FieldTracking.slug);
			fields.Add(FieldTracking.source);

			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1,fields,"");

            Assert.AreEqual("53cf265d3dd72435213fa015", tracking3.id,"#B1");
			Assert.AreEqual( "api",tracking3.source,"#B2");
			Assert.IsNull( tracking3.title,"#B3");

		}

		[Test]
		public void testGetTrackingByNumber6(){
			//courier require postalCode
			Tracking trackingGet1 = new Tracking("20098147105");
			trackingGet1.slug = "dynamic-logistics";
			trackingGet1.trackingAccountNumber = "159484";

			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

			Assert.AreEqual("53be255bfdacaaae7b178345", tracking3.id,"#C1");
			Assert.AreEqual( "Zyg7Iem1x",tracking3.uniqueToken,"#C2");

		}

		[Test]
		public void testGetTrackingByNumber7(){
			//courier require postalCode
			Tracking trackingGet1 = new Tracking("RT406182863DE");
			trackingGet1.slug = "deutsch-post";
			trackingGet1.trackingShipDate = "20140627";

			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

            Assert.AreEqual("54389b1328e9f0272ecc2513", tracking3.id,"#D1");
			Assert.AreEqual( false,tracking3.active,"#D2");

		}

		[Test]
		public void testGetTrackingByNumber8(){
			//courier require by id (not need string)
			Tracking trackingGet1 = new Tracking("");
			trackingGet1.id = "53be255bfdacaaae7b178345";

			trackingGet1.trackingAccountNumber = "159484";

			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

			Assert.AreEqual("20098147105", tracking3.trackingNumber,"#E1");
			Assert.AreEqual( "159484",tracking3.trackingAccountNumber,"#E2");
		}

		[Test]
		public void testGetTrackingByNumber9(){
			//courier require postalCode
			Tracking trackingGet1 = new Tracking("");
            trackingGet1.id = "54389b1328e9f0272ecc2513";

			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

            Assert.AreEqual("RT406182863DE", tracking3.trackingNumber,"#F1");
			Assert.AreEqual( "20140627",tracking3.trackingShipDate,"#F2");

		}
            

        [Test]
        public void testGetTrackingByNumber10(){
            //courier require postalCode
            Tracking trackingGet1 = new Tracking("9405510897700003230737");
            trackingGet1.slug = "usps";
            Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

            Assert.AreEqual("Priority Mail 2-Day&#153;", tracking3.shipmentType,"#C1");
            Assert.AreEqual( null,tracking3.expectedDelivery,"#C2");

        }

        [Test]
        public void testGetTrackingByNumber11(){
            //courier require postalCode
            Tracking trackingGet1 = new Tracking("9400110897700003231250");
            trackingGet1.slug = "usps";
            Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

            Assert.AreEqual("First-Class Package Service", tracking3.shipmentType,"#C1");
            Assert.AreEqual( "Shipping Label Created",tracking3.checkpoints[0].message,"#C2");

    }
        [Test]
        public void testGetTrackingByNumber12(){
            int i;
            List<Tracking> listTrackings = connection.getTrackings (1);
            Console.WriteLine ("number of trackings"+listTrackings.Count);
            for (i = 0; i < listTrackings.Count; i++) {
                listTrackings [i].ToString ();
            }
  
        }
        [Test]
        public void testGetLastCheckpointID(){
            Tracking trackingGet1 = new Tracking("whatever");
            trackingGet1.id = "53d1e35405e166704ea8adb9";
            Checkpoint newCheckpoint = connection.getLastCheckpoint(trackingGet1);
            Assert.AreEqual( "Network movement commenced", newCheckpoint.message);
            Assert.AreEqual( "WIGAN HUB", newCheckpoint.countryName);
            Assert.AreEqual( "InTransit", newCheckpoint.tag);
        }

        [Test]
        public void testGetLastCheckpoint2ID() {
            List<FieldCheckpoint> fields = new List<FieldCheckpoint>();
            fields.Add(FieldCheckpoint.message);
            Tracking trackingGet1 = new Tracking("whatever");
            trackingGet1.id = "53d1e35405e166704ea8adb9";

            Checkpoint newCheckpoint1 = connection.getLastCheckpoint(trackingGet1,fields,"");
            Assert.AreEqual( "Network movement commenced", newCheckpoint1.message);
            Assert.AreEqual("0001-01-01T00:00:00+00:00",DateMethods.ToString(newCheckpoint1.createdAt));

            fields.Add(FieldCheckpoint.created_at);
//            System.out.println("list:"+fields.toString());
            Checkpoint newCheckpoint2 = connection.getLastCheckpoint(trackingGet1,fields,"");
            Assert.AreEqual( "Network movement commenced", newCheckpoint2.message);
            Assert.AreEqual("2014-07-25T04:55:49+00:00", DateMethods.ToString(newCheckpoint2.createdAt));
        }

        [Test]
        public void testGetLastCheckpoint3ID(){
            List<FieldCheckpoint> fields = new List<FieldCheckpoint>();
            fields.Add(FieldCheckpoint.message);
            Tracking trackingGet1 = new Tracking("whatever");
            trackingGet1.id = "53d1e35405e166704ea8adb9";


            Checkpoint newCheckpoint1 = connection.getLastCheckpoint(trackingGet1,fields,"");
            Assert.AreEqual("Network movement commenced", newCheckpoint1.message);

        }

        [Test]
        public void testGetTrackings(){


            //get the first 100 Trackings
            List<Tracking> listTrackings100 = connection.getTrackings(1);
           // Assert.AreEqual(10, listTrackings100.Count);
            //at least we have 10 elements
            Assert.IsNotNullOrEmpty(listTrackings100[0].ToString());
            Assert.IsNotNullOrEmpty(listTrackings100[10].ToString());

     
        }

        [Test]
        public void testPutTracking(){
            Tracking tracking = new Tracking("868271682145");
            tracking.slug = "sto";
            tracking.title = "another title";

            Tracking tracking2 = connection.putTracking(tracking);
            Assert.AreEqual("another title", tracking2.title);

            //test post tracking number doesn't exist
            Tracking tracking3 = new Tracking(trackingNumberToDetectError);
            tracking3.title ="another title";

            try{
                connection.putTracking(tracking3);
                //always should give an exception before this
                Assert.AreEqual("This never should be executed",false);
            }catch (Exception e){
                Assert.AreEqual("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//asdq\"}}", e.Message);
            }
        }

        [Test]
        public void testGetAllCouriers(){

            List<Courier> couriers = connection.getAllCouriers();

            //check first courier
            Assert.AreEqual( firstCourier["slug"], couriers[0].slug);
            Assert.AreEqual( firstCourier["name"], couriers[0].name);
            Assert.AreEqual( firstCourier["phone"], couriers[0].phone);
            Assert.AreEqual( firstCourier["other_name"], couriers[0].other_name);
            Assert.AreEqual(firstCourier["web_url"], couriers[0].web_url);
            //total Couriers returned
            Assert.AreEqual( TOTAL_COURIERS_API, couriers.Count);
            //try to acces with a bad API Key
            ConnectionAPI connectionBadKey = new ConnectionAPI("badKey");

            try{
                connectionBadKey.getCouriers();
            }catch (Exception e){
                Assert.AreEqual("{\"meta\":{\"code\":401,\"message\":\"Invalid API key.\",\"type\":\"Unauthorized\"},\"data\":{}}", e.Message);
            }
        }

        [Test]
        public void testGetCouriers(){

            List<Courier> couriers = connection.getCouriers();
            //total Couriers returned
            Assert.AreEqual(11, couriers.Count);
            //check first courier
            Assert.AreEqual(firstCourierAccount["slug"], couriers[0].slug);
            Assert.AreEqual(firstCourierAccount["name"], couriers[0].name);
            Assert.AreEqual(firstCourierAccount["phone"], couriers[0].phone);
            Assert.AreEqual(firstCourierAccount["other_name"], couriers[0].other_name);
            Assert.AreEqual(firstCourierAccount["web_url"], couriers[0].web_url);

            //try to acces with a bad API Key
            ConnectionAPI connectionBadKey = new ConnectionAPI("badKey");

            try{
                connectionBadKey.getCouriers();
            }catch (Exception e){
                Assert.AreEqual("{\"meta\":{\"code\":401,\"message\":\"Invalid API key.\",\"type\":\"Unauthorized\"},\"data\":{}}", e.Message);
            }

        }

        [Test]
        public void testGetTrackings_A(){

            ParametersTracking parameters = new ParametersTracking();
            parameters.addSlug("dhl");
            DateTime date = DateTime.Today.AddMonths(-1);


            parameters.createdAtMin = date;
            parameters.limit = 50;

            List<Tracking> totalDHL = connection.getTrackings(parameters);
            Assert.AreEqual(2, totalDHL.Count);
        }

        [Test]
        public void testGetTrackings_B(){

            ParametersTracking param1 = new ParametersTracking();
            param1.addDestination(ISO3Country.DEU);
            param1.limit = 20;
            List<Tracking> totalSpain =connection.getTrackings(param1);
            Assert.AreEqual(1, totalSpain.Count);
        }

        [Test]
        public void testGetTrackings_C(){
            ParametersTracking param2 = new ParametersTracking();
            param2.addTag(StatusTag.Delivered);
            param2.limit = 50;

            List<Tracking> totalOutDelivery=connection.getTrackings(param2);
            Assert.AreEqual( 4, totalOutDelivery.Count);

        }

        [Test]
        public void testGetTrackings_D(){
            ParametersTracking param3 = new ParametersTracking();
            param3.limit = 50;
            List<Tracking> totalOutDelivery1=connection.getTrackings(param3);
            Assert.AreEqual( 18, totalOutDelivery1.Count);
        }

        [Test]
        public void testGetTrackings_E(){

            ParametersTracking param4 = new ParametersTracking();
            param4.keyword = "title";
            param4.addField(FieldTracking.title);
            param4.limit = 50;

            List<Tracking> totalOutDelivery2=connection.getTrackings(param4);
          //  Assert.AreEqual( 2, totalOutDelivery2.Count);
            Assert.AreEqual( "this title", totalOutDelivery2[0].title);
        }

        [Test]
        public void testGetTrackings_F(){

            ParametersTracking param5 = new ParametersTracking();
            param5.addField(FieldTracking.tracking_number);
            //param5.setLimit(50);

            List<Tracking> totalOutDelivery3=connection.getTrackings(param5);
            Assert.AreEqual( null, totalOutDelivery3[0].title);
        }
        [Test]
        public void testGetTrackings_G(){

            ParametersTracking param6 = new ParametersTracking();
            param6.addField(FieldTracking.tracking_number);
            param6.addField(FieldTracking.title);
            param6.addField(FieldTracking.checkpoints);
            param6.addField(FieldTracking.order_id);
            param6.addField(FieldTracking.tag);
            param6.addField(FieldTracking.order_id);
            //param6.setLimit(50);

            List<Tracking> totalOutDelivery4=connection.getTrackings(param6);
            Assert.AreEqual( null, totalOutDelivery4[0].slug);
        }
        [Test]
        public void testGetTrackings_H(){
        
            ParametersTracking param7 = new ParametersTracking();
            param7.addOrigin(ISO3Country.ESP);
           // param7.setLimit(50);

            List<Tracking> totalOutDelivery5=connection.getTrackings(param7);
            Assert.AreEqual(2, totalOutDelivery5.Count);
        }
  
        [Test]
        public void testRetrack(){

            Tracking tracking = new Tracking("RT224265042HK");
            tracking.slug = "hong-kong-post";
            try{
                connection.retrack(tracking);
                Assert.IsTrue(false);

            }catch (Exception e){
                Assert.IsTrue(e.Message.Contains("4016"));
                Assert.IsTrue(e.Message.Contains("Retrack is not allowed. You can only retrack each shipment once."));

            }
        }
	}
}

