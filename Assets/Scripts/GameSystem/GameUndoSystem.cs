using System;
using System.Collections.Generic;
using Events.UnitEvent;

public class GameUndoSystem : IDisposable
{
    Stack<IUndoableAction> turnActions = new Stack<IUndoableAction>();

    private readonly EventBinding<UnitMoveCommittedEvent> commitBinding;

    public GameUndoSystem()
    {
        commitBinding = new EventBinding<UnitMoveCommittedEvent>();
        commitBinding.Add(OnMoveCommitted);
        EventBus<UnitMoveCommittedEvent>.Register(commitBinding);
    }

    public void Push(IUndoableAction action)
    {
        turnActions.Push(action);
    }

    private void OnMoveCommitted(UnitMoveCommittedEvent evt)
    {
        Push(evt.ActionData);
    }

    public void Undo(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (turnActions.TryPop(out IUndoableAction action))
                action.Undo();
        }
    }

    public void UndoAll()
    {
        for(int i = 0; i < turnActions.Count; i++)
        {
            if(turnActions.TryPop(out IUndoableAction action))
            {
                action.Undo();
            }
        }
    }

    public void Dispose()
    {
        EventBus<UnitMoveCommittedEvent>.Deregister(commitBinding);
    }
}