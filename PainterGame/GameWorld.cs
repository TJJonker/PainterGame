﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PainterGame
{
    internal class GameWorld
    {
        private Cannon cannon;
        private Texture2D background, life, gameover, scoreBar;
        private Ball ball;
        private PaintCan can1, can2, can3;
        private int lives;
        private SpriteFont gameFont;

        public Cannon Cannon { get { return cannon; } }
        public Ball Ball { get { return ball; } }

        public int Score { get; set; }

        public bool IsGameOver
        {
            get { return lives <= 0; }
        }

        public GameWorld(ContentManager Content)
        {
            background = Content.Load<Texture2D>("spr_background");
            life = Content.Load<Texture2D>("spr_lives");
            gameover = Content.Load<Texture2D>("spr_gameover");
            scoreBar = Content.Load<Texture2D>("spr_scorebar");
            gameFont = Content.Load<SpriteFont>("PainterFont");
            cannon = new Cannon(Content);
            ball = new Ball(Content);
            can1 = new PaintCan(Content, 480.0f, Color.Red);
            can2 = new PaintCan(Content, 610.0f, Color.Green);
            can3 = new PaintCan(Content, 740.0f, Color.Blue);
            lives = 5;
            Score = 0;
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (!IsGameOver)
            {
                cannon.HandleInput(inputHelper);
                ball.HandleInput(inputHelper);
            }
            else if (inputHelper.KeyPressed(Keys.Space)) Reset();
        }

        public void Update(GameTime gameTime)
        {
            if (IsGameOver) return;
            ball.Update(gameTime);
            can1.Update(gameTime);
            can2.Update(gameTime);
            can3.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            ball.Draw(gameTime, spriteBatch);
            cannon.Draw(gameTime, spriteBatch);
            can1.Draw(gameTime, spriteBatch);
            can2.Draw(gameTime, spriteBatch);
            can3.Draw(gameTime, spriteBatch);
            for (int i = 0; i < lives; i++)
            {
                spriteBatch.Draw(life, new Vector2(i * life.Width + 15, 60), Color.White);
            }
            spriteBatch.Draw(scoreBar, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(gameFont, "Score: " + Score, new Vector2(20, 18), Color.White);
            if (IsGameOver)
            {
                spriteBatch.Draw(gameover,
                    new Vector2(Painter.ScreenSize.X - gameover.Width, Painter.ScreenSize.Y - gameover.Height) / 2,
                    Color.White);
            }
            spriteBatch.End();
        }

        public bool IsOutsideWorld(Vector2 position)
        {
            return position.X < 0 || position.X > Painter.ScreenSize.X
                || position.Y > Painter.ScreenSize.Y;
        }

        public void LoseLife()
        {
            lives--;
        }

        private void Reset()
        {
            lives = 5;
            Cannon.Reset();
            ball.Reset();
            can1.Reset();
            can1.ResetMinSpeed();
            can2.Reset();
            can2.ResetMinSpeed();
            can3.Reset();
            can3.ResetMinSpeed();
            Score = 0;
        }
    }
}