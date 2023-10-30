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
    public partial class DialogConfirm
    {
        /// <summary>
        /// Gets or sets the mud dialog.
        /// </summary>
        /// <value>The mud dialog.</value>
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// Gets or sets the content text.
        /// </summary>
        /// <value>The content text.</value>
        [Parameter] public string ContentText { get; set; }

        /// <summary>
        /// Gets or sets the button text.
        /// </summary>
        /// <value>The button text.</value>
        [Parameter] public string ButtonText { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        [Parameter] public Color Color { get; set; }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        /// <summary>
        /// Cancels this instance.
        /// </summary>
        void Cancel() => MudDialog.Cancel();
    }
}
