using Libmirobot.Core;

namespace Libmirobot.IO
{
    public interface ISerialConnection
    {
        void Attach(ISixAxisRobot robot);

        void Connect();
    }
}
