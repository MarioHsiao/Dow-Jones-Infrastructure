// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranslationManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Properties;
using DowJones.Utilities.Core;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers.AutomatedTranslation.Core;
using DowJones.Utilities.Managers.AutomatedTranslation.Providers.Core;
using DowJones.Utilities.Managers.AutomatedTranslation.Providers.LanguageWeaver;
using DowJones.Utilities.Mapping.AutomatedTranslation;
using Factiva.Gateway.Utils.V1_0;
using log4net;

namespace DowJones.Utilities.Managers.AutomatedTranslation
{
    /// <summary>
    /// The translation manager.
    /// </summary>
    public class TranslationManager : AbstractAggregationManager, ITranslationManager
    {
        /// <summary>
        /// The s_ log.
        /// </summary>
        private static readonly ILog s_Log = LogManager.GetLogger(typeof(TranslationManager));

        /// <summary>
        /// The s_ translation provider.
        /// </summary>
        private static readonly ITranslationProvider s_TranslationProvider = new TranslationProvider();

        /// <summary>
        /// The _sync object.
        /// </summary>
        private static readonly object _syncObject = new object();

        /// <summary>
        /// The s_ supported language pairs.
        /// </summary>
        private static List<LanguagePair> s_SupportedLanguagePairs;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationManager"/> class.
        /// </summary>
        /// <param name="sessionId">
        /// The session id.
        /// </param>
        /// <param name="clientTypeCode">
        /// The client type code.
        /// </param>
        /// <param name="accessPointCode">
        /// The access point code.
        /// </param>
        /// <param name="interfaceLangugage">
        /// The interface langugage.
        /// </param>
        public TranslationManager(string sessionId, string clientTypeCode, string accessPointCode, string interfaceLangugage)
            : base(sessionId, clientTypeCode, accessPointCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationManager"/> class.
        /// </summary>
        /// <param name="controlData">
        /// The control data.
        /// </param>
        /// <param name="interfaceLanguage">
        /// The interface language.
        /// </param>
        public TranslationManager(ControlData controlData, string interfaceLanguage)
            : base(controlData)
        {
        }

        /// <summary>
        /// Gets Log.
        /// </summary>
        protected override ILog Log
        {
            get { return s_Log; }
        }

        #region ITranslationManager Members

        /// <summary>
        /// This method is not implemented. Please use the source code method
        /// </summary>
        /// <param name="ipCode">The IP Code.</param>
        /// <returns>
        /// 	<c>true</c> if [is IP allowed for automated translation]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsIPAllowedForAutomatedTranslation(string ipCode)
        {
            // bool isAllowed;
            throw new NotImplementedException("This method is not implemented. Please use the source code method");

// return isAllowed;
        }

        /// <summary>
        /// Determines whether [is source allowed for automated translation].
        /// </summary>
        /// <param name="sourceCode">The source Code.</param>
        /// <returns>
        /// 	<c>true</c> if [is source allowed for automated translation]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSourceAllowedForAutomatedTranslation(string sourceCode)
        {
            var xlateSrcMapper = new TranslationSourceWhiteListMapper();

            var isAllowed = xlateSrcMapper.IsSourceCodeAllowedForTranslation(sourceCode);

            return isAllowed;
        }

        /// <summary>
        /// The translate.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ITranslateResult Translate(ITranslateRequest request)
        {
            ValidateRequest(request);

            var sourceItem = request.GetTranslateItem();

            var result = s_TranslationProvider.Translate(
                sourceItem.GetFragments(), sourceItem.GetFormat(), sourceItem.GetLanguage(), request.TargetLanguage);

            var status = GetTranslateStatus(result);
            var fragments = s_TranslationProvider.GetTranslatedFragments(result);

            var targetItem = (ITranslateItem) sourceItem.Clone();
            targetItem.SetFragments(fragments);

            var translateResult = new TranslateResult(status, targetItem);

            return translateResult;
        }

        /// <summary>
        /// The begin translate task.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ITranslateTask BeginTranslateTask(ITranslateRequest request)
        {
            ValidateRequest(request);

            var sourceItem = request.GetTranslateItem();

            var task = s_TranslationProvider.BeginTranslateTask(
                sourceItem.GetFragments(), sourceItem.GetFormat(), sourceItem.GetLanguage(), request.TargetLanguage);

            return new TranslateTask(task, request.TargetLanguage, sourceItem);
        }

        /// <summary>
        /// The query translate task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public ITranslateResult QueryTranslateTask(ITranslateTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            var result = s_TranslationProvider.QueryTranslateTask(task.Identifier);

            var status = GetTranslateStatus(result);
            var fragments = s_TranslationProvider.GetTranslatedFragments(result);

            var targetItem = (ITranslateItem) task.SourceItem.Clone();
            targetItem.SetFragments(fragments);

            return new TranslateResult(status, targetItem);
        }

        /// <summary>
        /// The end translate task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public ITranslateResult EndTranslateTask(ITranslateTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            var result = s_TranslationProvider.EndTranslateTask(task.Identifier);

            var status = GetTranslateStatus(result);
            var fragments = s_TranslationProvider.GetTranslatedFragments(result);

