using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowJones.Tools.Ajax.Converters;
using DowJones.Tools.Ajax.HeadlineList;

namespace DowJones.Managers.Syndication
{
    public class SyndicationManager
    {
        private readonly HeadlineListConversionManager _conversionManager;

        public SyndicationManager(HeadlineListConversionManager conversionManager) 
        {
            _conversionManager = conversionManager;
        }

        public List<SyndicationDataResult> GetFeeds(IEnumerable<string> feedUris)
        {
            var tasks = feedUris.Select(uri => Task.Factory.StartNew(() => _conversionManager.ProcessFeed(uri), TaskCreationOptions.None)).ToList();
            Task.WaitAll(tasks.ToArray());
            return tasks.Select(task => task.Result).ToList();
        } 
    }
}
