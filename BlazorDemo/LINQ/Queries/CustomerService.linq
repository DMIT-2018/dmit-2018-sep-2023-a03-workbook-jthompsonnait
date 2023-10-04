<Query Kind="Program" />

//	the following is equivalent to using in C#
//	the load method allows for us to reference the view models
#load "..\ViewModels\*.cs"

void Main()
{

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

	#endregion

	/*
		actual code for the method
	*/
	
	//	check if there are any aggreate exception(s)
	if(errorList.Count() > 0)
	{
		//	throw the list of business processing error(s)
		throw new AggregateException("Unabled to proceeed! Check concerns", errorList);
	}
	
	//	NOTE: we return a "null" so that the apllication does not thow an exception
	//			"not all code paths return a value"
	return null;

}
