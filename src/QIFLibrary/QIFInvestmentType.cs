using System.Reflection;
using System;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Investment Record Types.
/// </summary>
public enum QIFInvestmentType
{
    /// <summary>
    /// 
    /// </summary>
    UNKNOWN,
    /// <summary>
    /// 
    /// </summary>
    Buy,
    /// <summary>
    /// 
    /// </summary>
    BuyX,
    /// <summary>
    /// 
    /// </summary>
    Sell,
    /// <summary>
    /// 
    /// </summary>
    SellX,
    /// <summary>
    /// 
    /// </summary>
    ShtSell,
    /// <summary>
    /// 
    /// </summary>
    CvrShrt,
    /// <summary>
    /// 
    /// </summary>
    CGLong,
    /// <summary>
    /// 
    /// </summary>
    CGLongX,
    /// <summary>
    /// 
    /// </summary>
    CGMid,
    /// <summary>
    /// 
    /// </summary>
    CGMidX,
    /// <summary>
    /// 
    /// </summary>
    CGShort,
    /// <summary>
    /// 
    /// </summary>
    CGShortX,
    /// <summary>
    /// 
    /// </summary>
    Div,
    /// <summary>
    /// 
    /// </summary>
    DivX,
    /// <summary>
    /// 
    /// </summary>
    IntInc,
    /// <summary>
    /// 
    /// </summary>
    IntIncX,
    /// <summary>
    /// 
    /// </summary>
    ReinvDiv,
    /// <summary>
    /// 
    /// </summary>
    ReinvInt,
    /// <summary>
    /// 
    /// </summary>
    ReinvLg,
    /// <summary>
    /// 
    /// </summary>
    ReinvMd,
    /// <summary>
    /// 
    /// </summary>
    ReinvSh,
    /// <summary>
    /// 
    /// </summary>
    Reprice,
    /// <summary>
    /// 
    /// </summary>
    XIn,
    /// <summary>
    /// 
    /// </summary>
    XOut,
    /// <summary>
    /// 
    /// </summary>
    MiscExp,
    /// <summary>
    /// 
    /// </summary>
    MiscExpX,
    /// <summary>
    /// 
    /// </summary>
    MiscInc,
    /// <summary>
    /// 
    /// </summary>
    MiscIncX,
    /// <summary>
    /// 
    /// </summary>
    MargInt,
    /// <summary>
    /// 
    /// </summary>
    MargIntX,
    /// <summary>
    /// 
    /// </summary>
    RtrnCap,
    /// <summary>
    /// 
    /// </summary>
    RtrnCapX,
    /// <summary>
    /// 
    /// </summary>
    StkSplit,
    /// <summary>
    /// 
    /// </summary>
    ShrsOut,
    /// <summary>
    /// 
    /// </summary>
    ShrsIn,
}

/// <summary>
/// Extensions for the QIFInvestment Data Type.
/// </summary>
public static class QIFInvestmentTypeExtensions
{
    /// <summary>
    /// 'Friendly' string representation of the data type.
    /// </summary>
    /// <param name="type">QIFInvestmentType to extend.</param>
    /// <returns>A string representation of the investment type.</returns>
    public static string AsString(this QIFInvestmentType type) => type.Description();

    /// <summary>
    /// Description of the investment type.
    /// </summary>
    /// <param name="type">QIFInvestmentType to extend.</param>
    /// <returns>A description of the data type.</returns>
    public static string Description(this QIFInvestmentType type)
    {
        return type switch
        {
            QIFInvestmentType.Buy => "Buy Shares",
            QIFInvestmentType.BuyX => "Buy Shares Xfer",
            QIFInvestmentType.Sell => "Sell Shares",
            QIFInvestmentType.SellX => "Sell Shares Xfer",
            QIFInvestmentType.ShtSell => "Sell Short",
            QIFInvestmentType.CvrShrt => "Cover Short",
            QIFInvestmentType.CGLong => "Long-term capital gains distribution received in the account",
            QIFInvestmentType.CGLongX => "Long-term capital gains distribution transferred to another account",
            QIFInvestmentType.CGMid => "Medium-term capital gains distribution received in the account",
            QIFInvestmentType.CGMidX => "Medium-term capital gains distribution transferred to another account",
            QIFInvestmentType.CGShort => "Short-term capital gains distribution received in the account",
            QIFInvestmentType.CGShortX => "Short-term capital gains transferred to another account",
            QIFInvestmentType.Div => "Dividend",
            QIFInvestmentType.DivX => "Dividend Xfer",
            QIFInvestmentType.IntInc => "Interest",
            QIFInvestmentType.IntIncX => "Interest Xfer",
            QIFInvestmentType.ReinvDiv => "Reinvest Dividend",
            QIFInvestmentType.ReinvInt => "Reinvest Interest",
            QIFInvestmentType.ReinvLg => "Long-term capital gains reinvested in additional shares of the security",
            QIFInvestmentType.ReinvMd => "Medium-term capital gains reinvested in additional shares of the security",
            QIFInvestmentType.ReinvSh => "Short-term capital gains reinvested in additional shares of the security",
            QIFInvestmentType.Reprice => "Reprice employee stock options",
            QIFInvestmentType.XIn => "Cash In",
            QIFInvestmentType.XOut => "Cash Out",
            QIFInvestmentType.MiscExp => "Miscellaneous expense",
            QIFInvestmentType.MiscExpX => "Miscellaneous expense Xfer",
            QIFInvestmentType.MiscInc => "Miscellaneous income",
            QIFInvestmentType.MiscIncX => "Miscellaneous income Xfer",
            QIFInvestmentType.MargInt => "Margin Interest",
            QIFInvestmentType.MargIntX => "Margin Interest Xfer",
            QIFInvestmentType.RtrnCap => "Return Capital",
            QIFInvestmentType.RtrnCapX => "Return Capital Xfer",
            QIFInvestmentType.StkSplit => "Split Shares",
            QIFInvestmentType.ShrsOut => "Remove Shares",
            QIFInvestmentType.ShrsIn => "Add Shares",
            _ => "Unknown Investment Action",
        };
    }
}
