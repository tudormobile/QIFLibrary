using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using Tudormobile.QIFLibrary;
using Tudormobile.QIFLibrary.Entities;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // create a document
            var doc = new OFXDocument().DefaultHeaders();
            var set = new OFXMessageSet(OFXMessageSetTypes.SIGNON, OFXMessageDirection.RESPONSE);
            var status = new OFXStatus() { Code = 0, Message = "Successful signon", Severity = OFXStatus.StatusSeverity.INFO };
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

