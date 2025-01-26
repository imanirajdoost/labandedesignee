using UnityEngine;

public class BubbleCold : BubbleBase
{
    override public void DoYourThing()
    {
        base.DoYourThing();

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.OnCreatedPlatform(transform.gameObject);

        DisableObjectAfter(5);
    }

    protected override void Despawn()
    {
        // Despawn Audio
    }
}
