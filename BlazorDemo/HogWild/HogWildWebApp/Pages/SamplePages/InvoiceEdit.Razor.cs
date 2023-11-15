#nullable disable
using BlazorDialog;
using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using HogWildWebApp.Components;
using HogWildWebApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace HogWildWebApp.Pages.SamplePages
{
    public partial class InvoiceEdit
    {
        #region Field
        // The description
        private string description;

        // The category id        
        private int categoryID;

        // The part Categories        
        private List<LookupView> partCategories;

        // The parts
        private List<PartView> parts = new List<PartView>();

        // The invoice        
        private InvoiceView invoice;

        //  feedback message        
        private string feedbackMessage;

        //  error messasge        
        private string errorMessage;

        //  has feedback
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        //  has errors
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage);

        //  has error details
        private List<string> errorDetails = new();
        #endregion

        #region Properties
        //  The navigation manager.
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        //  The invoice service.
        [Inject]
        protected InvoiceService InvoiceService { get; set; }

        //  The part service.
        [Inject]
        protected PartService PartService { get; set; }

        //  The category lookup service.
        [Inject]
        protected CategoryLookupService CategoryLookupService { get; set; }

        //  The dialog service.
        [Inject]
        protected IDialogService DialogService { get; set; }

        // The blazor dialog service
        [Inject]
        IBlazorDialogService BlazorDialogService { get; set; }

        //  The invoice identifier.
        [Parameter] public int InvoiceID { get; set; }

        //  The customer identifier.
        [Parameter] public int CustomerID { get; set; }

        //  The employee identifier.
        [Parameter] public int EmployeeID { get; set; }
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                //  get categories
                partCategories = CategoryLookupService.GetLookups("Part Categories");

                //  get the invoice
                invoice = InvoiceService.GetInvoice(InvoiceID, CustomerID, EmployeeID);

                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = String.Empty;

                await InvokeAsync(StateHasChanged);
            }
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }

                errorMessage = $"{errorMessage}Unable to search for invoice";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        /// <summary>
        /// Searches this instance.
        /// </summary>
        /// <exception cref="System.ArgumentException">Please provide either a category and/or description</exception>
        private async Task SearchParts()
        {
            try
            {
                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = String.Empty;

                //  clear the part list before we do our search
                parts.Clear();

                if (categoryID == 0 && string.IsNullOrWhiteSpace(description))
                {
                    throw new ArgumentException("Please provide either a category and/or description");
                }

                //  search for our parts
                List<int> existingPartIDs =
                    invoice.InvoiceLines
                    .Select(x => x.PartID)
                    .ToList();
                ;
                parts = PartService.GetParts(categoryID, description, existingPartIDs);
                await InvokeAsync(StateHasChanged);

                if (parts.Count() > 0)
                {
                    feedbackMessage = "Search for part(s) was successful";
                }
                else
                {
                    feedbackMessage = "No part were found for your search criteria";
                }
            }
            //  Your Catch Code Below
            //  code here
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }
                errorMessage = $"{errorMessage}Unable to search for part";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }
        //  Add part to invoice line and remove part from part list
        private async Task AddPart(int partID)
        {
            PartView part = PartService.GetPart(partID);
            InvoiceLineView invoiceLine = new InvoiceLineView();
            invoiceLine.PartID = partID;
            invoiceLine.Description = part.Description;
            invoiceLine.Price = part.Price;
            invoiceLine.Taxable = part.Taxable;
            invoiceLine.Quantity = 0;
            invoice.InvoiceLines.Add(invoiceLine);

            //  remove the current part from the part list.
            parts.Remove(parts
                .Where(x => x.PartID == partID)
                .FirstOrDefault());
            await InvokeAsync(StateHasChanged);
        }

        //  delete invoice line
        private async Task DeleteInvoiceLine(int partID)
        {
            InvoiceLineView invoiceLine = invoice.InvoiceLines
                .Where(x => x.PartID == partID)
                .Select(x => x).FirstOrDefault();

            string bodyText = $"Are you sure that you wish to remove {invoiceLine.Description}?";

            string dialogResult =
                     await BlazorDialogService.ShowComponentAsDialog<string>(
                         new ComponentAsDialogOptions(typeof(SimpleComponentDialog))
                         {
                             Size = DialogSize.Normal,
                             Parameters = new()
                             {
                                { nameof(SimpleComponentDialog.Input), "Remove Invoice Line" },
                                {nameof(SimpleComponentDialog.BodyText), bodyText}

                             }
                         });

            //  return results can be either "Ok" or "Cancel"
            if (dialogResult == "Ok")
            {
                //  remove invoice line
                //  second half of the "Simple List to List"                
                invoice.InvoiceLines.Remove(invoiceLine);
                //  update search results incase the part we removed
                //  is part of the search results
                if (categoryID > 0 || !string.IsNullOrEmpty(description))
                {
                    await SearchParts();
                }
                UpdateSubtotalAndTax();
                await InvokeAsync(StateHasChanged);
            }
        }

        //  update subtotal and tax
        private void UpdateSubtotalAndTax()
        {
            invoice.SubTotal = invoice.InvoiceLines
                .Where(x => !x.RemoveFromViewFlag)
                .Sum(x => x.Quantity * x.Price);
            invoice.Tax = invoice.InvoiceLines
                .Where(x => !x.RemoveFromViewFlag)
                .Sum(x => x.Taxable ? x.Quantity * x.Price * 0.05m : 0);
        }

        private void Save()
        {
            try
            {
                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = String.Empty;

                //SAVE NEEDS TO BE CODED
                InvoiceService.Save(invoice);
            }
            //  Your Catch Code Below
            //  code here
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }
                errorMessage = $"{errorMessage}Unable to save invoice";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        private async Task Close()
        {
            string dialogResult =
                                 await BlazorDialogService.ShowComponentAsDialog<string>(
                                     new ComponentAsDialogOptions(typeof(SimpleComponentDialog))
                                     {
                                         Size = DialogSize.Normal,
                                         Parameters = new()
                                         {
                                { nameof(SimpleComponentDialog.Input), "Do you wish to close the invoice editor?" }
                                         }
                                     });

            //  return results can be either "Ok" or "Cancel"
            if (dialogResult == "Ok")
            {
                //  return to the customer edit screen
                NavigationManager.NavigateTo($"/SamplePages/CustomerEdit/{CustomerID}");
            }
        }
    }
}
