using Microsoft.Xna.Framework;
using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot
{
    public class Tube : Sprite
    {
        public readonly TubeType TubeType;

        public Tube(TubeType tubeType) 
            : base(Constants.FlappySpriteSheet)
        {
            Scale = Vector2.One * Constants.Scale;
            this.TubeType = tubeType;
            switch (TubeType)
            {
                case TubeType.Up:
                    SourceRect = Constants.SpriteRects.TubeUp;
                    break;
                case TubeType.Down:
                    SourceRect = Constants.SpriteRects.TubeDown;
                    break;
                default:
                    break;
            }
        }
    }
}
