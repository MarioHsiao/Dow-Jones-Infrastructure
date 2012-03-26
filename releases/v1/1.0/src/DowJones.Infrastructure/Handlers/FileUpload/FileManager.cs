using System;
using System.IO;


namespace EMG.Utility.Handlers.FileUpload
{

    #region FileManager
    /// <summary>
    /// FileManager Utility
    /// </summary>
    public class FileManager
    {
        #region Local members


        #endregion

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

                string directoryName = (new FileInfo(filePath)).DirectoryName;

                if (Directory.Exists(directoryName) == false)
                {
                    Directory.CreateDirectory(directoryName);
                }

                using (FileStream binaryFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(binaryFile))
                    {
                        writer.Write(fileContent);
                        writer.Close();
                    }
                    binaryFile.Close();
                }

            }
            catch (Exception ex)
            {
                throw new FileManagerException(ex.Message, ex);
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
                //string getFileName = filePath + @"\" + fileName;

                if (File.Exists(getFileName) == false)
                    throw new FileManagerException("File does not exist");

                using (FileStream binaryFile = new FileStream(getFileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] fileContent = new byte[binaryFile.Length];
                    binaryFile.Read(fileContent, 0, fileContent.Length);
                    binaryFile.Close();
                    return fileContent;
                }

            }
            catch (Exception ex)
            {
                throw new FileManagerException(ex.Message, ex);
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
                if (File.Exists(filePath) == false)
                    throw new FileManagerException("File does not exist");

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                throw new FileManagerException(ex.Message, ex);
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

        public FileManagerException(string message): base(message)
        {

        }
        public FileManagerException(string message, Exception inner) : base(message, inner)
        {

        }
    }

    #endregion

}
