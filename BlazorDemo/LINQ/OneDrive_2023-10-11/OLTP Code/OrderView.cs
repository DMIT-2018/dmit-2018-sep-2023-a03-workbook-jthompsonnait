
    public class OrderView
    {
        public int OrderID { get; set; }
        public int SalesRepID { get; set; }
        public string CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal Freight { get; set; }
        public bool Shipped { get; set; }
        public string ShipName { get; set; }
        public int ShipAddressID { get; set; }
        public string Comments { get; set; }
    }
