using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // Public
    [Header("Objects")]
    public Rigidbody2D rb;
    public Collider2D headCollider;
    public Sprite snakeBody;

    [Header("Variables")]
    public int gridWidth;
    public int gridHeight;

    // Private
    SnakeInputActions snakeInputActions;

    // Movement Input controls
    Vector2 movementInput;

    //Current Direction based on movementInput
    Vector2Int moveDirection;
    Vector2Int prevMoveDirection;
    //Snake head current gridPosition
    Vector2Int gridPosition;
    float moveTimer;
    float maxMoveTime = .20f;
    float angle;

    int bodySize = 0;
    // Holds the positions of each snake body segment
    List<Vector2Int> snakeBodyPositions;
    // Holds all the snake body game objects
    List<SnakeBodySegment> snakeBodySegments;

    float collisionTime = -1;
    float maxCollisionTime = 0;

    private void Awake()
    {
        SetupInputActions();
    }
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        SetupMovement();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        Move();
    }

    void SetupInputActions()
    {
        snakeInputActions = new SnakeInputActions();
        snakeInputActions.SnakeControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
    }

    void SetupMovement()
    {
        snakeBodyPositions = new List<Vector2Int>();
        snakeBodySegments = new List<SnakeBodySegment>();

        gridPosition = new Vector2Int(gridWidth / 2, gridHeight / 2);
        moveDirection = new Vector2Int(0, 1);
        moveTimer = maxMoveTime;
    }

    void HandleInput()
    {
        float h = movementInput.x;
        float v = movementInput.y;
        if (h != 0)
            h = h < 0 ? -1 : 1;
        if (v != 0)
            v = v < 0 ? -1 : 1;

        // Right
        if (h == 1)
        {
            if (moveDirection.x != -1)
            {
                moveDirection.x = 1;
                moveDirection.y = 0;
                angle = 270f;
            }
        }
        // Left
        else if (h == -1)
        {
            if (moveDirection.x != 1)
            {
                moveDirection.x = -1;
                moveDirection.y = 0;
                angle = 90;
            }
        }
        // Up
        else if (v == 1)
        {
            if (moveDirection.y != -1)
            {
                moveDirection.x = 0;
                moveDirection.y = 1;
                angle = 0f;
            }
        }
        // Down
        else if (v == -1)
        {
            if (moveDirection.y != 1)
            {
                moveDirection.x = 0;
                moveDirection.y = -1;
                angle = 180f;
            }
        }
    }

    void Move()
    {
        moveTimer += Time.deltaTime;
        //if (maxCollisionTime != 0)
        //    collisionTime += Time.deltaTime;

        if (moveTimer >= maxMoveTime)
        {
            // Insert the current position into the body list
            snakeBodyPositions.Insert(0, gridPosition);

            gridPosition += moveDirection;
            moveTimer -= maxMoveTime;

            // If the bodySize has increased since last move
            // Create the new Body segment
            if (bodySize > snakeBodySegments.Count)
                CreateSnakeBodySegment();

            // If the number of postions is greater than the body size, remove position
            if (snakeBodyPositions.Count > bodySize)
                snakeBodyPositions.RemoveAt(snakeBodyPositions.Count - 1);

            // Update the heads position
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, angle);

            // Update the tail positions
            for (int i = 0; i < snakeBodyPositions.Count; i++)
            {
                Vector2Int bodyPosition = snakeBodyPositions[i];
                snakeBodySegments[i].SetBodyPosition(bodyPosition);

                // If there is a mismatch in size, Error
                if (i > snakeBodySegments.Count)
                    Debug.LogError($"Snake body size={bodySize} and number of body segments={snakeBodySegments.Count} don't match.");
            }
        }

        // Death Check
        /*
        if (collisionTime > maxCollisionTime)
        {
            OnDeath();
        }
        */
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case Constants.SNAKE_TAG:
            case Constants.BORDER_TAG:
                Debug.Log($"HIT: {collision.gameObject.name}");
                collisionTime = 0f;
                maxCollisionTime = 100 * maxMoveTime;
                break;
        }
    }
    */
    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case Constants.SNAKE_TAG:
            case Constants.BORDER_TAG:
                Debug.Log($"EXIT: {collision.gameObject.name}");
                collisionTime = 0f;
                maxCollisionTime = -1f;
                break;
        }
    }
    */
    // Trigger Collision for Snake Head
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case Constants.SNAKE_TAG:
            case Constants.BORDER_TAG:
                Debug.Log($"HIT: {collision.gameObject.name}");
                collisionTime = 0f;
                maxCollisionTime = 100*maxMoveTime;
                break;
        }
    }
    */
    // Trigger Collision Exit for Snake Head
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case Constants.SNAKE_TAG:
            case Constants.BORDER_TAG:
                Debug.Log($"EXIT: {collision.gameObject.name}");
                collisionTime = 0f;
                maxCollisionTime = -1f;
                break;
        }
    }
    */

    public void OnFoodCollision(int sizeDelta)
    {
        bodySize += sizeDelta;
    }

    public void OnDeath()
    {
        collisionTime = -1;
        for (int i = 0; i < snakeBodySegments.Count; i++)
        {
            Destroy(snakeBodySegments[i].bodySegmentGameObj);
        }
        Destroy(this.gameObject);
    }

    void CreateSnakeBodySegment()
    {
        snakeBodySegments.Add(new SnakeBodySegment(snakeBodySegments.Count, snakeBody));
    }

    public List<Vector2Int> GetSnakePositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>() { gridPosition };
        positions.AddRange(snakeBodyPositions);
        return positions;
    }

    private void OnEnable()
    {
        snakeInputActions.Enable();
    }

    private void OnDisable()
    {
        snakeInputActions.Disable();
    }

    private class SnakeBodySegment
    {
        public GameObject bodySegmentGameObj;
        private Transform _transform;
        private Vector2Int gridPosition;

        public SnakeBodySegment(int index, Sprite snakeBodySprite)
        {
            GameObject segment = new GameObject($"SnakeBody{index}");
            this.bodySegmentGameObj = segment;
            segment.tag = Constants.SNAKE_TAG;
            SpriteRenderer sr = segment.AddComponent<SpriteRenderer>();
            BoxCollider2D bodyCollider = segment.AddComponent<BoxCollider2D>();
            SnakeBody snakeBody = segment.AddComponent<SnakeBody>();
            bodyCollider.size = new Vector2(.9f, .9f);
            sr.sprite = snakeBodySprite;
            sr.sortingOrder = -index;
            _transform = segment.transform;
        }

        public void SetBodyPosition(Vector2Int inPosition)
        {
            this.gridPosition = inPosition;
            _transform.position = new Vector3(inPosition.x, inPosition.y);
        }
    }
}
