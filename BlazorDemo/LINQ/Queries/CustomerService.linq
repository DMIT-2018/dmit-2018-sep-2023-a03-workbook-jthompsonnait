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

void Main()
{

}

/// <summary>
/// Retrieves the detailed view of a specific customer based on their ID.
/// </summary>
/// <param name="customerID">The unique identifier of the customer to be retrieved.</param>
/// <returns>A CustomerEditView object containing detailed information about the customer.</returns>
public CustomerEditView GetCustomer(int customerID)
{
	#region Business Logic and Parameter Exception
	//	create a List<Exception> to conatin all discoverd errors
	List<Exception> errorList = new List<Exception>();
	//	Business Rules (all business rules are listed here)
	//		rule: 	Customer ID must be valie
	//		rule:	Customer must exist in the database

	//	parameter validation
	//	customer ID must be valid
	//	if the id is invalid, no reason to cintinue in the process
	if (customerID < 1)
	{
		throw new Exception("Customer ID is invalid (less than 1)");
	}
	//	customer must exist in the database
	//	if the customer does not exist, no reason to continue in the process 
	CustomerEditView customer = Customers
									.Where(x => x.CustomerID == customerID
											&& x.RemoveFromViewFlag == false)
									.Select(x => new CustomerEditView
									{
										CustomerID = x.CustomerID,
										FirstName = x.FirstName,
										LastName = x.LastName,
										Address1 = x.Address1,
										Address2 = x.Address2,
										City = x.City,
										ProvStateID = x.ProvStateID,
										CountryID = x.CountryID,
										PostalCode = x.PostalCode,
										Phone = x.Phone,
										Email = x.Email,
										StatusID = x.StatusID,
										RemoveFromViewFlag = x.RemoveFromViewFlag

									}).FirstOrDefault();
	if (customer == null)
	{
		throw new ArgumentNullException($"No customer found for customer ID {customerID}");
	}
	#endregion
	return customer;
}

/// <summary>
/// Retrieves a list of customers based on the provided search criteria.
/// </summary>
/// <param name="lastName">The last name of the customer to search for.</param>
/// <param name="phone">The phone number of the customer to search for</param>
/// <returns>A list of <see cref="CustomerSearchView"/> objects that match the 
/// 	provided search criteria. Returns a null exception if nothing is return.</returns>
public List<CustomerSearchView> GetCustomers(string lastName, string phone)
{
	#region Business Logic and Parameter Exceptions
	//	Create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();

	//	Business Rules (all business rules are listed here)
	//		rule: both last name and phone number cannot be empty
	//		rule: if both values are provided
	//				both values will be used in returning
	//				a list of customer search views
	//		rule:	RemoveFromViewFlag must be false

	//	parameter validation
	//	both last name and phone number cannot be empty
	//	we are going to use the ArgumentNullException because we can't
	//		return data unless we have a minium of one of these.
	if (string.IsNullOrWhiteSpace(lastName) && string.IsNullOrWhiteSpace(phone))
	{
		throw new ArgumentNullException("Please provide either a last name and/or phone number");
	}
	#endregion

	/*
		actual code for the method
	*/

	//	check if there are any aggreate exception(s)
	if (errorList.Count() > 0)
	{
		//	throw the list of business processing error(s)
		throw new AggregateException("Unabled to proceeed! Check concerns", errorList);
	}

	//	NOTE: we return a "null" so that the apllication does not thow an exception
	//			"not all code paths return a value"
	return null;

}

/// <summary>
/// Adds a new customer or edits an existing one based on the provided CustomerEditView details.
/// </summary>
/// <param name="customerEditView">The detailed view of the customer to be added or edited.</param>
/// <returns>A CustomerEditView object reflecting the finalized state after addition or modification.</returns>
public CustomerEditView AddEditCustomer(CustomerEditView customerEditView)
{

	#region Business Logic and Parameter Exceptions
	// 	create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();

	//	Businees Rules (all business rules are listed here)
	//		rule:	Excluding the customer ID and address 2, all fields must be valid.
	//		rule:	Cannot add a customer if their phone number already exists in the database.
	
	//	parameter validation
	//	check if all properties are valid
	//	for each invalid property, add it to the errorList

	//  create a list of string for properties that we do not to check.
	List<string> ignoreProperties = new List<string>();
	ignoreProperties.Add("CustomerID");
	ignoreProperties.Add("Address2");

	//	check for missing string properties or int at are zero (all int properties are lookup)
	CheckForInvalidProperties(errorList, customerEditView, ignoreProperties);

	//	if our errorlist has any errors, we will throw a AggregateException
	if (errorList.Count() > 0)
	{
		//	throw the list of business processing error(s)
		throw new AggregateException("Unable to process! Check concerns", errorList);
	}
	#endregion

	return null;
}

/// <summary>
/// Validates properties of the given object. It checks if string properties are null or empty and if int properties have a value of 0.
/// Any detected invalid properties are added as exceptions to the provided error list.
/// </summary>
/// <param name="errorList">List to collect exceptions related to invalid properties.</param>
/// <param name="src">Object to be inspected for invalid properties.</param>
/// <param name="ignoreProperties">List of property names that should be excluded from validation.</param>
private void CheckForInvalidProperties(List<Exception> errorList, object src, List<string> ignoreProperties)
{
	// Check if the source object is null before proceeding.
	if (src == null)
		throw new ArgumentNullException(nameof(src));

	// Iterate through each property of the source object using reflection.
	foreach (PropertyInfo property in src.GetType().GetProperties())
	{
		// If the current property is in the ignore list, skip its validation.
		if (ignoreProperties.Contains(property.Name))
			continue;

		// Fetch the value of the current property.
		var value = property.GetValue(src);

		// Validate string properties for null or empty values.
		if (property.PropertyType == typeof(string))
		{
			if (string.IsNullOrEmpty((string)value))
			{
				errorList.Add(new Exception($"Property '{property.Name}' is null or empty."));
			}
		}
		// Validate integer properties for a value of 0.
		else if (property.PropertyType == typeof(int))
		{
			if ((int)value == 0)
			{
				errorList.Add(new Exception($"Property '{property.Name}' has a value of 0."));
			}
		}
	}
}