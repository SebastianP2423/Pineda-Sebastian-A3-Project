using System;

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

