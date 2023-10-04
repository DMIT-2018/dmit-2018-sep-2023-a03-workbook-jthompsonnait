<Query Kind="Program">
  <Connection>
    <ID>544742c2-956f-4ff6-965a-61ab55d5434e</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>ChinookSept2018</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	try
	{
		//	passing in an empty first and last name
		//  AggregateExceptionTest("", "");
		
		//	passing in a track ID is larger than the track ID in the table
		// ArgumentNullExceptionTest(10000);
		
		//	passing an invalid track ID (less than 1)
		ExceptionTest(0);
	}
	#region catch all exception
	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	#endregion
}

private Exception GetInnerException(Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}


public void AggregateExceptionTest(string firstName, string lastName)
{
	#region Business Logic and Parameter Exceptions
	// 	create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();
	//	Businees Rules
	//		rule:	first name cannot be empty or null
	//		rule:	last name cannot be empty or null

	//	parameter validation
	if (string.IsNullOrWhiteSpace(firstName))
	{
		errorList.Add(new Exception("First name is required"));
	}

	if (string.IsNullOrWhiteSpace(lastName))
	{
		errorList.Add(new Exception("Last name is required"));
	}
	#endregion
	
	/*
		actual code for the method
	*/
	
	
	if(errorList.Count() > 0)
	{
		//	throw the list of business processing error(s)
		throw new AggregateException("Unable to proceed! Check concerns", errorList);
	}
	
	//  code to run if there are no errors
	//	ie:  Save.
}

private void ArgumentNullExceptionTest(int trackID)
{
	#region Business Logic and Parameter Exceptions
	// 	create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();
	//	Businees Rules
	//		rule:	Track must exist in the database

	//	parameter validation
	var track = Tracks
					.Where(x => x.TrackId == trackID)
					.Select(x => x).FirstOrDefault();
					
	if (track == null)
	{
		throw new ArgumentNullException($"No track found for Track ID{trackID}");
	}
	#endregion

	/*
		actual code for the method
	*/


	if (errorList.Count() > 0)
	{
		//	throw the list of business processing error(s)
		throw new AggregateException("Unable to proceed! Check concerns", errorList);
	}

	//  code to run if there are no errors
	//	ie:  Save.
}

private void ExceptionTest(int trackID)
{
	#region Business Logic and Parameter Exceptions
	// 	create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();
	//	Businees Rules
	//		rule:	Track ID must be valid

	//	parameter validation
	//	This is a show stopper and no reason to go beyond this
	//		point of code.
	if (trackID < 1)
	{
		throw new Exception("Track ID is invalid");
	}
	#endregion

	/*
		actual code for the method
	*/


	if (errorList.Count() > 0)
	{
		//	throw the list of business processing error(s)
		throw new AggregateException("Unable to proceed! Check concerns", errorList);
	}

	//  code to run if there are no errors
	//	ie:  Save.
}





















