using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FlappyBot
{
    public class TubePlacer : BaseObject
    {
        public float OffsetX => 300.0f;
        public float GapSize => 70.0f;

        private Camera2D _camera;
        private float _lastTubeX;
        private Random _rand;
        private int _drawOrder;

        private List<Tube> _tubes = new List<Tube>();

        public TubePlacer(int drawOrder) 
            : base()
        {
            _drawOrder = drawOrder;
            _rand = new Random(Guid.NewGuid().GetHashCode());
            _camera = Game1.Instance.LoadedScene.ActiveCamera;
        }

        protected override void Update(GameTime gameTime)
        {
            if(_lastTubeX < _camera.Position.X + _camera.VisibleArea.Width)
            {
                CreateTubePair();
            }

            for (int i = _tubes.Count - 1; i >= 0; i--)
            {
                if(_tubes[i].Position.X < _camera.Position.X - _tubes[i].Size.X / 2)
                {
                    _tubes[i].Destroy();
                    _tubes.RemoveAt(i);
                }
            }
        }

        public void CreateTubePair()
        {
            float gapY = _rand.Next((int)(Constants.Resolution.Y * 0.2f), (int)(Constants.Resolution.Y * 0.7f)); 

            Tube downTube = new Tube(TubeType.Down);
            downTube.DrawOrder = _drawOrder;
            downTube.Position = new Vector2(_camera.Position.X + _camera.VisibleArea.Width + OffsetX, gapY - GapSize - downTube.Size.Y / 2);
            downTube.EnablePhysicsRectangle(FarseerPhysics.Dynamics.BodyType.Static, downTube.Bounds);
            downTube.PhysicsBody.IsSensor = true;

            Tube upTube = new Tube(TubeType.Up);
            upTube.DrawOrder = _drawOrder;
            upTube.Position = new Vector2(_camera.Position.X + _camera.VisibleArea.Width + OffsetX, gapY + GapSize + upTube.Size.Y / 2);
            upTube.EnablePhysicsRectangle(FarseerPhysics.Dynamics.BodyType.Static, upTube.Bounds);
            upTube.PhysicsBody.IsSensor = true;

            _tubes.Add(downTube);
            _tubes.Add(upTube);

            _lastTubeX = downTube.Position.X;
        }
    }
}
