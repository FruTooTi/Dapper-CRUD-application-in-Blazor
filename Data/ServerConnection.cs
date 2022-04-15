namespace DapperCRUD.Data
{
    public class ServerConnection
    {
        public ServerConnection(string path)
        {
            connectionString = path;
        }
        public string connectionString { get;}
    }
}
