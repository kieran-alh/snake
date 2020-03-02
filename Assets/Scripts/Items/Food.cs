using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodManager manager;
    public bool spawnRandom;

    private int amount = 1;
    private Transform _transform;

    void Start()
    {
        // Cache transform
        _transform = transform;
        if (manager && spawnRandom)
            manager.RespawnFood(this, _transform);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Snake>().OnFoodCollision(amount);
        manager.RespawnFood(this, _transform);
    }
}
