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

The QIFLibrary ...
