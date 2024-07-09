using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.Services
{
    public class WordSpeakerService
    {
        public WordSpeakerService() 
        {

        }

        public bool HasSoundFile(string word)
        {
            string path = Directory.GetCurrentDirectory() + $"\\Sounds\\{word}.mp3";
            return File.Exists(path);
        }

        public async Task PlaySound(string word)
        {
            await Task.Delay(500);
            string path = Directory.GetCurrentDirectory() + $"\\Sounds\\{word}.mp3";
            using (var audioFile = new AudioFileReader(path))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                // Đợi cho đến khi phát xong
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}
