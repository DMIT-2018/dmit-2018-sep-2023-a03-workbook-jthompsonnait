<Query Kind="Program">
  <Connection>
    <ID>4b5da6fb-5c95-4544-814e-55861abf7485</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>.</Server>
    <Database>OLTP-DMIT2018</Database>
    <DisplayName>OLTP-DMIT2018-Entity</DisplayName>
    <DriverData>
      <EncryptSqlTraffic>False</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
      <TrustServerCertificate>False</TrustServerCertificate>
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

	#region Add/Edit Customer 
	//	Test Ensure the Add/Edit Customer functionality handles missing data appropriately
	Console.WriteLine("-------  Test_AddEditCustomer_Missing_Data  -------");
	Test_AddEditCustomer_Missing_Data();
	Console.WriteLine();

	//	Test Validate the functionality of adding a new customer to the system.
	Console.WriteLine("-------  Test_AddCustomer  -------");
	Test_AddCustomer();
	Console.WriteLine();

	// Test Validate the functionality of editing an existing customer's details.
	Console.WriteLine("-------  Test_EditCustomer  -------");
	Test_EditCustomer();
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
	string lastName = string.Empty;
	string phone = string.Empty;

	// Act: Execute the functionality being tested.
	//	We are assuming that our method needs a last name or phone number
	//		to return customer records.
	string actual = string.Empty;
	try
	{
		GetCustomers(lastName, phone);
	}
	//	catch the null exception from our method.
	catch (ArgumentNullException ex)
	{
		//	refactor to use the parameter name
		actual = ex.ParamName;
	}


	// Assert: Verify that the output matches expected results.
	string expected = "XXXXXPlease provide either a last name and/or phone number";
	string isValid = actual == expected ? "Pass" : "Fail";

	//	show the result to the user.
	//	console message formmated for readability
	Console.WriteLine($"-- {isValid} -- Test_GetCustomers_Missing_Parameters");
	Console.WriteLine($"Expected: {expected}");
	Console.WriteLine($"Actual: {actual}");
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

#region Customer Add/Edit
//	Test Ensure the Add/Edit Customer functionality handles missing data appropriately
public void Test_AddEditCustomer_Missing_Data()
{
	// 	Arrange: Set up necessary preconditions and inputs.

	// Act: Execute the functionality being tested.

	// Assert: Verify that the output matches expected results.
}

//	Test Validate the functionality of adding a new customer to the system.
public void Test_AddCustomer()
{
	// 	Arrange: Set up necessary preconditions and inputs.

	// Act: Execute the functionality being tested.

	// Assert: Verify that the output matches expected results.
}

// Test Validate the functionality of editing an existing customer's details.
public void Test_EditCustomer()
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