            var targetItem = (ITranslateItem) task.SourceItem.Clone();
            targetItem.SetFragments(fragments);

            return new TranslateResult(status, targetItem);
        }

        /// <summary>
        /// The get supported target languages.
        /// </summary>
        /// <param name="sourceLanguage">The source language.</param>
        /// <returns></returns>
        /// <exception cref="DowJonesUtilitiesException">
        /// </exception>
        public ContentLanguage[] GetSupportedTargetLanguages(ContentLanguage sourceLanguage)
        {
            /* NN - Don't need to check for session here
            if (!IsValidSession())
            {
                throw new EmgUtilitiesException(EmgUtilitiesException.ERROR_INVALID_SESSION_LONG);
            }
             * */
            if (!IsSupportedSourceLanguage(sourceLanguage))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.NotSupportedSourceLanguageException);
            }
            return (from pair in GetSupportedLanguagePairs() where sourceLanguage == pair.FromLanguage select pair.IntoLanguage).ToArray();
        }

        #endregion

        /// <summary>
        /// The validate request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="DowJonesUtilitiesException">
        /// </exception>
        private static void ValidateRequest(ITranslateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            /* NN - Don't need to check for session here
            if (!IsValidSession())
            {
                throw new EmgUtilitiesException(EmgUtilitiesException.ERROR_INVALID_SESSION_LONG);
            }
            */
            var sourceLanguage = request.GetTranslateItem().GetLanguage();
            var targetLangauge = request.TargetLanguage;

            if (!IsSupportedSourceLanguage(sourceLanguage))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.NotSupportedSourceLanguageException);
            }

            if (!IsSupportedTargetLanguage(sourceLanguage, targetLangauge))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.NotSupportedTargetLanguageException);
            }
        }

        /// <summary>
        /// The get translate status.
        /// </summary>
        /// <param name="translateResult">
        /// The translate result.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="DowJonesUtilitiesException">
        /// </exception>
        private static TranslateStatus GetTranslateStatus(object translateResult)
        {
            if (s_TranslationProvider.IsDone(translateResult))
                return TranslateStatus.Done;

            if (s_TranslationProvider.IsTranslating(translateResult))
                return TranslateStatus.Running;

            if (s_TranslationProvider.IsFailed(translateResult))
                return TranslateStatus.Failed;

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.UnknownTranslationStatusException);
        }

        // private bool IsValidSession()
        // {
        // ValidateSessionIdRequest request = new ValidateSessionIdRequest();

        // ServiceResponse response = MembershipService.ValidateSessionId(ControlData, request);

        // return (response != null && response.rc == 0);
        // }

        /// <summary>
        /// The is supported source language.
        /// </summary>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <returns>
        /// The is supported source language.
        /// </returns>
        private static bool IsSupportedSourceLanguage(ContentLanguage language)
        {
            return GetSupportedLanguagePairs().Any(pair => language == pair.FromLanguage);
        }

        /// <summary>
        /// The is supported target language.
        /// </summary>
        /// <param name="sourceLanguage">
        /// The source language.
        /// </param>
        /// <param name="targetLanguage">
        /// The target language.
        /// </param>
        /// <returns>
        /// The is supported target language.
        /// </returns>
        private static bool IsSupportedTargetLanguage(ContentLanguage sourceLanguage, ContentLanguage targetLanguage)
        {
            return GetSupportedLanguagePairs().Where(pair => sourceLanguage == pair.FromLanguage).Any(pair => targetLanguage == pair.IntoLanguage);
        }

        /// <summary>
        /// The get supported language pairs.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="DowJonesUtilitiesException">
        /// </exception>
        private static IEnumerable<LanguagePair> GetSupportedLanguagePairs()
        {
            if (s_SupportedLanguagePairs == null)
            {
                lock (_syncObject)
                {
                    var temp = Settings.Default.AutomatedTranslationSupportedLaguagePairs;

                    var languagePairs = temp.Split(',');

                    if (languagePairs.Length == 0)
                    {
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ConfigInvalidException);
                    }

                    s_SupportedLanguagePairs = new List<LanguagePair>();
                    foreach (var item in languagePairs)
                    {
                        var languages = item.Split('.');

                        if (languages.Length < 2)
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ConfigInvalidException);
                        }

                        temp = languages[0].Trim();
                        ContentLanguage? fromLanguage = null;
                        if (Enum.IsDefined(typeof (ContentLanguage), temp))
                        {
                            fromLanguage = (ContentLanguage) Enum.Parse(typeof (ContentLanguage), temp);
                        }

                        temp = languages[1].Trim();
                        ContentLanguage? intoLanguage = null;
                        if (Enum.IsDefined(typeof (ContentLanguage), temp))
                        {
                            intoLanguage = (ContentLanguage) Enum.Parse(typeof (ContentLanguage), temp);
                        }

                        if (!fromLanguage.HasValue || !intoLanguage.HasValue)
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.ConfigInvalidException);
                        }

                        var languagePair = new LanguagePair(fromLanguage.Value, intoLanguage.Value);

                        if (!s_SupportedLanguagePairs.Contains(languagePair))
                        {
                            s_SupportedLanguagePairs.Add(languagePair);
                        }
                    }
                }
            }

            return s_SupportedLanguagePairs;
        }
    }
}