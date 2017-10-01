using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGameToolkit;

namespace FlappyBot
{
    public class PlatformPlacer : BaseObject
    {
        public List<Sprite> Parts { get; } = new List<Sprite>();

        private Camera2D _camera;
        private int _drawOrder;
       
        public PlatformPlacer(int drawOrder)
        {
            _drawOrder = drawOrder;

            for (int i = 0; i < 5; i++)
            {
                Parts.Add(new Sprite(Constants.FlappySpriteSheet, Constants.SpriteRects.Platform));
            }
        }

        protected override void Initialize()
        {
            _camera = Game1.Instance.LoadedScene.ActiveCamera;

            Position = new Vector2(0.0f, Constants.Resolution.Y - Parts[0].Size.Y * 2);

            for (int i = 0; i < Parts.Count; i++)
            {
                Parts[i].Parent = this;
                Parts[i].DrawOrder = _drawOrder;
                Parts[i].Scale = Vector2.One * Constants.Scale;
                Parts[i].Position = Parts[i].Parent.Position + new Vector2(
                    (Parts[i].Size.X / 2) + i * Parts[i].Size.X,
                    Parts[i].Size.Y / 2);
                Parts[i].EnablePhysicsRectangle(FarseerPhysics.Dynamics.BodyType.Static, Parts[i].Bounds);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            foreach(Sprite part in Parts)
            {
                if(part.Position.X < _camera.Position.X - (part.Size.X / 2))
                {
                    Sprite furthest = Parts.OrderByDescending(x => x.Position.X).First();
                    part.PhysicsBody.Position = new Vector2(furthest.Position.X + part.Size.X, part.Position.Y) * Physics.UPP;
                }
            }
        }
    }
}
