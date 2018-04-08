using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;

namespace CoT
{
    public enum ScreenState
    {
        Active,
        Inactive,
        TransitionOn,
        TransitionOff,
    }

    public abstract class GameScreen
    {
        public TimeSpan TransitionOnTime { get; set; }
        public TimeSpan TransitionOffTime { get; set; }

        public bool IsTransitioningOn { get; set; }
        public bool IsTransitioningOff { get; set; }

        public float TransitionAlpha { get; set; }

        public ScreenState ScreenState { get; set; }

        public ScreenManager ScreenManager { get; set; }

        public Grid Grid { get; set; }

        public bool IsExiting { get; set; }

        public virtual void Activate() { }
        public virtual void Deactivate() { }

        protected GameScreen()
        {
            ScreenManager = GameManager.Instance.ScreenManager;
            ScreenState = ScreenState.TransitionOn;
            TransitionOffTime = TimeSpan.FromSeconds(1);
            TransitionOnTime = TimeSpan.FromSeconds(1);

            Grid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RowSpacing = 10,
                ColumnSpacing = 10
            };

            Grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            Grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            Grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            Grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
        }

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                IsExiting = true;
                ScreenState = ScreenState.TransitionOff;
            }
        }

        public virtual void Unload() { }

        public virtual void Update()
        {
            switch (ScreenState)
            {
                case ScreenState.Active:
                    break;

                case ScreenState.Inactive:
                    break;

                case ScreenState.TransitionOn:
                    if (!UpdateTransition(TransitionOnTime))
                    {
                        ScreenState = ScreenState.Active;
                    }
                    break;

                case ScreenState.TransitionOff:
                    if (!UpdateTransition(TransitionOffTime.Negate()))
                    {
                        if (IsExiting)
                        {
                            ScreenManager.RemoveScreen(this);
                        }
                        else
                        {
                            ScreenState = ScreenState.Inactive;
                        }
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool UpdateTransition(TimeSpan time)
        {
            TransitionAlpha += Time.DeltaTime / time.Seconds;

            if (TransitionAlpha < 0 || TransitionAlpha > 1)
            {
                TransitionAlpha = MathHelper.Clamp(TransitionAlpha, 0, 1);
                return false;
            }
            return true;
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void DrawUserInterface(SpriteBatch spriteBatch) { }
    }
}
