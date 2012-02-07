using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace AlienGrab
{
    class SoundPlayer
    {
        private ContentManager content;
        private Dictionary<String, SoundEffect> sounds;
        private Dictionary<String, SoundEffectInstance> soundInstances;
        public SoundPlayer(ContentManager _content)
        {
            content = _content;
            sounds = new Dictionary<string, SoundEffect>();
            soundInstances = new Dictionary<String, SoundEffectInstance>();
        }

        public void AddSound(String name, String path, bool looping)
        {
            SoundEffect s = content.Load<SoundEffect>(path);
            sounds.Add(name, s);
            //if its a looping one then create an instance to loop
            if (looping)
            {
                soundInstances[name] = s.CreateInstance();
                soundInstances[name].IsLooped = looping;
            }
        }

        public void PlaySound(String name)
        {
            if (sounds.ContainsKey(name))
            {
                if (soundInstances.ContainsKey(name))
                {
                    if (soundInstances[name].State == SoundState.Stopped)
                    {
                        soundInstances[name].Play();
                    }
                    else
                    {
                        soundInstances[name].Resume();
                    }
                }
                else
                {
                    sounds[name].Play();
                }
            }
        }

        public void StopSound(String name)
        {
            if (soundInstances.ContainsKey(name))
            {
                if (soundInstances[name].State == SoundState.Playing)
                {
                    soundInstances[name].Stop();
                }
            }
        }

        public void PauseSound(String name)
        {
            if (soundInstances.ContainsKey(name))
            {
                if (soundInstances[name].State == SoundState.Playing)
                {
                    soundInstances[name].Pause();
                }
            }
        }

        public void StopAllSounds()
        {
            foreach (KeyValuePair<String, SoundEffectInstance> sei in soundInstances)
            {
                if (sei.Value.State == SoundState.Playing)
                {
                    sei.Value.Stop();
                }
            }
        }

        public void PauseAllSounds()
        {
            foreach (KeyValuePair<String, SoundEffectInstance> sei in soundInstances)
            {
                if (sei.Value.State == SoundState.Playing)
                {
                    sei.Value.Pause();
                }
            }
        }
    }
}
