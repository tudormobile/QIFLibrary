using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Represents a QIF Category record.
/// </summary>
public class QIFCategoryRecord : QIFRecord
{
    /// <summary>
    /// The category specification.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Budgeted amount for the category.
    /// </summary>
    public decimal Budgeted { get; }

    /// <summary>
    /// Create and initialize a new category record.
    /// </summary>
    /// <param name="category">The category specification.</param>
    /// <param name="memo">Category memo.</param>
    /// <param name="budgeted">Budgeted amount for the category.</param>
    public QIFCategoryRecord(string category, string memo, decimal budgeted = 0)
        : base(DateTime.MinValue, 0, memo, String.Empty)
    {
        Category = category;
        Budgeted = budgeted;
    }
}
