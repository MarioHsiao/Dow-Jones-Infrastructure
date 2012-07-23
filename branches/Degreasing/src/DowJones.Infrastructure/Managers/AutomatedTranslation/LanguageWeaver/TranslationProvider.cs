using System;
using System.Collections.Generic;
using System.Text;
using EMG.Utility.Core;
using EMG.Utility.Exceptions;
using EMG.Utility.Managers.AutomatedTranslation.Core;

namespace EMG.Utility.Managers.AutomatedTranslation.LanguageWeaver
{
    class TranslationProvider : ITranslationProvider
    {
        public object Translate(string[] fragments, TextFormat format, ContentLanguage sourceLanguage, ContentLanguage targetLanguage)
        {
            if (fragments == null || fragments.Length == 0)
            {
                throw new ArgumentNullException("fragments");
            }

            string sourceIso639Language;
            if(!LanguageMapper.TryConvertToIso639Language(sourceLanguage, out sourceIso639Language))
            {
                throw new EmgUtilitiesException("Unsupported langauge");
            }

            string targetIso639Language;
            if(!LanguageMapper.TryConvertToIso639Language(sourceLanguage, out targetIso639Language))
            {
                throw new EmgUtilitiesException("Unsupported langauge");
            }

            LanguagePair pair = LanguagePairs.Get(sourceIso639Language, targetIso639Language);
            if (pair == null)
            {
                throw new EmgUtilitiesException("Unsupported language pair");
            }

            string sourceText = GetText(fragments);

            TranslationJob job = ServiceWrapper.SubmitNonBlockingTranslationRequest(sourceText, format, pair);

            string status = string.Empty;
            TranslationResult result = null;
            while(status == "Running")
            {
                result = ServiceWrapper.QueryNonBlockingTranslationRequest(job);
                status = result.Status;
            }

            return result;
        }

        public object BeginTranslateTask(string[] fragments, TextFormat format, ContentLanguage sourceLanguage, ContentLanguage targetLanguage)
        {
            if(fragments == null  || fragments.Length == 0)
            {
                throw new ArgumentNullException("fragments");
            }

            string sourceIso639Language;
            if (!LanguageMapper.TryConvertToIso639Language(sourceLanguage, out sourceIso639Language))
            {
                throw new EmgUtilitiesException("Unsupported langauge");
            }

            string targetIso639Language;
            if (!LanguageMapper.TryConvertToIso639Language(sourceLanguage, out targetIso639Language))
            {
                throw new EmgUtilitiesException("Unsupported langauge");
            }

            LanguagePair pair = LanguagePairs.Get(sourceIso639Language, targetIso639Language);
            if (pair == null)
            {
                throw new EmgUtilitiesException("Unsupported language pair");
            }

            string sourceText = GetText(fragments);

            return ServiceWrapper.SubmitNonBlockingTranslationRequest(sourceText, format, pair);            
        }

        public object QueryTranslateTask(object task)
        {
            if(task == null)
            {
                throw new ArgumentNullException("task");
            }

            TranslationJob job = (TranslationJob)task;

            return ServiceWrapper.QueryNonBlockingTranslationRequest(job);
        }

        public object EndTranslateTask(object task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            TranslationJob job = (TranslationJob)task;

            string status = string.Empty;
            TranslationResult result = null;
            while (status == "Running")
            {
                result = ServiceWrapper.QueryNonBlockingTranslationRequest(job);
                status = result.Status;
            }

            return result;
        }

        public ContentLanguage[] GetSupportedTargetLanguages(ContentLanguage sourceContentLanguage)
        {
            string sourceIso639Language;

            if (!LanguageMapper.TryConvertToIso639Language(sourceContentLanguage, out sourceIso639Language))
            {
                throw new EmgUtilitiesException("Unsupported langauge");
            }

            List<ContentLanguage> targetLanguages = new List<ContentLanguage>();

            foreach (LanguagePair pair in LanguagePairs)
            {
                if (pair.FromLanguage != sourceIso639Language)
                    continue;

                ContentLanguage? intoContentLanguage;
                if (!LanguageMapper.TryConvertToContentLanguage(pair.IntoLanguage, out intoContentLanguage))
                    continue;

                targetLanguages.Add(intoContentLanguage.Value);
            }

            return targetLanguages.ToArray();
        }

        public string[] GetTranslatedFragments(object result)
        {
            if(result == null)
            {
                throw new ArgumentNullException("result");
            }

            TranslationResult temp = (TranslationResult)result;
            
            return GetFragments(temp.TranslatedText);
        }

        public bool IsRunning(object result)
        {
            if(result == null)
            {
                throw new ArgumentNullException("result");
            }

            TranslationResult temp = (TranslationResult)result;

            return temp.Status == "Running";
        }

        public bool IsDone(object result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            TranslationResult temp = (TranslationResult)result;

            return temp.Status == "Done";
        }

        public bool IsFailed(object result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            TranslationResult temp = (TranslationResult)result;

            return temp.Status == "Failed";
        }

        private static string GetText(IEnumerable<string> fragments)
        {
            StringBuilder textBuilder = new StringBuilder();

            foreach (string fragment in fragments)
            {
                textBuilder.Append(s_StartFragmentSeparator);
                textBuilder.Append(fragment);
                textBuilder.Append(s_EndFragmentSeparator);
            }

            return textBuilder.ToString();
        }

        private static string[] GetFragments(string text)
        {
            List<string> fragments = new List<string>();

            int nextFragmentIndex = 0;
            while (nextFragmentIndex != text.Length - 1)
            {
                int startFragmentIndex = text.LastIndexOf(s_StartFragmentSeparator, nextFragmentIndex) + 1;

                int endFragmentIndex = text.IndexOf(s_EndFragmentSeparator, nextFragmentIndex) - 1;

                string fragment = text.Substring(startFragmentIndex, endFragmentIndex);

                fragments.Add(fragment);

                nextFragmentIndex = text.LastIndexOf(s_EndFragmentSeparator, nextFragmentIndex) + 1;
            }

            return fragments.ToArray();
        }

        private static LanguagePairCollection s_LanguagePairs;

        private static LanguagePairCollection LanguagePairs
        {
            get
            {
                if (s_LanguagePairs == null)
                    s_LanguagePairs = ServiceWrapper.GetApprovedLanguagePairs();

                return s_LanguagePairs;
            }
        }

        private static readonly Guid s_FragmentsSeparator = new Guid("27D60FC3-0FB6-4fc7-83F1-28E26F04D949");

        private static readonly string s_StartFragmentSeparator = String.Format("<{0}>", s_FragmentsSeparator);
        private static readonly string s_EndFragmentSeparator = String.Format("</{0}>", s_FragmentsSeparator);


    }
}

