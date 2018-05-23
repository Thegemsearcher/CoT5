using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class HuD
    {
        private Player player;
        private SpriteFont font = ResourceManager.Get<SpriteFont>("font1");
        private Vector2 infoPos, hitBoxOffset = new Vector2(-50,-50), hpBarOffset = new Vector2(-60,120);
        private bool hoverOverOnGround = false;
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
                    }
                    else
                    {
                        hoverOverOnGround = false;
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
                    e.HpBar.Position = e.Position + hpBarOffset;
                    e.HpBar.UpdateHP(e.Stats.Health, e.Stats.MaxHealth);
                }
            }
        }
        public void Draw(SpriteBatch sb)
        {
            //if (gameObject != null) {
            //    sb.DrawString(font, "" + gameObject.Hitbox.Size.X + "    " + gameObject.Hitbox.Size.Y, position, Color.Yellow, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0.9f);
            //}
            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), mouseHitBox, null, Color.Magenta * 0.3f, 0, Vector2.Zero, SpriteEffects.None, 0.9f);
            sb.DrawString(font, fullInfoBox, infoPosition, Color.Yellow, 0, new Vector2(0,0), 0.8f, SpriteEffects.None, 0.9f);
            if (hoverOverOnGround)
            {
                sb.DrawString(font, "E to Pick up", textAtMouse, Color.Yellow, 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0.8f);
            }
            if (gameObject is Enemy e)
            {
                e.HpBar.Draw(sb);
            }
        }
        public void PickUpInstruction()
        {
            hoverOverOnGround = true;
            textAtMouse.X = Input.CurrentMousePosition.X + 25;
            textAtMouse.Y = Input.CurrentMousePosition.Y;
        }
    }
}
