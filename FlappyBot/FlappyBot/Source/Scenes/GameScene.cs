using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameToolkit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Xml.Serialization;

namespace FlappyBot.Scenes
{
    public class GameScene : Scene
    {
        
        private Bird _bird;
        private Vector2 _nextGap;
        private TubePlacer _tubes;
        
        protected override void Load()
        {
            Log.Message(string.Format("Scale: {0}", Constants.Scale));

            new BackgroundPlacer(-3);
            new PlatformPlacer(-1);
            _tubes = new TubePlacer(-2);
            new UICanvas();
            _bird = new Bird();

            AI.Environment.ClearVars();
            AI.Environment.AddVar<float>(Constants.EnvironmentVars.AngleToNextGap, new AI.EnvironmentVar<float>(new AI.EnvironmentVar<float>.Getter(GetAngleToNextGap)));
            AI.Environment.AddVar<float>(Constants.EnvironmentVars.DistanceToNextGap, new AI.EnvironmentVar<float>(new AI.EnvironmentVar<float>.Getter(GetDistanceToNextGap)));

            AI.Learner learner = new AI.Learner()
            {
                GenerationSize = 6
            };
            learner.InputAction = _bird.Flap;
            learner.Start();

            _bird.OnKilled += () =>
            {
                if(AI.Learner.CurrentSample != null)
                {
                    AI.Learner.CurrentSample.MaxDistance *= 0.75f;
                }

                if (AI.Learner.TrainingData.Count > 0)
                {
                    AI.Learner.TrainingData.RemoveAt(AI.Learner.TrainingData.Count - 1);
                }

                learner.Stop();
                Game1.Instance.LoadScene<GameScene>();
            };
        }
        
        private float GetAngleToNextGap()
        {
            return MathHelper.ToDegrees((float)Math.Atan2(_nextGap.Y - _bird.Bounds.Center.Y, _nextGap.X - _bird.Bounds.Center.X));
        }

        private float GetDistanceToNextGap()
        {
            return Vector2.Distance(_bird.Position, _nextGap);
        }

        private void UpdateNextGap()
        {
            float closest = float.PositiveInfinity;
            foreach (KeyValuePair<Tube, Tube> tubePair in _tubes.Tubes)
            {
                Vector2 gap = new Vector2(
                        (tubePair.Key.Bounds.Center.X),
                        (tubePair.Key.Bounds.Center.Y + tubePair.Key.Size.Y / 2) + _tubes.GapSize);
                float d = Vector2.Distance(_bird.Position, gap);

                if (d < closest && _bird.Bounds.Center.X < gap.X)
                {
                    closest = d;
                    _nextGap = gap;    
                }
            }
        }

        public override void DebugDraw(DebugDrawer drawer)
        {
            drawer.DrawCircle(_nextGap, 12.0f, Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateNextGap();
            
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
