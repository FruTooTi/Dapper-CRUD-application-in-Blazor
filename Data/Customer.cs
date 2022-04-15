namespace DapperCRUD.Data
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public int? BookId { get; set; }
        public Books BookBorrowed { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? DateToBeReceived { get; set; }
    }
}
