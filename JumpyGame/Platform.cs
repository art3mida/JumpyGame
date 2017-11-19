using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JumpyGame
{
    // glavna klasa
    public abstract class Platform
    {
        #region Atributi
        public Texture2D platform;
        public Vector2 position;
        public Rectangle collisionRec;
        public bool active = true;
        public int code;
        public float velocity = 2;
        protected float velocityX = -4;
        #endregion

        #region Svojstva
        public Rectangle CollisionRec
        {
            get { return collisionRec; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public int Code
        {
            get { return code; }
        }

        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        #endregion



        #region Metode
        public virtual void Update(GameTime gameTime)
        {
            position.Y += velocity;
            collisionRec.Y = (int)position.Y;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        #endregion

    }

    public class YellowPlatform : Platform
    {
        public YellowPlatform(Texture2D texture, Vector2 position1) : base()
        {
            code = 1;
            position = position1;
            platform = texture;

            collisionRec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(platform, collisionRec, Color.White);
        }

    }

    public class BasePlatform : Platform
    {
        public BasePlatform(Texture2D texture, Vector2 position1) : base()
        {
            code = 2;
            position = position1;
            platform = texture;

            collisionRec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(platform, collisionRec, Color.White);
        }
    }

    public class BluePlatform : Platform
    {
        public BluePlatform(Texture2D texture, Vector2 position1) : base()
        {
            code = 3;
            position = position1;
            platform = texture;

            collisionRec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(SpriteBatch spriteBatch)
        {
            spriteBatch.Dispose();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(platform, collisionRec, Color.White);
        }
    }

    public class GreenPlatform : Platform
    {
        public GreenPlatform(Texture2D texture, Vector2 position1)
            : base()
        {
            code = 4;
            position = position1;
            platform = texture;

            collisionRec = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            position.Y += velocity;
            collisionRec.Y = (int)position.Y;
            position.X += velocityX;
            collisionRec.X = (int)position.X;
            if (position.X < 0 || position.X > 605)
            {
                velocityX *= -1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(platform, collisionRec, Color.White);
        }
    }
}
