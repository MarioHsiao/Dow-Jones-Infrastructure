using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Session;
using DowJones.Preferences;
using Factiva.Gateway.Messages.Search.V2_0;
using DowJones.Managers.Search.Responses;

namespace DowJones.Infrastructure
{
    public class SearchManagerTests : UnitTestFixture
    {

        private IControlData m_ControlData = new ControlData { UserID = "joyful", UserPassword = "joyful", ProductID = "16" };
        private IPreferences m_preferences = new DowJones.Preferences.Preferences("en");

        public void PerformAccessionNumberSearch()
        {

            SearchManager manager = new SearchManager(m_ControlData, m_preferences);
            AccessionNumberSearchRequestDTO requestDTO = new AccessionNumberSearchRequestDTO();
            requestDTO.AccessionNumbers = GetAccessionNumbers().ToArray();
            requestDTO.SortBy = SortBy.FIFO;
            requestDTO.MetaDataController.Mode = CodeNavigatorMode.All;
            requestDTO.MetaDataController.MaxBuckets = 10;
            requestDTO.MetaDataController.MinBucketValue = 2;
            requestDTO.DescriptorControl.Mode = DescriptorControlMode.All;
            requestDTO.DescriptorControl.Language = "en";
            requestDTO.ClusterMode = ClusterMode.On;
            AccessionNumberSearchResponse response = manager.PerformAccessionNumberSearch<PerformContentSearchRequest,PerformContentSearchResponse>(requestDTO);

            foreach (AccessionNumberBasedContentItem item in response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
            {
                Console.WriteLine(item.AccessionNumber);
            }
        }

        private static List<string> GetAccessionNumbers()
        {
            return new List<string>(
                new string[]
                    {
                        "ENTWK00020111201e7c20002i", "COMWKN0020111201e7c100045", "TWTONE0020111201e7c1005mz", "TWTONE0020111201e7c10075s", "TWTONE0020111201e7c1009ml",
                        "MRKWN00020111201e7c30002z", "MMSAJQ0020111130e7bu00001", "MMSAWI0020111130e7bu00001", "MMSAWD0020111128e7bm0002v", "MMSAWD0020111122e7bg00008",
                        "APPIC00020111201e7bu0007y", "APPIC00020111128e7bs009vn", "APPIC00020111117e7bg00002", "PPGZ000020081126e4bq0001j", "PPGZ000020081126e4bq0000x",
                        "X027450020111115e7bf0002t", "QQPTR00020081126e4bq0000r", "SFC0000020081126e4bq0005m", "LATM000020081126e4bq0000x", "KRTTB00020081126e4bq0000e",
                        "KRTAK00020081126e4bq00061", "TMPA000020081126e4bp0004k", "APGOV00020081125e4bp0006o", "SEPI000020081126e4bp0001g", "APGOV00020081125e4bp000wk",
                        "HOU0000020081126e4bp0009s", "APGOV00020081125e4bp0003j", "BHLD000020081126e4bp00001", "APGOV00020081125e4bp0000l", "DJ00000020081125e4bp000m2",
                        "APRS000020081125e4bp0023y", "USAT000020081125e4bp0001t", "USAT000020081125e4bp0001j", "APGOV00020081125e4bp0025z", "APRS000020081125e4bp0025z",
                        "LANC000020081125e4bp0007s", "LANC000020081125e4bp0005t", "APGOV00020081125e4bp001ml", "NYDN000020081125e4bp0002t", "NYDN000020081125e4bp0001g",
                        "DNVR000020081125e4bp0000h", "APGOV00020081125e4bp001vp", "APRS000020081125e4bp001vp", "APGOV00020081125e4bp001va", "PPGZ000020081125e4bp000c7",
                        "PPGZ000020081125e4bp000c6", "WPCOM00020081125e4bp00003", "WATI000020081125e4bp00013", "KRTHC00020081125e4bp0008p", "KRTVB00020081125e4bp00009",
                        "KRTVB00020081125e4bp00008", "DJ00000020081125e4bp000b2", "DJ00000020081125e4bp000a5", "NYPO000020081125e4bp0001r", "NYPO000020081125e4bp00004",
                        "BSTNGB0020081125e4bp0004z", "ALBJ000020081125e4bp0000c", "PPGZ000020081125e4bp0002b", "PPGZ000020081125e4bp0002i", "PPGZ000020081125e4bp0008k",
                        "KRTBZ00020081125e4bp0005o", "APGOV00020081125e4bp001ab", "KRTNN00020081125e4bp00009", "NFLK000020081125e4bp00022", "KRTAL00020081125e4bp0000n",
                        "KCST000020081125e4bp00014", "KCST000020081125e4bp0000r", "KRTBZ00020081125e4bp00034", "NYTF000020081125e4bp0005n", "KRTAG00020081125e4bp0000c",
                        "KRTAK00020081125e4bp0005q", "SCLT000020081121e4bp002sv", "SDU0000020081126e4bo0003g", "NYER000020081126e4bo00009", "APGOV00020081124e4bo002kf",
                        "APGOV00020081124e4bo002v1", "APGOV00020081124e4bo002ot", "PPGZ000020081125e4bo000c3", "PPGZ000020081125e4bo000c2", "APGOV00020081124e4bo002ee",
                        "PPGZ000020081124e4bo0000r", "SEPI000020081125e4bo0001i", "APGOV00020081124e4bo000c0", "SLTR000020081125e4bo0000b", "BHLD000020081125e4bo0000f",
                        "APGOV00020081124e4bo00009", "USAT000020081124e4bo0001i", "USAT000020081124e4bo0001g", "NYDN000020081124e4bo00014", "NYDN000020081124e4bo00011",
                        "KRTVB00020081124e4bo0002t", "LANC000020081124e4bo0009c", "DNVR000020081124e4bo0000u", "LANC000020081124e4bo00092", "NYPO000020081124e4bo00049",
                        "NYPO000020081124e4bo0003o", "DJ00000020081124e4bo000eq", "PROV000020081124e4bo0001y", "PROV000020081124e4bo0001v", "APGOV00020081124e4bo0028y"
                    }
                );
        }
    }
}