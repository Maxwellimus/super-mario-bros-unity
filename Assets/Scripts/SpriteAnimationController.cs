using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteAnimation[]))]
public class SpriteAnimationController : MonoBehaviour
{
    private SpriteAnimation[] spriteAnimations;
    private Dictionary<string, SpriteAnimation> spriteAnimationDictionary;
    private string currentPlayingAnimation;

    void Awake()
    {
        spriteAnimations = GetComponents<SpriteAnimation>();

        spriteAnimationDictionary = new Dictionary<string, SpriteAnimation>();

        for (int i = 0; i < spriteAnimations.Length; i++)
        {
            spriteAnimationDictionary[spriteAnimations[i].animationName] = spriteAnimations[i];
        }
    }

    public void PlayAnimation(string name)
    {
        if (currentPlayingAnimation != null && name != currentPlayingAnimation)
        {
            StopCurrentAnimation();
        }

        if (!spriteAnimationDictionary[name].animationPlaying)
        {
            spriteAnimationDictionary[name].PlayAnimation();
            currentPlayingAnimation = name;
        }
    }

    public void StopCurrentAnimation()
    {
        spriteAnimationDictionary[currentPlayingAnimation].StopAnimation();
    }
}
