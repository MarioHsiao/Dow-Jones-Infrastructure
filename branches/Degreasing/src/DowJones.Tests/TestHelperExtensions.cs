using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones
{
    public static class TestHelperExtensions
    {
        public static void ExpectException<TException>(this Action action, string message = null) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                // PASS!
                return;
            }

            Assert.Fail(
                message 
                ?? string.Format("Expected exception {0} but it was never thrown", typeof(TException))
            );
        }

    }
}
