<Query Kind="Program">
  <Connection>
    <ID>4b5da6fb-5c95-4544-814e-55861abf7485</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
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

#load "..\*.cs"

/// <summary>
/// Main entry point for the application.
/// </summary>
void Main()
{
	//  coding for the AddEditInvoice method
	//	setup Add Invoice
	//	before action (Add)
	InvoiceView beforeAdd = new InvoiceView();
	//  do not set the InvoiceID

	//	Sam Smith (Customer)
	beforeAdd.CustomerID = 1;
	// Willie Work (Employee)
	beforeAdd.EmployeeID = 2;

	//  add invoice items
	InvoiceLineView invoiceLine = new InvoiceLineView();
	invoiceLine.PartID = 1;
	invoiceLine.Description = "Forged pistons";
	invoiceLine.Quantity = 10;
	invoiceLine.Price = 50.00m;
	beforeAdd.InvoiceLines.Add(invoiceLine);

	invoiceLine = new InvoiceLineView();
	invoiceLine.PartID = 4;
	invoiceLine.Description = "Rear brakes";
	invoiceLine.Quantity = 20;
	invoiceLine.Price = 60.00m;
	beforeAdd.InvoiceLines.Add(invoiceLine);

	//	showing results
	beforeAdd.Dump("Before Add");

	//	execute
	InvoiceView afterAdd = AddEditInvoice(beforeAdd);

	//	after action (Add)
	//	showing results
	afterAdd.Dump("After Add");


	//	setup Edit Category
	//	before action (Edit)
	int invoiceID = Invoices
					.OrderByDescending(x => x.InvoiceID)
					.Select(x => x.InvoiceID).FirstOrDefault();
	InvoiceView beforeEdit = GetInvoice(invoiceID);

	//	showing results
	beforeEdit.Dump("Before Edit");

	//  change Employee
	beforeEdit.EmployeeID = 1;

	//	update the first invoice line quantity to 1
	beforeEdit.InvoiceLines[0].Quantity = 1;

	//	soft delete second line
	beforeEdit.InvoiceLines[1].RemoveFromViewFlag = true; ;

	//	add one more item
	invoiceLine = new InvoiceLineView();
	invoiceLine.PartID = 3;
	invoiceLine.Description = "Exhaust system";
	invoiceLine.Quantity = 5;
	invoiceLine.Price = 400.00m;
	beforeEdit.InvoiceLines.Add(invoiceLine);

	//	execute
	InvoiceView afterEdit = AddEditInvoice(beforeEdit);

	//	after action (Edit)
	//	showing results
	afterEdit.Dump("After Edit");

}

/// <summary>
/// Defines methods that are a part of the application (service).
/// </summary>
#region Method

/// <summary>
/// Adds or edits a customer/address/invoice etc in the system based on the provided xxxx view model.
/// </summary>
/// <param name="XXXView">The  view model containing details to be added or edited.</param>
/// <returns>
/// The result of the addition or edit operation, currently set to return null.
/// NOTE: The return value may need adjustment based on intended behavior.
/// </returns>
public InvoiceView AddEditInvoice(InvoiceView invoiceView)
{

	#region Business Logic and Parameter Exceptions
	//	create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();
	//  Business Rules
	//	These are processing rules that need to be satisfied
	//		for valid data

	//	rule:	invoice cannot be null
	if (invoiceView == null)
	{
		throw new ArgumentNullException("No invoice was supply");
	}

	//	rule:	customer id must be supply
	if (invoiceView.CustomerID == 0)
	{
		errorList.Add(new Exception("Customer is required"));
	}

	//	rule:	employee id must be supply	
	if (invoiceView.EmployeeID == 0)
	{
		errorList.Add(new Exception("Employee is required"));
	}

	//	rule:	there must be invoice lines provided
	if (invoiceView.InvoiceLines.Count == 0)
	{
		errorList.Add(new Exception("Invoice details are required"));
	}

	//	rule:	for each invoice line, there must be a part
	//	rule:	for each invoice line, the price cannot be less than zero
	foreach (var invoiceLine in invoiceView.InvoiceLines)
	{
		if (invoiceLine.PartID == 0)
		{
			throw new ArgumentNullException("Missing part ID");
		}

		if (invoiceLine.Price < 0)
		{
			string partName = Parts
								.Where(x => x.PartID == invoiceLine.PartID)
								.Select(x => x.Description)
								.FirstOrDefault();
			errorList.Add(new Exception($"Part {partName} has a price that is less than zero"));
		}
	}

	//	rule:	parts cannot be duplicated on more than one line.
	List<string> duplicatedParts = invoiceView.InvoiceLines
								.GroupBy(x => new { x.PartID })
								.Where(gb => gb.Count() > 1)
								.OrderBy(gb => gb.Key.PartID)
								.Select(gb => Parts
												.Where(p => p.PartID == gb.Key.PartID)
												.Select(p => p.Description)
												.FirstOrDefault()
								).ToList();

	if (duplicatedParts.Count > 0)
	{
		foreach (var partName in duplicatedParts)
		{
			errorList.Add(new Exception($"Part {partName} can only be added to the invocie lines once."));
		}
	}

	#endregion

	// Retrieve the invoice from the database or create a new one if it doesn't exist.
	Invoice invoice = Invoices
						.Where(x => x.InvoiceID == invoiceView.InvoiceID)
						.FirstOrDefault();

	// If the invoice doesn't exist, initialize it.
	if (invoice == null)
	{
		invoice = new Invoice();
		invoice.InvoiceDate = DateTime.Now; // Set the current date for new invoices.
	}
	else
	{
		// Update the date for existing invoices.
		invoice.InvoiceDate = invoiceView.InvoiceDate;
	}

	// Map attributes from the view model to the data model.
	invoice.CustomerID = invoiceView.CustomerID;
	invoice.EmployeeID = invoiceView.EmployeeID;
	invoice.SubTotal = 0;
	invoice.Tax = 0;

	// Process each line item in the provided view model.
	foreach (var invoiceLineView in invoiceView.InvoiceLines)
	{
		InvoiceLine invoiceLine = InvoiceLines
									.Where(x => x.InvoiceLineID == invoiceLineView.InvoiceLineID
											&& x.PartID == invoiceLineView.PartID)
									.FirstOrDefault();

		// If the line item doesn't exist, initialize it.
		if (invoiceLine == null)
		{
			invoiceLine = new InvoiceLine();
			invoiceLine.PartID = invoiceLineView.PartID;
		}

		// Map fields from the line item view model to the data model.
		invoiceLine.Quantity = invoiceLineView.Quantity;
		invoiceLine.Price = invoiceLineView.Price;
		invoiceLine.RemoveFromViewFlag = invoiceLineView.RemoveFromViewFlag;

		// Handle new or existing line items.
		if (invoiceLine.InvoiceLineID == 0)
		{
			invoice.InvoiceLines.Add(invoiceLine); // Add new line items.
		}
		else
		{
			InvoiceLines.Update(invoiceLine); // Update existing line items.
		}

		//	need to update total and tax if the 
		//		invoice line item is not set to be removed from view.
		if (!invoiceLine.RemoveFromViewFlag)
		{
			invoice.SubTotal += invoiceLine.Quantity * invoiceLine.Price;
			bool isTaxable = Parts
								.Where(x => x.PartID == invoiceLine.PartID)
								.Select(x => x.Taxable)
								.FirstOrDefault();
			invoice.Tax += isTaxable ? invoiceLine.Quantity * invoiceLine.Price * .05m : 0;
		}
	}

	// If it's a new invoice, add it to the collection.
	if (invoice.InvoiceID == 0)
	{
		Invoices.Add(invoice);
	}

	// Handle any captured errors.
	if (errorList.Count > 0)
	{
		// Clear changes to maintain data integrity.
		ChangeTracker.Clear();
		string errorMsg = "Unable to add or edit Invoice or Invoice Lines.";
		errorMsg += " Please check error message(s)";
		throw new AggregateException(errorMsg, errorList);
	}
	else
	{
		// Persist changes to the database.
		SaveChanges();
	}
	return GetInvoice(invoice.InvoiceID);
}

