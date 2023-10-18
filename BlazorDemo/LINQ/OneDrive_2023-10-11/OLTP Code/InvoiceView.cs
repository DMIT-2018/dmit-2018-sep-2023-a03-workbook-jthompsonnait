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
