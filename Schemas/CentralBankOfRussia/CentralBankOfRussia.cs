using System;
using System.Data;
using System.ServiceModel;
using System.Threading.Tasks;
using ForeignExchange.CbrDailyInfo;

namespace ForeignExchange
{
    public sealed class CentralBankOfRussia : IBank
    {

        #region Constants
        private const string homeCurrency = "RUB";
        private const string bankName = "Central bank of the Russian Federation";
        #endregion

        #region Properties
        public string HomeCurrency => homeCurrency;
        public DateTime RateDate { get; private set; }
        #endregion

        #region Methods
        public Task<IBankResult> GetRateAsync(string currency, DateTime date)
        {
            IBankResult bankResult = new BankResult()
            {
                ExchangeRate = -1m,
                RateDate = date,
                HomeCurrency = homeCurrency,
                BankName = bankName
            };

            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endPointAddress = new EndpointAddress("http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
            DailyInfoSoap client = new DailyInfoSoapClient(binding, endPointAddress);

            Task<DataSet> fxRates = client.GetCursOnDateAsync(date);
            fxRates.Wait();
            DataTable table = fxRates.Result.Tables["ValuteCursOnDate"];

            string filterExpression = $"VchCode = '{currency}'";
            DataRow[] rows = table.Select(filterExpression);

            decimal.TryParse(rows[0]["Vnom"].ToString(), out decimal multiplier);
            decimal.TryParse(rows[0]["Vcurs"].ToString(), out decimal rate);
            decimal fxRate = rate / multiplier;
            bankResult.ExchangeRate = fxRate;

            return Task.FromResult(bankResult);
        }
        #endregion
    }
}