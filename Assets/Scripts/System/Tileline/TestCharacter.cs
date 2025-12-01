using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    public SkillData skillData;
    public List<TestCharacter> targets;

    public void SkillUse()
    {
        skillData.Use(targets);
    }
}
