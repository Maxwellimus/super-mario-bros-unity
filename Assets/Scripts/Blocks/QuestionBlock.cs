using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : Block
{

    private SpriteAnimationController animationController;
    private SpriteRenderer spriteRenderer;

    enum Content
    {
        Mushroom,
        Coin,
        TenCoin,
        FireFlower,
    }

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animationController = GetComponent<SpriteAnimationController>();

        animationController.PlayAnimation("Idle");
    }

    public override void HitBlock()
    {
        base.HitBlock();
        // TODO: Handle coin animation
    }
}
