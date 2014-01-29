using System;
using System.IO;
using DowJones.Exceptions;
using DowJones.Security;

namespace DowJones.Web.Handlers.Items
{

    #region FileManager
    /// <summary>
    /// FileManager Utility
    /// </summary>
    public class FileManager
    {
        #region Local members
        private ImpersonationUtil _util;
        private readonly string _domain;
        private readonly string _user;
        private readonly string _password;


        #endregion
        public FileManager(string domain, string user, string password)
        {
            _domain = domain;
            _user = user;
            _password = password;

        }
        #region Methods

        public string GetRelativePath(string userId, string userNamespace, string accountId, string fileName)
        {
            return GetRelativePath(accountId, userId, userNamespace, "img", fileName);
        }

        public string GetRelativePath(string userId, string userNamespace, string accountId, string fileType, string fileName)
        {
            return string.Format("{0}\\{1}\\{2}\\{3}\\{4}", accountId, userId, userNamespace, fileType, fileName);
        }

        public string GetFullPath(string basePath, string relativePath)
        {
            return Path.Combine(basePath, relativePath);
        }


        /// <summary>
        /// save the file
        /// </summary>
        /// <param name="filePath">Full file path</param>
        /// <param name="fileContent"> binary array - file content</param>
        public void SaveFile(string filePath, byte[] fileContent)
        {
            try
            {
                _util = new ImpersonationUtil();

                if (_util.ImpersonateValidUser(_user, _domain, _password))
                {
                    var directoryName = (new FileInfo(filePath)).DirectoryName;

                    if (directoryName != null && Directory.Exists(directoryName) == false)
                    {
                       Directory.CreateDirectory(directoryName);
                    }

                    using (var binaryFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        using (var writer = new BinaryWriter(binaryFile))
                        {
                            writer.Write(fileContent);
                            writer.Close();
                        }
                        binaryFile.Close();
                    }
                }
                else
                {
                    throw new ItemHandlerException("Impersonation failed during SaveFile.");
                }
            }
            catch (Exception ex)
            {
                throw new FileManagerException(ex.Message, ex);
            }
            finally
            {
                if (_util.IsUserImpersonated)
                    _util.UndoImpersonation();
            }
        }

        /// <summary>
        /// read the file content
        /// </summary>
        /// <param name="getFileName"> file name</param>
        /// <returns> binary array - content of the file </returns>
        public byte[] GetFile(string getFileName)
        {
            try
            {
                _util = new ImpersonationUtil();
                if (_util.ImpersonateValidUser(_user, _domain, _password))
                {

                    if (File.Exists(getFileName) == false)
                    {
                        throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerFileNotFound, "Get file: failed");
                    }

                    using (var binaryFile = new FileStream(getFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var fileContent = new byte[binaryFile.Length];
                        binaryFile.Read(fileContent, 0, fileContent.Length);
                        binaryFile.Close();
                        return fileContent;
                    }

                }
                else
                {
                    throw new ItemHandlerException("Impersonation failed during GetFile.");
                }

            }
            finally
            {
                if (_util.IsUserImpersonated)
                    _util.UndoImpersonation();
            }
        }

        /// <summary>
        /// delete the file 
        /// </summary>
        /// <param name="filePath">Full file path.</param>
        public void DeleteFile(string filePath)
        {
            try
            {
                _util = new ImpersonationUtil();
                if (_util.ImpersonateValidUser(_user, _domain, _password))
                {
                    if (File.Exists(filePath) == false)
                    {
                        throw new ItemHandlerException(DowJonesUtilitiesException.ItemHandlerFileNotFound, "Delete file: failed");
                    }
                    File.Delete(filePath);

                }
                else
                {
                    throw new ItemHandlerException("Impersonation failed during Delete File.");
                }
            }
            finally
            {
                if (_util.IsUserImpersonated)
                    _util.UndoImpersonation();
            }
        }

        #endregion

    }

    #endregion

    #region FileManagerException

    /// <summary>
    /// Exception for FileManager
    /// </summary>
    public class FileManagerException : ApplicationException
    {

        public FileManagerException()
        {
        }

        public FileManagerException(string message)
            : base(message)
        {

        }
        public FileManagerException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    #endregion

}
