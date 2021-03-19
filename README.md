# Canary - ABA Cemtext Bank File Validator

## Overview
Canary is a small utility for validating bank files in the Australian Bankers Association (ABA) format (aka Cemtext).

The application is written using C# .NET Core 3.1, with a WPF user interface.

![WPF App Screenshot](/docs/screenshot.png?raw=true "")

## License
[MIT Licence](http://en.wikipedia.org/wiki/MIT_License)

## Solution Structure
* <u>Canary.Core</u> - Contains core validation logic.
* <u>Canary.Form</u> - User interface layer consisting of a WPF form.
* <u>Canary.Logging</u> - Application logging layer leveraging Log4Net.
* <u>Canary.Tests</u> - Unit tests layer

![Solution Dependency Map](/docs/canary-solution-codemap.png?raw=true "Solution Structure")

## Getting Started
1. Clone the repo
```console
git clone https://github.com/sbartholomeusz/canary
```
<br />

2. Build the solution
```console
cd canary\src\
dotnet build
```
<br />

3. Launch the WPF form
```console
Canary.Form\bin\Debug\netcoreapp3.1\Canary.exe
```
<br />

## Dependencies
* [MahApps](https://mahapps.com/docs/guides/quick-start) - WPF application theming
* [Log4Net](https://logging.apache.org/log4net/release/manual/introduction.html) - Application logging

## References
* https://www.cemtexaba.com/aba-format/cemtex-aba-file-format-details
* http://www.anz.com/Documents/AU/corporate/clientfileformats.pdf