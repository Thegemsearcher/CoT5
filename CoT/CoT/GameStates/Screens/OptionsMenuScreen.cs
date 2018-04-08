using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;

namespace CoT
{
    public class OptionsMenuScreen : GameScreen
    {
        private Button playButton;
        private Grid grid;

        public OptionsMenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RowSpacing = 10,
                ColumnSpacing = 10
            };
            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));

            Button buttonOption1 = new Button
            {
                Text = "Option 1",
                TextColor = Color.Red,
                PaddingLeft = 20,
                PaddingRight = 20,
                PaddingBottom = 10,
                PaddingTop = 10,
                GridPositionX = 0,
                GridPositionY = 0
            };

            Button buttonReturn = new Button
            {
                Text = "Return",
                TextColor = Color.Red,
                PaddingLeft = 20,
                PaddingRight = 20,
                PaddingBottom = 10,
                PaddingTop = 10,
                GridPositionX = 0,
                GridPositionY = 1
            };

            grid.Widgets.Add(buttonOption1);
            grid.Widgets.Add(buttonReturn);
            Game1.Game.host.Widgets.Add(grid);
            base.Activate();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
