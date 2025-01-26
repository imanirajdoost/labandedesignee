using UnityEngine;

public class BubbleAggressive : BubbleBase
{
    public override void DoYourThing()
    {
        base.DoYourThing();

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.OnPlatformDestroyed();

        Destroy(gameObject, 5);
    }
}
