using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace QIFLibrary.Tests
{
    [TestClass]
    public class OFXInvestmentPositionListTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var data = new PositionList();
            var id = "MSFT";
            var date = new DateTime(2024, 11, 05, 12, 37, 06, DateTimeKind.Utc);
            var position = new Position(id)
            {
                Memo = "memo",
                PriceDate = date,
                PositionType = Position.PositionTypes.LONG,
                MarketValue = 12345.67m,
                SecurityType = Security.SecurityTypes.OPTION,
                UnitPrice = 987.65m,
                SubAccountType = Position.PositionAccountTypes.MARGIN,
                Units = 112233m,
            };
            data.Items.Add(position);

            var target = new OFXInvestmentPositionList(data);

            Assert.HasCount(1, target.Children);

            Assert.AreEqual("INVPOSLIST", target.Name);
        }
    }
}
