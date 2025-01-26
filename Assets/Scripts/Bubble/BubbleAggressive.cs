using UnityEngine;

public class BubbleAggressive : BubbleBase
{
    public override void DoYourThing()
    {
        base.DoYourThing();

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.OnPlatformDestroyed();

        GameManager.Instance.SpawnImpactVFX(transform.position);

        DisableObjectAfter(5);
    }

    protected override void Despawn()
    {
        // Despawn Audio
    }
}
