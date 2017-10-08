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
using MachineLearning;

namespace FlappyBot.Scenes
{
    public class GameScene : Scene
    {
        static bool brainInitialized = false;

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
            
            if (!brainInitialized)
            {
                MDomain domain = new MDomain();
                domain.AddVariable(new MDomainVar(new MDomainVar.MValuesDelegate(GetAngleToNextGap)));
                domain.AddVariable(new MDomainVar(new MDomainVar.MValuesDelegate(GetDistanceToNextGap)));
                MBrain.Instance.RegisterDomain(domain);

                MLearner learner = new MLearner();
                learner.AddInputAction(new MInputAction("Flap", new MInputAction.MInputActionDelegate(_bird.Flap))); // BUG: bird is re-created each run
                MBrain.Instance.RegisterLearner(learner);

                MBrain.Instance.StartPulsing();
                brainInitialized = true;
                Log.Message("[MBrain] initialized");
                Log.Message("[MBrain] domain count: " + MBrain.Instance.DomainCount);
                Log.Message("[MBrain] learner count: " + MBrain.Instance.LearnerCount);
                Log.Message("[MBrain] started pulsing...");
            }

            
        }
        
        private IEnumerable<float> GetAngleToNextGap()
        {
            return new float[] { MathHelper.ToDegrees((float)Math.Atan2(_nextGap.Y - _bird.Bounds.Center.Y, _nextGap.X - _bird.Bounds.Center.X)) };
        }

        private IEnumerable<float> GetDistanceToNextGap()
        {
            return new float[] { Vector2.Distance(_bird.Position, _nextGap) };
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
