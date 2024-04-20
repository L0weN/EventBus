public interface IEvent { }

public struct TestEvent : IEvent
{

}

public class PlayerEvent : IEvent
{
    public int health;
    public int mana;
}
