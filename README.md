# BanBrick.TypeScript.CodeGenerator
C# to TypeScript Code Generator Libarary for WebApi Request/Response Model

## Getting Started

https://www.nuget.org/packages/BanBrick.TypeScript.CodeGenerator/

PM > Install-Package BanBrick.TypeScript.CodeGenerator -Version 1.0.3


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


## Code Generation Workflow
![alt text](/Code%20Generation%20Work%20Flow.png)
