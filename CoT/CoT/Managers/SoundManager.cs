using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace CoT
{
    public class SoundManager : IManager
    {
        public static SoundManager Instance { get; set; }
        private Song song;

        public SoundManager()
        {
            Instance = this;
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
        }

        public void PlaySong(string name)
        {
            song = ResourceManager.Get<Song>(name);
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }

        public void PlaySound(string name, float volume, float pitch, float pan)
        {
            SoundEffect soundEffect;
            soundEffect = ResourceManager.Get<SoundEffect>(name);
            soundEffect.Play(volume, pitch, pan);
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch sb)
        {
        }

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
