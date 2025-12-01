using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DamagedBehaviour : PlayableBehaviour
{
    bool isDone;

    public override void OnGraphStart(Playable playable)
    {
        isDone = false;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (isDone) return;

        TestCharacter testCharacter = playerData as TestCharacter;

        if (testCharacter == null) return;

        testCharacter.SkillUse();

        isDone = true;
    }
}

[Serializable]
public class SkillData
{
    public int damage;

    public virtual void Use(List<TestCharacter> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] == null) continue;

            ApplyDamage(objects[i]);
        }
    }

    void ApplyDamage(TestCharacter obj)
    {
        Debug.Log($"Applying {damage} damage to {obj}");
    }
}
