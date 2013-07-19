using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GWShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;
namespace DowJones.Pages.Common
{
    public abstract class AbstractShareAssets<T> : IShareAssets
    {
        private List<T> _assets;
        public List<T> Assets
        {
            get { return _assets ?? (_assets = new List<T>()); }
            set { _assets = value; } 
        }
        public GWShareScope Scope { get; set; }
        public abstract void Share();
    
    }
}
