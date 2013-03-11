/* 
 * Author: Infosys
 * Date: Dec/12/2009
 * Purpose: Capturing error data to the ODS.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Diagnostics;
using System.Threading;
using System.Net;
using EMG.Tools.Session;
using EMG.Utility.OperationalData;
using System.Text.RegularExpressions;
using Factiva.BusinessLayerLogic.ODS;
using Factiva.BusinessLayerLogic.DataTransferObject;
using Factiva.BusinessLayerLogic.Exceptions;
using System.Globalization;
using EMG.Utility.OperationalData.ErrorData;
using Factiva.BusinessLayerLogic.Managers.V2_0;

namespace EMG.widgets.ui.ods
{
    public class ODSErrorCapture
    {
        /// <summary>
        ///  Singleton class  for capturing error data to the ODS
        /// </summary>
        private static readonly ODSErrorCapture instance = new ODSErrorCapture();

        private ODSErrorCapture()
        {

        }
        public static ODSErrorCapture GetInstance
        {
            get { return instance; }
        }
        /// <summary>
        /// ODS transaction for capturing error data to ODS
        /// </summary>
        /// <param name="code">error code</param>
        /// <param name="ex">exception</param> 
        public void RecordODOnError(string code, Exception ex)
        {
            try
            {
                //START - Infosys 04 May 2010: Moved inside try catch block
                ODSErrorOperationalDataCapture objODSErrorOperationalDataCapture = new ODSErrorOperationalDataCapture();
                HttpRequest request = HttpContext.Current.Request;
                //END - Infosys 04 May 2010: Moved inside try catch block

                Exception lastError = null;

                if (ex != null)
                {
                    lastError = ex;
                }
                else
                {
                    lastError = HttpContext.Current.Server.GetLastError();
                }

                //The code in the if block executes when there is an exception. This condition is satisfied i
                //almost all the cases.
                if (lastError != null)
                {
                    //if the exception has a baseexception, assign the exception as base exception.
                    Exception baseException = lastError.GetBaseException();

                    //Assigning the base exception to the exception object.
                    if (baseException != null)
                    {
                        lastError = baseException;
                    }

                    RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase
                        | RegexOptions.IgnorePatternWhitespace;

                    if (lastError.StackTrace != null)
                    {

                        //The stack trace is a large string containing the flow of the program.
                        //This string is a series of substrings in the format of 'at METHOD in FILE :line NUMBER'
                        //The below regular expression is used to match each substring in stack trace
                        Regex regex;
                        if (lastError.StackTrace.Contains(" in ") && lastError.StackTrace.Contains(":line"))
                        {
                            regex = new Regex(@"at\s+(?<method>[a-zA-Z\._]+).*\s+in\s+(?<file>.*):line\s+(?<lineNr>\d+)", regexOptions);
                        }
                        else
                        {
                            regex = new Regex(@"at\s+(?<method>[a-zA-Z\._]+).*", regexOptions);
                        }

                        //fetch the data from the stack trace and put all the substrings into a regex MatchCollection array
                        MatchCollection itemsArray = regex.Matches(lastError.StackTrace);

                        //The last item in the collection is the start point of the exception.
                        //Fetch the linenum, func name and file name from last element when the collection count
                        //is greater than 0.
                        //if collection count is 0, it means the exception is caused in the assembly.
                        if (itemsArray.Count > 0)
                        {
                            //fetch the last item of stack trace
                            string stackTrace = itemsArray[itemsArray.Count - 1].ToString();
                            stackTrace = stackTrace.Replace("\r\n", "");

                            //If there is no .pdb file available in the solution, the stacktrace doesnot contain linenumber
                            //and filename details in it.
                            //The code in the below IF block is executed if there is a pdb file available. This code check is 
                            //put so that no further code change will be required when we have a pdb file.
                            if (stackTrace.Contains(" in ") && stackTrace.Contains(":line"))
                            {
                                //Logic to find the file name in the string stack trace
                                //This logic is mere string manipulation based on the pattern of substrings in the stack trace
                                int fileNameStartIndex = stackTrace.IndexOf(" in ") + 4;//added 4 characters to get the index after " in "
                                int fileNameEndIndex = stackTrace.IndexOf(":line");
                                string fileName = stackTrace.Substring(fileNameStartIndex, fileNameEndIndex - fileNameStartIndex).Trim();

                                //Logic to find the line number in the last item of stack trace string
                                //This logic is mere string manipulation based on the pattern of substrings in the stack trace
                                int lineNumberStartIndex = fileNameEndIndex + 6;//added 6 characters to get the index after ":line "
                                string subStackTrace = stackTrace.Remove(0, lineNumberStartIndex);
                                string lineNumber = string.Empty;

                                //fetching the line number.
                                lineNumber = subStackTrace.Trim();

                                //Logic to find the method name in the last item of stack trace string
                                //This logic is mere string manipulation based on the pattern of substrings in the stack trace
                                string methodName = stackTrace.Substring(0, stackTrace.IndexOf(" in "));
                                methodName = methodName.Substring(methodName.LastIndexOf('.') + 1);

                                //Assigning the file level information
                                objODSErrorOperationalDataCapture.AppFileLineNumber = lineNumber;
                                objODSErrorOperationalDataCapture.AppFileName = fileName;
                                objODSErrorOperationalDataCapture.AppFunctionName = methodName;
                            }
                            //In general the release mode doesn't have pdb file. So only Function Name is available.
                            else
                            {
                                //Logic to find the method name in the last item of stack trace string by removing
                                //first three characters 'at '
                                string methodName = stackTrace.Substring(3);

                                //Assigning the file level information
                                objODSErrorOperationalDataCapture.AppFunctionName = methodName;

                                //Get the method name from the target site
                                //objODSErrorOperationalDataCapture.AppFunctionName = lastError.TargetSite.ReflectedType.ToString() + "." + lastError.TargetSite.Name.ToString();
                            }

                            //fetch each item from the stack trace append to next item with a ';' in between
                            //This done because the regular stack trace is spanning to multiple lines, which
                            //is causing a problem in the ODS
                            string completeStackTrace = string.Empty;
                            
                            //START - Infosys 10 May 2010: modified the procedure to capture stacktrace
                            //replace carriage return and newline characters and append a ‘;’ at the end to mark the end of the sentence.
                            completeStackTrace = lastError.StackTrace.Replace("\r\n", ";").Replace("\n", ";").Replace("\r", ";");
                            //incase there are any occurences of ';;' replace it with ';'
                            completeStackTrace = completeStackTrace.Replace(";;", ";");
                            //END - Infosys 10 May 2010: modified the procedure to capture stacktrace

                            objODSErrorOperationalDataCapture.StackTrace = completeStackTrace;
                        }
                    }

                    //START - Infosys 10 May 2010: modified the procedure to capture error description
                    //Fetching the error description
                    if (!String.IsNullOrEmpty(lastError.Message))
                    {
                        string errorDesc = lastError.Message.Replace("\r\n", ";").Replace("\n", ";").Replace("\r", ";");
                        objODSErrorOperationalDataCapture.ErrorDesc = errorDesc.Replace(";;", ";");
                    }
                    //END - Infosys 10 May 2010: modified the procedure to capture error description
                    
                    //// Values  extracted  from Response Control Data and populated in Factiva exception Object
                    Type exceptionType = lastError.GetType();
                    //The function level details are available if the exception type is of FactivaException
                    if (exceptionType == typeof(FactivaBusinessLogicException))
                    {
                        FactivaBusinessLogicException factExp = (FactivaBusinessLogicException)lastError;
                        if (!String.IsNullOrEmpty(factExp.FunctionCode))
                        {
                            objODSErrorOperationalDataCapture.OcpFunctionCode = factExp.FunctionCode;
                        }
                        if (!String.IsNullOrEmpty(factExp.TransactionName))
                        {
                            objODSErrorOperationalDataCapture.GtwTranName = factExp.TransactionName;
                        }
                        if (!String.IsNullOrEmpty(factExp.TransactionType))
                        {
                            objODSErrorOperationalDataCapture.BackEndTranName = factExp.TransactionType;
                        }
                        if (!String.IsNullOrEmpty(factExp.RequestTypeNameSpace))
                        {
                            objODSErrorOperationalDataCapture.GtwTranNameSpace = factExp.RequestTypeNameSpace;
                        }
                    }
                }
                //The else condition is executed when there is no exception object available. 
                //Some methods are handling errors using handle error. In scenario's where only the code is 
                //passed, this condition helps in fetching the details of the method.
                else
                {
                    //This logic records the ODS when an error occurs
                    StackTrace trace = new StackTrace(Thread.CurrentThread, true);
                    StackFrame[] frames = trace.GetFrames();

                    //count of the frames in the stack
                    int frameCount = 0;
                    //Frame index for the method calling the first handle error method
                    int heFrameIndex = 0;

                    foreach (StackFrame frame in frames)
                    {
                        //if the method name is handleerror then assign the frameCount to heFrameIndex
                        //This step is done to prevent from recording system calls and record only handle error method call.
                        if (frame.GetFileName() != null)
                        {
                            heFrameIndex = frameCount;
                        }
                        //foreach frame increment the counter
                        frameCount++;
                    }
                    //fetch the method details from the stack
                    if (trace.GetFrame(heFrameIndex).GetFileLineNumber() != null)
                    {
                        objODSErrorOperationalDataCapture.AppFileLineNumber = trace.GetFrame(heFrameIndex).GetFileLineNumber().ToString();
                    }
                    if (trace.GetFrame(heFrameIndex).GetFileName() != null)
                    {
                        objODSErrorOperationalDataCapture.AppFileName = trace.GetFrame(heFrameIndex).GetFileName().ToString();
                    }
                    if (trace.GetFrame(heFrameIndex).GetMethod() != null)
                    {
                        objODSErrorOperationalDataCapture.AppFunctionName = trace.GetFrame(heFrameIndex).GetMethod().ToString();
                    }
                }

                objODSErrorOperationalDataCapture.ErrorCode= code;
                objODSErrorOperationalDataCapture.RequestorIP = request.UserHostAddress;
                //objODSErrorOperationalDataCapture.webServer = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();

                objODSErrorOperationalDataCapture.WebServerName = Dns.GetHostName().ToString();

                //fetch the error log time , convert it to UTC format and format the date  to be similar to the
                //format of Request and Response Date/Time available from the Session layer
                DateTime errorLogTime = DateTime.Now.ToUniversalTime();
                CultureInfo cInfo = CultureInfo.InvariantCulture;
                objODSErrorOperationalDataCapture.ErrorLogTime = errorLogTime.ToString("yyyyMMddHHmmss.ffff", cInfo);

                //Fetch the count of QueryString, Cookies and Form from the http context
                objODSErrorOperationalDataCapture.ReqQueryStringCnt = HttpContext.Current.Request.QueryString.Count.ToString();
                objODSErrorOperationalDataCapture.ReqCookiesCnt = HttpContext.Current.Request.Cookies.Count.ToString();
                objODSErrorOperationalDataCapture.FormCount = HttpContext.Current.Request.Form.Count.ToString();

                //START - Infosys 04 May 2010: Capture RawUrl and Form Method
                objODSErrorOperationalDataCapture.AppFileName = request.RawUrl;
                objODSErrorOperationalDataCapture.FormMethod = request.HttpMethod;
                //END - Infosys 04 May 2010: Capture RawUrl and Form Method

                //Passing the OD transaction name
                objODSErrorOperationalDataCapture.ODSTranName = EMG.Utility.OperationalData.Asset.ODSTranName.UIErrorInfo.ToString();

                //call the RecordOperationalData method to record the error data to the ODS.
                RecordOperationalData(objODSErrorOperationalDataCapture);
            }
            catch (Exception recordException)
            {
                //Do nothing
            }
        }

        /// <summary>
        /// This method is used to record operational data to the ODS
        /// </summary>
        /// <param name="objODSErrorOperationalDataCapture">error data</param>
        private void RecordOperationalData(ODSErrorOperationalDataCapture objODSErrorOperationalDataCapture)
        {
            //OperationalDataDTO odDTO = new OperationalDataDTO();
            OperationalDataDTO _odsDTO = new OperationalDataDTO();
            _odsDTO.interfaceLanguage = SessionData.Instance().InterfaceLanguage;
            UserInterfaceAndDeviceOperationalData _commonOD = OperationalDataFactory.GetCommonOperationalData(_odsDTO);
            objODSErrorOperationalDataCapture.SetComponent(_commonOD);
            RecordData(objODSErrorOperationalDataCapture);
        }

        /// <summary>
        /// Makes a call to record data to ODS 
        /// </summary>
        /// <param name="keyValues">error data key value pair</param>
        private void RecordData(AbstractOperationalData objODSErrorOperationalDataCapture)
        {
            //new OperationalDataService().Send(keyValues);

           OperationalDataManager manager =
                    new OperationalDataManager(SessionData.Instance().SessionBasedControlData,
                              SessionData.Instance().InterfaceLanguage);
           manager.RecordOperationalData(objODSErrorOperationalDataCapture);
            
        }
    }
}
