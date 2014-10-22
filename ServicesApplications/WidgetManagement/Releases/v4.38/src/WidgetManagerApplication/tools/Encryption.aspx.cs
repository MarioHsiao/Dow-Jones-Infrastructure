using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using EMG.widgets.ui.page;

namespace EMG.widgets.ui.tools
{
    /// <summary>
    /// Use the RNGCryptoServiceProvider class to generate a cryptographically strong random number.
    /// Choose an appropriate key size. The recommended key lengths are as follows:
    /// For SHA1, set the validationKey to 64 bytes (128 hexadecimal characters).
    /// For AES, set the decryptionKey to 32 bytes (64 hexadecimal characters).
    /// For 3DES, set the decryptionKey to 24 bytes (48 hexadecimal characters).
    /// <machineKey  
    /// validationKey="21F090935F6E49C2C797F69BBAAD8402ABD2EE0B667A8B44EA7DD4374267A75D7AD972A119482D15A4127461DB1DC347C1A63AE5F1CCFAACFF1B72A7F0A281B"           
    /// decryptionKey="ABAA84D7EC4BB56D75D217CECFFB9628809BDB8BF91CFCD64568A145BE59719F"
    /// validation="SHA1"
    /// decryption="AES"
    /// />
    /// </summary>
    public class Encryption : BasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            int SHA1_Length = 128;
            int AES_Length = 64;
            int DES_Length = 48;

            ContentMain.Controls.Add(new LiteralControl(string.Format("<div>{0}</div><div><pre>{1}</pre></div>", "SHA1", GetEncrytedKey(SHA1_Length))));
            ContentMain.Controls.Add(new LiteralControl(string.Format("<div>{0}</div><div><pre>{1}</pre></div>", "AES", GetEncrytedKey(AES_Length))));
            ContentMain.Controls.Add(new LiteralControl(string.Format("<div>{0}</div><div><pre>{1}</pre></div>", "3DES", GetEncrytedKey(DES_Length))));
            
        }

        /// <summary>
        /// Gets the encryted key.
        /// </summary>
        /// <param name="len">The len.</param>
        /// <returns></returns>
        private static string GetEncrytedKey(int len)
        {
            byte[] buff = new byte[len/2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < buff.Length; i++)
                sb.Append(string.Format("{0:X2}", buff[i]));
            return sb.ToString();
        }
    }
}