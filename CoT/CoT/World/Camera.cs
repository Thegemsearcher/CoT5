using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CoT
{
    public static class Camera
    {
        public static float Scale { get; set; } = 1f;
        public static float Rotation { get; set; } = 0f;
        public static float FocusSpeed { get; set; } = 3f;
        public static float ScaleInput { get; set; } = 1f;

        public static Vector2 Position { get; set; } = new Vector2(0, 0);
        public static Vector2? Focus { get; set; } = null;

        private static float ScreenShakeDuration { get; set; }
        private static float ScreenShakeIntensity { get; set; }

        public static Matrix Transform =>
            Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Scale, Scale, 1f) *
            Matrix.CreateTranslation(new Vector3(Game1.Game.GraphicsDevice.Viewport.Width * 0.5f, Game1.Game.GraphicsDevice.Viewport.Height * 0.5f, 0));

        public static Matrix TransformToIsometric =>
            Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(1, 2, 1f) *
            Matrix.CreateTranslation(new Vector3(Game1.Game.GraphicsDevice.Viewport.Width * 0.5f, Game1.Game.GraphicsDevice.Viewport.Height * 0.5f, 0));

        public static void Update()
        {
            if (Input.IsScrollMvdUp)
            {
                ScaleInput = Scale * 1.3f;
            }
            if (Input.IsScrollMvdDown)
            {
                ScaleInput = Scale / 1.3f;
            }

            if (ScreenShakeDuration > 0)
            {
                ScreenShakeDuration -= Time.DeltaTime;
                Position += new Vector2(Game1.Random.NextFloat(-1, 1) * ScreenShakeIntensity, Game1.Random.NextFloat(-1, 1) * ScreenShakeIntensity);
            }

            if (Input.CurrentMouse.RightButton == ButtonState.Pressed && Focus == null)
            {
                Position -= Vector2.TransformNormal(Input.MouseMovement, Matrix.Invert(Transform));
            }

            if (Focus != null) Position = Vector2.Lerp(Position, Focus.Value, FocusSpeed * Time.DeltaTime);
            Scale = MathHelper.Lerp(Scale, ScaleInput, Time.DeltaTime * 5);
        }

        public static void ScreenShake(float duration, float intensity)
        {
            ScreenShakeDuration = duration;
            ScreenShakeIntensity = intensity;
        }

        private static Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(Transform));
        }

        private static Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, Transform);
        }
    }
}