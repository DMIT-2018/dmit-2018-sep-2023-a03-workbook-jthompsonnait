<Query Kind="Program">
  <Connection>
    <ID>c2e59a04-ac57-4d03-9ee7-b7b41891dc7e</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>OLTP-DMIT2018</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

//	the following is equivalent to using in C#
//	the load method allows for us to reference the view models
#load "..\ViewModels\*.cs"
#load "..\Queries\CustomerService.linq"

void Main()
{
	//	Business Rules (all business rules are listed here)
	//		rule: both last name and phone number cannot be empty
	//		rule: if both values are provided
	//				both values will be used in returning
	//				a list of customer search views
	//		rule:	RemoveFromViewFlag must be false

	#region Validate Customer Search
	//	Test both last name phone number cannot be empty
	Console.WriteLine("-------  Test_GetCustomers_Missing_Paramters  -------");
	Test_GetCustomers_Missing_Paramters();
	Console.WriteLine();

	//	Test that we bring back a valid customer list based on the last name
	Console.WriteLine("-------  Test_GetCustomers_LastName  -------");
	Test_GetCustomers_LastName();
	Console.WriteLine();

	//	Test that we bring back a valid customer list based on the phone number
	Console.WriteLine("-------  Test_GetCustomers_PhoneNumber  -------");
	Test_GetCustomers_PhoneNumber();
	Console.WriteLine();

	//	Test if both values are provided, both values will be used in 
	//		returning a list of customer search views.
	Console.WriteLine("-------  Test_GetCustomers_LastNameAndPhoneNumber  -------");
	Test_GetCustomers_LastNameAndPhoneNumber();
	Console.WriteLine();

	//  Test RemoveFromViewFlag must be false when retreiving a list of customers.
	Console.WriteLine("-------  Test_GetCustomers_RemoveFromViewFlag_False  -------");
	Test_GetCustomers_RemoveFromViewFlag_False();
	Console.WriteLine();
	#endregion
}

#region Tests
//  https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022
#region Customer Search
/// <summary>
/// Tests the GetCustomers method to ensure it handles cases with both missing parameters.
/// </summary>
public void Test_GetCustomers_Missing_Paramters()
{
	// Arrange: Set up necessary preconditions and inputs.

	// Act: Execute the functionality being tested.

	// Assert: Verify that the output matches expected results.
}

/// <summary>
/// Tests the GetCustomers method for retrieving customers based solely on the last name.
/// </summary>
public void Test_GetCustomers_LastName()
{
	// Arrange: Set up necessary preconditions and inputs.

	// Act: Execute the functionality being tested.

	// Assert: Verify that the output matches expected results.
}

/// <summary>
/// Tests the GetCustomers method for retrieving customers based solely on the phone number.
/// </summary>
public void Test_GetCustomers_PhoneNumber()
{

	// Arrange: Set up necessary preconditions and inputs.

	// Act: Execute the functionality being tested.

	// Assert: Verify that the output matches expected results.

}

/// <summary>
/// Tests the GetCustomers method for retrieving customers based on both last name and phone number.
/// </summary>
public void Test_GetCustomers_LastNameAndPhoneNumber()
{

	// Arrange: Set up necessary preconditions and inputs.

	// Act: Execute the functionality being tested.

	// Assert: Verify that the output matches expected results.

}

/// <summary>
/// Tests the GetCustomers method to ensure that customers with a RemoveFromViewFlag set to false are handled correctly.
/// </summary>
public void Test_GetCustomers_RemoveFromViewFlag_False()
{
	// Arrange: Set up necessary preconditions and inputs.

	// Act: Execute the functionality being tested.

	// Assert: Verify that the output matches expected results.
}
#endregion
#endregion

#region Helper
/// <summary>
/// Retrieves the innermost exception from a given exception.
/// </summary>
/// <param name="ex">The exception from which to extract the innermost exception.</param>
/// <returns>The innermost exception, 
/// 	or the original exception if it has no inner exceptions.
/// </returns>
private Exception GetInnerException(Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}
#endregion

