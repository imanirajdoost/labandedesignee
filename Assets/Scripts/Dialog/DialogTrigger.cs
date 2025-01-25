using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private int dialogIndex;
    [SerializeField] private bool _freezePlayer;
    [SerializeField] private bool _slowMotion;
    private int _triggerCount = 0;
    [SerializeField] private bool _shouldTriggerOnce;

    public int GetTriggerCount()
    {
        return _triggerCount;
    }

    public void SetTriggered()
    {
        _triggerCount++;
    }

    public bool ShouldTriggerOnce => _shouldTriggerOnce;

    public bool SlowMotion => _slowMotion;

    public int TriggerCount => _triggerCount;

    public bool FreezePlayer => _freezePlayer;

    public int Index => dialogIndex;
}
