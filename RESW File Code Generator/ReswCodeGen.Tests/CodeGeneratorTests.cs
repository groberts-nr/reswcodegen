using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChristianHelle.DeveloperTools.CodeGenerators.Resw.CustomTool.Tests
{
    [TestClass]
    [DeploymentItem("Resources/Resources.resw")]
    public class CodeGeneratorTests
    {
        private string reswFileContents;
        private const string FilePath = "Resources.resw";
        private string actual;
        private ICodeGenerator target;

        [TestInitialize]
        public void Initialize()
        {
            reswFileContents = File.ReadAllText(FilePath);

            target = new CodeGeneratorFactory().Create(FilePath, "TestApp", reswFileContents);
            actual = target.GenerateCode();
        }

        [TestMethod]
        public void GenerateCodeDoesNotReturnNull()
        {
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GeneratedCodeContainsPropertiesDefinedInResources()
        {
            var resourceItems = target.ResourceParser.Parse();
  
            foreach (var item in resourceItems.Where(item => !item.Name.Contains(".")))
                Assert.IsTrue(actual.Contains("public static string " + item.Name));
        }

        [TestMethod]
        public void GeneratedCodePropertiesContainsCommentsSimilarToValuesDefinedInResources()
        {
            var resourceItems = target.ResourceParser.Parse();
       
            foreach (var item in resourceItems.Where(item => !item.Name.Contains(".")))
                Assert.IsTrue(actual.Contains("Localized resource similar to \"" + item.Value + "\""));
        }

        [TestMethod]
        public void ClassNameEqualsFileNameWithoutExtension()
        {
            Assert.IsTrue(actual.Contains("class Resources"));
        }
    }
}