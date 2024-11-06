using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var doc = createRequest();
            //makeDoc();
            //var doc = OFXDocument.ParseFile("C:\\Users\\bill\\Downloads\\Chase6020_Activity20230101_20231231_20241104.QFX");
            var writer = new StringWriter();
            var request = new OFXWriter(writer, indent: true);
            request.Write(doc);
            var final = writer.ToString();
        }

        private static OFXDocument createRequest()
        {
            var ticker = "MSFT";
            var name = "Microsoft";
            var price = 431.95m;
            var priceDate = new DateTime(2024, 11, 4, 8, 2, 0, 0);
            var units = 0.040m;

            var positionList = new PositionList();
            var securityList = new SecurityList();

            positionList.Items.Add(new Position(ticker)
            {
                SecurityType = Security.SecurityTypes.STOCK,
                SubAccountType = Position.PositionAccountTypes.CASH,
                MarketValue = price * units,
                PriceDate = priceDate,
                PositionType = Position.PositionTypes.LONG,
                UnitPrice = price,
                Units = units,
            });
            securityList.Items.Add(new Security(ticker, ticker, name, price) { SecurityType = Security.SecurityTypes.STOCK });

            var doc = new OFXDocument().DefaultHeaders();
            var set = new OFXMessageSet(OFXMessageSetTypes.SIGNON, OFXMessageDirection.RESPONSE);
            var status = new OFXStatus("Successful Sign On");

            // Signon Response Message
            var signonMessage = new OFXMessage()
            {
                Name = "SONRS",
                Status = status,
            };
            signonMessage.Properties
                .Add(OFXLanguage.ENG)
                .Add(new Institution() { Name = "broker.com" });
            set.Messages.Add(signonMessage);

            // Investment Response Message
            var investmentMessageSet = new OFXMessageSet(OFXMessageSetTypes.INVSTMT, OFXMessageDirection.RESPONSE, version: 1);
            var investmentMessage = new OFXPositionListResponse(
                positionList,
                new Account()
                {
                    AccountId = "0123456789",
                    AccountType = Account.AccountTypes.INVESTMENT,
                    InstitutionId = "dummybroker.com",
                },
                DateTime.Now,
                id: "12345",
                cookie: "4");
            investmentMessageSet.Messages.Add(investmentMessage);
            // Security List Response
            var securityMessage = new OFXSecurityListResponse(securityList);


            doc.MessageSets.Add(set);
            doc.MessageSets.Add(investmentMessageSet);
            doc.MessageSets.Add(securityMessage);

            return doc;
        }

        private static void makeDoc()
        {
            var doc = new OFXDocument().DefaultHeaders();
            var set = new OFXMessageSet(OFXMessageSetTypes.SIGNON, OFXMessageDirection.RESPONSE);
            var status = new OFXStatus("Successful signon");
            var message = new OFXMessage()
            {
                Name = "SONRS",
                Status = status,
            };
            var fi = new Institution() { Name = "DI", Id = "221379824" };

            message.Properties
                .Add(OFXLanguage.ENG)
                .Add(fi);

            set.Messages.Add(message);
            doc.MessageSets.Add(set);

            var sw1 = new StringWriter();
            var writer = new OFXWriter(sw1);

            writer.Write(doc);
            var first = sw1.ToString();

            var sw2 = new StringWriter();
            writer = new OFXWriter(sw2);
            writer.Write(doc, indent: false);
            var second = sw2.ToString();
        }
    }

}

