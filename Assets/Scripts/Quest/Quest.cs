using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Quest : MonoBehaviour
{
    public Action OnSuccess;
    public Action OnFailure;

    public bool IsStarted { get; private set; }
    public bool IsSucceed { get; private set; }
    public bool IsFailed { get; private set; }

    public bool IsFinished => IsSucceed || IsFailed;
    public bool IsActive => IsStarted && (IsFinished == false);

    public virtual void Succeed()
    {
        IsSucceed = true;
        OnSuccess?.Invoke();
    }

    public virtual void Fail()
    {
        IsFailed = true;
        OnFailure?.Invoke();
    }
}
