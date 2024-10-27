using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXTransactionTypeTests
{
    [TestMethod]
    public void DescriptionTest()
    {
        // ensure they all have descriptions.
        foreach (var item in Enum.GetValues<OFXTransactionType>())
        {
            var actual = item.Description();
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        Assert.AreEqual("UNKNOWN", ((OFXTransactionType)1234).Description()); // test invalid enum
    }
}
