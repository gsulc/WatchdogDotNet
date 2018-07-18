# WatchdogDotNet
A watchdog timer for .NET. This simply adds the extension method Restart() to System.Timer. Should you download and use the [NuGet package](https://www.nuget.org/packages/WatchdogDotNet/)? Probably not. It's so thin, there's no reason to. Just copy the source file to your project.

## Usage
Remember, Timer is IDisposable. You'll likely not use a using block, so make sure to dispose of it. For things like what is in the ElapsedEventArgs, see the [Microsoft documentation](https://msdn.microsoft.com/en-us/library/system.timers.timer.elapsed(v=vs.110).aspx).

```csharp
TimeSpan timeout = TimeSpan.FromMinutes(10);
using (var watchdog = new Timer(timeout.TotalMilliseconds()))
{
      watchdog.AutoReset = false; // Often, you want to handle the problem only once. 
      watchdog.Elapsed += (s, e) => { RecoverFromCatastrophe() };
      // do stuff
      watchdog.Restart(); // kick the dog
      // do more stuff
      watchdog.Restart(); // kick the dog
      // etc.
}
```
## Targets / Requirements
The earliest implementations of System.Timer are in the .NET Framework v3.5 and in .NET Standard 2.0, so these are the lowest targets you can use.
