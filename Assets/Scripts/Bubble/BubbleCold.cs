using UnityEngine;

public class BubbleCold : BubbleBase
{
    override public void DoYourThing()
    {
        base.DoYourThing();

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.OnCreatedPlatform(transform.gameObject);

        Destroy(gameObject, 5);
    }
}
