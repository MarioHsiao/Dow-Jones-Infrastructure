using System.Collections.Generic;
using Factiva.Gateway.Messages.Search.V2_0;

namespace Factiva.Gateway.Messages.CodedNews
{
    public class CompanyKeyDevelopmentStructuredQuery : AbstractCompanyStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var searchString = GetRestrictorFilter(new [] {"Multi"});
            searchString.Scope = "sc";
            ListSearchString.Add(searchString);

            searchString = GetCompanyFilter(new [] { CompanyFilter.Fcode }, false);
            ListSearchString.Add(searchString);

            var ns = GetNewsSubjectsCode(QueryType);
            if (ns == null || ns.Length <= 0)
                return;
            searchString = GetNewsSubjectFilter(ns);
            ListSearchString.Add(searchString);
        }

        public static string[] GetNewsSubjectsCode(CodedNewsType newsType)
        {
            var list = new List<string>();
            switch (newsType)
            {
                case CodedNewsType.ContractsOrders:
                    list.Add("c33");
                    break;
                case CodedNewsType.NewProductsServices:
                case CodedNewsType.KeyDevNewProductsServices:
                    list.Add("c22");
                    break;
                case CodedNewsType.LegalJudicial:
                    list.Add("c12");
                    break;
                case CodedNewsType.Performance:
                case CodedNewsType.KeyDevPerformance:
                    list.Add("c15");
                    break;
                case CodedNewsType.OwnershipChanges:
                case CodedNewsType.KeyDevMAOC:
                    list.Add("c18");
                    break;
                case CodedNewsType.MergersAcquisition:
                    list.Add("c181");
                    break;
                case CodedNewsType.ManagementMoves:
                    list.Add("c411");
                    break;
                case CodedNewsType.KeyDevManagementChanges:
                    list.Add("c41");
                    list.Add("c02");
                    break;
                case CodedNewsType.Bankruptcy:
                case CodedNewsType.KeyDevBankruptcy:
                    list.Add("c16");
                    break;
                case CodedNewsType.KeyDevMarketChanges:
                    list.Add("c14");
                    break;
                case CodedNewsType.KeyDevNewFundingCapital:
                    list.Add("c17");
                    break;
                case CodedNewsType.KeyDevRegGvtPolicy:
                    list.Add("c13");
                    break;
                case CodedNewsType.Earnings:
                    list.Add("c151");
                    break;
                case CodedNewsType.CapacitiesFacilities:
                    list.Add("c24");
                    break;
            }
            return list.ToArray();
        }
    }
}