﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

    }
}