using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DowJones.Security
{
    public class ImpersonationUtil
    {
        public const int Logon32LogonInteractive = 2;
        public const int Logon32LogonNetwork = 3;
        public const int Logon32LogonBatch = 4;
        public const int Logon32LogonService = 5;
        public const int Logon32LogonUnlock = 7;
        public const int Logon32LogonNetworkCleartext = 8;
        public const int Logon32LogonNewCredentials = 9;
        public const int Logon32ProviderDefault = 0;
        public const int Logon32ProviderWinnt35 = 1;
        public const int Logon32ProviderWinnt40 = 2;
        public const int Logon32ProviderWinnt50 = 3;

        WindowsImpersonationContext _impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        public bool IsUserImpersonated { get; private set; }

        public bool ImpersonateValidUser(String userName, String domain, String password)
        {
            var token = IntPtr.Zero;
            var tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, Logon32LogonNewCredentials, Logon32ProviderDefault, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        var tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        _impersonationContext = tempWindowsIdentity.Impersonate();
                        if (_impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            IsUserImpersonated = true;
                            return true;
                        }
                    }
                }
            }

            if (token != IntPtr.Zero)
            {
                CloseHandle(token);
            }

            if (tokenDuplicate != IntPtr.Zero)
            {
                CloseHandle(tokenDuplicate);
            }

            return false;
        }

        public void UndoImpersonation()
        {
            _impersonationContext.Undo();
            IsUserImpersonated = false;
        }
    }
}
