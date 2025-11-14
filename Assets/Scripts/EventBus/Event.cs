public interface IEvent { }

public struct UnitEvent : IEvent 
{
    public int a;
    public int b;
}

public struct TestEvent : IEvent
{
    public UnitData UnitData;
}

