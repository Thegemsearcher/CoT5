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
        public OptionsMenuScreen(bool isPopup) : base(isPopup)
        {
        }

        public override void Load()
        {
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

            Grid.Widgets.Add(buttonReturn);
            base.Load();
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
