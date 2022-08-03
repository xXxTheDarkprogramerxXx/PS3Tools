using NAudio.Wave;
using SharpMod;
using SharpMod.DSP;
using SharpMod.Song;
using SharpMod.SoundRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS4_PSX_Classics_GUI.Util
{

    public class SoundClass : IDisposable
    {
        #region << Vars >>

        #region << Attrc 3 + 9 >>

        public static Vlc.DotNet.Forms.VlcControl atr3vlc;

        #endregion << Attrac 3 + 9 >>

        /// <summary>
        /// Sound to Play from enum SoundType
        /// </summary>
        public static SoundType SoundToPlay { get; set; }


        /// <summary>
        /// Is music currently playing
        /// </summary>
        public static bool IsMusicPlaying = false; 

        public static SongModule myMod { get; set; }

        /// <summary>
        /// Module Player
        /// </summary>
        public static ModulePlayer player { get; set; }


        /// <summary>
        /// Wave Out Put (Music Class)
        /// </summary>
        public static WaveOut waveOut { get; set; }


        /// <summary>
        /// Memory Stream for sound class
        /// </summary>
        public static Stream ms = new MemoryStream();


        #endregion << Vars >>

        #region << Enums >>
        /// <summary>
        /// Sound Types
        /// MOD (1) = anything like xm(old school 8bit sounds)
        /// MP3 (2) = MP3 Type
        /// Attrac3 (3) = Sony (?) Related Sounds
        /// Attrac9 (9) = PS4 (?) Related Sounds
        /// </summary>
        public enum SoundType
        {
            MOD = 1,
            MP3 = 2,
            At3 = 3,
            Attrac9 = 9
        }

        #endregion << Enums >>

        #region << Methods >>

        #region << MusicPlayer >>

        /// <summary>
        /// Play a specific song from a byte array 
        /// </summary>
        /// <param name="songbytes">Song in byte array</param>
        /// <param name="soundtype">Enum Sound Type</param>
        public static void PlayModSong(byte[] songbytes,SoundType soundtype = SoundType.MP3)
        {
            try
            {
                if (player != null)
                {
                    if (!player._isPlaying)
                    {
                        IsMusicPlaying =  true;
                        if (SoundToPlay == SoundType.MP3)
                        {
                            byte[] bytes = songbytes;
                            myMod = ModuleLoader.Instance.LoadModule(new MemoryStream(bytes));
                        }
                        player.Start();
                    }
                    else
                    {
                        IsMusicPlaying = false;
                        player.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Play a specifies song from a memory stream 
        /// </summary>
        /// <param name="songbytes">Song as memory Stream</param>
        /// <param name="soundtype">SongType enum</param>
        public static void PlayModSong(Stream songbytes, SoundType soundtype = SoundType.MP3)
        {
            try
            {
                if (player != null)
                {
                    if (!player._isPlaying)
                    {
                        IsMusicPlaying = true;
                        if (SoundToPlay == SoundType.MP3)
                        {
                            myMod = ModuleLoader.Instance.LoadModule(songbytes);
                        }
                        player.Start();
                    }
                    else
                    {
                        IsMusicPlaying = false;
                        player.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion << MusicPlayer >>

        /// <summary>
        /// Use this for mp3 files
        /// </summary>
        public static void Init_SoundPlayer(byte[] songbytes)
        {
            new Thread(delegate (object o)
            {
                byte[] bytes = songbytes;


                using (var stream = new MemoryStream(bytes))
                {
                    byte[] buffer = new byte[65536]; // 64KB chunks
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        var pos = ms.Position;
                        ms.Position = ms.Length;
                        ms.Write(buffer, 0, read);
                        ms.Position = pos;
                    }
                }


                // Pre-buffering some data to allow NAudio to start playing
                while (ms.Length < 65536 * 10)
                    Thread.Sleep(1000);

                ms.Position = 0;
                using (WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms))))
                {
                    WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());


                    waveOut.Init(blockAlignedStream);
                    waveOut.Play();
                    waveOut.PlaybackStopped += delegate
                    {
                        //if the user didnt stop it play again 
                        if (IsMusicPlaying == true)
                        {
                            Init_SoundPlayer(songbytes);
                        }
                    };
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }


                }
            }).Start();
        }

        /// <summary>
        /// Use This For XM Files
        /// </summary>
        public static void Init_XM()
        {
            byte[] bytes = null;//Properties.Resources.BRD___Teleport_Prokg;


            myMod = ModuleLoader.Instance.LoadModule(new MemoryStream(bytes));
            player = new ModulePlayer(myMod);
            // Or NAudio Driver
            player.DspAudioProcessor = new AudioProcessor(1024, 50);
            player.MixCfg.Rate = 48000;
            player.MixCfg.Style = SharpMod.Player.RenderingStyle.Stereo;
            player.DspAudioProcessor.OnCurrentSampleChanged += new AudioProcessor.CurrentSampleChangedHandler(DspAudioProcessor_OnCurrentSampleChanged);
            player.OnGetPlayerInfos += Player_OnGetPlayerInfos; ;
            NAudioWaveChannelDriver drv = new NAudioWaveChannelDriver(NAudioWaveChannelDriver.Output.WaveOut);
            player.RegisterRenderer(drv);
        }

        #endregion << Methods >>

        #region << Events >>
        private static void Player_OnGetPlayerInfos(object sender, SharpModEventArgs sme)
        {
            GetPlayerInfosHandler method = new GetPlayerInfosHandler(updateUIP);
            try
            {

            }
            catch
            {
            }
        }

        #region << NAudio >>

        void player_OnGetPlayerInfos(object sender, SharpMod.SharpModEventArgs e)
        {

            GetPlayerInfosHandler method = new GetPlayerInfosHandler(updateUIP);
            try
            {

            }
            catch
            {
            }


        }

        private static void updateUIP(object sender, SharpMod.SharpModEventArgs sme)
        {
            string lol = "";
            lol = String.Format("{0:000}/{1:000}", sme.PatternPosition, player.CurrentModule.Patterns[sme.SongPosition].RowsCount);

            lol = String.Format("{0:000}", sme.SongPosition);

        }

        private static void DspAudioProcessor_OnCurrentSampleChanged(int[] leftSample, int[] rightSample)
        {
            int[] vuMeterLeft;
            int[] vuMeterRight;
            vuMeterLeft = (leftSample);
            vuMeterRight = (rightSample);
        }

        public static void DisposeScreen()
        {

        }

        public void Dispose()
        {
            this.Dispose();
        }

        #endregion << >>

        #endregion << Events >>
    }
}
