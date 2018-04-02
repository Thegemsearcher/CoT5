using System;
using System.Collections.Generic;
using System.Linq;
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
        public static float FocusSpeed { get; set; } = 5f;
        public static float ScaleInput { get; set; } = 1f;

        public static Vector2 Position { get; set; } = new Vector2(0, 0);
        public static Vector2? Focus { get; set; } = null;

        public static Matrix Transform =>
            Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Scale) *
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

            if (Input.CurrentMouse.RightButton == ButtonState.Pressed)
            {
                Position -= Vector2.TransformNormal(Input.MouseMovement, Matrix.Invert(Transform));
            }

            if (Focus != null) Position = Vector2.Lerp(Position, Focus.Value, FocusSpeed * Time.DeltaTime);
            Scale = MathHelper.Lerp(Scale, ScaleInput, Time.DeltaTime * 5);
        }

        public static Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(Transform));
        }

        public static Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, Transform);
        }
    }
}