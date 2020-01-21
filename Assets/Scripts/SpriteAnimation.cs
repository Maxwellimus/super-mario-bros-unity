using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Sprite[]))]
public class SpriteAnimation : MonoBehaviour
{
    public string animationName;
    public float animationSpeed;
    public bool loop;
    public Sprite[] sprites;

    [HideInInspector]
    public bool animationPlaying;

    private SpriteRenderer spriteRenderer;
    private float deltaTime;
    private int frame;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animationPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(animationPlaying)
        {
            //Keep track of the time that has passed
            deltaTime += Time.deltaTime;

            /*Loop to allow for multiple sprite frame 
             jumps in a single update call if needed
             Useful if frameSeconds is very small*/
            while (deltaTime >= animationSpeed)
            {
                deltaTime -= animationSpeed;
                frame++;
                if (loop)
                    frame %= sprites.Length;
                //Max limit
                else if (frame >= sprites.Length)
                    frame = sprites.Length - 1;
            }
            //Animate sprite with selected frame
            spriteRenderer.sprite = sprites[frame];
        }
    }

    public void PlayAnimation()
    {
        animationPlaying = true;
    }

    public void StopAnimation()
    {
        animationPlaying = false;
    }
}
