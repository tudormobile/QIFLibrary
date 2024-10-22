using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFDocumentTypeTests
{
    [TestMethod]
    public void AsStringTest()
    {
        var values = new HashSet<string>();
        foreach (QIFDocumentType value in Enum.GetValues(typeof(QIFDocumentType)))
        {
            var actual = value.AsString();
            Assert.IsTrue(values.Add(actual), "All values mus be unique.");
            Assert.IsFalse(String.IsNullOrWhiteSpace(actual), "Must provide a string value.");
        }
    }

    [TestMethod]
    public void DescriptionTest()
    {
        var values = new HashSet<string>();
        foreach (QIFDocumentType value in Enum.GetValues(typeof(QIFDocumentType)))
        {
            var actual = value.Description();
            Assert.IsTrue(values.Add(actual), "All values mus be unique.");
            Assert.IsFalse(String.IsNullOrWhiteSpace(actual), "Must provide a string value.");
        }
    }
}