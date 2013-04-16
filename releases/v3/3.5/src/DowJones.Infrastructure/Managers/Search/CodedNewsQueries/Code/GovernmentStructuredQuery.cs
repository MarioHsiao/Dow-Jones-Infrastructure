using System;
using DowJones.Managers.Search.CodedNewsQueries.Government;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    //(((</=ExecFirstName/>) w/3 </=ExecLastName/>) and (</=Title/> or </=OfficeLegalName/>)) and date after -367
    public class GovernmentExecutiveNewsQuery : AbstractGovernmentStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var searchString = GetFirstNameAndLastNameSearchString(Official.Fname, Official.Lname);
            ListSearchString.Add(searchString);

            searchString = new SearchString
                               {
                                   Mode = SearchMode.Any, 
                                   Id = "OfficeLegalName", 
                                   Type = SearchType.Free, 
                                   Value = string.Format("\"{0}\" \"{1}\"", Official.Jobtitle, Official.Office)
                               };
            ListSearchString.Add(searchString);
        }
    }

    //(</=OfficeLegalName/>) and (rst=USA) and (rst=(TGOV or tusn or tmnbus)) and date after -90
    //(hlp=(</=State/>) and </=OfficeLegalName/>) and (rst=usa) and date after -367
    //((</=City/>) and (</=State/>) and </=OfficeLegalName/>) and date after -367
    public class GovernmentGeneralNewsQuery : AbstractGovernmentStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            string str = "";
            string stateQuery;

            switch (Organization.DataSet)
            {
                case Datatype.State:
                    stateQuery = StatesQueryManager.GetStateQuery(Organization.Address.Physaddrstate);
                    str = String.Format("rst:usa hlp:\"{0}\" \"{1}\"",
                                        stateQuery,
                                        Official.Office);
                    break;
                case Datatype.County:
                case Datatype.Municipal:
                    stateQuery = StatesQueryManager.GetStateQuery(Organization.Address.Physaddrstate);

                    str = String.Format("\"{0}\" \"{1}\" \"{2}\"",
                                        stateQuery,
                                        Organization.Address.Physaddrcity,
                                        Official.Office);
                    break;
                case Datatype.Federal:
                    str = string.Format("\"{0}\"", Organization.OfficeID.Value);
                    break;
            }

            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.All, 
                                       Id = "CountyQuery", 
                                       Type = SearchType.Free, 
                                       Value = str
                                   };
            ListSearchString.Add(searchString);

            if (Organization.DataSet != Datatype.Federal)
                return;
            searchString = GetRestrictorFilter(new [] { "USA" });
            ListSearchString.Add(searchString);

            searchString = GetRestrictorFilter(new [] {"TGOV", "tusn", "tmnbus"});
            ListSearchString.Add(searchString);
        }
    }

    //(</=OfficeLegalName/>) and (sc=CNDL) and date after -367
    public class GovernmentOpportunitiesContractsQuery : AbstractGovernmentStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var str = string.Format("sc:CNDL \"{0}\"", Organization.OfficeID.Value);
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.All, 
                                       Id = "StateQuery", 
                                       Type = SearchType.Free, 
                                       Value = str
                                   };
            ListSearchString.Add(searchString);
        }
    }
}