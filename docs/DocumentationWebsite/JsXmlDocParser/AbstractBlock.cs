using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JsXmlDocParser
{
    [DebuggerDisplay("{FullName}")]
	public abstract class AbstractBlock : IBlock
	{
        public IBlockStarter BlockStarter { get; protected set; }
		
        public List<IBlock> Children { get; protected set; }
		
        public List<string> Comments { get; protected set; }
        
        public virtual string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fullName))
                {
                    if (string.IsNullOrWhiteSpace(Namespace))
                        return Name;
                    return string.Format("{0}.{1}", Namespace, Name);
                }
                return _fullName;
            }
            protected set { _fullName = value; }
        }
        private string _fullName;

        public string Name { get; protected set; }

        public string Namespace { get; protected set; }


		protected AbstractBlock(string line)
		{
			Children = new List<IBlock>();
			Comments = new List<string>();
		}

        public IBlock Child(string name)
        {
            return Children.FirstOrDefault(x => x.Name == name);
        }

        public T Child<T>(string name)
            where T : IBlock
        {
            return Children.OfType<T>().FirstOrDefault(x => x.Name == name);
        }
	}
}