namespace DowJones.Utilities.Ajax.ConnectionResults.Converters
{
    using System;
    using System.Collections.Generic;
    using Ajax.Converters;
    using Factiva.Gateway.Messages.RelationshipMapping.V1_0;
    using Formatters;
    using Formatters.Numerical;
    using log4net;
    using Tools.Ajax;
    using RelationshipType = RelationshipType;

    public class PerformRelationshipMappingResponseConverter : AbstractRelationshipMappingResultConverter
    {
        protected static readonly ILog s_Log = LogManager.GetLogger(typeof(PerformRelationshipMappingResponseConverter));

        private readonly GenerateStartConnectionNavigateUrl m_GenerateStartConnectionNavigateUrl;
        private readonly GenerateEndConnectionNavigateUrl m_GenerateEndConnectionNavigateUrl;
        private readonly GenerateConnectionNavigateUrl m_GenerateConnectionNavigateUrl;

        private readonly PerformRelationshipMappingResponse m_Response;

        public PerformRelationshipMappingResponseConverter(PerformRelationshipMappingResponse response)
        {
            m_Response = response;
        }

        public PerformRelationshipMappingResponseConverter(
            PerformRelationshipMappingResponse response,
            GenerateConnectionNavigateUrl generateConnectionNavigateURL)
        {
            m_Response = response;
            m_GenerateConnectionNavigateUrl = generateConnectionNavigateURL;
        }

        public PerformRelationshipMappingResponseConverter(
            PerformRelationshipMappingResponse response,
            GenerateStartConnectionNavigateUrl generateStartConnectionNavigateURL,
            GenerateConnectionNavigateUrl generateConnectionNavigateURL,
            GenerateEndConnectionNavigateUrl generateEndConnectionNavigateURL)
        {
            m_Response = response;

            m_GenerateStartConnectionNavigateUrl = generateStartConnectionNavigateURL;
            m_GenerateConnectionNavigateUrl = generateConnectionNavigateURL;
            m_GenerateEndConnectionNavigateUrl = generateEndConnectionNavigateURL;
        }

        public override IListDataResult Process()
        {

            ConnectionResultsDataResult result = new ConnectionResultsDataResult();

            result.ResultSet = new ConnectionResultsDataResultSet();

            if (m_Response.RelationshipMappingResponse.RelationshipMappingResultSet.Count == 0)
                return result;

            result.HitCount =
                new WholeNumber(m_Response.RelationshipMappingResponse.RelationshipMappingResultSet.Total);
            NumberFormatter.Format(result.HitCount);

            result.ResultSet.First =
                new WholeNumber(m_Response.RelationshipMappingResponse.RelationshipMappingResultSet.First);
            NumberFormatter.Format(result.ResultSet.First);

            result.ResultSet.Count =
                new WholeNumber(m_Response.RelationshipMappingResponse.RelationshipMappingResultSet.Count);
            NumberFormatter.Format(result.ResultSet.Count);

            result.ResultSet.Relationships =
                GetRelationshipInfoList(
                    m_Response.RelationshipMappingResponse.RelationshipMappingResultSet.RelationshipCollection);

            result.ResultSet.SourceConnection =
                GetConnectionInfo(
                    m_Response.RelationshipMappingResponse.RelationshipMappingResultSet.RelationshipCollection[0].
                        StartConnection);

            result.ResultSet.TargetConnection =
                GetConnectionInfo(
                    m_Response.RelationshipMappingResponse.RelationshipMappingResultSet.RelationshipCollection[0].
                        EndConnection);

            return result;
        }

        #region Inner Methods
        private List<RelationshipInfo> GetRelationshipInfoList(IEnumerable<Relationship> relationships)
        {
            List<RelationshipInfo> list = new List<RelationshipInfo>();

            foreach (Relationship relationship in relationships)
            {
                RelationshipInfo relationshipInfo = new RelationshipInfo();

                relationshipInfo.Strength = new WholeNumber(relationship.Strength);
                NumberFormatter.Format(relationshipInfo.Strength);
                relationshipInfo.Type = GetRelationshipInfoType(relationship.Degrees);

                relationshipInfo.SourceConnection = GetConnectionInfo(relationship.StartConnection);

                relationshipInfo.Connections = GetRelationshipInfoConnectionList(relationship.ConnectionCollection);

                relationshipInfo.TargetConnection = GetConnectionInfo(relationship.EndConnection);

                list.Add(relationshipInfo);
            }

            return list;
        }

