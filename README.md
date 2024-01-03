# dotnet_winmerge_report

```ps1
dotnet new console -o ./src/WinMergeReport
dotnet add ./src/WinMergeReport package System.CommandLine --prerelease
# ※プロジェクトに WinMergeRapper.dll への参照追加
dotnet run --project ./src/WinMergeReport
```

```
dotnet run --project ./src/WinMergeReport --left-path ./TestData/a --right-path ./TestData/b --output-path ./Report
```

※プロジェクトに WinMergeRapper.dll への参照追加
```xml
<ItemGroup>
    <Reference Include="../../lib/WinMergeRapper.dll" />
</ItemGroup>
```

※ NativeAOT する場合は以下も追加
```xml
<PropertyGroup>
    <PublishAot>true</PublishAot>
</PropertyGroup>
```

Publish Native AOT using the CLI  
https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=net8plus%2Cwindows

```
dotnet publish ./src/WinMergeReport
```
