using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

internal class QIFRecordBuilder(QIFDocumentType dataType) : IBuilder<QIFRecord>
{
    private readonly Dictionary<Char, String> _details = [];

    /// <summary>
    /// Builds the QIFRecord from accumulated data.
    /// </summary>
    /// <returns>A new QIF Record.</returns>
    public QIFRecord Build()
    {
        if (Value('!') == "Account" || dataType == QIFDocumentType.Account)
        {
            return new QIFAccountRecord(Check(), Value('D'), Value('T'), Value('$', ParseDecimal), Value('L', ParseDecimal));
        }
        return dataType switch
        {
            QIFDocumentType.Investment => buildInvestment(),
            QIFDocumentType.Bank => buildBankRecord(),
            QIFDocumentType.Category => buildCategory(),
            _ => new QIFRecord(Date(), Amount(), Memo(), Status())
        };
    }

    internal QIFRecordBuilder Add(string line)
    {
        _details[line[0]] = line[1..];  // allows duplicates; last one wins.
        return this;
    }
    internal QIFRecordBuilder Clear()
    {
        _details.Clear();
        return this;
    }

    private QIFCategoryRecord buildCategory()
        => new QIFCategoryRecord(Category(), Memo(), Budgeted());

    private QIFBankRecord buildBankRecord()
        => new QIFBankRecord(Date(), Amount(), Memo(), Status(),
            Payee(),
            Category(),
            Address(),
            Check(), Flagged());

    private QIFInvestment buildInvestment()
    {
        if (Enum.TryParse<QIFInvestmentType>(InvestAction(), out var investmentType))
        {
            // Investment actions
            return InvestAction() switch
            {
                "Buy" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "BuyX" => throw new NotSupportedException(),
                "Sell" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "SellX" => throw new NotSupportedException(),
                "ShtSell" => throw new NotSupportedException(),
                "CvrShrt" => throw new NotSupportedException(),
                "CGLong" => throw new NotSupportedException(),
                "CGLongX" => throw new NotSupportedException(),
                "CGMid" => throw new NotSupportedException(),
                "CGMidX" => throw new NotSupportedException(),
                "CGShort" => throw new NotSupportedException(),
                "CGShortX" => throw new NotSupportedException(),
                "Div" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "DivX" => throw new NotSupportedException(),
                "IntInc" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "IntIncX" => throw new NotSupportedException(),
                "ReinvDiv" => throw new NotSupportedException(),
                "ReinvInt" => throw new NotSupportedException(),
                "ReinvLg" => throw new NotSupportedException(),
                "ReinvMd" => throw new NotSupportedException(),
                "ReinvSh" => throw new NotSupportedException(),
                "Reprice" => throw new NotSupportedException(),
                "XIn" => throw new NotSupportedException(),
                "XOut" => throw new NotSupportedException(),
                "MiscExp" => throw new NotSupportedException(),
                "MiscExpX" => throw new NotSupportedException(),
                "MiscInc" => throw new NotSupportedException(),
                "MiscIncX" => throw new NotSupportedException(),
                "MargInt" => throw new NotSupportedException(),
                "MargIntX" => throw new NotSupportedException(),
                "RtrnCap" => throw new NotSupportedException(),
                "RtrnCapX" => throw new NotSupportedException(),
                "StkSplit" => throw new NotSupportedException(),
                "ShrsOut" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "ShrsIn" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),

                _ => throw new NotSupportedException()
            };
        }
        return QIFInvestment.Empty;

    }

    private DateTime Date() => Value('D', ParseQIFDate, DateTime.MinValue);
    private Decimal Amount() => Value('T', s => Decimal.Parse(s), Value('U', s => Decimal.Parse(s), default));
    private string Memo() => Value('M');
    private string Status() => Value('C');
    private string Check() => Value('N');
    private string Payee() => Value('P');
    private string Address() => Value('A');
    private string Category() => Value('L');
    private bool Flagged() => Value('F', s => true, false);
    private string SplitCategory() => Value('S');
    private string SplitMemo() => Value('E');
    private Decimal SplitAmount() => Value('$', ParseDecimal);
    private Decimal SplitPercent() => Value('%', ParseDecimal);
    private string InvestAction() => Value('N');
    private string SecurityName() => Value('Y');
    private Decimal Price() => Value('I', ParseDecimal);
    private Decimal Quantity() => Value('Q', ParseDecimal);
    private Decimal Commission() => Value('O', ParseDecimal);
    private Decimal AmountTransferred() => Value('$', ParseDecimal);
    private Decimal Budgeted() => Value('B', ParseDecimal);
    private string ExtendedData() => Value('X');

    private string Value(char key)
        => Value(key, s => s, String.Empty);

    private T Value<T>(char key, Func<string, T> f) where T : struct
        => _details.TryGetValue(key, out string? value) ? f(value) : default;

    private T Value<T>(char key, Func<string, T> f, T defaultValue)
        => _details.TryGetValue(key, out string? value) ? f(value) : defaultValue;

    private static decimal ParseDecimal(string s)
        => Decimal.TryParse(s, out var decimalValue) ? decimalValue : 0;

    private static DateTime ParseQIFDate(string s)
        => DateTime.TryParse(s.Replace("'", "/"), out DateTime result) ? result : DateTime.MinValue;
}
