**[Table of Contents](http://tableofcontent.eu)**
- [Frends.Community.ConvertToCsv](#frendscommunityconverttocsv)
  - [Contributing](#contributing)
  - [Documentation](#documentation)
    - [Input](#input)
    - [Result](#result)
  - [License](#license)


# Frends.Community.ConvertToCsv
FRENDS Task to convert JSON or XML to CSV. Some Frends4 tasks already provide method .ToCSV() which does the same and this might be obsolete. 

## Contributing
When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Documentation

### Input

| Property				|  Type   | Description								| Example                     |
|-----------------------|---------|-----------------------------------------|-----------------------------|
| InputData				| string	| Supported formats JSON and XML | `<root><field>1</field></root>` |
| FileType			| FileType	| JSON or XML	| `Xml` |
| CsvSeparator			| string	| Separator for the output columns	| `;` |
| IncludeHeaders		| bool	| True if the column headers should be included into the output	| `true` |

### Result

| Property      | Type     | Description                      |
|---------------|----------|----------------------------------|
| Result        | string   | Result as CSV	|

## License

This project is licensed under the MIT License - see the LICENSE file for details
