using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DowJones.Security
{
    public class ImpersonationUtil
    {
        public enum Provider
        {
            Default = 0,
            Winnt35 = 1,
            Winnt40 = 2,
            Winnt50 = 3,
        }

        public enum Logon
        {
            Network = 3,
            Batch = 4,
            Service = 5,
            Unlock = 7,
            NetworkCleartext = 8,
            NewCredentials = 9
        }

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

        public bool ImpersonateValidUser(String userName, String domain, String password, Logon logon, Provider provider)
        {
            var token = IntPtr.Zero;
            var tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, (int)logon, (int)provider, ref token) != 0)
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

        public bool ImpersonateValidUser(String userName, String domain, String password)
        {
            return ImpersonateValidUser(userName, domain, password, Logon.NewCredentials, Provider.Default);
        }

        public void UndoImpersonation()
        {
            _impersonationContext.Undo();
            IsUserImpersonated = false;
        }
    }
}
