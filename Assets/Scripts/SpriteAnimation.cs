using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Sprite[]))]
public class SpriteAnimation : MonoBehaviour
{
    public enum Type
    {
        Loop,
        Bounce,
        Single,
    }

    public string animationName;
    public float animationSpeed;
    public Type animationType;
    public Sprite[] sprites;

    [HideInInspector]
    public bool animationPlaying;

    private SpriteRenderer spriteRenderer;
    private float deltaTime;
    private int frame;
    private bool animatingForward = true;
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
                frame = GetNextFrame(frame);
            }
            //Animate sprite with selected frame
            spriteRenderer.sprite = sprites[frame];
        }
    }

    int GetNextFrame(int currentFrame)
    {

        switch (animationType)
        {
            case Type.Bounce:
                if(animatingForward && currentFrame == sprites.Length - 1)
                {
                    animatingForward = false;
                } else if(!animatingForward && currentFrame == 0)
                {
                    animatingForward = true;
                }

                return animatingForward ? currentFrame + 1 : currentFrame - 1;
            case Type.Loop:
                return (currentFrame + 1) % sprites.Length;
            case Type.Single:
                currentFrame++;
                if (currentFrame >= sprites.Length)
                {
                    return sprites.Length - 1;
                }
                return currentFrame;
            default:
                return 0;
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
