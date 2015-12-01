using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.Events
{

  #region Death Event

  public class DeathEventData : BaseEventData
  {
    public string TargetData;

    public DeathEventData(EventSystem eventSystem, string targetData)
      : base(eventSystem)
    {
      TargetData = targetData;
    }
  }

  public interface IDeathHandler : IEventSystemHandler
  {
    void OnDeathTrigger(DeathEventData eventData);
  }

  public static class DeathTriggerEvents
  {
    // helper to return the function that should be invoked
    public static ExecuteEvents.EventFunction<IDeathHandler> DeathEventHandler
    {
      get { return Execute; }
    }

    // call that does the mapping
    private static void Execute(IDeathHandler handler, BaseEventData eventData)
    {
      // The ValidateEventData makes sure the passed event 
      // data is of the correct type and not null
      handler.OnDeathTrigger(ExecuteEvents.ValidateEventData<DeathEventData>(eventData));
    }
  }

  #endregion Death Event

  #region GameOver Event

  public class GameOverEventData : BaseEventData
  {
    public string TargetData;

    public GameOverEventData(EventSystem eventSystem, string targetData)
      : base(eventSystem)
    {
      TargetData = targetData;
    }
  }

  public interface IGameOverHandler : IEventSystemHandler
  {
    void OnGameOverTrigger(GameOverEventData eventData);
  }

  public static class GameOverTriggerEvents
  {
    // helper to return the function that should be invoked
    public static ExecuteEvents.EventFunction<IGameOverHandler> GameOverEventHandler
    {
      get { return Execute; }
    }

    // call that does the mapping
    private static void Execute(IGameOverHandler handler, BaseEventData eventData)
    {
      // The ValidateEventData makes sure the passed event 
      // data is of the correct type and not null
      handler.OnGameOverTrigger(ExecuteEvents.ValidateEventData<GameOverEventData>(eventData));
    }
  }

  #endregion GameOver Event
}