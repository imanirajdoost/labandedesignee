using DG.Tweening;
using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 0.2f;
    private void Start()
    {
        // Make the object turn around its pivot point
        transform.DORotate(new Vector3(0, 180, 0), _rotationSpeed).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
}
