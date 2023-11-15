#nullable disable
using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;

namespace HogWildWebApp.Pages.SamplePages
{
    public partial class WorkingVersions
    {
        #region Fields
        //  Property for holding any feedback messages
        private string feedback;
        // This private field holds a reference to the WorkingVersionsView instance.
        private WorkingVersionsView workingVersionsView = new WorkingVersionsView();
        #endregion

        #region Properties
        // This attribute marks the property for dependency injection.
        [Inject]
        // This property provides access to the 'WorkingVersionsService' service.
        protected WorkingVersionsService WorkingVersionsService { get; set; }
        #endregion

        #region Methods
        private void GetWorkingVersions()
        {
            try
            {
                workingVersionsView = WorkingVersionsService.GetWorkingVersion();
            }
            #region catch all exceptions
            catch (AggregateException ex)
            {
                foreach (var error in ex.InnerExceptions)
                {
                    feedback = error.Message;
                }
            }

            catch (ArgumentNullException ex)
            {
                feedback = GetInnerException(ex).Message;
            }

            catch (Exception ex)
            {
                feedback = GetInnerException(ex).Message;
            }
            #endregion

        }
        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        #endregion
    }
}
