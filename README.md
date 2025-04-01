# SalesApp

SalesApp is a .NET application designed to manage and analyze sales data. It provides functionalities to filter, count, and retrieve sales records from a CSV file.

## Table of Contents

- [Features](#features)
- [Setup](#setup)
- [Usage](#usage)
- [Dependencies](#dependencies)
- [Contributing](#contributing)
- [License](#license)

## Features

- Filter sales records by segment, country, product, and discount band.
- Count the number of sales records based on filters.
- Retrieve paginated sales records.
- Trim all string fields in sales records.

## Setup

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or later

### Installation
    
1. Clone the repository:
    git clone https://github.com/luchiansienna/SalesApp
    cd SalesApp  
2. Open the solution in Visual Studio:
    SalesApp.sln

3. Restore the NuGet packages:
    dotnet restore

4. Build the solution:
    dotnet build

    
## Usage

### Running the Application

1. Set the `SalesApp` project as the startup project.
2. Run the application using Visual Studio or the .NET CLI:
    dotnet run --project SalesApp


### Running Tests

1. Set the `SalesApp.Services.UnitTests` project as the startup project.
2. Run the tests using Visual Studio Test Explorer or the .NET CLI:

dotnet test


### Example Code

#### Filtering Sales Records

var salesService = new SalesService(fileManager, configuration); var filter = new SalesFilter { Segment = "Government", Country = "Canada", Product = "Carretera", DiscountBand = "None", PageIndex = 0, PageSize = 10 };
var salesRecords = await salesService.GetSales(filter); foreach (var sale in salesRecords) { Console.WriteLine($"{sale.Date}: {sale.Product} - {sale.UnitsSold} units sold"); }


## Dependencies

- CsvHelper
- Microsoft.Extensions.Configuration
- Moq (for unit tests)
- NUnit (for unit tests)

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes or improvements.

## License

This project is licensed under the MIT License.
