using System;
using System.Collections.Generic;
using System.Web;
using DowJones.Utilities.Generators;
using log4net.Appender;
using log4net.Core;

namespace DowJones.Utilities.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpContextAppender : AppenderSkeleton
    {
        /// <summary>
        /// Value indicating which fields in the event should be fixed
        /// </summary>
        /// <remarks>
        /// By default all fields are fixed
        /// </remarks>
        private FixFlags m_fixFlags = FixFlags.All;
        private readonly string m_HttpContextItemKey;

        public HttpContextAppender()
        {
            if (HttpContext.Current == null)
            {
                throw new NullReferenceException("Appender will not  work without access to HttpContext.Current");
            }
            m_HttpContextItemKey = RandomKeyGenerator.GetRandomKey(5, RandomKeyGenerator.CharacterSet.Alpha) + "__tracer";
        }


        /// <summary>
        /// Gets the tracer.
        /// </summary>
        /// <returns></returns>
        private List<LoggingEvent> m_EventList
        {
            get
            {
                List<LoggingEvent> items;
                if (HttpContext.Current == null)
                    return null;
                if (!HttpContext.Current.Items.Contains(m_HttpContextItemKey))
                {
                    items = new List<LoggingEvent>();
                    HttpContext.Current.Items[m_HttpContextItemKey] = items;
                }
                else
                {
                   items = (List<LoggingEvent>)HttpContext.Current.Items[m_HttpContextItemKey];
                }
                return items;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [requires layout].
        /// </summary>
        /// <value><c>true</c> if [requires layout]; otherwise, <c>false</c>.</value>
        protected override bool RequiresLayout
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets a the fields that will be fixed in the event
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging event needs to have certain thread specific values 
        /// captured before it can be buffered. See <see cref="LoggingEvent.Fix"/>
        /// for details.
        /// </para>
        /// </remarks>
        virtual public FixFlags Fix
        {
            get { return m_fixFlags; }
            set { m_fixFlags = value; }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (m_EventList == null)
                return;
            loggingEvent.Fix = Fix;
            m_EventList.Add(loggingEvent);
        }

        /// <summary>
        /// Gets the events that have been logged.
        /// </summary>
        /// <returns>The events that have been logged</returns>
        /// <remarks>
        /// <para>
        /// Gets the events that have been logged.
        /// </para>
        /// </remarks>
        virtual public LoggingEvent[] GetEvents()
        {
            if (m_EventList == null)
                return new LoggingEvent[0];
            return m_EventList.ToArray();
        }

        /// <summary>
        /// Gets the events that have been logged.
        /// </summary>
        /// <returns>The events that have been logged</returns>
        /// <remarks>
        /// <para>
        /// Gets the events that have been logged.
        /// </para>
        /// </remarks>
        virtual public List<LoggingEvent> GetEventsList()
        {
            if (m_EventList == null)
                return new List<LoggingEvent>();
            return new List<LoggingEvent>(m_EventList);
        }

        /// <summary>
        /// Clear the list of events
        /// </summary>
        /// <remarks>
        /// Clear the list of events
        /// </remarks>
        virtual public void Clear()
        {
            if (m_EventList == null)
                return;
            m_EventList.Clear();
        }

    }
}