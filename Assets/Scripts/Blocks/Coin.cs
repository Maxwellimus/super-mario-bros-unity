using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteAnimationController animationController;

    void Start()
    {
        animationController = GetComponent<SpriteAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        animationController.PlayAnimation("Spinning");
    }

    public IEnumerator BounceCoin(Vector3 startPos)
    {
        float coinBounceHeight = 3f;
        float coinBounceTime = 0.25f;

        Vector3 peakPos = new Vector3(startPos.x, startPos.y + coinBounceHeight);
        Vector3 endPos = new Vector3(startPos.x, startPos.y + 1);

        float i = 0.0f;
        float rate = 1.0f / coinBounceTime;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, peakPos, i);
            yield return null;
        }


        i = 0;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(peakPos, endPos, i);
            yield return null;
        }

        Destroy(gameObject);
    }
}