        private List<ConnectionInfo> GetRelationshipInfoConnectionList(IEnumerable<Connection> connections)
        {
            List<ConnectionInfo> list = new List<ConnectionInfo>();

            foreach (Connection connection in connections)
            {
                list.Add(GetConnectionInfo(connection));
            }

            return list;
        }

        private ConnectionInfo GetConnectionInfo(StartConnection connection)
        {
            ConnectionInfo info = new ConnectionInfo();

            info.Type = ConnectionType.SourceConnection;
            info.Name = GetConnectionInfoName(connection.Person, connection.Job1);
            info.FCode = GetConnectionInfoFCode(connection.Person, connection.Job1);
            info.EntityType = GetConnectionInfoEntityType(connection.Person, connection.Job1);

            if (m_GenerateStartConnectionNavigateUrl != null)
            {
                info.NavigateUrl = m_GenerateStartConnectionNavigateUrl(connection);
            }

            if (info.EntityType == EntityType.Me || info.EntityType == EntityType.Executive)
            {
                info.Jobs.Add(GetJobInfo(connection.Job1));
            }

            return info;
        }

        private ConnectionInfo GetConnectionInfo(Connection connection)
        {
            ConnectionInfo info = new ConnectionInfo();

            info.Name = GetConnectionInfoName(connection.Person, connection.Job1);
            info.EntityType = GetConnectionInfoEntityType(connection.Person, connection.Job1);
            info.FCode = GetConnectionInfoFCode(connection.Person, connection.Job1);
            info.Strength = new WholeNumber(connection.Strength);
            NumberFormatter.Format(info.Strength);
            info.Type = ConnectionType.IntermediateConnection;
            if (m_GenerateConnectionNavigateUrl != null)
            {
                info.NavigateUrl = m_GenerateConnectionNavigateUrl(connection);
            }

            if (info.EntityType == EntityType.Me || info.EntityType == EntityType.Executive)
            {
                info.Jobs.Add(GetJobInfo(connection.Job1));

                if (connection.Job2 != null)
                {
                    info.Jobs.Add(GetJobInfo(connection.Job2));
                }
            }

            return info;
        }

        private ConnectionInfo GetConnectionInfo(EndConnection connection)
        {
            ConnectionInfo info = new ConnectionInfo();

            info.Name = GetConnectionInfoName(connection.Person, connection.Job1);
            info.EntityType = GetConnectionInfoEntityType(connection.Person, connection.Job1);
            info.FCode = GetConnectionInfoFCode(connection.Person, connection.Job1);
            info.Strength = new WholeNumber(connection.Strength);
            NumberFormatter.Format(info.Strength);
            info.Type = ConnectionType.TargetConnection;

            if (m_GenerateEndConnectionNavigateUrl != null)
            {
                info.NavigateUrl = m_GenerateEndConnectionNavigateUrl(connection);
            }

            if (info.EntityType == EntityType.Me || info.EntityType == EntityType.Executive)
            {
                info.Jobs.Add(GetJobInfo(connection.Job1));
            }

            return info;
        }

        private static RelationshipType GetRelationshipInfoType(int degrees)
        {
            RelationshipType type;

            switch (degrees)
            {
                case 1:
                    type = RelationshipType.FirstDegree;
                    break;
                case 2:
                    type = RelationshipType.SecondDegree;
                    break;
                case 3:
                    type = RelationshipType.ThirdDegree;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return type;
        }

        private static EntityType GetConnectionInfoEntityType(Person person, Job job)
        {
            if (person != null && !string.IsNullOrEmpty(person.DjPersonCode))
                return EntityType.Executive;

            if (person != null && !string.IsNullOrEmpty(person.AffiliateID))
                return EntityType.Me;

            if (job != null && job.DjOrgCode != null)
                return EntityType.Company;

            throw new NotImplementedException();
        }

        private static string GetConnectionInfoFCode(Person person, Job job)
        {
            if (person != null && person.DjPersonCode != null)
                return person.DjPersonCode;

            if (job != null && job.DjOrgCode != null)
                return job.DjOrgCode;

            throw new NotImplementedException();
        }


        private static string GetConnectionInfoName(Person person, Job job)
        {
            if (person != null && person.DjPersonCode != null)
                return person.MixedValue;

            if (job != null && job.DjOrgCode != null)
                return job.OrgName;

            throw new NotImplementedException();

        }

        private static JobInfo GetJobInfo(Job job)
        {
            JobInfo jobInfo = new JobInfo();

            jobInfo.Organization = job.OrgName;
            jobInfo.Description = job.MixedValue;
            jobInfo.DjOrgCode = job.DjOrgCode;

            return jobInfo;
        }
        #endregion
    }
}
