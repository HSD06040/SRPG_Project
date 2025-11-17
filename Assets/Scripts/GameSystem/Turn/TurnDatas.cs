using GameSystemEnum;
using UnityEngine;

namespace SRPG.ActionData
{
    public struct MoveActionData : IUndoableAction
    {
        public IGameUnit Unit;
        public Vector2Int BeforePos;
        public Vector2Int AfterPos;

        public MoveActionData(IGameUnit unit, Vector2Int beforePos, Vector2Int afterPos)
        {
            this.Unit = unit;
            this.BeforePos = beforePos;
            this.AfterPos = afterPos;
        }

        public void Undo()
        {
            Unit.CurPos = BeforePos;
        }
    }

    public struct AttackActionData : IUndoableAction
    {
        public IGameUnit Attacker;
        public IGameUnit Target;
        public int DamageDone;

        public AttackActionData(IGameUnit attacker, IGameUnit target, int damageDone)
        {
            this.Attacker = attacker;
            this.Target = target;
            this.DamageDone = damageDone;
        }

        public void Undo()
        {
            //target.HP += damageDone;
        }
    }
}