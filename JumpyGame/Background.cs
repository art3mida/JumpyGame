using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpyGame
{
    class Background
    {
        Texture2D background;
        Vector2 position;

        public Background(Texture2D texture, Vector2 position1)
        {
            position = position1;
            background = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position, Color.White);
        }
    }
}
