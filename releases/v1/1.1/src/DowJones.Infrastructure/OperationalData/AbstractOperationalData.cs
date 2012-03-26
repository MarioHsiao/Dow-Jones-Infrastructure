using System;
using System.Collections.Generic;

namespace DowJones.Utilities.OperationalData
{
    /// <summary>
    /// This is a base class for operational data that encapsulate basic behaviour of creating memento and restoring state
    /// from memento. Also uses decorator pattern to add additional operational data when it used from client side.
    /// Producer of memento does not need to set component. 
    /// </summary>
    public abstract class AbstractOperationalData : IBaseOperationalData
    {
        private IBaseOperationalData _component;
        protected const char MULTIPLE_ART_VALUE_SPILTER = '~';

        protected IDictionary<string, string> List
        {
            get { return _list; }
        }

        private IDictionary<string, string> _list;


        protected AbstractOperationalData()
        {
            _list = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        protected AbstractOperationalData(IDictionary<string, string> list)
        {
            _list = list;
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
        /// </summary>
        public string GetMemento
        {
            get
            {
                IParser parser = GetParser;
                IParser decorator = new ParserDecorator(parser);
                return decorator.Encrypt(_list);
            }
        }

        /// <summary>
        /// Get encrypted string which is returns from GetMemento and restore dto state
        /// </summary>
        /// <param name="state"></param>
        public void SetMemento(string state)
        {
            if(string.IsNullOrEmpty(state))
            {
                return;
            }
            IParser parser = ParserFactory.GetParser(state.Substring(0, 2));
            ParserDecorator decorator = new ParserDecorator(parser);
            _list = decorator.Decrypt(state);
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
                IEnumerator<KeyValuePair<string, string>> enumerator = _list.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    nvp.Add(ODSConstants.ODS_PREFIX_FOR_KEY + enumerator.Current.Key, enumerator.Current.Value);
                }
                if (_component != null)
                {
                    IEnumerator<KeyValuePair<string, string>> childEnum = _component.GetKeyValues.GetEnumerator();
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
            _list[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected internal string Get(string key)
        {
            return _list.ContainsKey(key) ? _list[key] : null;
        }
    }
}