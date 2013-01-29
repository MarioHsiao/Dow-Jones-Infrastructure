using DowJones.Attributes;
using DowJones.Converters;
using Newtonsoft.Json;

namespace DowJones.Search
{
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum ExclusionFilter
    {
        [AssignedToken("abstractsCaptionsAndHeadlineOnly")]
        [ExcludeSubject("NABST,NHOC,NCAP")]
        AbstractsCaptionsAndHeadline,

        [AssignedToken("calendarsAndMediaAdvisories")]
        [ExcludeSubject("NADV,NCAL")] 
        CalendarsAndMediaAdvisories,

        [AssignedToken("commentaryOpinionAndAnalysis")]
        [ExcludeSubject("NEDC,NEDI,NPRED,NANL,NADC,NRVW,NADVTR,NLET")] 
        CommentaryOpinionAndAnalysis,

        [AssignedToken("crime")]
        [ExcludeSubject("GCRIM")] 
        Crime,

        [AssignedToken("entertainmentAndLifestyle")]
        [ExcludeSubject("GENT,GLIFE")] 
        EntertainmentAndLifestyle,

         [AssignedToken("newsSummaries")]
        [ExcludeSubject("NCHR,NSUM")] 
        NewsSummaries,

        [AssignedToken("republishedNews")]
        [ExcludeSubject("NNAM")] 
        RepublishedNews,

        [AssignedToken("routineGeneralNews")]
        [ExcludeSubject("NCONT,NLIST,GHORSC,NOBT,NPAN,GRELIS,GWERE,GTREP,GLIST")] 
        RoutineGeneralNews,

        [AssignedToken("routineMarketAndFinancialNews")]
        [ExcludeSubject("NRMF,NPRPCT,NSTACS")]
        RoutineMarketFinancialNews,

        [AssignedToken("sports")]
        [ExcludeSubject("GSPO")]
        Sports,

        [AssignedToken("prefTranscripts")]
        [ExcludeSubject("NTRA")]
        Transcripts
    }
}