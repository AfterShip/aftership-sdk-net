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

		ConnectionAPI connection  = new ConnectionAPI("a61d6204-6477-4f6d-93ec-86c4f872fb6b");

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
		String [] couriersDetected={"dpd","fedex"};

		//Tracking to Delete
		String trackingNumberDelete = "596454081704";
		String slugDelete ="fedex";


		//tracking to DeleteBad
		String trackingNumberDelete2 = "798865638020";

		static bool firstTime = true;


		public TestConnectionAPI ()
		{
		}

		[SetUp]		
		public void setUp(){
		
			if(firstTime) {
				firstTime=false;
				//delete the tracking we are going to post (in case it exist)
				Tracking tracking = new Tracking("05167019264110");
				tracking.slug = "dpd";
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
			}

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
				Assert.AreEqual ("{\"meta\":{\"code\":4008,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\",\"title\":\"asdq\"}}}",
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
				Assert.AreEqual("{\"meta\":{\"code\":4008,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"slug\":\"fedex\",\"tracking_number\":\"adfa\"}}}",
					e.Message,"#A22");
			}
		}

		[Test]
		public void testGetTrackingByNumber(){
			String trackingNumber = "RC328021065CN";
			String slug = "canada-post";

			Tracking trackingGet1 = new Tracking(trackingNumber);
			trackingGet1.slug = slug;

			Tracking tracking = connection.getTrackingByNumber(trackingGet1);
			Assert.AreEqual(trackingNumber, tracking.trackingNumber,"#A23");
			Assert.AreEqual( slug, tracking.slug,"#A24");
			Assert.AreEqual( "Lettermail", tracking.shipmentType,"#A25");

			List <Checkpoint> checkpoints = tracking.checkpoints;
			Checkpoint lastCheckpoint = checkpoints [checkpoints.Count - 1];
			Assert.IsTrue (checkpoints!=null,"A25-1");
			Assert.AreEqual (10 , checkpoints.Count,"A25-2");

			Assert.AreEqual ("Item successfully delivered" ,lastCheckpoint.message,"A25-3");
			Assert.AreEqual ("VARENNES" ,lastCheckpoint.countryName,"A25-4");


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
				Assert.AreEqual("{\"meta\":{\"code\":4008,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"slug\":\"fedex\",\"tracking_number\":\"adf\"}}}",
					e.Message,"#A29");
			}
		}

		[Test]
		public void testGetTrackingByNumber4(){
			List<FieldTracking> fields = new List<FieldTracking>();
			fields.Add(FieldTracking.tracking_number);
			fields.Add(FieldTracking.active);
			Tracking trackingGet1 = new Tracking("RC328021065CN");
			trackingGet1.slug = "canada-post";
			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1,fields,"");
			Assert.AreEqual( "RC328021065CN", tracking3.trackingNumber,"#A30");
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

			Assert.AreEqual("53bb4db6dcebe7242fe3283e", tracking3.id,"#B1");
			Assert.AreEqual( "api",tracking3.source,"#B2");
			Assert.IsNull(  tracking3.title,"#B3");

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

			Assert.AreEqual("53be255bfdacaaae7b17834b", tracking3.id,"#D1");
			Assert.AreEqual( true,tracking3.active,"#D2");

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
			trackingGet1.id = "53be255bfdacaaae7b17834b";

			Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

			Assert.AreEqual("RT406182863DE", tracking3.trackingNumber,"#F1");
			Assert.AreEqual( "20140627",tracking3.trackingShipDate,"#F2");

		}

	}
}

