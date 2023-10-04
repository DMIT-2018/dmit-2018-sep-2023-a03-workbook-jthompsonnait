<Query Kind="Program" />

void Main()
{
	Test_Math_Division_Valid();
}

//	validate that the division function
//		return a correct value
public void Test_Math_Division_Valid()
{
	//	Arrange
	//	Initialize any variable need for our test
	decimal num1 = 10;
	decimal num2 = 2;
	
	//	Act
	//	Only the call to the method/fucntion you are testing
	//	Rename result to actual for comparing
	var actual = Div(num1, num2);
	
	// Assert
	//	Verify that the ourcome of the "Act" phase matches
	//		the expected result of behavior
	//	NOTE: We set the expected result to 7 to forace a 
	//			failure.
	decimal expected = 5;
	
	//	Validate if we got the expected results
	string isValid = actual == expected? "Pass" : "Fail";

	// Display the result to the user
	Console.WriteLine($"-- {isValid} -- Test_Math_Division_Valid: Expected {expected} vs Actual {actual}");
	
}

//	Division function
public decimal Div(decimal a, decimal b)
{
	return (a / b); 
}















