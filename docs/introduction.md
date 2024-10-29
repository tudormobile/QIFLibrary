# Introduction
Provides mechanism for reading and writing data in Quicken Interchange Format (QIF).  

[Source Code Repository](https://github.com/tudormobile/QIFLibrary)

## Quick Start

```
using Tudormobile.QIFLibrary;

var filename = "test.qif";
var doc = QIFDocument.ParseFile(filename);

// ....

Console.WriteLine($"Document contains ${doc.Records.Count} records.");
```


#### At this time...

The QIFLibrary provides mechanisms for reading and writing QIF, QFX, and OFX documents. An exhausetive treatment of all file format options is not attempted - however, the most commonly used elements by both financial institutions and comsumer products are covered.

The parsers are also not exhaustive. The design is intentially *forgiving*, allowing sloppy inplementations, unknown tags, and missing tags - all of which are commonly found from files produced by financial institutions - to succeed. Further, the entities produced (and consumed) by this library do not contain all properties and features available in the file format. They are, in fact, designed or a specific financial product.

#### In the future...

This library can be extended to provide comprehensive treatment of QIF, QFX, and OFX documents, including revisions to the formats that have evolved over time.