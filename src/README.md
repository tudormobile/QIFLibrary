# QIF Library
Provides mechanism for reading and writing data in Quicken Interchange Format (QIF).  

[Source Code](https://github.com/tudormobile/QIFLibrary) | [Documentation](https://tudormobile.github.io/QIFLibrary/)

## Getting Started
### Install the package
```
dotnet add package Tudormobile.QIFLibrary
```
### Prerequisites
**NONE**

### Dependencies
- None

### Key Features
- Parse QIF files into document model
- Parse QIF data streams (and strings) into QIF documents and records
- Asynchronous parsing for large data sets.

### Feedback
**Tudormobile.QIFLibrary** is free and open source software released under the MIT license. Bug reports and contributions are welcome at [the GitHub repository](https://github.com/tudormobile/QIFLibrary).

## Quick Start

```
using Tudormobile.QIFLibrary;

var filename = "test.qif";
var doc = QIFDocument.ParseFile(filename);

// ....

Console.WriteLine($"Document contains ${doc.Records.Count} records.");
```

### QIFDocument Class
The *QIFDocument* class contains methods for parsing QIF data contained in streams, strings, and files using the methods listed below.

```
public static QIFDocument Parse(Stream utf8Stream, bool leaveOpen = true)
public static QIFDocument Parse(string qifData)
public static QIFDocument ParseFile(string path)
```

### QIFReader Class
Asynchronous parsing is provided via **QIFReader** class.

```
using Tudormobile.QIFLibrary;

var reader = QIFReader.FromStream(s);       // network stream, file stream, etc.
var record = await reader.ReadRecord();     // reads a record from the stream asynchronously

// Enumerate records asynchrounsly
await foreach (var record in reader.ReadRecords()) {  ... }

reader.Close();
```

### Additional Information
Complete information is provided in the library documentation.
