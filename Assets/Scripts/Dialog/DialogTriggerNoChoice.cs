using UnityEngine;

public class DialogTriggerNoChoice : DialogTrigger
{
    public override void SetTriggered()
    {
        base.SetTriggered();
        DialogManager.Instance.ShowDialog(Index);
        if (_shouldDestroyAfterTriggered)
            Destroy(gameObject, 2);
    }
}
