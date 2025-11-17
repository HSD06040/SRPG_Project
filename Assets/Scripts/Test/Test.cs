using AYellowpaper.SerializedCollections;
using HSD.DI;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    Context context;

    [SerializeField] int a1, a2;
    
    EventBinding<TestEvent> testEventBinding;

    [SerializedDictionary("string, int")]
    public SerializedDictionary<string, int> keyValuePairs = new();

    private void Start()
    {
        keyValuePairs.Add("d", 4);
    }

    private void Awake()
    {        
    }

    public void Init(Context context)
    {
        this.context = context;
    }

    private void Update()
    {

    }

    private void Test1()
    {

    }
}
