using System;
using System.Collections.Generic;
using DowJones.Tools.Ajax;
using Factiva.Gateway.Messages.Screening.V1_1;

namespace DowJones.Utilities.Ajax.NewsRadar.Converter
{
    public class GetCompanyScreeningListExResponseConverter : IListDataResultConverter
    {
        private readonly GetCompanyScreeningListExResponse _getCompanyScreeningListExResponse;

        public GetCompanyScreeningListExResponseConverter( GetCompanyScreeningListExResponse response)
        {
            _getCompanyScreeningListExResponse = response;
        }


        public  IListDataResult Process()
         {
            var newsRadarResultSet = new NewsRadarResultSet
                                         {
                                             Companies = new List<Company>(),
                                             QueryViews = new List<RadarViewQuery>()
                                         };

            foreach (var rViewQuery in _getCompanyScreeningListExResponse.CompanyScreeningListResult.RadarView.RadarQueryCollection)
            {
                var viewQuery = new RadarViewQuery();
                try
                {
                    viewQuery.Id = rViewQuery.Query.searchList[0].term[0].Value;
                }
                catch (Exception)
                {
                    viewQuery.Id = "";
                }
                viewQuery.Name = rViewQuery.Name;
                viewQuery.Uri = "";
                newsRadarResultSet.QueryViews.Add(viewQuery);
            }


            foreach (var compType in _getCompanyScreeningListExResponse.CompanyScreeningListResult.Companies.CompanyCollection)
            {
                var company = new Company
                                  {
                                      Name = compType.CompanyName,
                                      Code = compType.Fcode,
                                      Uri = compType.Url,
                                      IsNewsCoded = compType.IsNewsCoded
                                  };

                foreach(var nData in compType.NewsDataList)
                {
                    var newsDataType = new NewsDataType
                                           {
                                               day = nData.Day,
                                               month = nData.Month,
                                               week = nData.Week,
                                               twoMonth = nData.TwoMonth,
                                               threeMonth = nData.ThreeMonth,
                                               Uri = ""
                                           };
                    company.RadarValues.Add(newsDataType);
                }
                newsRadarResultSet.Companies.Add(company);
            }


            //newsRadarResultSet.Companies.AddRange(getCompanies()); 


            return newsRadarResultSet;
         }

        //private List<Company> getCompanies()
        //{
        //    List<Company> companies = new List<Company>();

        //    foreach (CompanyType compType in _getCompanyScreeningListExResponse.GetCompanyScreeningListExResult.CompanyScreeningListResult.Companies.CompanyCollection)
        //    {
        //        Company company = new Company();
        //        company.Name = compType.CompanyName;
        //        company.Code = compType.Fcode;
        //        company.Uri = compType.Url;

        //        foreach (RadarViewQuery rViewQuery in _getCompanyScreeningListExResponse.GetCompanyScreeningListExResult.CompanyScreeningListResult.RadarView.RadarQueryCollection) 



        //        companies.Add(company);
        //    }
        //    return companies;
            
        //}
        //private List<Company> getCompanies()
        //{
        //    List<Company> companies = new List<Company>();

        //    foreach (CompanyType compType in _getCompanyScreeningListExResponse.GetCompanyScreeningListExResult.CompanyScreeningListResult.Companies.CompanyCollection)
        //    {
        //        Company company = new Company();
        //        company.Name = compType.CompanyName;
        //        company.Code = compType.Fcode;
        //        company.Uri = compType.Url;
        //        companies.Add(company);
        //    }
        //    return companies;

        //}
    }
}
