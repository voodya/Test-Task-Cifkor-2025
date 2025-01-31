using System;
using UnityEngine;

public static class WebResultValidator
{
    public static void Validate(this ExecuteResult res, Action<ExecuteResult> action)
    {
        if (!res.Success)
        {
            Debug.LogError(res.ResultDescription);
            return;
        }
        else
        {
            action?.Invoke(res);
        }
    }
}
