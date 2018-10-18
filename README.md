# BanBrick.TypeScript.CodeGenerator
C# to TypeScript Code Generator Libarary for WebApi Request/Response Model

## Getting Started

https://www.nuget.org/packages/BanBrick.TypeScript.CodeGenerator/

PM > Install-Package BanBrick.TypeScript.CodeGenerator -Version 1.2.3


### Prerequisites

```
.net standard
```


## How To Use

#### Generate typescript code from list of types.

```
using BanBrick.TypeScript.CodeGenerator;

var types = new List<Type>() { type1, type2, type3, ...};
var codeProcesser = new TypeScriptProcesser();
var code = codeProcesser.GenerateTypeScript(types);
```

#### Generate typescript types from annotation configs.

current support typescript enum, interface, class, count generation using annotation

#### Output type inherit use annotation

```
using BanBrick.TypeScript.CodeGenerator.Annotation;

[TypeScriptObject(Type = TypeScriptObjectType.Interface, Inherit = true)]
public class Request
{
  public RequestDetail Details { get; set; }
}

public class RequestDetail 
{
  public string Name { get; set; }
}
```

this will output both Request and RequestDetail in Interface format of Typescript

## Roadmap

- [x] Generate const value output instead of class
- [x] Support for Interface generation
- [x] Auto-Indenting to multiple-layered tree structure values
- [x] Support Auto-Resolve for Type Name Duplication
- [x] Support for Inherit of Generation Types
- [x] Auto-Indenting to multiple-layered tree structure types
- [ ] Support for external config instead of only annotation
- [ ] Support for Generictypes generation



## Code Generation Workflow
![alt text](/Code%20Generation%20Work%20Flow%20v3.png)
