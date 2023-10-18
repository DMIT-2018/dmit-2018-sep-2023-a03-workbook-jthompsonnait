
    public class ShipmentView
    {
        public int ShipmentID { get; set; }
        public int OrderID { get; set; }
        public DateTime ShippedDate { get; set; }
        public int ShipVia { get; set; }
        public decimal FreightCharge { get; set; }
        public string TrackingCode { get; set; }
    }
