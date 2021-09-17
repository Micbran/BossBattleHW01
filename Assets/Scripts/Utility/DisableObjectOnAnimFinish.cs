using UnityEngine;

public class DisableObjectOnAnimFinish : MonoBehaviour
{
    public void FinishAnimation()
    {
        gameObject.SetActive(false);
    }
}
