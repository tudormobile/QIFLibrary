using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.QIFLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIFLibrary.Tests
{
    [TestClass]
    public class OFXWriterTests
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            var writer = new StringWriter();
            var target = new OFXWriter(writer);
            target.Write(new OFXDocument());
            Assert.IsTrue(writer.ToString().Contains($"<OFX>{Environment.NewLine}"));
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            var writer = new StringWriter();
            var target = new OFXWriter(writer, indent: false);
            target.Write(new OFXDocument());
            Assert.IsFalse(writer.ToString().Contains($"<OFX>{Environment.NewLine}"));
        }

        [TestMethod]
        public void ConstructorTest3()
        {
            var writer = new StringWriter();
            var target = new OFXWriter(writer, indent: false);
            target.Write(new OFXDocument(), indent: true);
            Assert.IsTrue(writer.ToString().Contains($"<OFX>{Environment.NewLine}"));
        }


        [TestMethod]
        public void WriteTest1()
        {
            var writer = new StringWriter();
            var target = new OFXWriter(writer, indent: false);

            var filename = Path.Combine("TestAssets", "Quicken.qfx");
            var document = OFXDocument.ParseFile(filename);

            target.Write(document);

            Assert.IsTrue(writer.ToString().EndsWith("</OFX>"));
        }

        [TestMethod]
        public void WriteTest2()
        {
            var writer = new StringWriter();
            var target = new OFXWriter(writer, indent: true);

            var filename = Path.Combine("TestAssets", "Quicken.qfx");
            var document = OFXDocument.ParseFile(filename);

            target.Write(document);

            Assert.IsTrue(writer.ToString().EndsWith("</OFX>"));
        }

        [TestMethod]
        public void WriteTest3()
        {
            var doc = new OFXDocument().DefaultHeaders();
            double quantity = 0.01 + (double)DateTime.Now.Day / 1000.0;

            var signon = new OFXMessageSet(OFXMessageSetTypes.SIGNON, OFXMessageDirection.RESPONSE);
            var invstm = new OFXMessageSet(OFXMessageSetTypes.INVSTMT, OFXMessageDirection.RESPONSE);
            var seclst = new OFXMessageSet(OFXMessageSetTypes.SECLIST, OFXMessageDirection.RESPONSE);

            var signonMsg = new OFXMessage() { Name = "SONRS" };
            var invstmMsg = new OFXMessage() { Name = "INVSTMTTRNRS", Id = "12345" };
            var seclstMsg = new OFXMessage() { Name = "SECLIST" };

            signon.Messages.Add(signonMsg);
            invstm.Messages.Add(invstmMsg);
            seclst.Messages.Add(seclstMsg);

            doc.MessageSets.Add(signon);
            doc.MessageSets.Add(invstm);
            doc.MessageSets.Add(seclst);

            using var sw = new StringWriter();
            var writer = new OFXWriter(sw, indent: true);
            writer.Write(doc);
            var result = sw.ToString();

            Assert.AreEqual(24, result.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Length);

        }

    }
}