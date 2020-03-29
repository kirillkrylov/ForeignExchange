using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Web.Common;

namespace ForeignExchange
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ExchangeRateWS : BaseService
    {
        #region Properties
        private SystemUserConnection _systemUserConnection;
        private SystemUserConnection SystemUserConnection
        {
            get
            {
                return _systemUserConnection ?? (_systemUserConnection = (SystemUserConnection)AppConnection.SystemUserConnection);
            }
        }
        #endregion

        #region Methods : REST
        /// <summary>
        /// Obtains the latest available foreign exchange rate from a given bank
        /// </summary>
        /// <param name="bank">Id of the bank from "Supported Banks" lookup </param>
        /// <param name="transactionDate">Foreign Exchange Date (observation date)</param>
        /// <param name="currency">Id of Foreign currency from "Currency" lookup</param>
        /// <returns>Latest available observation</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public BankResult ExchangeRate(Guid bank, DateTime transactionDate, Guid currency)
        {
            //
            //Available at [Application Address]/0/rest/ExchangeRateWS/ExchangeRate

            UserConnection userConnection = UserConnection ?? SystemUserConnection;
            BankFactory.SupportedBanks selectedBank = BankFactory.SupportedBanks.BOC;
            switch (bank.ToString()) {

                case "c64cb3d6-3c87-4eb6-88ec-13d03ef5391e":
                    selectedBank = BankFactory.SupportedBanks.BOC;
                    break;

                case "2cb64ddc-1e80-4a92-a647-843746968206":
                    selectedBank = BankFactory.SupportedBanks.CBR;
                    break;

                case "82966eac-b7fa-4a5f-b205-f03724c09226":
                    selectedBank = BankFactory.SupportedBanks.NBU;
                    break;

                case "3444ff4e-7f8f-4695-a0bb-21a3b2c6536a":
                    selectedBank = BankFactory.SupportedBanks.ECB;
                    break;

                default:
                    selectedBank = BankFactory.SupportedBanks.BOC;
                    break;
            }

            IBank cbr = BankFactory.GetBank(selectedBank);
            string cur = GetCurrencyNameById(userConnection, currency);
            IBankResult res = cbr.GetRateAsync(cur, transactionDate).Result;

            return (BankResult)res;
        }

        #endregion

        #region Methods : Private

        private string GetCurrencyNameById(UserConnection userConnection, Guid currencyId) {
            Select select = new Select(userConnection)
                .Column("ShortName")
                .From("Currency")
                .Where("Id").IsEqual(Column.Parameter(currencyId)) as Select;
            return select.ExecuteScalar<string>();
        }



        #endregion
    }
}



