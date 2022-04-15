
namespace DapperCRUD.Data
{
    public interface IBooksService
    {
        Task<List<Books>> ListBooks();
    }
}