# QIFLibrary
Library to read and write QIF data.  
[![dotnet](https://github.com/tudormobile/QIFLibrary/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/tudormobile/QIFLibrary/actions/workflows/dotnet.yml)

## Quick Start

```
using Tudormobile.QIFLibrary;

var filename = "test.qif";
var doc = QIFDocument.ParseFile(filename);
Console.WriteLine($"Document contains ${doc.Records.Count} records.");
```

- Creates a QIF Document model from a file.
- Support for asynchronous and synchronous parsing of data streams and strings.

[NuGET Package README](docs/README.md) | [Source Code README](src/README.md) | [API Documentation](https://tudormobile.github.io/QIFLibrary/)
