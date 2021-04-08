API Aftership .NET SDK
==============

Aftership SDK .NET Version 5.0.4

Before you start
--------------

There are two ways of use this SDK, download the package from Nuget, or download
the source files from github.

  - If you are using the source files you need to import Newtonsoft.Json,
  download the library compatible with your OS and .Net version. (Nuget will
  install the library automatically).
  - Don't forget to import the reference to System.Web, the project reference
  framework can't be Client Profile (otherwise you won't have access to this
  library).
  - If you want to run the test (using the source), you also need NUnit.Framework


Tips
--------------

  Is a good practice to control the exceptions throws by ConnectionAPI, they
  will give you information of what was wrong.


Examples of code
--------------

	using System;
	using Newtonsoft.Json;
	using Aftership.Enums;
	using System.Collections.Generic;
	//Create an instance of ConnectionAPI using the token of the user
	ConnectionAPI connection = new ConnectionAPI("a61d6204-6477-???-93ec-????????");

 	//create a new tracking to add to our account
 	Tracking newTracking = new Tracking ("7126900292");
 	newTracking.slug = "dhl";
 	newTracking.title = "this is a test";
 	newTracking.addEmails ("emailprueba@gmail.com");
 	newTracking.addEmails ("another@gmail.com");
 	newTracking.addSmses("+85295340110");
 	newTracking.addSmses("+85295349999");
 	newTracking.customerName = "Mr Smith";
 	newTracking.destinationCountryISO3 = ISO3Country.HKG;
 	newTracking.orderID = "10000";
 	newTracking.orderIDPath = "wwww.aaaaa.com";
 	newTracking.trackingAccountNumber = "1234567";
 	newTracking.trackingPostalCode = "28046";
 	newTracking.trackingShipDate = "today";
 	newTracking.addCustomFields("price","1000");
 	newTracking.addCustomFields("product","iphone 5");

 	//before adding it, lets try to delete (otherwise maybe it would fail if already exist)
 	try{
 		//is important to catch the exceptions and read the messages,
 		//it will give you information why the transactions went wrong
 		if(connection.deleteTracking(newTracking))
 			Console.Write("Tracking deleted!!");
 	}catch(Exception e){
 		Console.Write (e.Message);
 	}

 	//now lets add the tracking
 	Tracking trackingAdded = connection.createTracking(newTracking);

 	//the tracking is added, it return a Tracking with the information
 	//that the system have of the tracking (probably it won't have the
 	//Checkpoints, cause the system didn't have time to retrieve them from
 	//the couriers, so we can retrieve the Tracking
 	Tracking trackingGet = connection.getTrackingByNumber (trackingAdded);

 	//trackingGet will have all the information we want (including checkpoints)
 	// there is 2 ways of get the tracking:
 	//	* by id
 	//	* by slug, tracking number and optinally required params

 	//example id:
 	Tracking trackingGet1 = new Tracking("");//you dont care about tracking number
 	trackingGet1.id = "53be255bfdacaaae7b17834b";
 	Tracking tracking3 = connection.getTrackingByNumber(trackingGet1);

 	//example slug, tracking:
 	Tracking trackingGet2 = new Tracking("RC328021065CN");
 	trackingGet2.slug = "canada-post";
 	connection.getTrackingByNumber (trackingGet2);

 	//example slug,tracking + required fields
 	Tracking trackingGet3 = new Tracking("RT406182863DE");
 	trackingGet3.slug = "deutsch-post";
 	trackingGet3.trackingShipDate = "20140627";
 	connection.getTrackingByNumber (trackingGet3);
 	
 	//example ge trackings for our account, the 1 means get the first 100, 
 	//with a 2 we would get from 100 to 200 etc
 	List<Tracking> listTrackings = connection.getTrackings (1);
    Console.WriteLine ("Number of trackings-> "+listTrackings.Count);
    for (i = 0; i < listTrackings.Count; i++) {
    	Console.WriteLine (listTrackings [i].ToString ());
    }
    
    //example get only the trackings from Spain
    ParametersTracking param1 = new ParametersTracking();
    //in param1 we add all the options we want
	param1.addDestination(ISO3Country.ESP);  
 	List<Tracking> totalSpain =connection.getTrackings(param1);

#### 2021-03-26 v5.0.4
- Add Available For Pickup status tag.
- Checkpoint: add Slug, Location, Subtag Message.
- Tracking: Courier Tracking Link, Subtag, Subtag Message.
- Fix SignedBy field (always empty).
- Fix casting and type conversions.

#### 2021-03-26 v5.0.3
- Fix SSL exception

#### 2015-05-11 v5.0.0
- Change the way we deal with dates, so differents Date Culture don't have any issues.
- The user will receive the date to String in their local Timezone **breaking change**
- Change dependencies:
	- NewtonSoft 8.0.1 (latest stable). **breaking change**

#### 2015-05-11 v4.0.11
- Rearrange all the code (we wont bump the version cause is the same). 

#### 2015-05-11 v4.0.11
- Change dependecies:
	- Delete dependencies Microsoft added automatically not needed.
- Delete a line printing the request to the Aftership server.

#### 2015-05-11 v4.0.9
- Change dependencies:
	- NewtonSoft 6.0.8 (latest stable).
	- Change the test framework to Microsoft unitesting (instead of use Nunit).
	- Delete dependencies, now only need Newtonsoft.Json and System.

#### 2015-01-05 v4.0.8
- Dependencies:
	-  Newtonsoft.Json 3.5.8
	-  System.Web
	-  System.Net
	-  System
	
## License
Copyright (c) 2015 Aftership  
Licensed under the MIT license.
 	

For Aftership developers
--------------

Generate a new version of the project in Nuget:

- Mofify the file Aftership-API.nuspec with the new version (as 4.0.5) in the same directory as the *.csproj file (c# project file).
- Dowload Nuget.exe (in Windows).
- Run with cmd at the directory as the *.nuspec is: ```nuget pack sample.csproj```
- That generates an *.nupkg file. This is the file we have to upload to the the Nuget library.
- Before generate the *.nupkg, clean and build the solution, otherwise the changes won't be updated.
- For load a .nupkg from source execute in the Package Manager Console ```Install-Package Aftership -Source Z:\Users\jesus\git\aftership-net\Aftership-API```
