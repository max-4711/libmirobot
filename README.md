# libmirobot

## Description
Library to generate g code to control the [WLKATA Mirobot](http://www.wlkata.com/site/index.html). No official project and not affiliated to WLKATA at all.

G Code protol over a serial connection is being used; official specification can be downloaded from the [manufacturer's website](http://www.wlkata.com/site/downloads.html).

## Example Usage
```
var comPorts = robotConfigurator.GetAvailableComports();
using (var configuredRobot = robotConfigurator.PreconfigureRobot(comPorts[0]))
{
    configuredRobot.Robot.HomeAxes(HomingMode.InSequence);
    configuredRobot.Robot.MoveToCartesian(150, 20, 55, 0, 0, 0, 2000);
}
```

## Documentation / available methods for controlling the robot
Coming soon...

## Requirements
- C# 8.0
- .Net Standard 2.1 (-> not limited to windows!)
- Driver for the robot (can be downloaded from the [manufacturer's website](http://www.wlkata.com/site/downloads.html))