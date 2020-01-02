# StructureGo

这是一个使用`Graphviz`绘图工具来绘制C#项目命名空间图的工具

借鉴自[GraphVizTest](https://www.codeproject.com/Articles/1164156/Using-Graphviz-in-your-project-to-create-graphs-fr)项目



## 编译

本项目使用`.net core 3.1`运行时，需要下载

使用命令`dotnet publish`编译

编译后的程序可在`\bin\Debug\netcoreapp3.1\publish\`目录下找到

由于`GraphViz`暂不支持64位，所以本程序暂且使用32位作为编译目标。

## 运行

将程序拷贝至要分析的`.dll`文件处，除`.dll`之外；还必须要有`.deps.json`文件

命令行运行 `StructureGo C:\\xxxx\yourAssembly.dll yourNamespace `

例如如`StructureGo D:\Codes\StructureGo\StructureGo\bin\Debug\netcoreapp3.1\StructureGo.dll StructureGo`

即可得到一份`.svg`文件

![StructureGo](\StructureGo.svg)