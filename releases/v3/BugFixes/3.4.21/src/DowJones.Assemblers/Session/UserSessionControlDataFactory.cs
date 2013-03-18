using System;
using System.Web;
using DowJones.Session;

namespace DowJones.Assemblers.Session
{
    [Obsolete("Use ControlDataFactory instead (class renamed)")]
    public class UserSessionControlDataFactory : ControlDataFactory
    {
        public UserSessionControlDataFactory(IUserSession session, HttpRequestBase request) 
            : base(session, request)
        {
        }
    }
}
