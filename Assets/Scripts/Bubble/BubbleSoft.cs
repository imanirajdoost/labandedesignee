using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BubbleSoft : BubbleBase
{
    [SerializeField] private Transform _playerAttachPoint;

    [SerializeField]
    private Transform _targetToFlyTo;

    private bool _shouldFlyToTarget = false;

    [SerializeField] private PlayerManager _playerManager;

    public void AttachPlayer()
    {
        _playerManager = FindFirstObjectByType<PlayerManager>();

        _playerManager.ForceAttach();

        _playerManager.transform.SetParent(_playerAttachPoint);
        _playerManager.transform.localPosition = Vector3.zero;
    }

    public override void DoYourThing()
    {
        base.DoYourThing();
        AttachPlayer();

        _shouldFlyToTarget = true;

        AkSoundEngine.PostEvent("Play_SFX_Bubble_Balloon_Spawn", gameObject);
    }

    public void SetTarget(Transform target)
    {
        _targetToFlyTo = target;
    }

    private void Update()
    {
        if (_shouldFlyToTarget)
            FlyTowardsTheTarget();
    }

    private void FlyTowardsTheTarget()
    {
        var step = 2 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _targetToFlyTo.position, step);
        if (Vector3.Distance(transform.position, _targetToFlyTo.position) < 0.001f)
        {
            _shouldFlyToTarget = false;
            UnAttachPlayer();
        }
    }

    private void UnAttachPlayer()
    {
        if (_playerManager == null)
            FindFirstObjectByType<PlayerManager>();

        _playerManager.transform.SetParent(null);
        _playerManager.ForceDetach();

        DisableObjectAfter(1f);
    }

    protected override void Despawn()
    {
        // Despawn Audio

        // AkSoundEngine.PostEvent("Play_SFX_Bubble_Balloon_Despawn", gameObject);
    }
}
