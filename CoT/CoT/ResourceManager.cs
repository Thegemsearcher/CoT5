using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public static class ResourceManager
    {
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static void LoadContent(ContentManager content)
        {
            RegisterResource(content.Load<Texture2D>("isometricTile1"), "tile1"); // 160x80 textur
            RegisterResource(Helper.CreateCircleTexture(30), "circle");
            RegisterResource(Helper.CreateRectangleTexture(new Point(25, 50)), "rectangle");
        }

        public static void RegisterResource<T>(T t, string name)
        {
            if (t is Texture2D)
            {
                if (!textures.ContainsKey(name)) textures.Add(name, t.ConvertValue<Texture2D>());
            }
        }

        public static T Get<T>(string name)
        {
            if (typeof(T) == typeof(Texture2D))
                return textures[name].ConvertValue<T>();

            return default(T);
        }
    }
}