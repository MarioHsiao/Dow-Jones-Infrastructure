using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Ajax.AddressContact;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;
using WebSite = DowJones.Ajax.AddressContact.WebSite;

namespace DowJones.Assemblers.Assets
{
    public class AddressContactCoverter
    {
        private readonly GetReportListExResponse _getReportListExResponse;
        private readonly GetAssetsResponse _getAssetsResponse;

        public AddressContactCoverter(GetReportListExResponse getReportListExResponse, GetAssetsResponse getAssetsResponse)
           
        {
            _getReportListExResponse = getReportListExResponse;
            _getAssetsResponse = getAssetsResponse;
        }

        public AddressContactData Process()
        {
            AddressContactData data = new AddressContactData();

            CompanyInformation companyArchive = null;
            if (_getAssetsResponse != null && _getReportListExResponse != null
                && _getAssetsResponse.AssetsCollection.Count > 0
                && _getAssetsResponse.AssetsCollection[0] is CompanyInformationAsset)
            {
                CompanyInformationAsset primaryCompanyInfoAsset = (CompanyInformationAsset)_getAssetsResponse.AssetsCollection[0];
                if (primaryCompanyInfoAsset.AssetData != null
                    && primaryCompanyInfoAsset.AssetData.CompanyInformation != null)
                {
                    companyArchive = primaryCompanyInfoAsset.AssetData.CompanyInformation;
                }
            }

            Company companyScreening = null;
            if (_getReportListExResponse != null 
                && _getReportListExResponse.GetReportListExResult != null
                && _getReportListExResponse.GetReportListExResult.ReportListResult != null
                && _getReportListExResponse.GetReportListExResult.ReportListResult.ReportCategory != null)
            {
                companyScreening = _getReportListExResponse.GetReportListExResult.ReportListResult.ReportCategory as Company;
            }

            if (companyScreening == null && companyArchive == null)
            {
                throw new ArgumentException("Must provide at least a valid company inform GetAssetsResponse or GetReportListResponse.");
            }

            // name
            data.CompanyName = (companyScreening != null) 
                ? companyScreening.Descriptor.DescriptorCollection[0].Value
                    : companyArchive.Name.Value;

            // additional info
            data.Inn = (companyArchive != null) ? companyArchive.Inn.Descriptor.Value : null;
            
            // Olga 06/01/2010 
            // 1. Pick legal status from ReportList if it is not null
            // 2 If null, pick the legal status from archive document where scheme=”FactivaLegalStatus”. 
          
            data.LegalStatusString = (companyScreening != null && !String.IsNullOrEmpty(companyScreening.LegalStatusDescriptor))
                                         ? companyScreening.LegalStatusDescriptor
                                         : (companyArchive != null)
                                               ? GetLegalStatusDescription(companyArchive.LegalStatusCollection)
                                               : null;
            data.Okpo = (companyArchive != null) ? companyArchive.Okpo.Value : null;
            data.EmployeesHere = (companyArchive != null) ? companyArchive.EmployeesHere : 0;
            data.FilingOfficeName = (companyArchive != null) ? companyArchive.Registration.FilingOfficeName : null;
            data.RegistrationId = (companyArchive != null)
                ? ((companyArchive.Registration.RegistrationIdCollection.Count > 0) 
                    ? companyArchive.Registration.RegistrationIdCollection[0] : null)
                    : null;
            data.TradeName = (companyArchive != null && companyArchive.TradingName != null) ? companyArchive.TradingName.Value : null;
            data.YearStarted = (companyArchive != null) ? companyArchive.YearStart : 0;

            // address
            data.Address = (companyArchive != null) ? companyArchive.Address : new Address()
                {
                    Street1 = companyScreening.Address1, 
                    Street2 = companyScreening.Address2,
                    Street3 = companyScreening.Address3,
                    City = companyScreening.City,
                    StateProvinceCounty = companyScreening.State,
                    ZipPostalCode = companyScreening.PostalCode,
                    Country = companyScreening.Country,
                };
            //////data.CountryISOCode = (company != null) ? company.IsoRegion : null;
            //////data.RegionCode = (company != null) ? company.Region : null;

            // web sites and links
            if (companyArchive != null)
            {
                Factiva.Gateway.Messages.FCE.Assets.V1_0.WebSiteCollection webSiteCollection = companyArchive.WebSiteCollection;
                foreach (Factiva.Gateway.Messages.FCE.Assets.V1_0.WebSite ws in webSiteCollection)
                {
                    switch (ws.Category.Value.ToUpper())
                    {
                        case "HPG":
                            data.CompanyWebSite = new WebSite()
                            {
                                Url = ws.Url,
                                Description = ws.Category.Descriptor.Value,
                                Text = (ws.Url.Length > 32) ? ws.Url.Substring(0, 32) : ws.Url
                            };
                            break;
                        case "NRL":
                        case "INVR":
                        case "FIN":
                        case "PRF":
                        case "EXE":
                        case "PAS":
                        case "EMP":
                            data.AdditionalCompanyLinks.Add(new WebSite()
                            {
                                Url = ws.Url,
                                Description = ws.Category.Descriptor.Value,
                                Text = ws.Category.Descriptor.Value
                            });
                            break;
                        case "CNT":
                            // mailto:
                            data.AdditionalCompanyLinks.Add(new WebSite()
                            {
                                Url = "mailto:" + ws.Url,
                                Description = ws.Category.Descriptor.Value,
                                Text = ws.Category.Descriptor.Value
                            });
                            break;
                        default:
                            // do nothing for any other URLs
                            break;
                    }
                }
            }
 
            // contact info
            if (companyArchive != null)
            {
                data.FormattedPhone = AssembleCompanyPhoneFaxNumber(
                                           companyArchive.PhoneNumber.CountryRegionCode,
                                           companyArchive.PhoneNumber.CityAreaCode,
                                           companyArchive.PhoneNumber.Number);

                data.FormattedFax = AssembleCompanyPhoneFaxNumber(
                                           companyArchive.FaxNumber.CountryRegionCode,
                                           companyArchive.FaxNumber.CityAreaCode,
                                           companyArchive.FaxNumber.Number);
            }

            // market info
            if (companyArchive != null)
            {
                data.OwnershipType = (!companyArchive.__listingStatusSpecified)
                    ? OwnershipDisplayType.Unspecified
                        : (companyArchive.ListingStatus == ListingStatus.Listed)
                            ? OwnershipDisplayType.Listed : OwnershipDisplayType.Unlisted;
            }
            else
            {
                data.OwnershipType = (!companyScreening.__ownershipTypeSpecified) ? OwnershipDisplayType.Unspecified
                    : (companyScreening.OwnershipType == CompanyOwnershipType.Public) 
                        ? OwnershipDisplayType.Listed : OwnershipDisplayType.Unlisted;
            }

            data.Duns = (companyScreening != null && IsValid(companyScreening.Duns)) ? companyScreening.Duns
                : (companyArchive != null) ? companyArchive.DunsNumber.Value : null;

            ////data.AuditorName = primaryAsset.AssetData.CompanyInformation.xxxx;
            
            // bank details
            if (companyArchive != null)
            {
                BankDetailsCollection bankDetails = companyArchive.BankDetailsCollection;

                if (bankDetails != null && bankDetails.Count > 0)
                {
                    data.BankDetails = new List<BankDisplayDetails>();
                    data.BankDetails.AddRange(from b in bankDetails.ToArray()
                                              select new BankDisplayDetails() { NameAndAddress = ((BankDetails)b).NameAndAddress });
                }
            }

            // family tree info
            data.LocationType = (companyScreening != null) ? companyScreening.LocationTypeDescriptor : null;

            if (companyScreening != null && companyScreening.FamilyTreeInfo != null)
            {
                if (companyScreening.FamilyTreeInfo.__numberFamilyMembersSpecified)
                {
                    data.FamilyTreeInfo = new FamilyTreeDisplayInfo()
                    {
                        Duns = companyScreening.FamilyTreeInfo.Duns,
                        Fcode = companyScreening.FamilyTreeInfo.Fcode,
                        NumberFamilyMembers = companyScreening.FamilyTreeInfo.NumberFamilyMembers
                    };
                }

                if (companyScreening.FamilyTreeInfo.GlobalUltimate != null)
                {
                    data.GlobalUltimateParent = new FamilyTreeParentDisplayInfo()
                    {
                        Duns = companyScreening.FamilyTreeInfo.GlobalUltimate.Duns,
                        Fcode = companyScreening.FamilyTreeInfo.GlobalUltimate.Fcode,
                        IsCompanyReportAvailable = companyScreening.FamilyTreeInfo.GlobalUltimate.IsCompanyReportAvailable,
                        Value = companyScreening.FamilyTreeInfo.GlobalUltimate.Value
                    };
                }

                if (companyScreening.FamilyTreeInfo.DomesticUltimate != null)
                {
                    data.GlobalUltimateParent = new FamilyTreeParentDisplayInfo()
                    {
                        Duns = companyScreening.FamilyTreeInfo.DomesticUltimate.Duns,
                        Fcode = companyScreening.FamilyTreeInfo.DomesticUltimate.Fcode,
                        IsCompanyReportAvailable = companyScreening.FamilyTreeInfo.DomesticUltimate.IsCompanyReportAvailable,
                        Value = companyScreening.FamilyTreeInfo.DomesticUltimate.Value
                    };
                }

                if (companyScreening.FamilyTreeInfo.HqParent != null)
                {
                    data.GlobalUltimateParent = new FamilyTreeParentDisplayInfo()
                    {
                        Duns = companyScreening.FamilyTreeInfo.HqParent.Duns,
                        Fcode = companyScreening.FamilyTreeInfo.HqParent.Fcode,
                        IsCompanyReportAvailable = companyScreening.FamilyTreeInfo.HqParent.IsCompanyReportAvailable,
                        Value = companyScreening.FamilyTreeInfo.HqParent.Value
                    };
                }
            }

            // splits
            if (companyArchive != null)
            {
                PublicListingCollection publicListingCollection = companyArchive.PublicListingCollection;
                if (publicListingCollection != null && publicListingCollection.Count > 0)
                {
                    data.Splits = publicListingCollection[0].Splits.SplitCollection;
                }
            }

            ////// providers
            ////data.PrimaryCompanyProvider = xxx;
            ////data.SecondaryCompanyProvider = xxx;

            return data;
        }

       
        public static string GetLegalStatusDescription(CodeCollection legalStatusCollection)
        {
            if (legalStatusCollection == null || legalStatusCollection.Count == 0) return null;
            foreach (var legalStatus in legalStatusCollection)
            {
                if (legalStatus != null && 
                    !string.IsNullOrEmpty(legalStatus.Scheme) &&
                    legalStatus.Scheme == "FactivaLegalStatus" && 
                    legalStatus.Descriptor != null && 
                    !string.IsNullOrEmpty(legalStatus.Descriptor.Value))
                {

                    return legalStatus.Descriptor.Value;
                }
            }
            
            return null;
        }

       
        private static bool IsValid(string input)
        {
            if (input != null && input.Trim() != String.Empty)
                return true;

            return false;
        }

        private static string AssembleCompanyPhoneFaxNumber(string ccode, string acode, string number)
        {
            StringBuilder sb = new StringBuilder();

            if (IsValid(ccode))
                sb.AppendFormat("{0}-", ccode);
            if (IsValid(acode))
                sb.AppendFormat("{0}-", acode);
            sb.Append(number);

            return sb.ToString();
        }

         
    }
}
