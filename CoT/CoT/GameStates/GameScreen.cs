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

        public bool FadeInTransitionOn { get; set; }
        public bool FadeOutTransitionOn { get; set; }

        public bool FadeInTransitionOff { get; set; }
        public bool FadeOutTransitionOff { get; set; }

        protected GameScreen()
        {
            Console.WriteLine("GameScreen - Constructor");
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

        public virtual void Load()
        {
            try
            {
                Game1.Game?.ResetElapsedTime();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Game1.Game.host.Widgets.Add(Grid);
        }

        public virtual void Unload()
        {
            Game1.Game.host.Widgets.Remove(Grid); 
        }

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
            TransitionAlpha += Time.DeltaTime / (float)time.TotalSeconds;

            if (TransitionAlpha < 0 || TransitionAlpha > 1)
            {
                TransitionAlpha = MathHelper.Clamp(TransitionAlpha, 0, 1);
                return false;
            }
            return true;
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void DrawUserInterface(SpriteBatch spriteBatch)
        {
            if (FadeOutTransitionOn && ScreenState == ScreenState.TransitionOn ||
                FadeInTransitionOff && ScreenState == ScreenState.TransitionOff)
            {
                ScreenManager.DrawBlackRectangle(spriteBatch, TransitionAlpha * -1 + 1);
            }
            else if (FadeInTransitionOn && ScreenState == ScreenState.TransitionOn ||
                     FadeOutTransitionOff && ScreenState == ScreenState.TransitionOff)
            {
                ScreenManager.DrawBlackRectangle(spriteBatch, TransitionAlpha);
            }
        }
    }
}
