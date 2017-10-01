using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameToolkit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlappyBot.Scenes
{
    public class GameScene : Scene
    {
        private Bird _bird;

        public override void Load()
        {
            Log.Message(string.Format("Scale: {0}", Constants.Scale));


            BackgroundPlacer background = new BackgroundPlacer(-3);
            PlatformPlacer platforms = new PlatformPlacer(-1);
            TubePlacer tubes = new TubePlacer(-2);

            _bird = new Bird();
            
        }

        public override void Update(GameTime gameTime)
        {
            ActiveCamera.Position = new Vector2(_bird.Position.X - _bird.OffsetX, ActiveCamera.Position.Y);

            if (!_bird.IsAlive)
            {
                if (Keyboard.GetState().IsKeyPressedOnce(Keys.Enter))
                {
                    Game1.Instance.LoadScene<GameScene>();
                }
            }
        }
    }
}
