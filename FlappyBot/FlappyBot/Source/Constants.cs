using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBot
{
    public static class Constants
    {
        public static Vector2 Resolution => new Vector2(1280.0f, 720.0f);
        public static float Scale => Resolution.Y / SpriteRects.Background.Height;
        public static string FlappySpriteSheet => "flappy-sprites";

        public static class EnvironmentVars
        {
            public static string AngleToNextGap => "AngleToNextGap";
            public static string DistanceToNextGap => "DistanceToNextGap";
        }

        public static class Fonts
        {
            public static string FlappyFont => "bird-font";
        }

        public static class Effects
        {
            public static string FlappyFontEffect => "bird-font-effect";
        }

        public static class SpriteRects
        {    
            public static Rectangle Background => new Rectangle(0, 0, 143, 255);
            public static Rectangle Platform => new Rectangle(146, 0, 154, 56);
            public static Rectangle BirdFlapDown => new Rectangle(222, 123, 18, 13);
            public static Rectangle BirdFlapNormal => new Rectangle(263, 89, 18, 13);
            public static Rectangle BirdFlapUp => new Rectangle(263, 63, 18, 13);
            public static Rectangle TapInstruction => new Rectangle(170, 122, 40, 50);
            public static Rectangle TubeDown => new Rectangle(301, 0, 28, 256);
            public static Rectangle TubeUp => new Rectangle(329, 0, 28, 256);
        }

        public static class SoundEffects
        {
            public static string SfxFlap => "sfx-flap";
            public static string SfxPling => "sfx-pling";
            public static string SfxDeath => "sfx-death";
            public static string SfxDeathShort => "sfx-death-short";
        }
    }
}
