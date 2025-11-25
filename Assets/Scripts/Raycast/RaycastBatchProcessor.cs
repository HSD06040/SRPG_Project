using System;
using Unity.Collections;
using UnityEngine;

public class RaycastBatchProcessor : Singleton<RaycastBatchProcessor>
{
    [SerializeField] int maxRaycastsPreJob = 1000;

    NativeArray<RaycastCommand> commandList;
    NativeArray<RaycastHit> hitResult;

    public void PreformRaycasts(
        Vector3[] origins,
        Vector3[] driections,
        int layerMask,
        bool hitBackfaces,
        bool hitTriggers,
        bool hitMultiFace,
        Action<RaycastHit[]> ccallback)
    {
        const float maxDistance = 0.4f;
        int rayCount = Mathf.Min(origins.Length, maxRaycastsPreJob);

        QueryTriggerInteraction queryTriggerInteraction = hitTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

        using (commandList = new NativeArray<RaycastCommand>(rayCount, Allocator.TempJob))
        {
        }
    }
}
