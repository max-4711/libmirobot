# libmirobot

![CI Build (.NET Core 3.1)](https://github.com/maxkde/libmirobot/workflows/.NET%20Core/badge.svg)

## Description
Library to easily control the [WLKATA Mirobot](https://www.wlkata.com/products/wlkata-mirobot-introduction). No official project and not affiliated to WLKATA at all.

G-code protocol over a serial connection is being used; official specification can be downloaded from the [manufacturer's website](https://www.wlkata.com/support/download-center).

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
    //Homing will be performed automatically, if needed (see 'Special features' down below):
    //configuredRobot.Robot.HomeAxes(HomingMode.InSequence);
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
- event EventHandler&lt;RobotResetEventArgs&gt; RobotResetOccurred;


The robot will need some configuration, before it can be used (for example wiring to the serial port). It is recommended to use the RobotConfigurator class (just like shown in the example), as this will produce a fully configured robot, ready for use, which should be sufficient for most use cases.

Documentary comments, with detailed information for the purpose of every method, all input parameters, events and so on are available and should be automatically shown by IntelliSense while coding ;-)

## Technical remarks

### General
- Code uses C# 8.0 and is fully [nullable aware](https://devblogs.microsoft.com/dotnet/embracing-nullable-reference-types/)
- "Main" Compile target is .NET Standard 2.1 (which means it is not limited to Windows!)
- As .NET Standard 2.1 targeting libraries can be only referenced from .NET Core projects version 3.0 and up and isn't supported by the "classic" .NET Framework at all, a "legacy" .NET Standard 2.0 version is also offered
- Documentary comments available for all methods and properties and included in the package
- On Windows and macOS, a driver for the robot is needed (can be downloaded from the [manufacturer's website](https://www.wlkata.com/support/download-center))


### Behavioural / Remarkable quirks / Special features
- The library buffers all instructions for the robot and sends one every 50ms at max (this is due to reported problems with the robot receiving commands in a higher frequency than 20 Hz)
- To prevent some errors with instructions being sent to the robot while it is busy, the library waits for the robot to report being in an idle state before sending one motion instruction after another.
    - To achieve this, the library automatically polls the robot for its state after sending a motion instruction
    - This behaviour can be deactivated by setting the 'delayInstructionUntilPreviousInstructionCompleted' parameter of the SixAxisMirobot.CreateNew()-method to false
- The library can automatically detect, if the robot refuses instruction execution because of locked axes and unlock them by executing a homing operation (and then repeating the failed instruction)
    - This behaviour can be deactivated by setting the 'autoHomeAxes' parameter of the SixAxisMirobot.CreateNew()-method to false
    - This function is not available, if the aforementioned option 'delayInstructionUntilPreviousInstructionCompleted' is deactivated


## Contribution
Feedback and/or pull requests are always welcome :-)