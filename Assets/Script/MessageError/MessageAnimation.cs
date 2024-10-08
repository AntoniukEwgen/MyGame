using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : MonoBehaviour
{
    public float animationTime = 1f; // ��� �������, ���� �� ������ ������ � �������� Unity
    public float delayTime = 1f; // ��� ��������, ���� �� ������ ������ � �������� Unity

    void OnEnable()
    {
        // ��������� �������� � 0 �� 1.5, � ���� �� 1
        transform.localScale = Vector3.zero; // ���������� �������
        transform.DOScale(1.5f, animationTime / 2).SetUpdate(true) // ��������� �������� �� 1.5
            .OnComplete(() =>
            {
                transform.DOScale(1f, animationTime / 2).SetUpdate(true) // ��������� �������� �� 1
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(delayTime, () =>
                    {
                        transform.DOScale(0, animationTime / 2).SetUpdate(true) // ��������� �������� �� 0
                        .OnComplete(() => gameObject.SetActive(false)); // ��������� ��'����
                    });
                });
            });
    }
}
