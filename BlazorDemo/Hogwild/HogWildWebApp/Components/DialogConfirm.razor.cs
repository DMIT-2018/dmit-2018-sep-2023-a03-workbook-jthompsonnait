// ***********************************************************************
// Assembly         : HogWildWebApp
// Author           : James Thompson
// Created          : 06-19-2023
//
// Last Modified By : James Thompson
// Last Modified On : 06-20-2023
// ***********************************************************************
// <copyright file="DialogConfirm.razor.cs" company="NAIT">
//     Copyright (c) Northern Alberta Institute of Technology. All rights reserved.
// </copyright>
// <summary>
//  Components using with MubBlazor 
// </summary>
// ***********************************************************************

using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HogWildWebApp.Components
{
    /// <summary>
    /// Class DialogConfirm.
    /// </summary>
    public partial class DialogConfirm : IDisposable
    {
        /// <summary>
        /// Gets or sets the mud dialog.
        /// </summary>
        //  The mud dialog
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// Gets or sets the content text.
        /// </summary>
        //  The content text
        [Parameter] public string ContentText { get; set; }

        /// <summary>
        /// Gets or sets the button text.
        /// </summary>
        //  The button text
        [Parameter] public string ButtonText { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        //  The color
        [Parameter] public Color Color { get; set; }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        private void Submit() => MudDialog.Close(DialogResult.Ok(true));
        /// <summary>
        /// Cancels this instance.
        /// </summary>
        private void Cancel() => MudDialog.Cancel();

        public void Dispose()
        {
         //   throw new NotImplementedException();
        }
    }
}
