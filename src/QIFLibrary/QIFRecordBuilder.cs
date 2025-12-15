namespace Tudormobile.QIFLibrary;

internal class QIFRecordBuilder(QIFDocumentType dataType) : IBuilder<QIFRecord>
{
    private const string INVESTMENT_ACTION_BUY = "Buy";
    private const string INVESTMENT_ACTION_SELL = "Sell";
    private const string INVESTMENT_ACTION_DIV = "Div";
    private const string ExceptionMessage = "Currently Not Implemented";
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
            QIFDocumentType.Investment => BuildInvestment(),
            QIFDocumentType.Bank => BuildBankRecord(),
            QIFDocumentType.Category => BuildCategory(),
            _ => new QIFRecord(Date(), Amount(), Memo(), Status())
        };
    }

    internal QIFRecordBuilder Add(string line)
    {
        if (!string.IsNullOrEmpty(line))
        {
            _details[line[0]] = line.Length > 1 ? line[1..] : "";
        }
        return this;
    }
    internal QIFRecordBuilder Clear()
    {
        _details.Clear();
        return this;
    }

    private QIFCategoryRecord BuildCategory()
        => new(Category(), Memo(), Budgeted());

    private QIFBankRecord BuildBankRecord()
        => new(Date(), Amount(), Memo(), Status(),
            Payee(),
            Category(),
            Address(),
            Check(), Flagged());

    private QIFInvestment BuildInvestment()
    {
        if (Enum.TryParse<QIFInvestmentType>(InvestAction(), out var investmentType))
        {
            // Investment actions
            return InvestAction() switch
            {
                INVESTMENT_ACTION_BUY => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                INVESTMENT_ACTION_SELL => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "BuyX" => throw new NotSupportedException(ExceptionMessage),
                "SellX" => throw new NotSupportedException(ExceptionMessage),
                "ShtSell" => throw new NotSupportedException(ExceptionMessage),
                "CvrShrt" => throw new NotSupportedException(ExceptionMessage),
                "CGLong" => throw new NotSupportedException(ExceptionMessage),
                "CGLongX" => throw new NotSupportedException(ExceptionMessage),
                "CGMid" => throw new NotSupportedException(ExceptionMessage),
                "CGMidX" => throw new NotSupportedException(ExceptionMessage),
                "CGShort" => throw new NotSupportedException(ExceptionMessage),
                "CGShortX" => throw new NotSupportedException(ExceptionMessage),
                INVESTMENT_ACTION_DIV => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "DivX" => throw new NotSupportedException(ExceptionMessage),
                "IntInc" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "IntIncX" => throw new NotSupportedException(ExceptionMessage),
                "ReinvDiv" => throw new NotSupportedException(ExceptionMessage),
                "ReinvInt" => throw new NotSupportedException(ExceptionMessage),
                "ReinvLg" => throw new NotSupportedException(ExceptionMessage),
                "ReinvMd" => throw new NotSupportedException(ExceptionMessage),
                "ReinvSh" => throw new NotSupportedException(ExceptionMessage),
                "Reprice" => throw new NotSupportedException(ExceptionMessage),
                "XIn" => throw new NotSupportedException(ExceptionMessage),
                "XOut" => throw new NotSupportedException(ExceptionMessage),
                "MiscExp" => throw new NotSupportedException(ExceptionMessage),
                "MiscExpX" => throw new NotSupportedException(ExceptionMessage),
                "MiscInc" => throw new NotSupportedException(ExceptionMessage),
                "MiscIncX" => throw new NotSupportedException(ExceptionMessage),
                "MargInt" => throw new NotSupportedException(ExceptionMessage),
                "MargIntX" => throw new NotSupportedException(ExceptionMessage),
                "RtrnCap" => throw new NotSupportedException(ExceptionMessage),
                "RtrnCapX" => throw new NotSupportedException(ExceptionMessage),
                "StkSplit" => throw new NotSupportedException(ExceptionMessage),
                "ShrsOut" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),
                "ShrsIn" => new QIFInvestment(Date(), Amount(), Memo(), Status(), Check(), Payee(), Address(), Category(), investmentType, SecurityName(), Price(), Quantity(), Commission(), SplitAmount()),

                _ => throw new NotSupportedException($"Investment action '{InvestAction()}' is not supported.")
            };
        }
        return QIFInvestment.Empty;

    }

    private DateTime Date() => Value('D', ParseQIFDate, DateTime.MinValue);
    private Decimal Amount() => Value('T', s => ParseDecimal(s), Value('U', s => ParseDecimal(s), default));
    private string Memo() => Value('M').TrimEnd();
    private string Status() => Value('C');
    private string Check() => Value('N');
    private string Payee() => Value('P');
    private string Address() => Value('A');
    private string Category() => Value('L');
    private bool Flagged() => Value('F', s => true, false);
    //TODO: private string SplitCategory() => Value('S');
    //TODO: private string SplitMemo() => Value('E');
    //TODO: private Decimal SplitPercent() => Value('%', ParseDecimal);
    private Decimal SplitAmount() => Value('$', ParseDecimal);
    private string InvestAction() => Value('N');
    private string SecurityName() => Value('Y');
    private Decimal Price() => Value('I', ParseDecimal);
    private Decimal Quantity() => Value('Q', ParseDecimal);
    private Decimal Commission() => Value('O', ParseDecimal);
    private Decimal Budgeted() => Value('B', ParseDecimal);
    //TODO: private Decimal AmountTransferred() => Value('$', ParseDecimal);
    //TODO: private string ExtendedData() => Value('X');

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
