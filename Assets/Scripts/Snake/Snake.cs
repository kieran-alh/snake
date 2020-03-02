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

    // Input
    SnakeInputActions snakeInputActions;
    Vector2 movementInput;

    // Movement
    //Current Direction based on movementInput
    Vector2Int moveDirection;

    //Snake head current gridPosition
    Vector2Int gridPosition;
    Vector2Int prevGridPosition;
    float moveTimer;
    float maxMoveTime = .12f;
    float angle;

    // Body
    int bodySize = 0;
    // Holds the positions of each snake body segment
    List<SnakeBodyPosition> snakeBodyPositions;
    // Holds all the snake body game objects
    List<SnakeBodySegment> snakeBodySegments;

    // Collision
    float collisionTime = -1;
    float maxCollisionTime = 0;
    bool isCollidingObstacle = false;
    float collisionBufferMultiplier = 2f;
    Vector2Int onCollisionMoveDirection;

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
        snakeBodyPositions = new List<SnakeBodyPosition>();
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
        if (maxCollisionTime != 0)
            collisionTime += Time.deltaTime;

        if (moveTimer >= maxMoveTime)
        {
            moveTimer -= maxMoveTime;
            if (!isCollidingObstacle)
            {
                // Insert the current position into the body list
                snakeBodyPositions.Insert(0, new SnakeBodyPosition(gridPosition, moveDirection, angle));
                gridPosition += moveDirection;
                transform.position = new Vector3(gridPosition.x, gridPosition.y);
            }
            transform.eulerAngles = new Vector3(0, 0, angle);


            // If the bodySize has increased since last move
            // Create the new Body segment
            if (bodySize > snakeBodySegments.Count)
                CreateSnakeBodySegment();

            // If the number of postions is greater than the bodySize+1, remove position
            // Keep an extra buffer position to spawn new snake bodies
            // Instead of spawning them at the origin
            if (snakeBodyPositions.Count > bodySize + 1)
                snakeBodyPositions.RemoveAt(snakeBodyPositions.Count - 1);

            // Don't move the tail if snake is colliding with obstacle
            if (!isCollidingObstacle)
            {
                // Update the tail positions, ignoring last buffer position
                for (int i = 0; i < snakeBodyPositions.Count - 1; i++)
                {
                    snakeBodySegments[i].SetBodyPosition(snakeBodyPositions[i]);

                    // If there is a mismatch in size, Error
                    if (i > snakeBodySegments.Count)
                        Debug.LogError($"Snake body size={bodySize} and number of body segments={snakeBodySegments.Count} don't match.");
                }
            }
        }

        CheckDeath();
    }

    // Trigger Collision for Snake Head
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case Constants.SNAKE_TAG:
            case Constants.BORDER_TAG:
                isCollidingObstacle = true;
                onCollisionMoveDirection = moveDirection;
                moveDirection = Vector2Int.zero;
                collisionTime = 0f;
                maxCollisionTime = collisionBufferMultiplier*maxMoveTime;
                break;
        }
    }
   
    // Trigger Collision Exit for Snake Head
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case Constants.SNAKE_TAG:
            case Constants.BORDER_TAG:
                isCollidingObstacle = false;
                collisionTime = 0f;
                moveTimer += maxMoveTime;
                maxCollisionTime = -1f;
                break;
        }
    }


    public void OnFoodCollision(int sizeDelta)
    {
        bodySize += sizeDelta;
    }

    public void CheckDeath()
    {
        // Death Check
        if (isCollidingObstacle && collisionTime > maxCollisionTime)
        {
            Debug.Log("Death");
            for (int i = 0; i < snakeBodySegments.Count; i++)
            {
                Destroy(snakeBodySegments[i].bodySegmentGameObj);
            }
            Destroy(this.gameObject);
        }
    }

    void CreateSnakeBodySegment()
    {
        snakeBodySegments.Add(
            new SnakeBodySegment(snakeBodySegments.Count, snakeBodyPositions[snakeBodyPositions.Count-1], snakeBody)
            );
    }

    public List<Vector2Int> GetSnakePositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>() { gridPosition };
        for (int i = 0; i < snakeBodyPositions.Count; i++)
        {
            positions.Add(snakeBodyPositions[i].GridPosition);
        }
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
        public Vector2Int GridPosition
        {
            get { return this.gridPosition; }
        }

        public SnakeBodySegment(int index, SnakeBodyPosition spawnPosition, Sprite snakeBodySprite)
        {
            GameObject segment = new GameObject($"SnakeBody{index}");
            this.bodySegmentGameObj = segment;
            segment.tag = Constants.SNAKE_TAG;
            SpriteRenderer sr = segment.AddComponent<SpriteRenderer>();
            BoxCollider2D bodyCollider = segment.AddComponent<BoxCollider2D>();
            SnakeBody snakeBody = segment.AddComponent<SnakeBody>();
            bodyCollider.size = new Vector2(1, .9f);
            sr.sprite = snakeBodySprite;
            sr.sortingOrder = -index;
            _transform = segment.transform;
            SetBodyPosition(spawnPosition);
        }

        public void SetBodyPosition(SnakeBodyPosition bodyPosition)
        {
            this.gridPosition = bodyPosition.GridPosition;
            this._transform.position = new Vector3(this.gridPosition.x, this.gridPosition.y);
            this._transform.eulerAngles = new Vector3(0, 0, bodyPosition.Angle);
        }
    }

    private struct SnakeBodyPosition
    {
        Vector2Int gridPosition;
        Vector2Int moveDirection;
        float angle;
        public Vector2Int GridPosition
        {
            get { return this.gridPosition; }
        }
        public Vector2Int MoveDirection
        {
            get { return this.moveDirection; }
        }
        public float Angle
        {
            get { return this.angle; }
        }
        public SnakeBodyPosition(Vector2Int gridPosition, Vector2Int moveDirection, float angle)
        {
            this.gridPosition = gridPosition;
            this.moveDirection = moveDirection;
            this.angle = angle;
        }
    }
}
