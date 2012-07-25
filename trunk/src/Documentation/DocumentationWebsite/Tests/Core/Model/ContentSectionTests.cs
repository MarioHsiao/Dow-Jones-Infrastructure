using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Documentation.Tests.Core.Model
{
    [TestClass]
    public class ContentSectionTests
    {
        [TestMethod]
        public void ShouldAddChild()
        {
            var child = new ContentSection();
            var parent = new ContentSection();
            parent.Add(child);

            Assert.AreSame(child, parent.Children.First());
        }

        [TestMethod]
        public void ShouldSetSelfAsParentWhenAddingAChild()
        {
            var child = new ContentSection();
            var parent = new ContentSection();
            parent.Add(child);

            Assert.AreSame(child.Parent, parent);
        }

        [TestMethod]
        public void ShouldFindChildByName()
        {
            var child = new ContentSection("Test");
            var parent = new ContentSection();
            parent.Add(child);

            Assert.AreSame(child, parent.Find(child.Name));
        }
    }
}
