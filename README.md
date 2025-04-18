# BarcodeLookup - A C# Library for GTIN, UPC, EAN and ISBN Lookup and Validation

BarcodeLookup is a C# library for performing EAN and ISBN barcode lookups using the [EAN-Search.org](https://www.ean-search.org/) API. This library allows developers to seamlessly integrate product and book barcode lookups, checksum verification, and related operations into their applications.

## Features

- **EAN barcode lookup**: Retrieve product names or detailed product information using EAN barcodes.
- **ISBN lookup**: Fetch book titles using ISBN-10 or ISBN-13 barcodes.
- **Checksum verification**: Validate the checksum of EAN barcodes.
- **Product search**: Search for products by name or find similar product names.
- **Category search**: Look up products within specific categories.
- **Barcode prefix search**: Search for products based on barcode prefixes.
- **Issuing country lookup**: Identify the issuing country of an EAN barcode.
- **Barcode image retrieval**: Get a barcode image for a given EAN.

## Getting Started

### Prerequisites

- .NET 6.0 or later
- An API token from [EAN-Search.org](https://www.ean-search.org/ean-database-api.html)

### Installation

The library is available as a NuGet package. Install it using the .NET CLI:

```bash
dotnet add package BarcodeLookup
```

## Usage
## Initialization

To use the library, create an instance of the BarcodeLookup class and provide your API token.

```csharp
using BarcodeLookup;

var lookup = new BarcodeLookup("your-api-token");
```

### GTIN / EAN Lookup

Retrieve the product name for a given EAN barcode:

```csharp
string productName = lookup.GTIN("1234567890123");
Console.WriteLine(productName ?? "Product not found");
```

### ISBN Lookup

Fetch the book title for an ISBN:

```csharp
string bookTitle = lookup.ISBN("9781234567897");
Console.WriteLine(bookTitle ?? "Book not found");
```

### Verify Checksum

Check if an EAN barcode is valid:

```csharp
bool? isValid = lookup.VerifyChecksum("1234567890123");
Console.WriteLine(isValid.HasValue && isValid.Value ? "Valid" : "Invalid");
```

### Product Search

Search for products by name:

```csharp
var products = lookup.ProductSearch("Laptop", 0, 1);

foreach (var product in products)
{
    Console.WriteLine(product["name"]);
}
```

### Barcode Image Creation

Get the image of a barcode:

```csharp
string barcodeImage = lookup.BarcodeImage("1234567890123");
Console.WriteLine(barcodeImage ?? "Image not available");
```

Contributions are welcome! Feel free to submit a pull request or open an issue on our GitHub repository.
License

This library is licensed under the MIT License. See the LICENSE file for details.
Acknowledgments

This library uses the EAN-Search.org API. Please visit [EAN-Search.org](https://www.ean-search.org/) for more information.

