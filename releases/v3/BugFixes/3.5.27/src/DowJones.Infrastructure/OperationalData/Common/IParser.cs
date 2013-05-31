using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using EMG.Tools.Security;

namespace EMG.Utility.OperationalData
{
    /// <summary>
    /// 
    /// </summary>
    public interface IParser
    {
        string Encrypt(IDictionary<string, string> list);

        IDictionary<string, string> Decrypt(string str);

        string VersionId { get; }
    }

    internal class ParserFactory
    {
        public const string PLAIN_NVP = "V1";
        public const string ENCRYPTED_NVP = "V2";


        public static IParser GetParser(string versionId)
        {
            switch (versionId)
            {
                case PLAIN_NVP:
                    return new OperationalDataParser();
                case ENCRYPTED_NVP:
                    return new EncryptedOperationalDataParser();
                default:
                    throw new NotSupportedException(versionId);
            }
        }
    }

    internal class ParserDecorator : IParser
    {
        private readonly IParser component = null;

        public ParserDecorator(IParser component)
        {
            this.component = component;
        }

        public string Encrypt(IDictionary<string, string> list)
        {
            string str = component.Encrypt(list);
            return String.Format("{0}{1}", component.VersionId, str);
        }

        public IDictionary<string, string> Decrypt(string str)
        {
            if (str.StartsWith(component.VersionId))
            {
                str = str.Substring(component.VersionId.Length);
            }
            return component.Decrypt(str);
        }

        public string VersionId
        {
            get { throw new NotImplementedException(); }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class EncryptedOperationalDataParser : IParser
    {
        private static readonly string OP_ENC_KEY = "O79ration68ATA";

        #region IParser Members

        public string Encrypt(IDictionary<string, string> list)
        {
            Encryption encryptor = new Encryption();
            NameValueCollection nvp = new NameValueCollection();
            foreach (KeyValuePair<string, string> keyValuePair in list)
            {
                nvp.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return encryptor.Encrypt(nvp, OP_ENC_KEY);
        }

        public IDictionary<string, string> Decrypt(string str)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            if (!string.IsNullOrEmpty(str))
            {
                Encryption encryptor = new Encryption();
                NameValueCollection nvp = encryptor.Decrypt(str, OP_ENC_KEY);
                foreach (string s in nvp)
                {
                    switch(s)
                    {
                        // Implemented for backward compatibility aopt/dopt
                        case "dopt":
                            dictionary["aopt"] = nvp[s]; 
                            break;
                        default:
                            dictionary[s] = nvp[s];
                            break;
                    } 
                }
            }
            return dictionary;
        }

        public string VersionId
        {
            get { return ParserFactory.ENCRYPTED_NVP; }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class OperationalDataParser : IParser
    {
        /// <summary>
        /// ToDo: Find other way to build string without using separator!
        /// ToDo: Or
        /// ToDo: Escape these chars
        /// </summary>
        private static readonly char NVP_SPILTER = '~';

        public static char VALUE_SPLITER = '|';

        #region IParser Members

        public string Encrypt(IDictionary<string, string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in list)
            {
                sb.Append(pair.Key).Append(VALUE_SPLITER).Append(pair.Value).Append(NVP_SPILTER);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public IDictionary<string, string> Decrypt(string odsString)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            if (!string.IsNullOrEmpty(odsString))
            {
                string[] nvps = odsString.Split(new char[] {NVP_SPILTER});
                foreach (string s in nvps)
                {
                    if (s.IndexOf(VALUE_SPLITER) > 0)
                    {
                        string[] nvp = s.Split(new char[] {VALUE_SPLITER});
                        switch(nvp[0])
                        {
                            // Implemented for backward compatibility aopt/dopt
                            case "dopt":
                                dictionary["aopt"] =  nvp[1];
                                break;
                            default:
                                dictionary[nvp[0]] = nvp[1];
                                break;
                        }
                        
                    }
                }
            }
            return dictionary;
        }

        public string VersionId
        {
            get { return ParserFactory.PLAIN_NVP; }
        }

        #endregion
    }
}