using System;
namespace DapperCRUD.Data
{
    public interface ICustomerService
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<List<Customer>> ListCustomer();
        Task<Customer> GetCustomer(int customerId);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(int customerId);
    }
}