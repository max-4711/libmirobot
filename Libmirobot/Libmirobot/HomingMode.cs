namespace Libmirobot
{
    /// <summary>
    /// Specifies the type of homing mode, which shall be used to perform a homing operation.
    /// </summary>
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
