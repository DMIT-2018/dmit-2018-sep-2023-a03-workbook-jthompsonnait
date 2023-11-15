// ***********************************************************************
// Assembly         : HogWildSystem
// Author           : James Thompson
// Created          : 05-31-2023
//
// Last Modified By : James Thompson
// Last Modified On : 08-25-2023
// ***********************************************************************
// <copyright file="InvoiceView.cs" company="HogWildSystem">
//     Copyright (c) Northern Alberta Institute of Technology. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace HogWildSystem.ViewModels
{
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
        public decimal Total => SubTotal + Tax;
        public List<InvoiceLineView> InvoiceLines { get; set; } = new();
        public bool RemoveFromViewFlag { get; set; }
    }
}
