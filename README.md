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

