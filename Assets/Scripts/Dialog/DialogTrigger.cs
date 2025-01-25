using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private int dialogIndex;
    [SerializeField] private bool _freezePlayer;

    public bool FreezePlayer => _freezePlayer;

    public int Index => dialogIndex;
}
