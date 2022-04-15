using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperCRUD.Data
{
    public class CustomerService : ICustomerService
    {
        private ServerConnection _Connection;
        public CustomerService(ServerConnection Connection)
        {
            _Connection = Connection;
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            using (var conn = new SqlConnection(_Connection.connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Name", customer.Name, System.Data.DbType.String);
                parameters.Add("@BookId", customer.BookId, System.Data.DbType.Int64);
                parameters.Add("@DateTaken", customer.DateTaken, System.Data.DbType.DateTime);
                parameters.Add("@DateToBeReceived", customer.DateToBeReceived, System.Data.DbType.DateTime);

                // Raw SQL Method
                // string query = $"INSERT INTO Customer (Name, BookId, DateTaken, DateToBeReceived) VALUES (@Name, @BookId, @DateTaken, @DateToBeReceived)";
                // await conn.ExecuteAsync(query, parameters);

                await conn.ExecuteAsync("storedProcCustomer_Insert", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return customer;
        }

        public async Task<List<Customer>> ListCustomer()
        {
            IEnumerable<Customer> AllCustomers;
            using(var conn = new SqlConnection(_Connection.connectionString))
            {
                // Raw SQL Method
                // string query = "SELECT Customer.CustomerId, Customer.Name, Customer.BookId, Customer.DateTaken, Customer.DateToBeReceived, Books.Id, Books.BookName, Books.Type, Books.PageCount FROM Customer INNER JOIN Books ON Customer.BookId = Books.Id";
                /* AllCustomers = await conn.QueryAsync<Customer, Books, Customer>(query, map: (customer, book) =>
                {
                    customer.BookBorrowed = book;
                    return customer;
                }, splitOn: "Id"); */

                AllCustomers = await conn.QueryAsync<Customer, Books, Customer>("storedProcCustomer_Select", map: (customer, book) =>
                {
                    customer.BookBorrowed = book;
                    return customer;
                }, splitOn: "Id");
            }
            return AllCustomers.ToList();
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            Customer selectedCustomer;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerID", customerId, System.Data.DbType.Int64);
            using (var conn = new SqlConnection(_Connection.connectionString))
            {
                selectedCustomer = await conn.QueryFirstOrDefaultAsync<Customer>("storedProcCustomer_SelectOne", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            return selectedCustomer;
        }

        public async Task UpdateCustomer(Customer customer)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerID", customer.CustomerId, System.Data.DbType.Int64);
            parameters.Add("@Name", customer.Name, System.Data.DbType.String);
            parameters.Add("@DateTaken", customer.DateTaken, System.Data.DbType.DateTime);
            parameters.Add("@DateToBeReceived", customer.DateToBeReceived, System.Data.DbType.DateTime);
            parameters.Add("@BookId", customer.BookId, System.Data.DbType.Int64);
            using (var conn = new SqlConnection(_Connection.connectionString))
            {
                await conn.ExecuteAsync("storedProcCustomer_Update", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task DeleteCustomer(int customerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", customerId, System.Data.DbType.Int64);
            using(var conn = new SqlConnection(_Connection.connectionString))
            {
                await conn.ExecuteAsync("storedProcCustomer_Delete", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
