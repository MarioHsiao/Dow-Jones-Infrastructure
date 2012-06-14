using System;

namespace DowJones.Documentation.Tests.Tools.VSDocPreprocessor.TestData
{
    /// <summary>
    /// A test enum
    /// </summary>
    /// <remarks>
    /// This class is awesome.
    /// </remarks>
    /// <example>
    /// To use this class, just instantiate it like this:
    /// <code>var class = new TestClass("Awesome!");</code>
    /// </example>
    public enum TestEnum
    {
        /// <summary>
        /// The first value
        /// </summary>
        Value1,

        /// <summary>
        /// The second value
        /// </summary>
        Value2
    }

    /// <summary>
    /// A test class with a name that looks very similar to TestClass
    /// </summary>
    public class TestClassBuilder
    {
        /// <summary>
        /// Creates a new TestClassBuilder
        /// </summary>
        public TestClassBuilder()
        {
        }

        /// <summary>
        /// Builds the TestClass
        /// </summary>
        /// <param name="class">The TestClass to build</param>
        public void Build(TestClass @class)
        {
        }
    }

    /// <summary>
    /// A test class that emits VSDoc documentation
    /// </summary>
    public class TestClass
    {
        /// <summary>
        /// A read/write static property
        /// </summary>
        public static string StaticProperty { get; set; }

        /// <summary>
        /// A test enum declared as an inner class
        /// </summary>
        public enum TestInnerEnum
        {
            /// <summary>
            /// The first value
            /// </summary>
            Value1,

            /// <summary>
            /// The second value
            /// </summary>
            Value2
        }

        /// <summary>
        /// Public event
        /// </summary>
        public event EventHandler Event;

        /// <summary>
        /// A primary type property
        /// </summary>
        public string Property1 { get; set; }

        /// <summary>
        /// This is a complex type property
        /// </summary>
        public Tuple<int, string> Property2 { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public TestClass()
        {
        }

        /// <summary>
        /// A constructor with one parameter
        /// </summary>
        /// <param name="param1">The first parameter</param>
        public TestClass(string param1)
        {
        }

        /// <summary>
        /// A constructor with two parameters
        /// </summary>
        /// <param name="param1">The first parameter</param>
        /// <param name="param2">The second parameter</param>
        public TestClass(string param1, string param2)
        {
        }


        /// <summary>
        /// A method that doesn't actually do or return anything
        /// </summary>
        /// <param name="param">The parameter for the method</param>
        public void VoidMethod(int param)
        {
        }

        /// <summary>
        /// A method that doesn't accept parameters
        /// </summary>
        public void MethodWithNoParameters()
        {
        }

        /// <summary>
        /// A method that returns a complex type
        /// </summary>
        /// <param name="param">The parameter for the method</param>
        public Uri ComplexMethod(Uri param)
        {
            return param;
        }


        /// <summary>
        /// The static version of the VoidMethod
        /// </summary>
        /// <param name="param">The parameter for the method</param>
        public static void StaticVoidMethod(string param)
        {
        }
    }
}
