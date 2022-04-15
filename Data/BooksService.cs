using Dapper;
using Microsoft.Data.SqlClient;
namespace DapperCRUD.Data
{
    public class BooksService : IBooksService
    {
        ServerConnection _Connection;

        public BooksService(ServerConnection Connection)
        {
            _Connection = Connection;
        }

        public async Task<List<Books>> ListBooks()
        {
            IEnumerable<Books> books;
            using (var conn = new SqlConnection(_Connection.connectionString))
            {
                books = await conn.QueryAsync<Books>("storedProcBooks_Select", commandType: System.Data.CommandType.StoredProcedure);
            }
            return books.ToList();
        }
    }
}
