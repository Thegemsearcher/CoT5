using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class HuD
    {
        private Player player;
        private SpriteFont font = ResourceManager.Get<SpriteFont>("font1");
        private Vector2 infoPos, hitBoxOffset = new Vector2(-50,-50), hpBarOffset = new Vector2(10,70), drawFireballCount = new Vector2(50, 200);
        private bool hoverOverOnGround = false, hoverOverInBag = false;
        private Vector2 infoPosition = new Vector2(0,0), textAtMouse = new Vector2(0,0);
        private GameObject gameObject;//det gameobject som vi vill visa info från
        private string fullInfoBox = "";
        private FloatRectangle mouseHitBox = new FloatRectangle(new Vector2(0,0), new Vector2(55,55));

        public HuD(Player player)
        {
            this.player = player;
        }
        public void Update()
        {
            mouseHitBox.Position = Input.CurrentMousePosition + hitBoxOffset;
            foreach (Item item in ItemManager.Instance.Items)
            {
                if (mouseHitBox.Contains(item.Position.WorldToScreen()) || mouseHitBox.Contains(new Vector2(item.rectItemInv.X,item.rectItemInv.Y)))
                {
                    infoPosition.X = 50;
                    infoPosition.Y = 100;
                    gameObject = item;
                    if (!item.isInBag)
                    {
                        PickUpInstruction();
                    } else
                    {
                        UseInstruction();
                    }
                    if (item is Potion p)
                    {
                        switch (p.currentPotionType)
                        {
                            case Potion.PotionType.FireBall:
                                fullInfoBox = "item type: Fireball Potion" + "\n" + "On Use: you get two fireballs which can be used by pressing 1 and then attacking" + "\n" + "Use by hovering over the item and pressing RIGHT CLICK";
                                break;
                            case Potion.PotionType.HealthLarge:
                                fullInfoBox = "item type: Large Health Potion" + "\n" + "On Use: You gain 80 hp" + "\n" + "Use by hovering over the item and pressing RIGHT CLICK";
                                break;
                            case Potion.PotionType.HealthMedium:
                                fullInfoBox = "item type: Medium Health Potion\nOn Use: Uou gain 40 hp\nUse by hovering over the item and pressing RIGHT CLICK";
                                break;
                            case Potion.PotionType.HealthSmall:
                                fullInfoBox = "item type: Small Health Potion" + "\n" + "On Use: You gain 20 hp" + "\n" + "Use by hovering over the item and pressing RIGHT CLICK";
                                break;
                            case Potion.PotionType.SpeedPotion:
                                fullInfoBox = "item type: Speed Potion" + "\n" + "On Use: Your speed doubles, OBS: DOES NOT STACK" + "\n" + "Use by hovering over the item and pressing RIGHT CLICK";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            foreach (Creature c in CreatureManager.Instance.Creatures)
            {
                if (c.Hitbox.Contains(Input.CurrentMousePosition.ScreenToWorld()) && c is Enemy e)
                {
                    gameObject = e;
                }
            }
            if (gameObject is Enemy enemy)
            {
                enemy.HpBar.Position = enemy.Position.WorldToScreen() + (hpBarOffset * Camera.ScaleInput);
                enemy.HpBar.Scale = Camera.ScaleInput;
                enemy.HpBar.UpdateHP(enemy.Stats.Health, enemy.Stats.MaxHealth);
            }
            if (gameObject is Item i){
                if (!mouseHitBox.Contains(i.Position.WorldToScreen()))
                {
                    hoverOverOnGround = false;
                }
                if (!mouseHitBox.Contains(new Vector2(i.rectItemInv.X, i.rectItemInv.Y)))
                {
                    hoverOverInBag = false;
                }
            }
        }
        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, fullInfoBox, infoPosition, Color.Yellow, 0, new Vector2(0,0), 0.8f, SpriteEffects.None, 0.9f);
            if (hoverOverOnGround)
            {
                sb.DrawString(font, "E to Pick up", textAtMouse, Color.Yellow, 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0.8f);
            }
            if (gameObject is Enemy e)
            {
                e.HpBar.Draw(sb);
            }
            if (GameplayScreen.Instance.Player.CanFireBall > 0)
            {
                sb.DrawString(font, "Press 1 and click to throw a fireball\nYou currently have " + GameplayScreen.Instance.Player.CanFireBall + " Fireballs", drawFireballCount, Color.Yellow, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0.8f);
            }
            if (hoverOverInBag)
            {
                sb.DrawString(font, "RIGHT CLICK to use", textAtMouse, Color.Yellow, 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0.8f);
            }
            sb.DrawString(font, "Level: " + Game1.Level, new Vector2(100, 10), Color.Yellow, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0.8f);
        }
        public void PickUpInstruction()
        {
            hoverOverOnGround = true;
            textAtMouse.X = Input.CurrentMousePosition.X + 20;
            textAtMouse.Y = Input.CurrentMousePosition.Y;
        }
        public void UseInstruction()
        {
            hoverOverInBag = true;
            textAtMouse.X = Input.CurrentMousePosition.X + 20;
            textAtMouse.Y = Input.CurrentMousePosition.Y;
        }
    }
}
