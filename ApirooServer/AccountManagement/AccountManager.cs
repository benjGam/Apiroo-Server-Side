using ApirooServer.Utils;
using System.Net.Sockets;

namespace ApirooServer.AccountManagement
{
    public class AccountManager : Singleton<AccountManager>
    {
        private Dictionary<string, Account> Accounts = new Dictionary<string, Account>();
        private List<Account> UncheckedAccounts = new List<Account>();

        private const int YEAR_DELETE_ACCOUNT_DELAY = 0;
        private const int MONTH_DELETE_ACCOUNT_DELAY = 0;
        private const int DAY_DELETE_ACCOUNT_DELAY = 2;

        public AccountManager() : base()
        {
            new Logger<AccountManager>();
        }

        #region Private Methods
        private bool AccountDeletationDelayExpired(DateTime? accountCreationTime) => accountCreationTime != null ? ((DateTime)accountCreationTime).CompareTo(new DateTime(DateTime.Now.Year + YEAR_DELETE_ACCOUNT_DELAY, DateTime.Now.Month + MONTH_DELETE_ACCOUNT_DELAY, DateTime.Now.Day + DAY_DELETE_ACCOUNT_DELAY)) >= 0 : false;

        #endregion
        #region Public Methods
        public void AddAccount(Account toAdd)
        {
            switch(toAdd.Type)
            {
                case AccountType.Unverified:
                    if (AccountDeletationDelayExpired(toAdd.CreationDate))
                    {
                        UncheckedAccounts.Add(toAdd);
                        Logger<AccountManager>.Instance.Log($"{toAdd.Username} delete due non verified", LogLevel.Info);
                    }
                    else
                        Accounts.Add(toAdd.Username, toAdd);
                    break;
                default:
                    Accounts.Add(toAdd.Username, toAdd);
                    break;
            }
        }
        public void DeleteAccount(Account toDelete)
        {
            Accounts.Remove(toDelete.Username);
            Logger<AccountManager>.Instance.Log($"Account : {toDelete.Username} deleted", LogLevel.Info);
        }
        #endregion
        #region Getters
        public Account? GetAccount(string username) => Accounts.ContainsKey(username) ? Accounts[username] : null;
        public Account? GetAccountByMail(string mail) => Accounts.Keys.ToList().Find(username => Accounts[username].Email == mail) != null ? Accounts[Accounts.Keys.ToList().Find(username => Accounts[username].Email == mail)!] : null;
        public Account? GetAccount(TcpClient client) => Accounts.Keys.ToList().Find(username => Accounts[username].Client == client) != null ? Accounts[Accounts.Keys.ToList().Find(username => Accounts[username].Client == client)!] : null;
        public int AccountCount => Accounts.Count;
        public int UncheckedAccountCout => UncheckedAccounts.Count;
        #endregion
    }
}
