using BundlerMinifier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUglify;
using NUglify.Css;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BundlerMinifierTest
{
    [TestClass]
    public class EncodingTest
    {
        private const string TEST_BUNDLE = "../../../artifacts/test1.json";
        private BundleFileProcessor _processor;
        private Guid _guid;

        [TestInitialize]
        public void Setup()
        {
            _processor = new BundleFileProcessor();
            _guid = Guid.NewGuid();
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete("../../../artifacts/" + _guid + ".json");
            File.Delete("../../../artifacts/foo.js");
            File.Delete("../../../artifacts/foo.min.js");
            File.Delete("../../../artifacts/foo.min.js.map");
            File.Delete("../../../artifacts/foo.css");
            File.Delete("../../../artifacts/foo.min.css");
            File.Delete("../../../artifacts/foo.html");
            File.Delete("../../../artifacts/foo.min.html");
            File.Delete("../../../artifacts/encoding/encoding.js");
            File.Delete("../../../artifacts/encoding/encoding.min.js");
        }

        [TestMethod, TestCategory("Encoding")]
        public void ProcessWithDifferentEncoding()
        {
            _processor.Process("../../../artifacts/encoding/encoding.json");

            string jsResult = File.ReadAllText("../../../artifacts/encoding/encoding.js");
            Assert.AreEqual("var bom = 'àèéèùì';\r\nvar nobom = 'àèéèùì'", jsResult);
        }

        [TestMethod, TestCategory("Encoding")]
        public void Encoding()
        {
            string jsResult = FileHelpers.ReadAllText("../../../artifacts/encoding.js");
            Assert.AreEqual("var test = 'æøå';", jsResult);
        }
        [TestMethod, TestCategory("Encoding")]
        public void EncodingCSS()
        {
            string cssResult = FileHelpers.ReadAllText("../../../artifacts/encoding.css");
            Assert.AreEqual("content: \"\\f002\";", cssResult);
        }

        [TestMethod, TestCategory("Encoding")]
        public void EncodingCSS_2()
        {
            string cssResult = FileHelpers.ReadAllText("../../../artifacts/encoding.css");
            File.WriteAllText("../../../artifacts/output/encoding.css", "content: \"\\f002\";", new UTF8Encoding(false));
            Assert.AreEqual("content: \"\\f002\";", cssResult);
        }

        [TestMethod, TestCategory("Encoding")]
        public void EncodingCSS_3()
        {
            string cssResult = FileHelpers.ReadAllText("../../../artifacts/encoding_3.css");

            var uglyResult = Uglify.Css(cssResult);
            File.WriteAllText("../../../artifacts/output/encoding_3.css", uglyResult.Code, new UTF8Encoding(false));



            cssResult = cssResult
                .Replace("\t", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("    ", string.Empty)
                .Replace(" {", "{")
                .Replace(": ", ":")
                .Replace(" 0px", "0")
                ;
            File.WriteAllText("../../../artifacts/output/encoding_3_regex.css", cssResult, new UTF8Encoding(false));

            Assert.AreEqual("content: \"\\f002\";", "content: \"\\f002\";");
        }
    }
}
