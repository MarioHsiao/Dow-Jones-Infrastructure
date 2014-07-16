using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.DependencyInjection;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Numerical;
using DowJones.Globalization;

namespace DowJones.Assemblers.Newsletters
{
    public abstract class AbstractNewsletterListDataResultConverter : IListDataResultConverter
    {
        private readonly DateTimeFormatter _dateTimeFormatter;
        private readonly NumberFormatter _numberFormatter;
        private readonly IResourceTextManager _resources;

        protected AbstractNewsletterListDataResultConverter(string interfaceLanguage)
            : this(new DateTimeFormatter(interfaceLanguage))
        {
        }

        protected AbstractNewsletterListDataResultConverter(DateTimeFormatter dateTimeFormatter)
            : this(dateTimeFormatter, null, null)
        {
        }

        protected AbstractNewsletterListDataResultConverter(DateTimeFormatter dateTimeFormatter, NumberFormatter numberFormatter, IResourceTextManager resource)
        {
            _dateTimeFormatter = dateTimeFormatter ?? new DateTimeFormatter("en");
            _numberFormatter = numberFormatter ?? new NumberFormatter();
            _resources = resource ?? ServiceLocator.Resolve<IResourceTextManager>();
        }

        public abstract Ajax.IListDataResult Process();

        public DateTimeFormatter DateTimeFormatter
        {
            get { return _dateTimeFormatter; }
        }

        public NumberFormatter NumberFormatter
        {
            get { return _numberFormatter; }
        }

        public IResourceTextManager ResourceText
        {
            get { return _resources; }
        }
    }
}
