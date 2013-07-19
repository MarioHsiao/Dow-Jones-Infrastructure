using System;
using System.Collections.Generic;
using DowJones.Extensions;

namespace DowJones.OperationalData
{
    /// <summary>
    /// This is a base class for operational data that encapsulate basic behaviour of creating memento and restoring state
    /// from memento. Also uses decorator pattern to add additional operational data when it used from client side.
    /// Producer of memento does not need to set component. 
    /// </summary>
    public abstract class AbstractOperationalData : IBaseOperationalData
    {
        private IBaseOperationalData _component;
        protected const char MultipleArtValueSpilter = '~';

        protected IDictionary<string, string> List { get; private set; }


        protected AbstractOperationalData()
        {
            List = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        protected AbstractOperationalData(IDictionary<string, string> list)
        {
            List = list;
        }

        #region Decorator method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseComponent"></param>
        public void SetComponent(IBaseOperationalData baseComponent)
        {
            _component = baseComponent;
        }

        #endregion

        /// Returns operational data state as a encrypted string that can be pass to SetMemento method
        /// <value>
        /// </value>
        public string GetMemento
        {
            get
            {
                var parser = GetParser;
                IParser decorator = new ParserDecorator(parser);
                return decorator.Encrypt(List);
            }
        }

        /// <summary>
        /// Get encrypted string which is returns from GetMemento and restore dto state
        /// </summary>
        /// <param name="state"></param>
        public void SetMemento(string state)
        {
            if(state.IsNullOrEmpty())
            {
                return;
            }
            var parser = ParserFactory.GetParser(state.Substring(0, 2));
            var decorator = new ParserDecorator(parser);
            List = decorator.Decrypt(state);
        }

        
        private static IParser GetParser
        {
            get
            {
                return ParserFactory.GetParser(ParserFactory.ENCRYPTED_NVP);
            }
        }


        #region IBaseOperationalData Members

        public virtual IDictionary<string, string> GetKeyValues
        {
            get
            {
                IDictionary<string, string> nvp = new Dictionary<string, string>();
                var enumerator = List.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    nvp.Add(ODSConstants.ODS_PREFIX_FOR_KEY + enumerator.Current.Key, enumerator.Current.Value);
                }
                if (_component != null)
                {
                    var childEnum = _component.GetKeyValues.GetEnumerator();
                    while (childEnum.MoveNext())
                    {
                        if (!nvp.ContainsKey(childEnum.Current.Key))
                        {
                            nvp.Add(childEnum.Current.Key, childEnum.Current.Value);
                        }
                    }
                }
                return nvp;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected internal void Add(string key, string value)
        {
            List[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected internal string Get(string key)
        {
            return List.ContainsKey(key) ? List[key] : null;
        }
    }
}