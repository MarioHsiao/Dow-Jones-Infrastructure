using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DowJones.Exceptions;
using DowJones.Globalization;
using DowJones.Managers.AutomatedTranslation.Providers.Core;

namespace DowJones.Managers.AutomatedTranslation.Providers.LanguageWeaver
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
            if(!LanguageMapper.TryConvertToIso639Language(sourceLanguage, out sourceIso639Language)
                || !IsSupportedSourceLanguage(sourceIso639Language))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.NotSupportedSourceLanguageException);
            }

            string targetIso639Language;
            if (!LanguageMapper.TryConvertToIso639Language(targetLanguage, out targetIso639Language)
                || !IsSupportedTargetLanguage(sourceIso639Language, targetIso639Language))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.NotSupportedTargetLanguageException);
            }

            LanguagePair pair = GetLanguagePairs().Get(sourceIso639Language, targetIso639Language);

            string sourceText = GetText(fragments, format);

            Dictionary<string, string> serviceResponse = 
                ServiceWrapper.SubmitNonBlockingTranslationRequest(sourceText, TextFormat.Html, pair);

            var task = new TranslationTask(serviceResponse["uri"], format); 

            TranslationResult translationResult;
            do
            {
                serviceResponse = ServiceWrapper.QueryNonBlockingTranslationRequest(task.Uri);
                translationResult = new TranslationResult(serviceResponse["status"], serviceResponse["translatedText"], format);
            }
            while (IsTranslating(translationResult));

            return translationResult;
        }

        public object BeginTranslateTask(string[] fragments, TextFormat format, ContentLanguage sourceLanguage, ContentLanguage targetLanguage)
        {
            if (fragments == null || fragments.Length == 0)
            {
                throw new ArgumentNullException("fragments");
            }

            string sourceIso639Language;
            if (!LanguageMapper.TryConvertToIso639Language(sourceLanguage, out sourceIso639Language)
                || !IsSupportedSourceLanguage(sourceIso639Language))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.NotSupportedSourceLanguageException);
            }

            string targetIso639Language;
            if (!LanguageMapper.TryConvertToIso639Language(targetLanguage, out targetIso639Language)
                || !IsSupportedTargetLanguage(sourceIso639Language, targetIso639Language))
            {
                throw new DowJonesUtilitiesException(DowJonesUtilitiesException.NotSupportedTargetLanguageException);
            }

            LanguagePair pair = GetLanguagePairs().Get(sourceIso639Language, targetIso639Language);
            
            string sourceText = GetText(fragments, format);

            Dictionary<string, string> serviceResponse = ServiceWrapper.SubmitNonBlockingTranslationRequest(sourceText, TextFormat.Html, pair);            

            var task = new TranslationTask(serviceResponse["uri"], format);

            return task;
        }

        public object QueryTranslateTask(object task)
        {
            var translationTask = task as TranslationTask;

            if (translationTask == null)
            {
                throw new ArgumentNullException("task");
            }

            Dictionary<string, string> serviceResponse = ServiceWrapper.QueryNonBlockingTranslationRequest(translationTask.Uri);

           var result = new TranslationResult(serviceResponse["status"], serviceResponse["translatedText"], translationTask.Format);

            return result;
        }

        public object EndTranslateTask(object task)
        {
            var translationTask = task as TranslationTask;

            if (translationTask == null)
            {
                throw new ArgumentNullException("task");
            }

            TranslationResult result;
            do
            {
                Dictionary<string, string> serviceResponse = ServiceWrapper.QueryNonBlockingTranslationRequest(translationTask.Uri);
                result = new TranslationResult(serviceResponse["status"], serviceResponse["translatedText"], translationTask.Format);
            }
            while (IsTranslating(result));

            return result;
        }
  

        public string[] GetTranslatedFragments(object result)
        {
            if(result == null)
            {
                throw new ArgumentNullException("result");
            }

            var temp = (TranslationResult)result;
            return GetFragments(temp.TranslatedText);
        }

        public bool IsTranslating(object result)
        {
            if(result == null)
            {
                throw new ArgumentNullException("result");
            }

            var temp = (TranslationResult)result;
            return temp.Status == Translating;
        }

        public bool IsDone(object result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            var temp = (TranslationResult)result;
            return temp.Status == Done;
        }

        public bool IsFailed(object result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            var temp = (TranslationResult)result;
            return temp.Status == Failed;
        }

        #region Helpers
        private static string GetText(IEnumerable<string> fragments, TextFormat format)
        {
            var textBuilder = new StringBuilder();

            foreach (var fragment in fragments)
            {
                textBuilder.Append(StartFragmentSeparator);
                textBuilder.Append(format == TextFormat.Plain ? HttpUtility.HtmlEncode(fragment) : fragment);
                textBuilder.Append(EndFragmentSeparator);
            }

            return textBuilder.ToString();
        }

        private static string[] GetFragments(string text)
        {
            var fragments = new List<string>();

            var nextFragmentIndex = 0;
            while (nextFragmentIndex < text.Length )
            {
                var startFragmentIndex = GetStartIndex(text, StartFragmentSeparator, nextFragmentIndex);
                var endFragmentIndex = GetEndIndex(text, EndFragmentSeparator, nextFragmentIndex);

                if(startFragmentIndex == -1)
                    break;

                if (endFragmentIndex == -1)
                    endFragmentIndex = text.Length - 1;
                
                var fragment = HttpUtility.HtmlDecode(text.Substring(startFragmentIndex, endFragmentIndex - startFragmentIndex));
                
                fragments.Add(fragment);

                nextFragmentIndex = endFragmentIndex + EndFragmentSeparator.Length;
            }

            return fragments.ToArray();
        }

        private static LanguagePairCollection GetLanguagePairs()
        {
            return s_LanguagePairs ?? (s_LanguagePairs = ServiceWrapper.GetApprovedLanguagePairs());
        }

        private static int GetStartIndex(string text, string separator, int startIndex)
        {
            int index = -1;

            int i = startIndex;
            while(i < text.Length)
            {
                int j = 0;

                while(j < separator.Length && text[i] == separator[j])
                {
                    j++;
                    i++;
                }

                if (j == separator.Length)
                {
                    index = i;
                    break;
                }

                i++;
            }

            return index;
        }

        private static int GetEndIndex(string text, string separator, int startIndex)
        {
            int index = -1;

            int i = startIndex;
            while (i < text.Length)
            {
                int j = 0;

                while (j < separator.Length && text[i] == separator[j])
                {
                    j++;
                    i++;
                }

                if (j == separator.Length)
                {
                    index = i - separator.Length;
                    break;
                }

                i++;
            }

            return index;
        }

        private static bool IsSupportedSourceLanguage(string language)
        {
            return GetLanguagePairs().Any(pair => language == pair.FromLanguage);
        }

        private static bool IsSupportedTargetLanguage(string sourceLanguage, string targetLanguage)
        {
            return GetLanguagePairs().Where(pair => sourceLanguage == pair.FromLanguage).Any(pair => targetLanguage == pair.IntoLanguage);
        }

        #endregion

        private static LanguagePairCollection s_LanguagePairs;

        private const string StartFragmentSeparator = "<fs>";
        private const string EndFragmentSeparator = "</fs>";

        private const string Translating= "TRANSLATING";
        private const string Failed= "FAILED";
        private const string Done = "DONE";


    }
}

