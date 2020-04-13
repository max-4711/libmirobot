# libmirobot

![CI Build (.NET Core 3.1)](https://github.com/maxkde/libmirobot/workflows/.NET%20Core/badge.svg)

## Description
Library to generate g code to control the [WLKATA Mirobot](http://www.wlkata.com/site/index.html). No official project and not affiliated to WLKATA at all.

G Code protocol over a serial connection is being used; official specification can be downloaded from the [manufacturer's website](http://www.wlkata.com/site/downloads.html).

## License
MIT License

## Installation
Libmirobot is available as package on [nuget.org](https://www.nuget.org/packages/libmirobot/).

Use package manager console to install:
```
PM> Install-Package Libmirobot
```
or .NET CLI:
```
dotnet add package Libmirobot
```

## Example Usage
```C#
using Libmirobot;

//...

var comPorts = RobotConfigurator.GetAvailableComports();
using (var configuredRobot = RobotConfigurator.PreconfigureRobot(comPorts[0])) //Select the com port fitting to your setup, doesn't have to be comPorts[0]!
{
    configuredRobot.Robot.HomeAxes(HomingMode.InSequence);
    configuredRobot.Robot.MoveToCartesian(150, 20, 55, 0, 0, 0, 2000, MovementMode.Linear);
}
```

## Documentation / available methods for controlling the robot
Most important interface in the library, ISixAxisRobot, is being used to control the robot and offers the following methods for this purpose:

- void MoveToCartesian(float xCoordinate, float yCoordinate, float zCoordinate, float xRotation, float yRotation, float zRotation, int speed, MovementMode movementMode);
- void IncrementCartesian(float xCoordinateIncrement, float yCoordinateIncrement, float zCoordinateIncrement, float xRotationIncrement, float yRotationIncrement, float zRotationIncrement, int speed, MovementMode movementMode);
- void MoveAxesTo(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed);
- void IncrementAxes(float axis1, float axis2, float axis3, float axis4, float axis5, float axis6, int speed);
- void HomeAxes(HomingMode homingMode);
- void SetGripperAperture(int pwm);
- void SetAirPumpPower(int pwm);
- void UpdateCurrentPosition();

The robot will fire events in some cases, which can be subscribed also via the ISixAxisRobot interface. The events are:
- event EventHandler&lt;RobotTelegram&gt; InstructionSent;
- event EventHandler&lt;RobotStateChangedEventArgs&gt; RobotStateChanged;
- event EventHandler&lt;RobotErrorEventArgs&gt; RobotErrorOccurred;


The robot will need some configuration, before it can be used (for example wiring to the serial port). It is recommended to use the RobotConfigurator class, as this will produce a fully configured robot, ready for use, which should be sufficient for most use cases.

Documentary comments, with detailed information for the purpose of every method and all input parameters, are available and should be automatically shown by IntelliSense while coding ;-)

## Technical remarks

### General
- Code uses C# 8.0 and is fully [nullable aware](https://devblogs.microsoft.com/dotnet/embracing-nullable-reference-types/)
- "Main" Compile target is .Net Standard 2.1, which means it is not limited to Windows!
- As .NET Standard 2.1 requires .NET Core 3.0 and isn't supported by the "classic" .NET Framework at all, a "legacy" .NET Standard 2.0 version is also offered
- Documentary comments available for all methods and properties
- On Windows, driver for the robot is needed (can be downloaded from the [manufacturer's website](http://www.wlkata.com/site/downloads.html))


### Behavioural / Remarkable quirks
- The library buffers all instructions for the robot and sends one every 50ms at max (this is due to reported problems with the robot receiving commands in a higher frequency than 20 Hz)
- To prevent some errors with unexpected behaviours, the library waits for the robot to report being in an idle state before sending one motion instruction after another.
    - To achieve this, the library automatically polls the robot for its state after sending a motion instruction
    - This behaviour can be deactivated by setting the 'delayInstructionUntilPreviousInstructionCompleted' parameter of the SixAxisMirobot.CreateNew()-method to false

## Contribution
Feedback and/or pull requests are always welcome :-)