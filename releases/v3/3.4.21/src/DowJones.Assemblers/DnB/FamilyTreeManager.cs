using DowJones.Ajax.FamilyTree;
using DowJones.Formatters;
using DowJones.Formatters.Numerical;
using Factiva.Gateway.Messages.FCE.DnB.Utilities.V1_0;
using Factiva.Gateway.Messages.FCE.DnB.V1_0;
using Address = DowJones.Ajax.FamilyTree.Address;

namespace DowJones.Assemblers.DnB
{
    public class FamilyTreeManager
    {
        public FamilyTreeDataResult Process(GetChildrenResponse getChildrenResponse)
        {
            var result = new FamilyTreeDataResult { resultSet = new FamilyTreeDataResultSet() };
            var _numberFormatter = new NumberFormatter();

            foreach (Company company in getChildrenResponse.dnBTreeResultSet.dnBTreeResultCollection[0].companyCollection)
            {
                var industryCollection = new IndustryCollection();
                foreach (Code code in company.industryCollection)
                {
                    industryCollection.Code = code.code;
                    industryCollection.Scheme = code.scheme;
                    industryCollection.Descriptor = code.descriptor;
                }

                var address = new Address
                {
                    Street1 = company.address.street1,
                    Street2 = company.address.street2,
                    Street3 = company.address.street3,
                    City = company.address.city,
                    StateProvinceCounty = company.address.stateProvinceCounty,
                    ZipPostalCode = company.address.zipPostalCode,
                    Country = company.address.country,
                    PhoneNumber = company.address.phoneNumber,
                    FaxNumber = company.address.faxNumber,
                    Url = company.address.url,
                };
                var salesUSD = new DoubleMoney(company.numerics.salesUSD, "USD");
                var employees = new WholeNumber(company.numerics.totalEmployees);

                _numberFormatter.Format(salesUSD);
                _numberFormatter.Format(employees);

                var numerics = new Numerics
                {
                    SalesUSD = salesUSD.Text.Value,
                    TotalEmployees = employees.Text.Value
                };
                var ftni = new FamilyTreeNodeInfo
                               {
                                   DunsNumber = company.DUNS.duns,
                                   GlobalUltimateDunsNumber = company.globalUltimateDUNS.duns,
                                   HQParentDunsNumber = company.HQParentDUNS.duns,
                                   BusinessName = company.DUNS.businessName,
                                   GlobalUltimateBusinessName = company.globalUltimateDUNS.businessName,
                                   HierarchyDesignation = company.hierarchyDesignation,
                                   NumberFamilyMembers = company.numberFamilyMembers,
                                   NumberBranches = company.numberBranches,
                                   NumberofImmediateBranches = company.numberImmediateBranches,
                                   NumberofImmediateChildren = company.numberImmediateChildren,
                                   IndustryCollection = industryCollection,
                                   Address = address,
                                   Numerics = numerics
                               };
                result.resultSet.familyTreeNodes.Add(ftni);
            }
            return result;
        }

    }
}
