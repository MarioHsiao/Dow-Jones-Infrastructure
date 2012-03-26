using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IServicePartResults<TServicePartResult, TPackage> : IServiceResult
        where TServicePartResult : IServicePartResult<TPackage>
        where TPackage : IPackage
    {
        IEnumerable<TServicePartResult> PartResults
        {
            get;
        }

        int MaxPartsAvailable { get; set; }
    }

    public interface IServicePartResult<TPackage>  where TPackage : IPackage
    {
        string Identifier
        {
            get;
            set;
        }

        long ReturnCode
        {
            get;
            set;
        }

        long ElapsedTime
        {
            get;
            set;
        }

        string StatusMessage
        {
            get;
            set;
        }

        string PackageType
        {
            get; 
            set;
        }

        TPackage Package
        {
            get;
            set;
        }
    }


   
    public interface IPageServicePartResult<TPackage> where TPackage : IPackage
    {
        
        long ReturnCode
        {
            get;
            set;
        }

        long ElapsedTime
        {
            get;
            set;
        }

        string StatusMessage
        {
            get;
            set;
        }

        TPackage Package
        {
            get;
            set;
        }
    }
}