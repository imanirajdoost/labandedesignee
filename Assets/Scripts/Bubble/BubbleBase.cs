using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BubbleBase : MonoBehaviour
{
    public string BubbleType;

    public virtual void DoYourThing()
    {

    }

    protected void DisableObjectAfter(float time)
    {
        StartCoroutine(DisableAfter(time));
    }

    private IEnumerator DisableAfter(float time)
    {
        // make scale to 0 with dotween
        gameObject.transform.DOScale(Vector3.zero, time - 0.1f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
