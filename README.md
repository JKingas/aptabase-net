![Aptabase](https://raw.githubusercontent.com/aptabase/aptabase-com/main/public/og.png)

# .NET SDK for Aptabase

THIS IS FORK OF https://github.com/aptabase/aptabase-maui TO REDUCE MAUI DEPENDENCY.

Instrument your apps with Aptabase, an Open Source, Privacy-First and, Simple Analytics for Mobile, Desktop and, Web Apps.

## Install

Start by adding the Aptabase NuGet package to your .csproj:

```xml
<PackageReference Include="Kingas.Aptabase" Version="0.3" />
```

## Usage

First, you need to get your `App Key` from Aptabase, you can find it in the `Instructions` menu on the left side menu.

Need to configure `IServiceCollection` like:

```csharp
services.AddSingleton<IAptabaseClient>(x =>
{
    var appKey = "<YOUR_APP_KEY>"; // // 👈 this is where you enter your App Key or read from config
    var logger = x.GetService<ILogger<AptabaseClient>>();
    return new AptabaseClient(appKey, null, logger);
});
```

You will add the `IAptabaseClient` to your dependency injection container, and this allowing you to use it in your pages and view models.

As an example, to track events in your `MainPage`, you first need to add it to the DI Container in `MauiProgram.cs`:

```csharp
builder.Services.AddSingleton<MainPage>();
```         

And then inject and use it on your `MainPage.xaml.cs`:

```csharp
public partial class MainPage : ContentPage
{
    IAptabaseClient _aptabase;
    int count = 0;

    public MainPage(IAptabaseClient aptabase)
    {
        InitializeComponent();
        _aptabase = aptabase;
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
        _aptabase.TrackEvent("Increment");

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}
```

The `TrackEvent` method also supports custom properties:

```csharp
_aptabase.TrackEvent("app_started"); // An event with no properties
_aptabase.TrackEvent("screen_view", new() {  // An event with a custom property
    { "name", Settings" }
});
```

A few important notes:

1. The SDK will automatically enhance the event with some useful information, like the OS, the app version, and other things.
2. You're in control of what gets sent to Aptabase. This SDK does not automatically track any events, you need to call `TrackEvent` manually.
   - Because of this, it's generally recommended to at least track an event at startup
3. The `TrackEvent` function is a non-blocking operation as it runs in the background.
4. Only strings and numbers values are allowed on custom properties
