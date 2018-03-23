using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CoT
{
    public static class Input
    {
        public static KeyboardState LastKeyboard { get; private set; }
        public static KeyboardState CurrentKeyboard { get; private set; }

        public static MouseState LastMouse { get; private set; }
        public static MouseState CurrentMouse { get; private set; }

        public static Vector2 CurrentMousePosition => CurrentMouse.Position.ToVector2();
        public static Vector2 LastMousePosition => LastMouse.Position.ToVector2();
        public static Vector2 MouseMovement() => CurrentMousePosition - LastMousePosition;

        public static bool IsKeyReleased(Keys key) => LastKeyboard.IsKeyDown(key) && CurrentKeyboard.IsKeyUp(key);
        public static bool IsKeyPressed(Keys key) => LastKeyboard.IsKeyUp(key) && CurrentKeyboard.IsKeyDown(key);

        public static bool IsLeftClickReleased() => LastMouse.LeftButton == ButtonState.Pressed && CurrentMouse.LeftButton == ButtonState.Released;
        public static bool IsLeftClickPressed() => LastMouse.LeftButton == ButtonState.Released && CurrentMouse.LeftButton == ButtonState.Pressed;

        public static bool IsRightClickReleased() => LastMouse.RightButton == ButtonState.Pressed && CurrentMouse.RightButton == ButtonState.Released;
        public static bool IsRightClickPressed() => LastMouse.RightButton == ButtonState.Released && CurrentMouse.RightButton == ButtonState.Pressed;

        public static bool IsScrollMvdUp() => CurrentMouse.ScrollWheelValue > LastMouse.ScrollWheelValue;
        public static bool IsScrollMvdDown() => CurrentMouse.ScrollWheelValue < LastMouse.ScrollWheelValue;

        public static void Update()
        {
            LastKeyboard = CurrentKeyboard;
            LastMouse = CurrentMouse;

            CurrentKeyboard = Keyboard.GetState();
            CurrentMouse = Mouse.GetState();
        }
    }
}
