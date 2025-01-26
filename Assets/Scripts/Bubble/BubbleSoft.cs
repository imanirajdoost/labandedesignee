using UnityEngine;

public class BubbleSoft : BubbleBase
{
    [SerializeField] private Transform _playerAttachPoint;

    [SerializeField]
    private Transform _targetToFlyTo;

    private bool _shouldFlyToTarget;

    public void AttachPlayer()
    {
        var player = FindFirstObjectByType<PlayerManager>();

        player.transform.SetParent(_playerAttachPoint);
        player.transform.localPosition = Vector3.zero;
    }

    public override void DoYourThing()
    {
        base.DoYourThing();
        AttachPlayer();

        _shouldFlyToTarget = true;
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
        }
    }
}
