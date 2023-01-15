using ApirooServer.Utils.MemoryBlock;
using ApirooServer.Utils;
using ApirooServer.AccountManagement;
using Console = ConsoleBetter.Console;

namespace ApirooServer
{
    public class FileManager : Singleton<FileManager>
    {
        #region Privates Meembers
        private static string GlobalDirPath = string.Empty;
        private static string UsersDirPath = string.Empty;
        private static string LogsDirPath = string.Empty;
        #endregion

        public FileManager() : base()
        {
            new Logger<FileManager>();
            Init();
        }
        #region Methods
        private void Init()
        {
            BuildStaticsPath();
            if (!Directory.Exists(GlobalDirPath))
                Directory.CreateDirectory(GlobalDirPath);
            if (!Directory.Exists(UsersDirPath))
                Directory.CreateDirectory(UsersDirPath);
            if (!Directory.Exists(LogsDirPath))
                Directory.CreateDirectory(LogsDirPath);
            GetAccountsFiles();
        }
        private void BuildStaticsPath()
        {
            GlobalDirPath = $@"{GetSysRacine()}\ApirooServer";
            UsersDirPath = $@"{GlobalDirPath}\users";
            LogsDirPath = $@"{GlobalDirPath}\logs";
        }
        private void GetAccountsFiles()
        {
            long totalBytesCount = 0;
            foreach(string AccountFilePath in Directory.GetFiles(UsersDirPath))
            {
                totalBytesCount += new FileInfo(AccountFilePath).Length;
                StreamReader Reader = new StreamReader(AccountFilePath);
                BasicAccountMemoryBlock BasicInfosBlock = new BasicAccountMemoryBlock();
                BasicInfosBlock.Read(ref Reader);
                Reader.Close();
                Reader.Dispose();
                AccountManager.Instance.AddAccount(new Account(BasicInfosBlock));
                Logger<FileManager>.Instance.Log($"{BasicInfosBlock.GetSectionByKey("username").value} account initialized", LogLevel.Info);
            }
            Logger<FileManager>.Instance.Log($"{AccountManager.Instance.AccountCount} accounts initialized | {AccountManager.Instance.UncheckedAccountCout} unchecked accounts | ({totalBytesCount} bytes)", LogLevel.Info);
        }
        private string GetSysRacine() => $@"{Environment.SystemDirectory.Split('\\')[0]}";
        #endregion
    }
}
