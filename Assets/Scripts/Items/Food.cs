using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    private int amount = 1;
    private Transform _transform;
    private FoodManager manager;

    void Start()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();
        // Cache transform
        _transform = transform;
        manager.RespawnFood(this, _transform);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Snake>().OnFoodCollision(amount);
        manager.RespawnFood(this, _transform);
    }

    public void SetFoodManager(FoodManager manager)
    {
        this.manager = manager;
    }
}
