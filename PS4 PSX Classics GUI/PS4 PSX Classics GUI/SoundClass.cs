using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS4_PSX_Classics_GUI
{
    public class SoundClass
    {
        public static IWavePlayer waveOutDevice = new WaveOut();

        //this device is dedicated to the PS4BGM
        public static IWavePlayer PS4BGMDevice = new WaveOut();

        public enum Sound
        {
            PS4_Info_Pannel_Sound = 0,
            Navigation = 1,
            Options = 2,
            Error = 3,
            Shutdown = 4,
            PS4_Music = 5,
            Notification = 6
        }

        public static string AppCommonPath()
        {
            string returnstring = "";
            if (Properties.Settings.Default.OverwriteTemp == true && Properties.Settings.Default.TempPath != string.Empty)
            {
                returnstring = Properties.Settings.Default.TempPath + @"\Ps4Tools\";
            }
            else
            {
                returnstring = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ps4Tools\";
            }
            return returnstring;
        }

        #region << New Sound Class This Will Get Sounds From The Assembly >>
        private static Stream GetResourceStream(string filename)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string resname = asm.GetName().Name + "." + filename;
            return asm.GetManifestResourceStream(resname);
        }
        private static WaveStream CreateInputStream(byte[] resource)
        {
            WaveChannel32 inputStream;

            MemoryStream ms = new MemoryStream(resource);

            WaveStream mp3Reader = new Mp3FileReader(ms);
            inputStream = new WaveChannel32(mp3Reader);


            return inputStream;
        }

        private static WaveStream CreateInputStreamAT9(byte[] resource)
        {
            WaveChannel32 inputStream;

            MemoryStream ms = new MemoryStream(resource);

            WaveStream mp3Reader = new WaveFileReader(ms);
            //inputStream = new WaveChannel32(mp3Reader);


            return mp3Reader;
        }

        #endregion << New Sound Class This Will Get Sounds From The Assembly >>

        public static void PlayPS4Sound(Sound Soundtoplay)
        {
            try
            {
                switch (Soundtoplay)
                {

                    case Sound.Notification:
                        {
                            new Thread(() =>
                            {
                            //set the thread as a background worker
                            Thread.CurrentThread.IsBackground = true;

                                IWavePlayer waveOutDevice = new WaveOut();
                                WaveStream mp3file = CreateInputStream(Properties.Resources.PS4_Notification);

                                TimeSpan ts = mp3file.TotalTime;

                                waveOutDevice.Init(mp3file);

                                waveOutDevice.Volume = 0.7f;
                                waveOutDevice.Play();


                            /* run your code here */
                                Thread.Sleep(ts);
                                waveOutDevice.Dispose();
                                waveOutDevice.Stop();

                            }).Start();
                        }
                        break;
                    case Sound.Error:
                        {
                            new Thread(() =>
                            {
                            //set the thread as a background worker
                            Thread.CurrentThread.IsBackground = true;

                                IWavePlayer waveOutDevice = new WaveOut();
                                WaveStream mp3file = CreateInputStream(Properties.Resources.Ps4_Error_Sound);

                                TimeSpan ts = mp3file.TotalTime;

                                waveOutDevice.Init(mp3file);

                                waveOutDevice.Volume = 0.5f;
                                waveOutDevice.Play();


                            /* run your code here */
                                Thread.Sleep(ts);
                                waveOutDevice.Dispose();
                                waveOutDevice.Stop();

                            }).Start();
                        }
                        break;
                    case Sound.Shutdown:
                        {
                            WaveStream mp3file = CreateInputStream(Properties.Resources.PS4_Shutdown);
                            TimeSpan ts = mp3file.TotalTime;
                            waveOutDevice.Init(mp3file);

                            waveOutDevice.Volume = 0.5f;
                            waveOutDevice.Play();

                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                            /* run your code here */
                                Thread.Sleep(ts);
                                waveOutDevice.Stop();
                            //waveOutDevice.Dispose();
                        }).Start();
                        }
                        break;
                    case Sound.PS4_Info_Pannel_Sound:
                        {
                            new Thread(() =>
                            {
                            //set the thread as a background worker
                            Thread.CurrentThread.IsBackground = true;

                                IWavePlayer waveOutDevice = new WaveOut();

                                WaveStream mp3file = CreateInputStream(Properties.Resources.PS4_Notification);
                                TimeSpan ts = mp3file.TotalTime;
                                waveOutDevice.Init(mp3file);

                                waveOutDevice.Volume = 0.5f;
                                waveOutDevice.Play();

                                Thread.Sleep(ts);
                                waveOutDevice.Stop();
                            }).Start();
                            break;
                        }
                    case Sound.Options:
                        {

                            WaveStream mp3file = CreateInputStream(Properties.Resources.PS4_Options_Pannel);
                            TimeSpan ts = mp3file.TotalTime;
                            waveOutDevice.Init(mp3file);

                            waveOutDevice.Volume = 0.5f;
                            waveOutDevice.Play();

                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                            /* run your code here */
                                Thread.Sleep(ts);
                                waveOutDevice.Stop();
                            //waveOutDevice.Dispose();
                        }).Start();
                            break;
                        }
                    case Sound.Navigation:
                        {
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                IWavePlayer waveOutDevice = new WaveOut();

                                WaveStream mp3file = CreateInputStream(Properties.Resources.PS4_Navigation_Sound);
                                TimeSpan ts = mp3file.TotalTime;
                                waveOutDevice.Init(mp3file);

                                waveOutDevice.Volume = 0.5f;
                                waveOutDevice.Play();


                            /* run your code here */
                                Thread.Sleep(ts);
                                waveOutDevice.Stop();
                            //waveOutDevice.Dispose();
                        }).Start();


                            break;
                        }
                    case Sound.PS4_Music:
                        {
                            //no longer play ps4 bgm
                            //WaveStream mp3file = CreateInputStream(Properties.Resources.ps4BGM);

                            //////PS4BGMDevice = new AsioOut("ASIO4ALL v2");
                            //////PS4BGMDevice.Init(mp3file);
                            //////PS4BGMDevice.Play();

                            //TimeSpan ts = mp3file.TotalTime;

                            //PS4BGMDevice.Init(mp3file);

                            //PS4BGMDevice.Volume = 0.5f;
                            //PS4BGMDevice.Play();
                        }
                        break;

                    default:
                        break;



                }
            }
            catch(Exception ex)
            {
                //error sound driver or whatever
            }
        }
    }
}
