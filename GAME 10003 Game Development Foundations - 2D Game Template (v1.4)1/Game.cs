using System;
using System.Numerics;

namespace MohawkGame2D
{
    public class Game
    {
        Paddle leftPaddle;
        Paddle rightPaddle;
        Ball ball;
        PaddleAI rightPaddleAI;
        Score score;

        float windowWidth = 400;
        float windowHeight = 400;
        float paddleSpeed = 5f;

        public void Setup()
        {
            Window.SetTitle("Pong Clone");
            Window.SetSize((int)windowWidth, (int)windowHeight);

            leftPaddle = new Paddle(new Vector2(20, windowHeight / 2 - 30), new Vector2(10, 60), Color.Red);
            rightPaddle = new Paddle(new Vector2(windowWidth - 30, windowHeight / 2 - 30), new Vector2(10, 60), Color.Blue);
            ball = new Ball(new Vector2(windowWidth / 2 - 5, windowHeight / 2 - 5), new Vector2(10, 10));
            rightPaddleAI = new PaddleAI(rightPaddle, ball, 4f);
            score = new Score();
        }

        public void Update()
        {
            HandleInput();
            rightPaddleAI.Update();
            ball.Move();

            // bounce off top/bottom
            if (ball.position.Y <= 0 || ball.position.Y + ball.size.Y >= windowHeight)
            {
                ball.velocity.Y *= -1;
                ball.ChangeColor();
            }

            // bounce off paddles
            if (Collides(ball, leftPaddle) && ball.velocity.X < 0)
            {
                ball.velocity.X *= -1;
                ball.ChangeColor();
                score.AddHit();
                ball.IncreaseSpeed();
            }

            if (Collides(ball, rightPaddle) && ball.velocity.X > 0)
            {
                ball.velocity.X *= -1;
                ball.ChangeColor();
                score.AddHit();
                ball.IncreaseSpeed();
            }

            // scoring
            if (ball.position.X < 0)
            {
                score.AddRightPoint();
                ResetBall(-1);
            }
            else if (ball.position.X > windowWidth)
            {
                score.AddLeftPoint();
                ResetBall(1);
            }

            DrawScene();
        }

        void HandleInput()
        {
            // Player controls left paddle
            if (Input.IsKeyboardKeyDown(KeyboardInput.W))
                leftPaddle.position.Y -= paddleSpeed;
            if (Input.IsKeyboardKeyDown(KeyboardInput.S))
                leftPaddle.position.Y += paddleSpeed;

            leftPaddle.position.Y = Math.Clamp(leftPaddle.position.Y, 0, windowHeight - leftPaddle.size.Y);
        }

        void ResetBall(int direction)
        {
            ball.position = new Vector2(windowWidth / 2 - ball.size.X / 2, windowHeight / 2 - ball.size.Y / 2);
            float randomY = Random.Float(-2f, 2f);
            ball.velocity = new Vector2(4 * direction, randomY);
            ball.speedMultiplier = 1f; // resets the speed multiplier of the bal after each scored point 
            ball.ChangeColor();
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
            Draw.FillColor = Color.Black;
            Draw.LineColor = Color.Black;
            Draw.Rectangle(new Vector2(0, 0), new Vector2(windowWidth, windowHeight));

            leftPaddle.Render();
            rightPaddle.Render();
            ball.Render();

            // Draws the score text
            Text.Color = Color.White;
            Text.Size = 16;
            Text.Draw($"Left: {score.leftPoints} | Right: {score.rightPoints}", 10, 10);
            Text.Draw($"Hits: {score.hits}", 10, 30);
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
        public Color color = Color.White;
        public float speedMultiplier = 1f;
        private float maxSpeedMultiplier = 2f; // max 2x speed

        public Ball(Vector2 pos, Vector2 sz)
        {
            position = pos;
            size = sz;
        }

        public void Move()
        {
            position += velocity * speedMultiplier;
        }

        public void ChangeColor()
        {
            color = Random.Color();
        }

        public void IncreaseSpeed()
        {
            if (speedMultiplier < maxSpeedMultiplier)
                speedMultiplier += 0.1f;
        }

        public void Render()
        {
            Draw.FillColor = color;
            Draw.LineColor = Color.Black;
            Draw.Rectangle(position, size);
        }
    }

    public class PaddleAI
    {
        public Paddle paddle;
        public Ball ball;
        public float speed;

        public PaddleAI(Paddle paddle, Ball ball, float speed)
        {
            this.paddle = paddle;
            this.ball = ball;
            this.speed = speed;
        }

        public void Update()
        {
            if (ball.velocity.X > 0)
            {
                float ballCenterY = ball.position.Y + ball.size.Y / 2;
                float paddleCenterY = paddle.position.Y + paddle.size.Y / 2;

                if (ballCenterY < paddleCenterY - 5)
                    paddle.position.Y -= speed;
                else if (ballCenterY > paddleCenterY + 5)
                    paddle.position.Y += speed;
            }

            paddle.position.Y = Math.Clamp(paddle.position.Y, 0, 400 - paddle.size.Y);
        }
    }

    public class Score
    {
        public int leftPoints = 0;
        public int rightPoints = 0;
        public int hits = 0;

        public void AddLeftPoint() => leftPoints++;
        public void AddRightPoint() => rightPoints++;
        public void AddHit() => hits++;
    }
}

