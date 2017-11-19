using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpyGame
{
    class Jumper
    {
        #region Atributi
        Texture2D sprite;
        Rectangle collisionRec;
        Texture2D sprite1;

        public Vector2 position;
        public Vector2 velocity;

        bool hasJumped;
        bool active = true;
        #endregion

        #region Konstruktori
        public Jumper(Texture2D newSprite, Texture2D spriteJump, Vector2 position1)
        {

            sprite = newSprite;
            sprite1 = spriteJump;
            position = position1;
            hasJumped = true;
            collisionRec = new Rectangle((int)position1.X + 10, (int)position1.Y + 10, newSprite.Width - 20, newSprite.Height - 20);
        }
        #endregion

        #region Svojstva
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Rectangle CollisionRec
        {
            get { return collisionRec; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public bool HasJumped
        {
            get { return hasJumped; }
            set { hasJumped = value; }
        }
        #endregion

        #region Metode
        public void Update(GameTime gameTime)
        {
            position += velocity;
            collisionRec = new Rectangle((int)position.X + 10, (int)position.Y + 10, sprite.Width - 20, sprite.Height - 20);

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) velocity.X = 4f;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left)) velocity.X = -4f;
            else velocity.X = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && hasJumped == false)
            {
                position.Y -= 15f;
                velocity.Y = -6f;
                hasJumped = true;
            }

            // Gravitacija

            float i = 1;
            velocity.Y += 0.22f * i;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (hasJumped == true)
                spriteBatch.Draw(sprite1, position, Color.White);
            else
                spriteBatch.Draw(sprite, position, Color.White);
        }
        #endregion
    }
}
