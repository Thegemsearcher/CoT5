using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Console = System.Console;

namespace CoT
{
    public abstract class GameScreen
    {
        public ScreenManager ScreenManager { get; set; }
        public Grid Grid { get; set; }
        public bool IsPopup { get; set; }

        protected GameScreen(bool isPopup)
        {
            IsPopup = isPopup;
            Console.WriteLine("GameScreen - Constructor");
            ScreenManager = GameManager.Instance.ScreenManager;

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

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void DrawUserInterface(SpriteBatch spriteBatch) { }
    }
}
