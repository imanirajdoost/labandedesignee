using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private int dialogIndex;
    [SerializeField] private bool _freezePlayer;
    [SerializeField] private bool _slowMotion;
    private bool _isAlreadyTriggered = false;

    public void SetAlreadyTriggered()
    {
        _isAlreadyTriggered = true;
    }

    public bool SlowMotion => _slowMotion;

    public bool IsAlreadyTriggered => _isAlreadyTriggered;

    public bool FreezePlayer => _freezePlayer;

    public int Index => dialogIndex;
}
