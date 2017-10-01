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

namespace FlappyBot
{
    public class Bird : AnimatedSprite
    {
        public float HorizontalSpeed => 3.0f;
        public float FlapForce => 200.0f;
        public float GravityScale => 2.0f; 
        public float OffsetX => (Constants.Resolution.X / 2) - 75;
        public float RotateIntensity => 4.0f;

        public bool IsAlive { get; private set; }

        private MouseState _prevMouseState;

        private SoundEffect _sfxFlap;
        private SoundEffect _sfxPling;
        private SoundEffect _sfxDeath;
        private SoundEffect _sfxDeathShort;

        public Bird() : base(Constants.FlappySpriteSheet)
        {
            _sfxFlap = Content.Load<SoundEffect>(Constants.SoundEffects.SfxFlap);
            _sfxPling = Content.Load<SoundEffect>(Constants.SoundEffects.SfxPling);
            _sfxDeath = Content.Load<SoundEffect>(Constants.SoundEffects.SfxDeath);
            _sfxDeathShort = Content.Load<SoundEffect>(Constants.SoundEffects.SfxDeathShort);

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
        }
        
        public void Flap()
        {
            PhysicsBody.LinearVelocity = new Vector2(PhysicsBody.LinearVelocity.X, 0.0f);
            PhysicsBody.ApplyForce(-Vector2.UnitY * FlapForce * PhysicsBody.GravityScale);
            _sfxFlap.Play();
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
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsAlive)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
                {
                    Flap();
                }
            }
            
            _prevMouseState = Mouse.GetState();
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
            Kill();
        }
    }
}