public InvoiceView GetInvoice(int invoiceId)
{
	//  Business Rules
	//	These are processing rules that need to be satisfied
	//		for valid data
	//		rule:	invoice id must be valid 

	if (invoiceId == 0)
	{
		throw new ArgumentNullException("Please provide a invoice id");
	}

	return Invoices
				.Where(x => x.InvoiceID == invoiceId
						&& !x.RemoveFromViewFlag)
				.Select(x => new InvoiceView
				{
					InvoiceID = x.InvoiceID,
					InvoiceDate = x.InvoiceDate,
					CustomerID = x.CustomerID,
					CustomerName = $"{x.Customer.FirstName} {x.Customer.LastName}",
					EmployeeID = x.EmployeeID,
					EmployeeName = $"{x.Employee.FirstName} {x.Employee.LastName}",
					SubTotal = x.SubTotal,
					Tax = x.Tax,
					RemoveFromViewFlag = x.RemoveFromViewFlag,
					InvoiceLines = InvoiceLines
										.Where(il => il.InvoiceID == invoiceId
												&& !il.RemoveFromViewFlag)
										.Select(il => new InvoiceLineView
										{
											InvoiceLineID = il.InvoiceLineID,
											InvoiceID = il.InvoiceID,
											PartID = il.PartID,
											Description = il.Part.Description,
											Quantity = il.Quantity,
											Price = il.Price,
											RemoveFromViewFlag = il.RemoveFromViewFlag
										}).ToList()
				}).FirstOrDefault();
}

#endregion


/// <summary>
/// Contains class definitions that are referenced in the current LINQ file.
/// </summary>
/// <remarks>
/// It's crucial to highlight that in standard development practices, code and class definitions 
/// should not be mixed in the same file. Proper separation of concerns dictates that classes 
/// should have their own dedicated files, promoting modularity and maintainability.
/// </remarks>
#region Class
public class InvoiceView
{
	public int InvoiceID { get; set; }
	public DateTime InvoiceDate { get; set; }
	public int CustomerID { get; set; }
	public string CustomerName { get; set; }
	public int EmployeeID { get; set; }
	public string EmployeeName { get; set; }
	public decimal SubTotal { get; set; }
	public decimal Tax { get; set; }
	public List<InvoiceLineView> InvoiceLines { get; set; } = new List<InvoiceLineView>();
	public bool RemoveFromViewFlag { get; set; }
}


public class InvoiceLineView
{
	public int InvoiceLineID { get; set; }
	public int InvoiceID { get; set; }
	public int PartID { get; set; }
	public string Description { get; set; }
	public int Quantity { get; set; }
	public decimal Price { get; set; }
	public bool RemoveFromViewFlag { get; set; }
}

#endregion
