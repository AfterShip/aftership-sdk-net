using Microsoft.VisualStudio.TestTools.UnitTesting;
using AftershipAPI;
using AftershipAPI.Enums;
using System;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class TestConnectionAPI
    {

        ConnectionAPI connection; 
        //static int TOTAL_COURIERS_API = 225;

        String[] couriersDetected = { "dpd", "fedex" };

        //post tracking number
        String trackingNumberPost = "05167019264110";
        String slugPost = "dpd";
        String orderIDPathPost = "www.whatever.com";
        String orderIDPost = "ID 1234";
        String customerNamePost = "Mr Smith";
        String titlePost = "this title";
        ISO3Country countryDestinationPost = ISO3Country.USA;
        String email1Post = "email@yourdomain.com";
        String email2Post = "another_email@yourdomain.com";
        String sms1Post = "+85292345678";
        String sms2Post = "+85292345679";
        String customProductNamePost = "iPhone Case";
        String customProductPricePost = "USD19.99";

        //tracking numbers to detect
        String trackingNumberToDetect = "92748902410411000226601999";
        String trackingNumberToDetectError = "asdq";

        //Tracking to Delete
        String trackingNumberDelete = "596454081704";
        String slugDelete = "fedex";


        //tracking to DeleteBad
        String trackingNumberDelete2 = "798865638020";

        //tracking to mark
        String trackingNumberMark = "798865638021";
        String slugMark = "fedex";

        static bool firstTime = true;

        Dictionary<string, string> firstCourier = new Dictionary<string, string>();
        Dictionary<string, string> firstCourierAccount = new Dictionary<string, string>();

        [TestInitialize()]
        public void setUp()
        {
            String key = System.IO.File.ReadAllText(@"\\psf\Home\Documents\aftership-key.txt");
            connection = new ConnectionAPI(key);

            if (firstTime)
            {

                Console.WriteLine("****************SET-UP BEGIN**************");
                firstTime = false;
                //delete the tracking we are going to post (in case it exist)
                Tracking tracking = new Tracking("05167019264110");
                tracking.slug = "dpd";

                //first courier
                firstCourier.Add("slug", "india-post-int");
                firstCourier.Add("name", "India Post International");
                firstCourier.Add("phone", "+91 1800 11 2011");
                firstCourier.Add("other_name", "भारतीय डाक, Speed Post & eMO, EMS, IPS Web");
                firstCourier.Add("web_url", "http://www.indiapost.gov.in/");

                //first courier in your account
                firstCourierAccount.Add("slug", "usps");
                firstCourierAccount.Add("name", "USPS");
                firstCourierAccount.Add("phone", "+1 800-275-8777");
                firstCourierAccount.Add("other_name", "United States Postal Service");
                firstCourierAccount.Add("web_url", "https://www.usps.com");

                try { connection.deleteTracking(tracking); }
                catch (Exception e)
                {
                    Console.WriteLine("**1" + e.Message);
                }
                Tracking tracking1 = new Tracking(trackingNumberToDetect);
                tracking1.slug = "dpd";
                try { connection.deleteTracking(tracking1); }
                catch (Exception e)
                {
                    Console.WriteLine("**2" + e.Message);
                }
                try
                {
                    Tracking newTracking = new Tracking(trackingNumberDelete);
                    newTracking.slug = slugDelete;
                    connection.createTracking(newTracking);
                }
                catch (Exception e)
                {
                    Console.WriteLine("**3" + e.Message);
                }
                try
                {
                    Tracking newTracking1 = new Tracking("9400110897700003231250");
                    newTracking1.slug = "usps";
                    connection.createTracking(newTracking1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("**4" + e.Message);

                }
                try
                {

                    Tracking newTracking2 = new Tracking(trackingNumberMark);
                    newTracking2.slug = slugMark;
                    try {
                        connection.deleteTracking(newTracking2);
                    }catch(Exception e)
                    {
                        Console.WriteLine("**5" + e.Message);
                    }
                    connection.createTracking(newTracking2);
                }
                catch (Exception e)
                {
                    Console.WriteLine("**6" + e.Message);

                }
                Console.WriteLine("****************SET-UP FINISH**************");


            }

        }

        [TestMethod]
        public void testDetectCouriers()
        {

            //get trackings of this number.
            List<Courier> couriers = connection.detectCouriers(trackingNumberToDetect);
            Assert.AreEqual(3, couriers.Count);
            //the couriers should be dpd or fedex
            Console.WriteLine("**0" + couriers[0].slug);
            Console.WriteLine("**1" + couriers[1].slug);

            Assert.IsTrue(Equals(couriers[0].slug, couriersDetected[0])
                || Equals(couriers[1].slug, couriersDetected[0]));
            Assert.IsTrue(Equals(couriers[0].slug, couriersDetected[1])
                || Equals(couriersDetected[1], couriers[1].slug));

            //if the trackingNumber doesn't match any courier defined, should give an error.

            try
            {
                List<Courier> couriers1 = connection.detectCouriers(trackingNumberToDetectError);
                Assert.AreEqual(0, couriers1.Count);
            }
            catch (Exception e)
            {
                Assert.AreEqual("{\"meta\":{\"code\":4005,\"message\":\"The value of `tracking_number` is invalid.\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\"}}}", e.Message);
            }

            List<String> slugs = new List<String>();
            slugs.Add("dtdc");
            slugs.Add("ukrposhta");
            slugs.Add("usps");
            //   slugs.add("asdfasdfasdfasd");
            slugs.Add("dpd");
            List<Courier> couriers2 = connection.detectCouriers(trackingNumberToDetect, "28046", "", null, slugs);
            Assert.AreEqual(1, couriers2.Count);
        }
        [TestMethod]
        public void TestCreateTracking()
        {
            Tracking tracking1 = new Tracking(trackingNumberPost);
            tracking1.slug = slugPost;
            tracking1.orderIDPath = orderIDPathPost;
            tracking1.customerName = customerNamePost;
            tracking1.orderID = orderIDPost;
            tracking1.title = titlePost;
            tracking1.destinationCountryISO3 = countryDestinationPost;
            tracking1.addEmails(email1Post);
            tracking1.addEmails(email2Post);
            tracking1.addCustomFields("product_name", customProductNamePost);
            tracking1.addCustomFields("product_price", customProductPricePost);
            tracking1.addSmses(sms1Post);
            tracking1.addSmses(sms2Post);
            Tracking trackingPosted = connection.createTracking(tracking1);

            Assert.AreEqual(trackingNumberPost, trackingPosted.trackingNumber, "#A01");
            Assert.AreEqual(slugPost, trackingPosted.slug, "#A02");
            Assert.AreEqual(orderIDPathPost, trackingPosted.orderIDPath, "#A03");
            Assert.AreEqual(orderIDPost, trackingPosted.orderID, "#A04");
            Assert.AreEqual(countryDestinationPost,
                trackingPosted.destinationCountryISO3, "#A05");

            Assert.IsTrue(trackingPosted.emails.Contains(email1Post), "#A06");
            Assert.IsTrue(trackingPosted.emails.Contains(email2Post), "#A07");
            Assert.AreEqual(2, trackingPosted.emails.Count, "#A08");

            Assert.IsTrue(trackingPosted.smses.Contains(sms1Post), "#A09");
            Assert.IsTrue(trackingPosted.smses.Contains(sms2Post), "#A10");
            Assert.AreEqual(2, trackingPosted.smses.Count, "#A11");

            Assert.AreEqual(customProductNamePost,
                trackingPosted.customFields["product_name"], "#A12");
            Assert.AreEqual(customProductPricePost,
                trackingPosted.customFields["product_price"], "#A13");
        }

        [TestMethod]
        public void TestCreateTrackingEmptySlug()
        {
            //test post only informing trackingNumber (the slug can be dpd and fedex)
            Tracking tracking2 = new Tracking(trackingNumberToDetect);
            Tracking trackingPosted2 = connection.createTracking(tracking2);
            Assert.AreEqual(trackingNumberToDetect, trackingPosted2.trackingNumber, "#A14");
            Assert.AreEqual("usps", trackingPosted2.slug, "#A15");//the system assign dpd (it exist)
            try
            {
                connection.deleteTracking(trackingPosted2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        [TestMethod]
        public void TestCreateTrackingWithAllFields()
        {
            String trackingNumber = "9374889680005800658136";
            //test post with all fields
            Tracking tracking = new Tracking(trackingNumber);
            tracking.slug = "usps";
            tracking.title = "sample title";
            tracking.orderID = "4029727637682";
            tracking.orderIDPath = "https://hxzuo8823.aftership.com/9374889681005855144409";
            tracking.addCustomFields("item_names", "短袖T恤");
            tracking.addEmails("test@aftership.com");
            tracking.addSubscribedEmails("test@aftership.com");
            tracking.addEmails("test@aftership.com");
            tracking.language = "zh";
            tracking.orderPromisedDeliveryDate = "2021-09-30";
            tracking.deliveryType = "pickup_at_store";
            tracking.pickupLocation = "shenzhen";
            tracking.pickupNote = "carefully";
            tracking.customerName = "watson";
            tracking.originCountryISO3 = ISO3Country.CHN;
            tracking.destinationCountryISO3 = ISO3Country.CHN;
            tracking.note = "sample note";
            tracking.orderDate = "2021-09-26T11:23:51Z";

            Tracking trackingPosted = connection.createTracking(tracking);
            Assert.AreEqual(trackingNumber, trackingPosted.trackingNumber, "#A14");
            Assert.AreEqual("usps", trackingPosted.slug, "#A15");
            Assert.AreEqual("sample title", trackingPosted.title, "#A16");
            Assert.AreEqual("4029727637682", trackingPosted.orderID, "#A17");
            Assert.AreEqual("https://hxzuo8823.aftership.com/9374889681005855144409", trackingPosted.orderIDPath, "#A18");
            Assert.IsNotNull(trackingPosted.customFields);
            Assert.AreEqual("zh", trackingPosted.language, "#A19");
            Assert.AreEqual("2021-09-30", trackingPosted.orderPromisedDeliveryDate, "#A20");
            Assert.AreEqual("pickup_at_store", trackingPosted.deliveryType, "#A21");
            Assert.AreEqual("shenzhen", trackingPosted.pickupLocation, "#A22");
            Assert.AreEqual("carefully", trackingPosted.pickupNote, "#A23");
            Assert.AreEqual("watson", trackingPosted.customerName, "#A24");
            Assert.AreEqual(ISO3Country.CHN, trackingPosted.originCountryISO3, "#A25");
            Assert.AreEqual(ISO3Country.CHN, trackingPosted.destinationCountryISO3, "#A26");
            Assert.AreEqual("sample note", trackingPosted.note, "#A27");
            Assert.AreEqual("09/26/2021 11:23:51", trackingPosted.orderDate, "#A28");
            try
            {
                connection.deleteTracking(trackingPosted);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //test post tracking number doesn't exist
        [TestMethod]
        public void TestCreateTrackingError()
        {
            Tracking tracking3 = new Tracking(trackingNumberToDetectError);

            try
            {
                connection.createTracking(tracking3);
                //always should give an exception before this
                Assert.IsTrue(false, "#A16");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.AreEqual("{\"meta\":{\"code\":4012,\"message\":\"Cannot detect courier. Activate courier at https://secure.aftership.com/settings/courier\",\"type\":\"BadRequest\"},\"data\":{\"tracking\":{\"tracking_number\":\"asdq\",\"title\":\"asdq\"}}}",
                    e.Message, "#A17");
            }
        }


        [TestMethod]
        public void testDeleteTracking()
        {

            //delete a tracking number (posted in the setup)
            Tracking deleteTracking = new Tracking(trackingNumberDelete);
            deleteTracking.slug = slugDelete;
            Assert.IsTrue(connection.deleteTracking(deleteTracking), "#A18");

        }

        [TestMethod]
        public void testDeleteTracking1()
        {
            //if the slug is bad informed
            try
            {
                Tracking deleteTracking2 = new Tracking(trackingNumberDelete2);
                Assert.IsTrue(connection.deleteTracking(deleteTracking2), "#A19");
                //always should give an exception before this
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//798865638020\"}}",
                    e.Message, "#A20");
            }

        }

        [TestMethod]
        public void testDeleteTracking2()
        {

            //if the trackingNumber is bad informed
            try
            {
                Tracking deleteTracking3 = new Tracking("adfa");
                deleteTracking3.slug = "fedex";
                Assert.IsTrue(connection.deleteTracking(deleteTracking3), "#A20");
                //always should give an exception before this
                Assert.IsTrue(false, "#A21");
            }
            catch (Exception e)
            {
                Assert.AreEqual("{\"meta\":{\"code\":4004,\"message\":\"Tracking does not exist.\",\"type\":\"NotFound\"},\"data\":{}}",
                    e.Message, "#A22");
            }
        }

        [TestMethod]
        public void testGetTrackingByNumber()
        {
            String trackingNumber = "9300120111406558526311";
            String slug = "usps";

            Tracking trackingGet1 = new Tracking(trackingNumber);
            trackingGet1.slug = slug;
            Tracking tracking = connection.getTrackingByNumber(trackingGet1);
            Assert.AreEqual(trackingNumber, tracking.trackingNumber, "#A23");
            Assert.AreEqual(slug, tracking.slug, "#A24");

            List<Checkpoint> checkpoints = tracking.checkpoints;
            Checkpoint lastCheckpoint = checkpoints[checkpoints.Count - 1];
            Assert.IsTrue(checkpoints != null, "A25-1");
            Assert.IsTrue(checkpoints.Count > 1, "A25-2");

            Assert.IsTrue(!string.IsNullOrEmpty(lastCheckpoint.message));
            Assert.IsTrue(!string.IsNullOrEmpty(lastCheckpoint.countryName));

        }

        [TestMethod]
        public void testGetTrackingByNumber2()
        {

            //slug is bad informed
            try
            {
                Tracking trackingGet2 = new Tracking("RC328021065CN");

                connection.getTrackingByNumber(trackingGet2);
                //always should give an exception before this
                Assert.IsTrue(false, "#A26");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.AreEqual("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//RC328021065CN\"}}",
                    e.Message, "#A27");
            }
        }

        [TestMethod]
        public void testGetTrackingByNumber3()
        {

            //if the trackingNumber is bad informed
            try
            {
                Tracking trackingGet3 = new Tracking("adf");
                trackingGet3.slug = "fedex";
                connection.getTrackingByNumber(trackingGet3);
                //always should give an exception before this
                Assert.IsTrue(false, "#A28");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Assert.AreEqual("{\"meta\":{\"code\":4004,\"message\":\"Tracking does not exist.\",\"type\":\"NotFound\"},\"data\":{}}",
                    e.Message, "#A29");
            }
        }

                [TestMethod]
        public void testGetLastCheckpointID()
        {
            Tracking trackingGet1 = new Tracking("whatever");
            trackingGet1.id = "l2go9phz0c0ajktjsgt0z02f";
            Checkpoint newCheckpoint = connection.getLastCheckpoint(trackingGet1);
            Assert.IsTrue(!string.IsNullOrEmpty(newCheckpoint.message));
            Assert.AreEqual("USA", newCheckpoint.countryName);
            Assert.AreEqual("Delivered", newCheckpoint.tag);
            Assert.AreEqual("Delivered_001", newCheckpoint.subTag);
        }
        
        [TestMethod]
        public void testGetTrackings()
        {


            //get the first 100 Trackings
            List<Tracking> listTrackings100 = connection.getTrackings(1);
            // Assert.AreEqual(10, listTrackings100.Count);
            //at least we have 10 elements
            Assert.IsNotNull(listTrackings100[0].ToString());
            Assert.IsNotNull(listTrackings100[10].ToString());
        }

        [TestMethod]
        public void testPutTracking()
        {
            Tracking tracking = new Tracking("9400110897700003231250");
            tracking.slug = "usps";
            tracking.title = "another title";

            Tracking tracking2 = connection.putTracking(tracking);
            Assert.AreEqual("another title", tracking2.title);

            //test post tracking number doesn't exist
            Tracking tracking3 = new Tracking(trackingNumberToDetectError);
            tracking3.title = "another title";

            try
            {
                connection.putTracking(tracking3);
                //always should give an exception before this
                Assert.AreEqual("This never should be executed", false);
            }
            catch (Exception e)
            {
                Assert.AreEqual("{\"meta\":{\"code\":404,\"message\":\"The URI requested is invalid or the resource requested does not exist.\",\"type\":\"NotFound\"},\"data\":{\"resource\":\"/v4/trackings//asdq\"}}", e.Message);
            }
        }

        [TestMethod]
        public void testGetAllCouriers()
        {

            List<Courier> couriers = connection.getAllCouriers();

            //check first courier
            Assert.IsTrue(!string.IsNullOrEmpty(couriers[0].slug));
            Assert.IsTrue(!string.IsNullOrEmpty(couriers[0].name));
            Assert.IsTrue(!string.IsNullOrEmpty(couriers[0].web_url));

            //total Couriers returned
            Assert.IsTrue(couriers.Count > 200);
            //try to acces with a bad API Key
            ConnectionAPI connectionBadKey = new ConnectionAPI("badKey");

            try
            {
                connectionBadKey.getCouriers();
            }
            catch (Exception e)
            {
                Assert.AreEqual("{\"meta\":{\"code\":401,\"message\":\"Invalid API key.\",\"type\":\"Unauthorized\"},\"data\":{}}", e.Message);
            }
        }

        [TestMethod]
        public void testGetCouriers()
        {

            List<Courier> couriers = connection.getCouriers();
            //total Couriers returned
            Assert.IsTrue(couriers.Count > 10);
            //check first courier

            Assert.IsTrue(!string.IsNullOrEmpty(couriers[0].slug));
            Assert.IsTrue(!string.IsNullOrEmpty(couriers[0].name));
            Assert.IsTrue(!string.IsNullOrEmpty(couriers[0].web_url));

            //try to acces with a bad API Key
            ConnectionAPI connectionBadKey = new ConnectionAPI("badKey");

            try
            {
                connectionBadKey.getCouriers();
            }
            catch (Exception e)
            {
                Assert.AreEqual("{\"meta\":{\"code\":401,\"message\":\"Invalid API key.\",\"type\":\"Unauthorized\"},\"data\":{}}", e.Message);
            }

        }

        [TestMethod]
        public void testGetTrackings_A()
        {

            ParametersTracking parameters = new ParametersTracking();
            parameters.addSlug("usps");
            DateTime date = DateTime.Today.AddMonths(-1);


            parameters.createdAtMin = date;
            parameters.limit = 50;

            List<Tracking> totalDHL = connection.getTrackings(parameters);
            Assert.IsTrue(totalDHL.Count >= 1);
        }

        [TestMethod]
        public void testGetTrackings_B()
        {

            ParametersTracking param1 = new ParametersTracking();
            param1.addDestination(ISO3Country.DEU);
            param1.limit = 20;
            List<Tracking> totalSpain = connection.getTrackings(param1);
            Assert.IsTrue(totalSpain.Count >=1);
        }

        [TestMethod]
        public void testGetTrackings_C()
        {
            ParametersTracking param2 = new ParametersTracking();
            param2.addTag(StatusTag.Delivered);
            param2.limit = 50;

            List<Tracking> totalOutDelivery = connection.getTrackings(param2);
            Assert.IsTrue(totalOutDelivery.Count > 10);
            Assert.IsTrue(totalOutDelivery.Count <= 50);

        }

        [TestMethod]
        public void testGetTrackings_D()
        {
            ParametersTracking param3 = new ParametersTracking();
            param3.limit = 50;
            List<Tracking> totalOutDelivery1 = connection.getTrackings(param3);
            Assert.IsTrue(totalOutDelivery1.Count > 10);
            Assert.IsTrue(totalOutDelivery1.Count <= 50);
        }

        [TestMethod]
        public void testGetTrackings_E()
        {

            ParametersTracking param4 = new ParametersTracking();
            param4.keyword = "09445246482536";
            param4.addField(FieldTracking.title);
            param4.limit = 2;

            List<Tracking> totalOutDelivery2 = connection.getTrackings(param4);
            //  Assert.AreEqual( 2, totalOutDelivery2.Count);
            Assert.AreEqual("09445246482536", totalOutDelivery2[0].title);
        }

        [TestMethod]
        public void testGetTrackings_F()
        {

            ParametersTracking param5 = new ParametersTracking();
            param5.addField(FieldTracking.tracking_number);
            //param5.setLimit(50);

            List<Tracking> totalOutDelivery3 = connection.getTrackings(param5);
            Assert.AreEqual(null, totalOutDelivery3[0].title);
        }
        [TestMethod]
        public void testGetTrackings_G()
        {

            ParametersTracking param6 = new ParametersTracking();
            param6.addField(FieldTracking.tracking_number);
            param6.addField(FieldTracking.title);
            param6.addField(FieldTracking.checkpoints);
            param6.addField(FieldTracking.order_id);
            param6.addField(FieldTracking.tag);
            param6.addField(FieldTracking.order_id);
            //param6.setLimit(50);

            List<Tracking> totalOutDelivery4 = connection.getTrackings(param6);
            Assert.AreEqual(null, totalOutDelivery4[0].slug);
        }
        [TestMethod]
        public void testGetTrackings_H()
        {

            ParametersTracking param7 = new ParametersTracking();
            param7.addOrigin(ISO3Country.ESP);
            // param7.setLimit(50);

            List<Tracking> totalOutDelivery5 = connection.getTrackings(param7);
            Assert.AreEqual(1, totalOutDelivery5.Count);
        }

        [TestMethod]
        public void testRetrack()
        {

            Tracking tracking = new Tracking("9400110897700003231250");
            tracking.slug = "usps";
            try
            {
                connection.retrack(tracking);
                Assert.IsTrue(false);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Assert.IsTrue(e.Message.Contains("4013"));
                Assert.IsTrue(e.Message.Contains("Retrack is not allowed. You can only retrack an inactive tracking."));

            }
        }

        [TestMethod]
        public void testMarkTrackingAsCompeleted()
        {
            Tracking tracking = new Tracking(trackingNumberMark);
            tracking.slug = slugMark;
            Tracking trackingMarked = connection.markTrackingAsCompeleted(tracking, "DELIVERED");
            Assert.AreEqual(trackingMarked.tag, StatusTag.Delivered);
            trackingMarked = connection.markTrackingAsCompeleted(tracking, "LOST");
            Assert.AreEqual(trackingMarked.tag, StatusTag.Exception);
        }
    }
}
