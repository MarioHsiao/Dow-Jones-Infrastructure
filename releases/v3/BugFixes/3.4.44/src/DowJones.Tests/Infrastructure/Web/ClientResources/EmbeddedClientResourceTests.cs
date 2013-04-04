using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Web.ClientResources
{
    [TestClass]
    public class EmbeddedClientResourceTests : UnitTestFixture
    {
        private Type _declaringType;
        private IEnumerable<ClientResourceAttribute> _resourceAttributes;
        private IEnumerable<ClientResource> _resources;

        [TestInitialize]
        public void TestInitialize()
        {
            _declaringType = typeof (DemoComponent);
            _resourceAttributes = _declaringType.GetClientResourceAttributes();
            _resources = _resourceAttributes.Select(x => x.ToClientResource(_declaringType));
        }

        [TestMethod]
        public void ShouldLocateClientTemplates()
        {
            var resourcesWithClientTemplates = _resources.Where(x => x.ClientTemplates.Any());

            Assert.IsTrue(resourcesWithClientTemplates.Any());
        }

        [TestMethod]
        public void ShouldOnlyAttachClientTemplatesToOneScriptReference()
        {
            var scripts = _resources.Where(x => x.ResourceKind == ClientResourceKind.Script);
            var scriptsWithClientTemplates = scripts.Where(x => x.ClientTemplates.Any());

            Assert.AreEqual(1, scriptsWithClientTemplates.Count());
        }

        [TestMethod]
        public void ShouldNotAttachClientTemplatesToInheritedScriptReferences()
        {
            var declaringTypeScripts = _declaringType.GetClientResourceAttributes(false);
            var scripts = declaringTypeScripts.Select(x => x.ToClientResource(_declaringType));
            var declaringTypeScriptsWithClientTemplates = scripts.Where(x => x.ClientTemplates.Any());

            Assert.AreEqual(1, declaringTypeScriptsWithClientTemplates.Count());
        }

        [TestMethod]
        public void ShouldAttachClientTemplatesToFirstScriptReference()
        {
            var scripts = _resources.Where(x => x.ResourceKind == ClientResourceKind.Script).ToArray();
            var firstScript = scripts.First();

            Assert.IsTrue(firstScript.ClientTemplates.Any());
        }


        [ScriptResource(RelativeResourceName= "Demo.Demo.js", DeclaringType = typeof(DemoComponent))]
        [ScriptResource(RelativeResourceName = "Demo.some-other-script.js", DeclaringType = typeof(DemoComponent))]
        [ClientTemplateResource(RelativeResourceName = "Demo.ClientTemplates.awesome.js", DeclaringType = typeof(DemoComponent))]
        public class DemoComponent : DemoComponentBase
        {}

        [ScriptResource(RelativeResourceName= "Demo.DemoBase.js", DeclaringType = typeof(DemoComponentBase))]
        public class DemoComponentBase
        {}
    }
}
