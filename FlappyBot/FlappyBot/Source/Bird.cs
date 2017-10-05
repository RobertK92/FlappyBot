using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameToolkit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace FlappyBot
{
    public class Bird : AnimatedSprite
    {
        public float HorizontalSpeed => 3.5f; 
        public float FlapForce => 200.0f;
        public float GravityScale => 2.0f; 
        public float OffsetX => (Constants.Resolution.X / 2) - 75;
        public float RotateIntensity => 4.0f;

        public bool IsAlive { get; private set; }

        public event Action OnTubePast = delegate { };
        public event Action OnKilled = delegate { };

        private MouseState _prevMouseState;

        private TubePlacer _tubePlacer;
        private SoundEffect _sfxFlap;
        private SoundEffect _sfxPling;
        private SoundEffect _sfxDeath;
        private SoundEffect _sfxDeathShort;
        private Sprite _flash;
        private Camera2D _camera;

        private List<Tube> _pastDownTubes;

        public Bird() : base(Constants.FlappySpriteSheet)
        {
            _camera = Game1.Instance.LoadedScene.ActiveCamera;
            _pastDownTubes = new List<Tube>();
            _sfxFlap = Content.Load<SoundEffect>(Constants.SoundEffects.SfxFlap);
            _sfxPling = Content.Load<SoundEffect>(Constants.SoundEffects.SfxPling);
            _sfxDeath = Content.Load<SoundEffect>(Constants.SoundEffects.SfxDeath);
            _sfxDeathShort = Content.Load<SoundEffect>(Constants.SoundEffects.SfxDeathShort);
            _tubePlacer = Game1.Instance.LoadedScene.GetObject<TubePlacer>();

            Scale = Vector2.One * Constants.Scale;
            Animations.Add("Flap", new KeyFrame[]
            {
                new KeyFrame(Constants.SpriteRects.BirdFlapDown),
                new KeyFrame(Constants.SpriteRects.BirdFlapNormal),
                new KeyFrame(Constants.SpriteRects.BirdFlapUp)
            });

            PlayAnimation("Flap");

            float diameter = (Bounds.Width + Bounds.Height) / 2.0f;
            EnablePhysicsCircle(BodyType.Dynamic, diameter / 2);
            PhysicsBody.Position = new Vector2((Constants.Resolution.X / 2), Constants.Resolution.Y / 2) * Physics.UPP;
            PhysicsBody.GravityScale = GravityScale;
            PhysicsBody.LinearVelocity = new Vector2(HorizontalSpeed, 0.0f);
            IsAlive = true;

            Texture2D flashTexture = new Texture2D(
                    Game1.Instance.GraphicsDevice, (int)Constants.Resolution.X, (int)Constants.Resolution.Y, false, SurfaceFormat.Color);
            Color[] colorData = Enumerable.Range(0, (int)Constants.Resolution.X * (int)Constants.Resolution.Y).Select(i => Color.White).ToArray();
            flashTexture.SetData<Color>(colorData);
            _flash = new Sprite(flashTexture);
            _flash.Position = new Vector2(_flash.Size.X / 2, _flash.Size.Y / 2);
            _flash.DrawingSpace = DrawingSpace.Screen;
            _flash.Opacity = 0.0f;

            
        }

        static float bestX = 0.0f;
        protected override void DebugDraw(DebugDrawer drawer)
        {
            if (Position.X > bestX)
                bestX = Position.X;
            drawer.DrawLine(new Vector2(bestX, 0.0f), new Vector2(bestX, Constants.Resolution.Y), Color.Green);
        }

        public void Flap()
        {
            if (IsAlive)
            {
                PhysicsBody.LinearVelocity = new Vector2(PhysicsBody.LinearVelocity.X, 0.0f);
                PhysicsBody.ApplyForce(-Vector2.UnitY * FlapForce * PhysicsBody.GravityScale);
                _sfxFlap.Play();
            }
        }

        public void Kill()
        {
            if (IsAlive)
            {
                IsAlive = false;
                PhysicsBody.LinearVelocity = Vector2.Zero;
                PauseAnimation();
                if(Position.Y > Constants.Resolution.Y - 200.0f)
                {
                    _sfxDeathShort.Play();
                }
                else
                {
                    _sfxDeath.Play();
                }
                _flash.Opacity = 1.0f;
                Log.Message("Bird died");
                OnKilled();
            }
        }

        

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /* Input */
            if (IsAlive)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
                {
                    Flap();
                }
            }

            /* Score check */
            foreach(KeyValuePair<Tube,Tube> kvp in _tubePlacer.Tubes)
            {
                if(Position.X > kvp.Key.Position.X && !_pastDownTubes.Contains(kvp.Key))
                {
                    _sfxPling.Play();
                    _pastDownTubes.Add(kvp.Key);
                    OnTubePast();
                }
            }

            _pastDownTubes.RemoveAll(x => x == null || x.Destroying);
            
            if(Position.Y < 0.0f)
            {
                Kill();
            }

            if(_flash.Opacity > 0.0f)
            {
                _flash.Opacity = MathHelper.Clamp(_flash.Opacity - Time.DeltaTime, 0.0f, 1.0f);
            }
            
            _prevMouseState = Mouse.GetState();
            _camera.Position = new Vector2(Position.X - OffsetX, _camera.Position.Y);
        }
        
        protected override void FixedUpdate()
        {
            
            if (PhysicsBody.LinearVelocity.Y != 0.0f)
            {
                PhysicsBody.Rotation = PhysicsBody.LinearVelocity.Y * RotateIntensity * Physics.UPP;
            }
        }

        protected override void OnContact(ContactInfo contact)
        {
            Kill();
        }

        protected override void OnContactSensor(ContactInfo contact)
        {
#if !CHEATING
            Kill();
#endif
        }
    }
}
