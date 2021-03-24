**[Table of Contents](http://tableofcontent.eu)**
- [Frends.Community.ConvertToCsv](#frendscommunityconverttocsv)
  - [Contributing](#contributing)
  - [Documentation](#documentation)
    - [Input](#input)
    - [Result](#result)
  - [License](#license)


# Frends.Community.ConvertToCsv
FRENDS Task to convert XML to CSV. XML needs to represent a table, othervice conversion fails.

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
| InputData				| string	| XML string to be converted into csv. | See bellow. |
| CsvSeparator			| string	| Separator for the output columns.	| `;` |
| IncludeHeaders		| bool	| True if the column headers should be included into the output	| `true` |

Example input:
```
<root>
	<row id='1'>
		<name>Google</name>
		<url>https://en.wikipedia.org/wiki/Google</url>
		<fancy_characters>comma (,) inside field</fancy_characters>
	</row>
	<row id='2'>
		<name>Apple</name>
		<url>https://en.wikipedia.org/wiki/Apple_Inc.</url>
		<fancy_characters>Kanji 漢字</fancy_characters>
	</row>
	<row id='3'>
		<name>Missing columns</name>
	</row>
</root>
```

### Result

| Property      | Type     | Description                      | Example                     |
|---------------|----------|----------------------------------|-----------------------------|
| Result        | string   | Result as CSV	| See bellow. |

Example output, for input given above, with comma as a delimeter and headers included:
```
name,url,fancy_characters,id
Google,https://en.wikipedia.org/wiki/Google,"comma (,) inside field",1
Apple,https://en.wikipedia.org/wiki/Apple_Inc.,Kanji 漢字,2
Missing columns,,,3
```

If input XML string contains multiple fields with same name, they are omited. Also rows must be in element with same name. If id is not given for row as a attribute filed named rowname_Id is added, with row number. 

For example, following XML:
```
<table>
	<foo>
		<bar>700</bar>
		<foobar>12</foobar>
	</foo>
	<foo>
		<bar>800</bar>
		<bar>800</bar>
		<foobar>5</foobar>
	</foo>
	<invalid>
		<bar>200</bar>
		<foobar>7</foobar>
	</invalid>
</table>
```

is thus converted to, using comma as a delimeter and headers included:

```
foo_Id,foobar
0,12
1,5
```

## License

This project is licensed under the MIT License - see the LICENSE file for details

# Change Log

| Version             | Changes                 |
| ---------------------| ---------------------|
| 2.0.0 | Task renamed (old name was ConvertToCsv). New task can only convert XML to CSV. Csv.Create should be used to convert JSOn to CSV. |
| 2.1.0 | Multitarget support. Everything in namespaces etc. is now renamed to ConvertXmlToCsv. |
