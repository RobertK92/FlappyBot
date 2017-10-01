using Microsoft.Xna.Framework;
using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot
{
    public class BackgroundPlacer : BaseObject
    {
        public BackgroundPlacer(int drawOrder)
            : base()
        {
            for (int i = 0; i < 4; i++)
            {
                Sprite bgPart = new Sprite(Constants.FlappySpriteSheet, Constants.SpriteRects.Background)
                {
                    Scale = Vector2.One * Constants.Scale,
                    DrawOrder = drawOrder,
                    DrawingSpace = DrawingSpace.Screen
                };

                bgPart.Position = Position + new Vector2(
                    (bgPart.Size.X / 2) + i * bgPart.Size.X,
                    bgPart.Size.Y / 2 - 1);
            }
        }
    }
}
