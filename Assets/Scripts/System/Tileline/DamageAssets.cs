using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DamageAssets : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DamagedBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        return playable;
    }
}
