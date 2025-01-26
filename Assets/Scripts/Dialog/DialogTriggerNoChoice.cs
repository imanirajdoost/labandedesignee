using UnityEngine;

public class DialogTriggerNoChoice : DialogTrigger
{
    public override void SetTriggered()
    {
        base.SetTriggered();
        if(_shouldDestroyAfterTriggered)
            Destroy(gameObject, 2);
    }
}
