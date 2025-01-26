using UnityEngine;

public class BubbleCold : BubbleBase
{
    private bool _shouldAttach = true;

    public void ForceAttach(bool attach)
    {
        _shouldAttach = attach;
    }

    override public void DoYourThing()
    {
        base.DoYourThing();

        if (_shouldAttach)
        {
            PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
            playerManager.OnCreatedPlatform(transform.gameObject);
        }
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
