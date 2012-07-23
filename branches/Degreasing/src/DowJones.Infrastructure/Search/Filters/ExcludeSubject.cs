using System;
using System.Collections.Generic;

namespace DowJones.Search
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ExcludeSubject : Attribute
    {
        private readonly string[] _subjects = new string[0];


        public ExcludeSubject(string subjects)
        {
            if (!String.IsNullOrEmpty(subjects))
            {
                _subjects = subjects.Split(',');
            }
        }
        public IList<string> Subjects
        {
            get { return _subjects; }
        }
    }
}