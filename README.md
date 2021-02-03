# Tado API
This library compiled for .NET Standard 2.0 will allow you to easily communicate with the Tado API and retrieve details about your Tado thermostats and zones and set their temperature.

## Version History

0.4.2.0 - released February 3, 2021

- Added `GetZoneTemperatureOffset` to allow for getting the current temperature offset in a zone
- Added several variants of `SetZoneTemperatureOffsetFahrenheit` and `SetZoneTemperatureOffsetCelcius` which allow for the zone/room temperature offset to be set

0.4.1.0 - released April 19, 2020

- Added `SetHomePresence` to allow forcing the home state. Only seems to work for Tado v3+ members.
- Added `SetOpenWindow` and `ResetOpenWindow` to force the window state on a zone.
- Added `OpenWindowDetected` to `State` to get information on if an open window has been detected.
Thanks to [AndreasHassing](https://github.com/AndreasHassing) for his contributions to add these features.

0.4.0.1 - released January 15, 2020

- GetMe() wasn't returning any data when using the NuGet package. Fixed that in this version. Thanks to [twiettwiet](https://github.com/twiettwiet) for reporting this through [Issue #3](https://github.com/KoenZomers/TadoApi/issues/3)

0.4.0 - released March 4, 2019

- Converted the .NET Framework API library to .NET Standard 2.0 so it can be used from non Windows environments as well
- Updated Newtonsoft.JSON to version 12.0.1
- Updated the Unit Test packages to the latest version so the Unit Tests work again with the latest version of Visual Studio 2017

0.3.1 - released July 17, 2018

- Marked SetTemperatureCelsius and SetTemperatureFahrenheit as deprecated in favor of using SetHeatingTemperatureCelsius and SetHeatingTemperatureFahrenheit so it becomes clearer that the methods will set a heating temperature. The deprecated methods will stay for a few versions longer for backwards compatibility but will eventually be removed to avoid clutter in the code.
- Added a SetTemperature method which is the basic method used by the other temperature methods and removes duplicate code. As it may be tricky to use this method properly due to the many parameters, it is advised to use one of the methods that are targeting a specific goal, i.e. SetHeatingTemperatureCelcius or SetHotWaterTemperatureCelcius.
- Added comments to more entities to clarify what information they contain
- Added more enums for entities which seem to only be certain values. It may be that this breaks your current code by entities changing type from i.e. string to an enumerator.
- For the Hot Water boiler in Tado I'm currently assuming that this is always zone 0. If this isn't the case for your Tado setup, please let me know and I'll have to change the code to suit this scenario.

0.3 - released March 26, 2018

- Fixed an issue where a recursive loop would arise causing a StackOverflow exception as soon as the access token would expire and the refresh token would be used to get a new access token
- Updated the Client ID and Client Secret to be the special ones set up by Tado for 3rd parties, as by their request
- Fixed a disposal issue when providing proxy configuration to the Tado session instance

0.2.1 - released March 13, 2018

- Added option to provide duration of the heating or off to SetTemperatureCelcius, SetTemperatureFahrenheit and SwitchHeatingOff. This can be: until next manual change, until next scheduled event or for a specific duration.

0.2 - released January 31, 2018

- With big thanks to http://blog.scphillips.com/posts/2017/01/the-tado-api-v2/ added various methods that were listed there but not yet implemented. Full list of available functionalities can be found below.
- Removed old code and some code cleanup

0.1 - released January 30, 2018

- Initial version

## System Requirements

This API is built using the Microsoft .NET Standard 2.0 framework and is fully asynchronous

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

To set the heating temperature to 19 degrees Celcius on Tado zone 1:

```C#
await session.SetHeatingTemperatureCelsius(123456, 1, 19);
```

To switch off the heating in zone 1:

```C#
await session.SwitchHeatingOff(123456, 1);
```

Switch the hot water boiler on and set it to 65 degrees Celcius:

```C#
await session.SetHotWaterTemperatureCelsius(123456, 65);
```

To switch off the hot water boiler:

```C#
await session.SwitchHotWaterOff(123456);
```

Check out the UnitTest project in this solution for full insight in the possibilities and working code samples. If you want to run the Unit Tests, copy the App.sample.config file to become App.config and fill in the appSettings values with the proper values valid for your scenario.

## Available via NuGet

You can also pull this API in as a NuGet package by adding "KoenZomers.Tado.Api" or running:

Install-Package KoenZomers.Tado.Api

Package statistics: https://www.nuget.org/packages/KoenZomers.Tado.Api

## Current functionality

With this API at its current state you can:

- Authenticate to the Tado v2 API using an username and password to get an OAuth2 token
- Validate with each request if the access token is still valid (lifetime is 10 minutes / 599 seconds) and if not, uses the refresh token to get a new access token (lifetime is 1 day) and if that also fails, uses the username/password to get a new context token
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
- Set the desired heating temperature in a zone in Celsius and Fahrenheit
- Switch off the heating in a zone
- Switch the hot water boiler on and off
- Show Hi on Tado thermostats or Tado knobs

## Still missing

- Setting heating schedules

## Feedback

Any kind of feedback is welcome! Feel free to drop me an e-mail at koen@zomers.eu