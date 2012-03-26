using Factiva.Gateway.Messages.Search.V2_0;

namespace Factiva.Gateway.Messages.CodedNews
{
    public class IndustryEditorChoiceStructuredQuery : AbstractIndustryStructuredQuery
    {
        protected override void BuildSearchStringList()
        {
            var ns = GetIndustryNewsSubject(Industry.Fcode);
            if (ns == null)
                return;
            var ss = GetNewsSubjectFilter(new[] {ns});
            ListSearchString.Add(ss);
        }


        protected override Dates GetDefaultDateOption()
        {
            var dates = base.GetDefaultDateOption();
            dates.After = DATE_RANGE_LAST_3MONTHS;
            return dates;
        }


        public static string GetIndustryNewsSubject(string fcode)
        {
            string ns = null;
            if (fcode == null)
                return null;

            switch (fcode.Trim().ToLower())
            {
                case "iacc":
                    ns = "REQRAC";
                    break; //ACCOUNTING/CONSULTING
                case "iadv":
                    ns = "REQRAP";
                    break; //ADVERTISING/PR/MARKETING
                case "iaer":
                    ns = "REQRAD";
                    break; //AEROSPACE/DEFENSE
                case "i0":
                    ns = "REQRAF";
                    break; //AGRICULTURE/FORRESTRY
                case "i75":
                    ns = "REQRAI";
                    break; //AIRLINES
                case "iaut":
                    ns = "REQRAU";
                    break; //AUTOMOBILES
                case "ibnk":
                    ns = "REQRBC";
                    break; //BANKING
                    //case "ibcs":		ns="";			break;	//BUSINESS/CONSUMER SERVICES
                case "i25":
                    ns = "REQRCH";
                    break; //CHEMICALS
                case "iclt":
                    ns = "REQRCT";
                    break; //CLOTHING/TEXTILES
                case "i3302":
                    ns = "REQRCM";
                    break; //COMPUTERS/ELECTRONICS
                case "icre":
                    ns = "REQRCR";
                    break; //CONSTRUCTION/REAL ESTATE
                case "icnp":
                    ns = "REQRCP";
                    break; //CONSUMER PRODUCTS
                case "i1":
                    ns = "REQREN";
                    break; //ENERGY
                    //case "iewm":		ns="";			break;	//ENVIRONMENT/WASTE MANAGEMENT
                case "i41":
                    ns = "REQRFB";
                    break; //FOOD/BEVERAGES/TOBACCO
                case "i951":
                    ns = "REQRHC";
                    break; //HEALTH CARE
                case "i66":
                    ns = "REQRHR";
                    break; //HOTELS/RESTAURANTS/CASINOS
                case "i82":
                    ns = "REQRIN";
                    break; //INSURANCE
                case "iint":
                    ns = "REQRIO";
                    break; //INTERNET/ONLINE SERVICES
                case "iinv":
                    ns = "REQRIS";
                    break; //INVESTING/SECURITIES
                    //case "ilea":		ns="";			break;	//LEISURE/ARTS
                    //case "i32":		ns="";			break;	//MACHINERY/INDUSTRIAL GOODS
                case "imed":
                    ns = "REQRME";
                    break; //MEDIA
                case "imet":
                    ns = "REQRMM";
                    break; //METALS/MINING
                    //case "ipap":		ns="";			break;	//PAPER PACKAGING
                case "i257":
                    ns = "REQRPH";
                    break; //PHARMACEUTICALS
                case "i64":
                    ns = "REQRRE";
                    break; //RETAIL
                case "i7902":
                    ns = "REQRTE";
                    break; //TELECOMMUNICATIONS
                case "itsp":
                    ns = "REQRTS";
                    break; //TRANSPORTATION/SHIPPING
            }
            return ns;
        }
    }
}