using HSD.DI;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    Context context;

    [SerializeField] int a1, a2;

    EventBinding<UnitEvent> eventBinding;
    EventBinding<TestEvent> testEventBinding;

    private void Awake()
    {
        eventBinding = new EventBinding<UnitEvent>(Change);
        eventBinding.Add(Change1);
        eventBinding.Add(Test1);

        testEventBinding = new EventBinding<TestEvent>(UnitDataChange);
    }

    public void Init(Context context)
    {
        this.context = context;
    }

    private void OnEnable()
    {
        EventBus<UnitEvent>.Register(eventBinding);
        EventBus<TestEvent>.Register(testEventBinding);
    }

    private void OnDisable()
    {
        EventBus<UnitEvent>.Deregister(eventBinding);
        EventBus<TestEvent>.Deregister(testEventBinding);
    }

    private void Update()
    {

    }

    private void Change(UnitEvent unitEvent)
    {
        Debug.Log(unitEvent.a + unitEvent.b);
    }
    private void Change1(UnitEvent unitEvent)
    {
        Debug.Log(unitEvent.a + unitEvent.b);
    }

    private void UnitDataChange(TestEvent unitEvent)
    {
        Debug.Log(unitEvent.UnitData.Level);
    }

    private void Test1()
    {

    }
}
