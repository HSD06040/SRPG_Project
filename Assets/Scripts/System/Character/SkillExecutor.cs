using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

public class SkillExecutor : MonoBehaviour
{
    public PlayableDirector director;
    public TimelineAsset skillTimeline;

    public List<AnimationTrack> attackerTrack;
    public List<AnimationTrack> targetTrack;

    public void PlaySkill(GameObject attacker, List<GameObject> targets)
    {
        director.playableAsset = skillTimeline;

        for (int i = 0; i < attackerTrack.Count; i++)
        {
            director.SetGenericBinding(attackerTrack[i], attacker);
        }

        for (int i = 0; i < targetTrack.Count; i++)
        {
            director.SetGenericBinding(targetTrack[i], targets[i]);
        }        

        director.Play();
    }
}

public class Test1
{
    public void Test()
    {
        List<Boss> boss = new();

        DoDamage(boss, 1);
    }

    public void DoDamage(IEnumerable<Enemy> enemy, int damage)
    {

    }

    public void DoDamages(List<Enemy> enemy, int daamge)
    {

    }
}

public class Enemy
{

}

public class Boss : Enemy
{

}