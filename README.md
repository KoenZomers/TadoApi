# Tado API
This library for C# will allow you to easily communicate with the Tado API and retrieve details about your Tado thermostats and zones and set their temperature.
## Version History

0.2 - released January 31, 2018

- With big thanks to http://blog.scphillips.com/posts/2017/01/the-tado-api-v2/ added various methods that were listed there but not yet implemented. Full list of available functionalities can be found below.
- Removed old code and some code cleanup

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

Check out the UnitTest project in this solution for full insight in the possibilities and working code samples. If you want to run the Unit Tests, copy the App.sample.config file to become App.config and fill in the appSettings values with the proper values valid for your scenario.

## Available via NuGet

You can also pull this API in as a NuGet package by adding "KoenZomers.Tado.Api" or running:

Install-Package KoenZomers.Tado.Api

Package statistics: https://www.nuget.org/packages/KoenZomers.Tado.Api

## Current functionality

With this API at its current state you can:

- Authenticate to the Tado v2 API using an username and password to get an OAuth2 token
- Validate with each request if the access token is still valid (lifetime is 10 minutes) and if not, uses the refresh token to get a new access token (lifetime is 1 day) and if that also fails, uses the username/password to get a new context token
- Retrieve information about the currently logged on user
- Retrieve information about all configured zones in your house
- Retrieve information about all registered Tado devices in your house
- Retrieve information about all connected mobile devices to your house
- Retrieve information about the Tado installations at your house
- Retrieve the users with access to a house
- Retrieve the details of a house
- Get the state if somebody is home
- Get the capabilities of a zone
- Get the settings of a mobile device with access to a house
- Get the summarized overview of zone
- Get information about the weather around your house
- Get the early start setting of a zone
- Switch the early start setting of a zone to enabled or disabled
- Set the desired temperature in a zone in Celsius and Fahrenheit
- Switch off the heating in a zone
- Show Hi on Tado thermostats or Tado knobs

## Still missing

- Setting heating schedules

## Feedback

Any kind of feedback is welcome! Feel free to drop me an e-mail at koen@zomers.eu