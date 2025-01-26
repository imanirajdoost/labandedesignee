using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BubbleAggressive : BubbleBase
{
    private GameObject _targetToDestroy;

    public void SetTarget(GameObject target)
    {
        _targetToDestroy = target;
    }

    public override void DoYourThing()
    {
        base.DoYourThing();

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.OnPlatformDestroyed();

        StartCoroutine(MoveTowardsThePlatformToDestroy());
    }

    private IEnumerator MoveTowardsThePlatformToDestroy()
    {
        transform.DOMove(_targetToDestroy.transform.position, 1f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(1f);

        GameManager.Instance.SpawnImpactVFX(_targetToDestroy.transform.position);

        if (_targetToDestroy != null)
            Destroy(_targetToDestroy);

        DisableObjectAfter(2);
    }

    protected override void Despawn()
    {
        // Despawn Audio
    }
}
