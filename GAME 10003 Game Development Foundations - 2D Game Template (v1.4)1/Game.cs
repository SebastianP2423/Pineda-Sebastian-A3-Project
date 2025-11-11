using System;
using System.Numerics;

namespace MohawkGame2D
{
    public class Game
    {
        // objects
        Paddle leftPaddle;
        Paddle rightPaddle;
        Ball ball;

        // movement
        float windowWidth = 400;
        float windowHeight = 400;
        float paddleSpeed = 5f;

        // Score tracking
        int leftScore = 0;
        int rightScore = 0;

        public void Setup()
        {
            Window.SetTitle("Pong Clone");
            Window.SetSize((int)windowWidth, (int)windowHeight);

            // color to differentiate paddles
            leftPaddle = new Paddle(new Vector2(20, windowHeight / 2 - 30), new Vector2(10, 60), Color.Red);
            rightPaddle = new Paddle(new Vector2(windowWidth - 30, windowHeight / 2 - 30), new Vector2(10, 60), Color.Blue);

            ball = new Ball(new Vector2(windowWidth / 2 - 5, windowHeight / 2 - 5), new Vector2(10, 10));
        }

        public void Update()
        {
            HandleInput();
            ball.Move();

            // collision to keep the ball within the frame 
            if (ball.position.Y <= 0 || ball.position.Y + ball.size.Y >= windowHeight)
            {
                ball.velocity.Y *= -1;
            }

            // collisions but with the paddles
            if (Collides(ball, leftPaddle) && ball.velocity.X < 0)
                ball.velocity.X *= -1;

            if (Collides(ball, rightPaddle) && ball.velocity.X > 0)
                ball.velocity.X *= -1;

            // scoring 
            if (ball.position.X < 0)
            {
                rightScore++;
                ResetBall(-1);
            }
            else if (ball.position.X > windowWidth)
            {
                leftScore++;
                ResetBall(1);
            }

            DrawScene();
        }

        void HandleInput()
        {
            // Left paddle (W/S)
            if (Input.IsKeyboardKeyDown(KeyboardInput.W))
                leftPaddle.position.Y -= paddleSpeed;
            if (Input.IsKeyboardKeyDown(KeyboardInput.S))
                leftPaddle.position.Y += paddleSpeed;

            // Right paddle (Up/Down)
            if (Input.IsKeyboardKeyDown(KeyboardInput.Up))
                rightPaddle.position.Y -= paddleSpeed;
            if (Input.IsKeyboardKeyDown(KeyboardInput.Down))
                rightPaddle.position.Y += paddleSpeed;

            // Clamps the paddle within the window 
            leftPaddle.position.Y = Math.Clamp(leftPaddle.position.Y, 0, windowHeight - leftPaddle.size.Y);
            rightPaddle.position.Y = Math.Clamp(rightPaddle.position.Y, 0, windowHeight - rightPaddle.size.Y);
        }

        void ResetBall(int direction)
        {
            // Center the ball
            ball.position = new Vector2(windowWidth / 2 - ball.size.X / 2, windowHeight / 2 - ball.size.Y / 2);

            float randomY = Random.Float(-2f, 2f);
            ball.velocity = new Vector2(4 * direction, randomY);
        }

        bool Collides(Ball b, Paddle p)
        {
            return b.position.X < p.position.X + p.size.X &&
                   b.position.X + b.size.X > p.position.X &&
                   b.position.Y < p.position.Y + p.size.Y &&
                   b.position.Y + b.size.Y > p.position.Y;
        }

        void DrawScene()
        {
            // background
            Draw.FillColor = Color.Black;
            Draw.LineColor = Color.Black;
            Draw.Rectangle(new Vector2(0, 0), new Vector2(windowWidth, windowHeight));

            // paddles and ball render
            leftPaddle.Render();
            rightPaddle.Render();
            ball.Render();

            // print score to console, not done YET
            Console.WriteLine($"Left: {leftScore} | Right: {rightScore}");
        }
    }

    public class Paddle
    {
        public Vector2 position;
        public Vector2 size;
        public Color color;

        public Paddle(Vector2 pos, Vector2 sz, Color col)
        {
            position = pos;
            size = sz;
            color = col;
        }

        public void Render()
        {
            Draw.FillColor = color;
            Draw.LineColor = Color.Black;
            Draw.Rectangle(position, size);
        }
    }

    public class Ball
    {
        public Vector2 position;
        public Vector2 size;
        public Vector2 velocity = new Vector2(4, 2);

        public Ball(Vector2 pos, Vector2 sz)
        {
            position = pos;
            size = sz;
        }

        public void Move()
        {
            position += velocity;
        }

        public void Render()
        {
            Draw.FillColor = Color.White;
            Draw.LineColor = Color.Black;
            Draw.Rectangle(position, size);
        }
    }
}

