using System;

public class Topic<T>
{
    private T m_Value;
    private Action<T> m_OnValueChanged;
    private Action m_OnValueChangedParameterless;

    public virtual T Value
    {
        get => m_Value;
        set
        {
            if (Equals(m_Value, value) == false)
            {
                m_Value = value;
                m_OnValueChanged?.Invoke(m_Value);
                m_OnValueChangedParameterless?.Invoke();
            }
        }
    }

    public Topic() : this(default) { }

    public Topic(T startingValue)
    {
        m_Value = startingValue;
    }

    public void Subscribe(Action<T> action)
    {
        action?.Invoke(m_Value);
        m_OnValueChanged += action;
    }

    public void Unsubscribe(Action<T> action)
    {
        m_OnValueChanged -= action;
    }

    public void Subscribe(Action action)
    {
        action?.Invoke();
        m_OnValueChangedParameterless += action;
    }

    public void Unsubscribe(Action action)
    {
        m_OnValueChangedParameterless -= action;
    }
}