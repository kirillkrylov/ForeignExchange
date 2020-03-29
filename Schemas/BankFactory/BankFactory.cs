namespace ForeignExchange
{
    public static class BankFactory
    {
        #region Enum
        public enum SupportedBanks
        {
            BOC = 0,
            CBR = 1,
            NBU = 2,
            ECB = 3
        }
        #endregion
        
        #region Methods
        public static IBank GetBank(SupportedBanks supportedBanks)
        {
            switch (supportedBanks)
            {
                case SupportedBanks.BOC:
                    return new BankOfCanada();

                case SupportedBanks.CBR:
                    return new CentralBankOfRussia();

                case SupportedBanks.NBU:
                    return new NationalBankOfUkraine();

                case SupportedBanks.ECB:
                    return new EuropeanCentralBank();
                default: return null;
            }
        }
        #endregion
    }
}