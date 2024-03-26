namespace Acht;

public abstract class Room(string Name, string Description)
{
    /// <summary>
    /// Activate room superpowers.
    /// </summary>
    /// <returns>Whether user is alive.</returns>
    public abstract bool EnterRoom();
}