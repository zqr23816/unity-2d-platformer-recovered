/// <summary>
/// 敌人状态的统一生命周期。FSM 只依赖该接口，不需要了解具体状态实现。
/// </summary>
public interface IState
{
    /// <summary>进入状态时执行一次。</summary>
    void OnEnter();

    /// <summary>处于状态期间每帧执行。</summary>
    void OnUpdate();

    /// <summary>离开状态时执行一次，用于清理临时数据。</summary>
    void OnExit();
}
