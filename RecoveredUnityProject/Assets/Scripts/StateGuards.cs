/// <summary>
/// 状态之间共用的判定函数，集中处理空引用和错误配置，减少每个状态的重复代码。
/// </summary>
public static class StateGuards
{
    public static bool TargetInsideChaseRange(Parameter parameter)
    {
        if (parameter == null || parameter.target == null ||
            parameter.chasePoints == null || parameter.chasePoints.Length < 2 ||
            parameter.chasePoints[0] == null || parameter.chasePoints[1] == null)
        {
            return false;
        }

        float left = parameter.chasePoints[0].position.x;
        float right = parameter.chasePoints[1].position.x;
        float targetX = parameter.target.position.x;
        return targetX >= UnityEngine.Mathf.Min(left, right) &&
               targetX <= UnityEngine.Mathf.Max(left, right);
    }
}
