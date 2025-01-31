using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    protected bool _isEnabled = true;

    [SerializeField] private int dialogIndex;
    [SerializeField] private bool _freezePlayer;
    [SerializeField] private bool _slowMotion;
    protected int _triggerCount = 0;
    [SerializeField] private bool _shouldTriggerOnce;
    [SerializeField] protected bool _shouldDestroyAfterTriggered;

    public bool IsEnabled => _isEnabled;

    public int GetTriggerCount()
    {
        return _triggerCount;
    }

    public virtual void SetTriggered()
    {
        if (!_isEnabled)
            return;
        _triggerCount++;
    }

    public bool ShouldTriggerOnce => _shouldTriggerOnce;

    public bool SlowMotion => _slowMotion;

    public int TriggerCount => _triggerCount;

    public bool FreezePlayer => _freezePlayer;

    public int Index => dialogIndex;
}
