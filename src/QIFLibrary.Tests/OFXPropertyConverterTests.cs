using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tudormobile.QIFLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Tudormobile.QIFLibrary.Interfaces;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Converters;

namespace QIFLibrary.Tests;

[TestClass]
public class OFXPropertyConverterTests
{
    [TestMethod, ExcludeFromCodeCoverage, ExpectedException(typeof(NotSupportedException))]
    public void ConvertTest1()
    {
        var target = new OFXPropertyConverter();
        var prop = new OFXProperty("some name");
        target.Convert<String>(prop);   // <-- throws
    }

    [TestMethod]
    public void ConvertTest2()
    {
        var name = "some name";
        var target = new OFXPropertyConverter();
        var prop = new OFXProperty(name);
        var actual = target.Convert<String>(prop, new TestConverter());
        Assert.AreEqual(name, actual);
    }

    [ExcludeFromCodeCoverage]
    internal class TestConverter : IPropertyConverter<string>
    {
        public string? Convert(OFXProperty root) => root?.Name;
    }

    [TestMethod]
    public void GetInstitutionTest1()
    {
        var name = "institution name";
        var id = "identifier";
        var p = new OFXProperty("FI");
        p.Children.Add(new OFXProperty("FID", id));
        p.Children.Add(new OFXProperty("ORG", name));

        var target = new OFXPropertyConverter();
        var result = target.GetInstitution(p);

        Assert.AreEqual(name, result!.Name);
        Assert.AreEqual(id, result.Id);

        result = target.Convert<Institution>(p);
        Assert.AreEqual(name, result!.Name);
        Assert.AreEqual(id, result!.Id);
    }

    [TestMethod]
    public void GetInstitutionTest2()
    {
        var name = "institution name";
        var id = "identifier";
        var p = new OFXProperty("FI");
        p.Children.Add(new OFXProperty("FID", id));
        p.Children.Add(new OFXProperty("ORG", name));

        var root2 = new OFXProperty("root2");
        var root1 = new OFXProperty("root1");
        root1.Children.Add(root2);
        root2.Children.Add(p);

        var target = new OFXPropertyConverter();
        var result = target.GetInstitution(root1);

        Assert.AreEqual(name, result!.Name);
        Assert.AreEqual(id, result.Id);

        result = target.Convert<Institution>(p);
        Assert.AreEqual(name, result!.Name);
        Assert.AreEqual(id, result!.Id);
    }

    [TestMethod]
    public void GetInstitutionTest3()
    {
        var data = new OFXProperty("not FI record");
        data.Children.Add(new OFXProperty("not child"));
        var target = new OFXPropertyConverter();
        var actual = target.GetInstitution(data);
        Assert.IsNull(actual, "Should have found (null) - no institution.");
    }

}