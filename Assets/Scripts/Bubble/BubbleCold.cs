using UnityEngine;

public class BubbleCold : BubbleBase
{
    override public void DoYourThing()
    {
        base.DoYourThing();

        PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.OnCreatedPlatform(transform.gameObject);

        AkSoundEngine.PostEvent("Play_SFX_Bubble_Ice_Spawn", gameObject);

        //AkSoundEngine.PostEvent("Play_SFX_Bubble_Ice_Despawn", gameObject);

        DisableObjectAfter(5);
    }

    protected override void Despawn()
    {
        // Despawn Audio
       // AkSoundEngine.PostEvent("Play_SFX_Bubble_Ice_Despawn", gameObject);

    }
}
