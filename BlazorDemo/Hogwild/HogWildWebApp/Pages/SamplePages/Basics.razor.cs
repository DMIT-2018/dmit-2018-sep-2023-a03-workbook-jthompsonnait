using Microsoft.AspNetCore.Components;

namespace HogWildWebApp.Pages.SamplePages
{
    public partial class Basics
    {
        #region Privates
        #region Methods
        //  used to display my name
        private string? myName;
        //  the odd or even value
        private int oddEvenValue;
        #endregion
        #region Text Boxes
        //  email address
        private string emailText;
        //  passowrd
        private string passwordText;
        //  date
        private DateTime? dateText = DateTime.Today;
        #endregion

        #region Radio Buttons, Check boxes & Text Area
        //  selected meal
        private string meal = "breakfast";
        private string[] meals { get; set; } = new string[] {"breakfast", "lunch", "dinner", "snack"};
        //  used to hold the value of the acceptance
        private bool acceptanceBox;
        //  used to hold the balue for the message body
        private string messageBody;
        #endregion
        //  used to display any feedback to the end user.
        private string feedback;
        #endregion

        //  This method is automatically called when the component is initialized
        protected override async Task OnInitializedAsync()
        {
            //  Call the base class OnInitializedAsync method (if any)
            await base.OnInitializedAsync();

            //  call the RandonValue method to perform custom initialization logic.
            RandomValue();
        }

        //  Generates a random number betwen 0 and 25 using the Random class
        //  Checks if the generated number is even
        //  Sets the myName variable to a message if even, or to null if odd
        private void RandomValue()
        {
            //  Create an instance of the Random class to generate random numbers
            Random rnd = new Random();

            //  Generate a random integer between 0 (inclusive) and 25 (exclusive)
            oddEvenValue = rnd.Next(0, 25);

            //  Check if the generated number is even.
            if (oddEvenValue % 2 == 0)
            {
                //  If the number is even, contstruct a message with the number and assign it to myName
                myName = $"James is even {oddEvenValue}";
            }
            else
            {
                //  If the number is odd, set myName to null,
                myName = null;
            }
            //  Trigger an asynchronous update of the component's state to reflect the changes made.
            InvokeAsync(StateHasChanged);
        }

        //  This method is called when a user submits text input.
        private void TextSubmit()
        {
            //  Combine the values of emailText, passordText, and dateText into a feedback message.
            feedback = $"Email {emailText}; Password {passwordText}; Date {dateText}";

            //  Trigger a re-render of the component to reflect the updated feedback.
            InvokeAsync(StateHasChanged);
        }

        //  Handle the selection of the loop meal
        private void HandleMealSelection(ChangeEventArgs e)
        {
            meal = e.Value.ToString();
        }
    }
}
