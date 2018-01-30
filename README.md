# Tado API
This library for C# will allow you to easily communicate with the Tado API and retrieve details about your Tado thermostats and zones and set their temperature.
## Version History

0.1 - released January 30, 2018

- Initial version

## System Requirements

This API is built using the Microsoft .NET 4.6.2 framework and is fully asynchronous

## Warning

Tado has not officially released an API yet that developers are free to use. This API has been created by mimicking the traffic to their own web application. This means they can change their Api at any time causing this Api and your code to break without advanced notice. Be sure you understand the consequences of this before using any Tado Api in your own software.

## Usage Instructions

To communicate with the Tado API, add the NuGet package to your solution and add a using reference in your code:

```C#
using KoenZomers.Tado.Api;
```

Then create a new session instance using:

```C#
var session = new Session("your@email.com", "yourpassword");
```

Note that this line does not perform any communications with the Tado API yet. You need to manually trigger authenticate before you can start using the session:

```C#
await session.Authenticate();
```

Once this succeeds, you can call one of the methods on the session instance to retrieve data, i.e.:

```C#
// Retrieves information about the currently logged on user
var me = await session.GetMe();
```

To set the temperature on a zone:

```C#
await session.SetTemperatureCelsius(123456, 0, 19);
```

To switch off the heating in a zone:

```C#
await session.SwitchHeatingOff(123456, 0);
```

Check out the UnitTest project in this solution for full insight in the possibilities and working code samples.

## Available via NuGet

You can also pull this API in as a NuGet package by adding "KoenZomers.Tado.Api" or running:

Install-Package KoenZomers.Tado.Api

Package statistics: https://www.nuget.org/packages/KoenZomers.Tado.Api

## Current functionality

With this API at its current state you can:

- Authenticate to the Tado v2 API
- Retrieve information about the currently logged on user
- Retrieve information about all configured zones in your house
- Retrieve information about all registered Tado devices in your house
- Retrieve information about all connected mobile devices to your house
- Retrieve information about the Tado installations at your house
- Get the state if somebody is home
- Get the summarized overview of zone
- Get information about the weather around your house
- Set the desired temperature in a zone in Celsius and Fahrenheit
- Switch off the heating in a zone

## Feedback

Any kind of feedback is welcome! Feel free to drop me an e-mail at koen@zomers.eu