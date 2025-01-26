using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BubbleAggressive : BubbleBase
{
    private GameObject _targetToDestroy;

    private float timeToOverride = 1f;

    private bool _shouldDestroyTarget = true;

    public void SetTarget(GameObject target)
    {
        _targetToDestroy = target;
    }

    public void SetDestroyObject(bool shouldDestroy)
    {
        _shouldDestroyTarget = shouldDestroy;
    }

    public void OverrideTime(float time)
    {
        timeToOverride = time;
    }

    public override void DoYourThing()
    {
        base.DoYourThing();

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.OnPlatformDestroyed();

        if(_targetToDestroy == null)
        {
            DisableObjectAfter(2);
            return;
        }

        StartCoroutine(MoveTowardsThePlatformToDestroy());
    }

    private IEnumerator MoveTowardsThePlatformToDestroy()
    {
        transform.DOMove(_targetToDestroy.transform.position, timeToOverride).SetEase(Ease.InBack);

        yield return new WaitForSeconds(timeToOverride);

        GameManager.Instance.SpawnImpactVFX(_targetToDestroy.transform.position);

        if (_shouldDestroyTarget)
        {
            if (_targetToDestroy != null)
                Destroy(_targetToDestroy);
        }

        DisableObjectAfter(2);
    }

    protected override void Despawn()
    {
        // Despawn Audio
    }
}
