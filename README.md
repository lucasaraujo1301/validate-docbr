# validate-docbr

> A .NET package for validating Brazilian documents — inspired by the Python library [validate_docbr](https://github.com/alvarofpp/validate-docbr).

![.NET](https://img.shields.io/badge/.NET-C%23-purple?logo=dotnet)
![License](https://img.shields.io/github/license/lucasaraujo1301/validate-docbr)

---

## About

**validate-docbr** is a C# library that provides utilities to validate common Brazilian identification documents such as CPF and CNPJ. It is designed to be simple, lightweight, and easy to integrate into any .NET project.

---

## Supported Documents

| Document | Description |
|----------|-------------|
| **CPF** | Cadastro de Pessoas Físicas (Individual Taxpayer Registry) |
| **CNPJ** | Cadastro Nacional da Pessoa Jurídica (Company Registry) |

> More document types may be added in future releases.

---

## Installation

Install via the [NuGet Package Manager](https://www.nuget.org/packages/ValidateDocBr):

**.NET CLI**
```bash
dotnet add package ValidateDocBr --version 1.0.2
```

**Package Manager Console**
```powershell
Install-Package ValidateDocBr -Version 1.0.2
```

**PackageReference** (add to your `.csproj`)
```xml
<PackageReference Include="ValidateDocBr" Version="1.0.2" />
```

---

## Usage

### Validating a CPF

```csharp
using ValidateDocBr;

var cpf = new CPF();

bool isValid = cpf.Validate("123.456.789-09");
Console.WriteLine(isValid); // true or false
```

### Validating a CNPJ

```csharp
using ValidateDocBr;

var cnpj = new CNPJ();

bool isValid = cnpj.Validate("11.222.333/0001-81");
Console.WriteLine(isValid); // true or false
```

> Documents can be passed with or without formatting masks — both formats are accepted.

---

## Running Tests

The solution includes a test project at `TestValidateDocBr`. To run the tests:

```bash
dotnet test
```

---

## Project Structure

```
validate-docbr/
├── ValidateDocBr/          # Main library
├── TestValidateDocBr/      # Unit tests
├── ValidateDocBr.sln       # Solution file
└── README.md
```

---

## Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes (`git commit -m 'Add my feature'`)
4. Push to the branch (`git push origin feature/my-feature`)
5. Open a Pull Request

---

## Inspiration

This project is a .NET port of the Python library [validate_docbr](https://github.com/alvarofpp/validate-docbr) by [@alvarofpp](https://github.com/alvarofpp).

---

## License

This project is licensed under the [MIT License](LICENSE).