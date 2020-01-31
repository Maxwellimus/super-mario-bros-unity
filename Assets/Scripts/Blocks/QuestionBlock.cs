using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : Block
{

    private SpriteAnimationController animationController;

    public enum Content
    {
        Mushroom,
        Coin,
        TenCoin,
        FireFlower,
    }

    public GameObject coinPrefab;
    public GameObject mushroomPrefab;

    public Content questionBoxItem;
    private int capacity;

    public override void Start()
    {
        base.Start();

        animationController = GetComponent<SpriteAnimationController>();

        switch (questionBoxItem)
        {
            case Content.Mushroom:
            case Content.Coin:
            case Content.FireFlower:
                capacity = 1;
                break;
            case Content.TenCoin:
                capacity = 10;
                break;
        }

        
    }

    private void Update()
    {
        if(capacity > 0)
        {
            animationController.PlayAnimation("Idle");
        } else
        {
            animationController.PlayAnimation("Empty");
        }
        
    }

    public override void HitBlock()
    {
        if(capacity == 0)
        {
            return;
        }

        base.HitBlock();
        // TODO: Handle coin animation
        capacity--;

        switch (questionBoxItem)
        {
            case Content.Mushroom:
                Mushroom mushroom = Instantiate(mushroomPrefab, transform).GetComponent<Mushroom>();
                StartCoroutine(mushroom.AnimateOutOfBox(transform.position));
                break;
            case Content.Coin:
            case Content.TenCoin:
                Coin coin = Instantiate(coinPrefab, transform).GetComponent<Coin>();
                StartCoroutine(coin.BounceCoin(transform.position));
                break;
            case Content.FireFlower:
                break;
        }
    }

    private void animateCoin()
    {
        


    }
}
