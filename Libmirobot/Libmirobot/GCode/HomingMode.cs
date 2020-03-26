namespace Libmirobot.GCode
{
    public enum HomingMode
    {
        /// <summary>
        /// All axes will move one after another
        /// </summary>
        InSequence = 0,

        /// <summary>
        /// All axes will move at the same time
        /// </summary>
        Simultaneous = 1
    }
}
