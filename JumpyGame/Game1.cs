using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace JumpyGame
{
    // Game States
    public enum GameState
    {
        StartScreen,
        Instructions,
        GameStarted,
        EndScreen,
        Scores
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        public int playerScore = 0;
        int x = 559;
        int[] scores;
        StreamReader sr;
        StreamWriter sw;
        bool hsUpdated = false;
        bool gameStarted = false;
        string s;

        // Objekti
        Jumper jumper;
        List<Platform> platforms = new List<Platform>();
        Background background;
        SpriteFont font;
        Texture2D start;
        Texture2D instructions;
        Texture2D end;
        Texture2D startHover;
        Texture2D scoresHover;
        Texture2D highscores;
        Texture2D backHover;
        Texture2D restartHover;
        Texture2D menuHover;

        Rectangle startButton = new Rectangle(115, 370, 250, 80);
        Rectangle scoreButton = new Rectangle(440, 370, 250, 80);
        Rectangle backButton = new Rectangle(25, 22, 80, 40);
        Rectangle restartButton = new Rectangle(620, 525, 160, 60);
        Rectangle menuButton = new Rectangle(50, 525, 160, 60);

        Random rand = new Random();

        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Velicina
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        }


        protected override void Initialize()
        {
            base.Initialize();
            gameState = new GameState();
            gameState = GameState.StartScreen;
            this.IsMouseVisible = true;
        }

        void platformLoad()
        {
            for (int i = 0; i < 1000; i++)
            {
                int pos = rand.Next(0, 10);
                if (i % 10 == 0) platforms.Add(new BasePlatform(Content.Load<Texture2D>("base"), new Vector2(0, x)));
                else
                {
                    if (pos % 3 == 0)
                        platforms.Add(new YellowPlatform(Content.Load<Texture2D>("platform1"), new Vector2(50 * pos, x)));
                    else if (pos % 3 == 1)
                        platforms.Add(new BluePlatform(Content.Load<Texture2D>("platform2"), new Vector2(50 * pos, x)));
                    else
                        platforms.Add(new GreenPlatform(Content.Load<Texture2D>("platform3"), new Vector2(50 * pos, x)));
                }
                x = x - 150;
            }
        }

        void ScoresFileLoad()
        {
            scores = new int[10];
            sr = new StreamReader("highscores.txt");
            for (int i = 0; i < 10; i++)
            {
                s = sr.ReadLine();
                scores[i] = Convert.ToInt32(s);
            }
            sr.Close();
        }

        protected override void LoadContent()
        {
            // Loadovanje grafike
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            start = Content.Load<Texture2D>("StartScreen");
            instructions = Content.Load<Texture2D>("instruct");
            end = Content.Load<Texture2D>("EndScreen");
            startHover = Content.Load<Texture2D>("startHover");
            scoresHover = Content.Load<Texture2D>("scoresHover");
            highscores = Content.Load<Texture2D>("highscores");
            backHover = Content.Load<Texture2D>("backButton");
            restartHover = Content.Load<Texture2D>("restartButton");
            menuHover = Content.Load<Texture2D>("menuHover");
            background = new Background(Content.Load<Texture2D>("background"), new Vector2(0, 0));
            jumper = new Jumper(Content.Load<Texture2D>("jumper"), Content.Load<Texture2D>("hasJumped"), new Vector2(50, 440));

            platformLoad();
            ScoresFileLoad();
        }



        protected override void UnloadContent()
        {

        }



        protected override void Update(GameTime gameTime)
        {
            // Izlaz
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Start Screen
            if (gameState == GameState.StartScreen)
            {
                Point mouse = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                if (startButton.Contains(mouse) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.Instructions;
                    this.IsMouseVisible = false;
                }
                else
                    if (scoreButton.Contains(mouse) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.Scores;
                    IsMouseVisible = true;
                }
            }

            // Instructions Screen
            if (gameState == GameState.Instructions)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    gameState = GameState.GameStarted;
            }

            // Gameplay
            if (gameState == GameState.GameStarted)
            {
                jumper.Update(gameTime);

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    gameStarted = true;
                }

                if (jumper.CollisionRec.Y > 520)
                {
                    jumper.Active = false;
                    gameState = GameState.EndScreen;
                }

                // Collision Engine
                foreach (Platform platform in platforms)
                {
                    if (jumper.Active == true)
                    {
                        if (jumper.CollisionRec.Y < 75)
                        {
                            platform.Velocity = 4f;
                        }
                        else if (jumper.CollisionRec.Y < 150)
                        {
                            platform.Velocity = 3.2f;
                        }
                        else if (jumper.CollisionRec.Y < 225)
                        {
                            platform.Velocity = 2.6f;
                        }
                        else if (platform.Velocity > 2)
                        {
                            platform.Velocity = 2;
                        }


                        if (gameStarted)
                        {
                            platform.Update(gameTime);
                        }

                        if (jumper.CollisionRec.isOnTop(platform.CollisionRec) && jumper.Velocity.Y >= 0)
                        {
                            if (platform.Code != 3 || platform.Active == true)
                            {
                                jumper.velocity.Y = 0f;
                                jumper.HasJumped = false;
                            }

                            if (platform.Active == true)
                            {
                                playerScore++;
                            }
                            platform.Active = false;
                        }
                    }
                }
            }


            // Scores Screen
            if (gameState == GameState.Scores)
            {
                Point mouse = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                if (backButton.Contains(mouse) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.StartScreen;
                }
            }

            // End Screen
            if (gameState == GameState.EndScreen)
            {
                IsMouseVisible = true;
                if (!hsUpdated)
                {
                    highUpdate();
                }
                Point mouse = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                if (restartButton.Contains(mouse) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {

                    gameState = GameState.GameStarted;
                    restart();
                }
                if (menuButton.Contains(mouse) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.StartScreen;
                    restart();
                }
            }
            base.Update(gameTime);

        }

        void restart()
        {
            platforms.Clear();
            x = 559;
            platformLoad();
            ScoresFileLoad();
            playerScore = 0;
            jumper = new Jumper(Content.Load<Texture2D>("jumper"), Content.Load<Texture2D>("hasJumped"), new Vector2(50, 440));
            gameStarted = false;
        }

        // Highscores funkcija
        void highUpdate()
        {
            int counter = 0;
            while (playerScore < scores[counter] && counter < 9)
            {
                counter++;
            }
            if (counter != 10)
            {
                for (int i = 9; i > counter; i--)
                {
                    scores[i] = scores[i - 1];
                }
                scores[counter] = playerScore;
                File.Delete("highscores.txt");
                sw = new StreamWriter("highscores.txt");
                for (int i = 0; i < 10; i++)
                {

                    sw.WriteLine(scores[i].ToString());
                }
                sw.Close();
            }
            hsUpdated = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // Crtanje start screena
            if (gameState == GameState.StartScreen)
            {
                if (startButton.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
                    spriteBatch.Draw(startHover, new Rectangle(0, 0, 800, 600), Color.White);
                else if (scoreButton.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
                    spriteBatch.Draw(scoresHover, new Rectangle(0, 0, 800, 600), Color.White); 
                else spriteBatch.Draw(start, new Rectangle(0, 0, 800, 600), Color.White);
            }

            // Crtanje scores screena
            if (gameState == GameState.Scores)
            {
                spriteBatch.Draw(highscores, new Rectangle(0, 0, 800, 600), Color.White);
                int a = 0;
                for (int i = 0; i < 10; i++)
                {
                    spriteBatch.DrawString(font, (i + 1).ToString() + ".", new Vector2(380, 192 + a), Color.Black);
                    spriteBatch.DrawString(font, scores[i].ToString(), new Vector2(440, 192 + a), Color.Black);
                    a += 25;
                }

                if (backButton.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
                    spriteBatch.Draw(backHover, new Rectangle(0, 0, backHover.Width, backHover.Height), Color.White);
            } 

            // Crtanje instructions screena
            if (gameState == GameState.Instructions)
            {
                spriteBatch.Draw(instructions, new Rectangle(0, 0, 800, 600), Color.White);
            }

            // Crtanje igrice
            if (gameState == GameState.GameStarted)
            {
                background.Draw(spriteBatch);
                foreach (Platform platform in platforms)
                {
                    if (platform.Code != 3)
                        platform.Draw(spriteBatch);
                    else if (platform.Active == true)
                        platform.Draw(spriteBatch);
                }

                jumper.Draw(spriteBatch);
                spriteBatch.DrawString(font, playerScore.ToString(), new Vector2(10, 10), Color.Black);
            }

            // Crtanje endscreena
            if (gameState == GameState.EndScreen)
            {
                spriteBatch.Draw(end, new Rectangle(0, 0, 800, 600), Color.White);
                spriteBatch.DrawString(font, playerScore.ToString(), new Vector2(380, 192), Color.Black);
                if (restartButton.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
                {
                    spriteBatch.Draw(restartHover, new Rectangle(589, 515, 211, 85), Color.White);
                }
                if (menuButton.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)))
                {
                    spriteBatch.Draw(menuHover, new Rectangle(0, 515, 250, 85), Color.White);
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}



// Rectangle collision detekcija
static class RectangleHelper
{
    const int margin = 5;
    public static bool isOnTop(this Rectangle r1, Rectangle r2)
    {
        return (r1.Bottom >= r2.Top - margin &&
                r1.Bottom <= r2.Bottom &&
                r1.Right >= r2.Left + margin &&
                r1.Left <= r2.Right - margin);
    }

    public static bool isOnLeft(this Rectangle r1, Rectangle r2)
    {
        return (
            r1.Left >= r2.Right - 1 &&
            r1.Left <= r2.Right + 1 &&
            r1.Bottom >= r2.Top + margin &&
            r1.Top <= r2.Bottom - margin
            );
    }
    public static bool isOnRight(this Rectangle r1, Rectangle r2)
    {
        return (
            r1.Right >= r2.Left - 1 &&
            r1.Right <= r2.Left + 1 &&
            r1.Bottom >= r2.Top + margin &&
            r1.Top <= r2.Bottom - margin
            );
    }

}