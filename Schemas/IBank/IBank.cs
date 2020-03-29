using System;
using System.Threading.Tasks;

namespace ForeignExchange
{
    public interface IBank
    {
        Task<IBankResult> GetRateAsync(string currency, DateTime date);
    }
}