namespace CorporateReporting.Web.Models
{
    public class SalesData
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// OrderNumber
        /// </summary>
        public string OrderNumber { get; set; } = null!;
        /// <summary>
        /// SaleDate
        /// </summary>
        public DateTime SaleDate { get; set; }
        /// <summary>
        /// CustomerName
        /// </summary>
        public string CustomerName { get; set; } = null!;
        /// <summary>
        /// ProductName
        /// </summary>
        public string ProductName { get; set; } = null!;
        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; } = null!;
        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// UnitPrice
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// TotalAmount
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// DepartmentId
        /// </summary>
        public int DepartmentId { get; set; }
        /// <summary>
        /// Department
        /// </summary>
        public Department Department { get; set; } = null!;
    }
}
