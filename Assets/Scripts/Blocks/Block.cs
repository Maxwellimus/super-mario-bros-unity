using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3 startPos;

    float bounceHeight = 0.1f;
    float bounceTime = 0.1f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        startPos = transform.position;
    }

    public virtual void HitBlock()
    {
        StartCoroutine(BounceBlock(transform));
    }

    protected IEnumerator BounceBlock(Transform thisTransform)
    {
        Vector3 peakPos = new Vector3(startPos.x, startPos.y + bounceHeight);

        float i = 0.0f;
        float rate = 1.0f / bounceTime;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, peakPos, i);
            yield return null;
        }


        i = 0;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(peakPos, startPos, i);
            yield return null;
        }
    }
}
