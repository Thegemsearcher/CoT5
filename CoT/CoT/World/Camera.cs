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
        public static float ScaleSpeed { get; set; } = 3f;

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

        public static Rectangle VisibleArea
        {
            get
            {
                Vector2 topLeft = Vector2.Transform(Vector2.Zero, InvertedTransform);
                Vector2 topRight = Vector2.Transform(new Vector2(Game1.ScreenWidth, 0), InvertedTransform);
                Vector2 bottomLeft = Vector2.Transform(new Vector2(0, Game1.ScreenHeight), InvertedTransform);
                Vector2 bottomRight = Vector2.Transform(new Vector2(Game1.ScreenWidth, Game1.ScreenHeight), InvertedTransform);

                Vector2 min = new Vector2(
                    MathHelper.Min(topLeft.X, MathHelper.Min(topRight.X, MathHelper.Min(bottomLeft.X, bottomRight.X))),
                    MathHelper.Min(topLeft.Y, MathHelper.Min(topRight.Y, MathHelper.Min(bottomLeft.Y, bottomRight.Y))));

                var max = new Vector2(
                    MathHelper.Max(topLeft.X, MathHelper.Max(topRight.X, MathHelper.Max(bottomLeft.X, bottomRight.X))),
                    MathHelper.Max(topLeft.Y, MathHelper.Max(topRight.Y, MathHelper.Max(bottomLeft.Y, bottomRight.Y))));

                return new Rectangle((int)min.X - 300, (int)min.Y - 300, (int)(max.X - min.X) + 300, (int)(max.Y - min.Y) + 300);
            }
        }

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

            GameDebugger.WriteLine("scale: " + Scale);
            GameDebugger.WriteLine($"scaleinput {ScaleInput}");
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
            Scale = MathHelper.Lerp(Scale, ScaleInput, Time.DeltaTime * ScaleSpeed);
        }

        public static void ScreenShake(float duration, float intensity)
        {
            ScreenShakeDuration = duration;
            ScreenShakeIntensity = intensity;
        }

        public static Matrix InvertedTransform => Matrix.Invert(Transform);

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