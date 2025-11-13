using HSD.DI;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    Context context;    

    public void Init(Context context)
    {
        this.context = context;
    }
}
