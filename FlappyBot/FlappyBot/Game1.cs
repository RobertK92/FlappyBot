using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameToolkit;
using System.Diagnostics;

namespace FlappyBot
{
    public class Game1 : MGTK
    {
        public override string DefaultTexture => "default-texture";
        public override string DefaultFont => "consolas12";

        protected override void LoadContent()
        {
            base.LoadContent();
#if DEBUG
            DebugPhysicsViewEnabled = true;
            DebugDrawEnabled = true;
            LoggerEnabled = true;
#endif
            IsFixedTimeStep = true;
            Graphics.SynchronizeWithVerticalRetrace = true;
            LoadScene<Scenes.GameScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

#if DEBUG
            /* temp kill process on escape */
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Process.GetCurrentProcess().Kill();
            }
#endif
        }
    }
}
