using MonoGameToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyBot
{
    public class UICanvas : BaseObject
    {
        private Bird _bird;
        private TextField _scoreField;
        private int _score;

        public UICanvas()
            : base()
        {
            _scoreField = new TextField(_score.ToString(), Constants.Fonts.FlappyFont);
            _scoreField.DrawingSpace = DrawingSpace.Screen;
            _scoreField.Position = new Vector2(Constants.Resolution.X / 2, Constants.Resolution.Y / 6.0f);
            _scoreField.Effect = Content.Load<Effect>(Constants.Effects.FlappyFontEffect);
        }

        protected override void Initialize()
        {
            _bird = Game1.Instance.LoadedScene.GetObject<Bird>();
            _bird.OnTubePast += () =>
            {
                _score++;
                _scoreField.Text = _score.ToString();
            };
        }
    }
}
