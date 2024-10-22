using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary;

namespace QIFLibrary.Tests;

[TestClass]
public class QIFCategoryRecordTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        var memo = "some memo";
        var category = "one:two:three";
        var budgeted = 1.23m;
        var target = new QIFCategoryRecord(category, memo, budgeted);
        Assert.AreEqual(category, target.Category);
        Assert.AreEqual(memo, target.Memo);
        Assert.AreEqual(budgeted, target.Budgeted);
    }
}
