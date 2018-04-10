using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CoT
{
    public static class ResourceManager
    {
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, SpriteFont> spriteFonts = new Dictionary<string, SpriteFont>();
        private static Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
        private static Dictionary<string, Song> songs = new Dictionary<string, Song>();

        public static void RegisterResource<T>(T t, string name)
        {
            if (t is Effect)
            {
                if (!effects.ContainsKey(name)) effects.Add(name, t.ConvertValue<Effect>());
            }
            else if (t is Texture2D)
            {
                if (!textures.ContainsKey(name)) textures.Add(name, t.ConvertValue<Texture2D>());
            }
            else if (t is SpriteFont)
            {
                if (!spriteFonts.ContainsKey(name)) spriteFonts.Add(name, t.ConvertValue<SpriteFont>());
            }
            else if (t is Song)
            {
                if (!songs.ContainsKey(name)) songs.Add(name, t.ConvertValue<Song>());
            }
            else if (t is SoundEffect)
            {
                if (!soundEffects.ContainsKey(name)) soundEffects.Add(name, t.ConvertValue<SoundEffect>());
            }
        }

        public static T Get<T>(string name)
        {
            if (typeof(T) == typeof(Effect))
                return effects[name].ConvertValue<T>();
            if (typeof(T) == typeof(Texture2D))
                return textures[name].ConvertValue<T>();
            if (typeof(T) == typeof(SpriteFont))
                return spriteFonts[name].ConvertValue<T>();
            if (typeof(T) == typeof(Song))
                return songs[name].ConvertValue<T>();
            if (typeof(T) == typeof(SoundEffect))
                return soundEffects[name].ConvertValue<T>();
            return default(T);
        }
    }
}