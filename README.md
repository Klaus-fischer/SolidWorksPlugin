# SolidWorksPlugin

This is a base library for create SolidWorks(R) plugins.

1. Create your first plugin by deriving from <code>SIM.SolidWorksPlugin.SolidWorksAddin</code> class.

```c#
[Guid("C0E8D5B0-5773-4FDD-9ECE-C2C570CA1F65")]
[ComVisible(true)]
[SolidWorksPlugin("Demo Addin", Description = "Add-In description")]
public class DemoAddin : SolidWorksAddin
{
   ..
}
```
   
2. Make Assembly COM-Visible by adding <code>EnableComHosting</code> tag in csproj. file.

```xml
<PropertyGroup>
  ..
  <EnableComHosting>true</EnableComHosting>
  ..
</PropertyGroup>
```
3. Build the assembly
4. Navigate to you <code>bin\Debug</code> output directory and Register the dll:

```cmd
REM for .Net Framework 4.8 and below:
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Regasm.exe /codebase "yourLibrary.dll" 

REM for .net 5 and above:
regsvr32 "yourLibrary.comhost.dll"
```

5. Run SolidWorks and enjoy.
