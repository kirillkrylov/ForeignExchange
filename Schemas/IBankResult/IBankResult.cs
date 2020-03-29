using System;

namespace ForeignExchange
{
    public interface IBankResult
    {
        string HomeCurrency { get; set; }
        decimal ExchangeRate { get; set; }
        DateTime RateDate { get; set; }
        string BankName { get; set; }
    }
    public sealed class BankResult : IBankResult
    {
        #region Properties
        public string HomeCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime RateDate { get; set; }
        public string BankName { get; set; }
        #endregion
    }
}