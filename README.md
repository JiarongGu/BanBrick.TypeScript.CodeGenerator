# BanBrick.TypeScript.CodeGenerator
C# to TypeScript Code Generator Libarary for WebApi

## Getting Started

Download and include this liabray into your project.

### Prerequisites

```
.net standard
```


## How To Use

#### Generate typescript code from list of types.

```
using BanBrick.TypeScript.CodeGenerator;

var types = new List<Type>() { type1, type2, type3, ...};
var codeGenerator = new CodeGenerator();
var code = codeGenerator.GenerateTypeScript(types);
```


## Code Generation Workflow
![alt text](https://lh4.googleusercontent.com/nW5XfHU2ipmW3dwoTDPQQrnpVKLxcdEn-JefF6HMqGmFFw_pc-4nPYghOapX20JiOgM6El5ZNojzLqnbZvZz=w1918-h974-rw)
