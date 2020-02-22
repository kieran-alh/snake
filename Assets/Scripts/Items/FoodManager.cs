using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public GlobalGameManager globalManager;
    public int gridWith;
    public int gridHeight;
    public Food food;

    private void Start()
    {
        if (!globalManager)
            globalManager = GetComponent<GlobalGameManager>();
        food.SetFoodManager(this);
    }

    public void RespawnFood(Food food, Transform foodTransform)
    {
        foodTransform.position = GetRandomPosition2D();
    }

    private Vector3 GetRandomPosition2D()
    {
        Vector2Int randPos;
        do
        {
            randPos = new Vector2Int(Random.Range(0, this.gridWith), Random.Range(0, this.gridHeight));
        } while (globalManager.GetAllSnakePositions().IndexOf(randPos) != -1);
        return new Vector3(randPos.x, randPos.y);
    }
}
