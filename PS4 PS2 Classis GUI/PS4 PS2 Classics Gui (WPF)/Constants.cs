using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PS4_PS2_Classics_Gui__WPF_.Constants.Config_Emu_PS4;

namespace PS4_PS2_Classics_Gui__WPF_
{
    /// <summary>
    /// All Information comes from the wiki 
    /// https://www.psdevwiki.com/ps4/PS2_Emulation
    /// </summary>
    public class Constants
    {
        //moving over to a constants(settings class)
        public static bool ConfigEditorUsed = false;

        public class Config_Emu_PS4
        {
            #region << PS4 Config Emu >>

            #region << Enums >>

            /// <summary>
            /// Values none, 2x2
            /// </summary>
            public enum Gs_Uprender
            {
                None = 0,
                _2x2 = 1,
                Disabled = 99,
            }

            /// <summary>
            /// Values none, gpu, edgesmooth, motionvec
            /// </summary>
            public enum Gs_Upscale
            {
                None = 0,
                GPU = 1,
                EdgeSmooth = 2,
                Motionvec = 3,
                Disabled = 99,
            }

            /// <summary>
            ///  normal,fast,fastest,slow,slower,slowest
            /// </summary>
            public enum Framelimit_Mode
            {
                Normal =0,
                Fast =1,
                Fastest = 2,
                Slow = 3,
                Slower = 4,
                Slowest = 5,
                Disabled = 99,
            }

            /// <summary>
            /// all,none,main,bgm
            /// --mute-streaming-audio=all
            /// </summary>
            public enum Mute_Streaming_Audio
            {
                All = 0,
                None = 1,
                Main = 2,
                BGM = 3,
                Disabled = 99,
            }

            /// <summary>
            /// all,none,main,bgm
            /// --mute-audio=all
            /// </summary>
            public enum Mute_Audio
            {
                All = 0,
                None = 1,
                Main = 2,
                BGM = 3,
                Disabled = 99,
            }
            #endregion << Enums >>

            /// <summary>
            /// Overide this value if the user wants to use anything in here
            /// </summary>
            public Gs_Uprender gs_uprender = Gs_Uprender.Disabled;

            /// <summary>
            /// Overide this value if the user wants to use anything in here
            /// </summary>
            public Gs_Upscale gs_upscale = Gs_Upscale.Disabled;

            /// <summary>
            /// No Information Provided on Wiki
            /// </summary>
            public string config_local_lua = "";

            /// <summary>
            /// No Information Provided on Wiki
            /// </summary>
            public string load_tooling_lua = "";

            /// <summary>
            /// Record
            /// </summary>
            public class Record
            {

                /// <summary>
                /// No Information Provided on Wiki
                /// </summary>
                public string audio = "";
                /// <summary>
                /// No Information Provided on Wiki
                /// </summary>
                public string audio_img = "";

                /// <summary>
                /// No Information Provided on Wiki
                /// </summary>
                public string audio_image = "";

                /// <summary>
                /// No Information Provided on Wiki
                /// </summary>
                public string audio_ext = "";
            }

            /// <summary>
            /// No Information Provided on Wiki
            /// </summary>
            public string max_console_spam = "";

            public class Path
            {

                /// <summary>
                /// dir/folder
                /// </summary>
                public string snaps = "/tmp/snapshots";//Default value

                /// <summary>
                /// dir/folder
                /// </summary>
                public string recordings = "/tmp/recordings";//Default value

                /// <summary>
                /// dir/folder
                /// </summary>
                public string audio_images = "";//no value on wiki

                /// <summary>
                /// dir/folder
                /// </summary>
                public string memcards = "";//no value on wiki

                /// <summary>
                /// dir/folder
                /// </summary>
                public string vmc = "/tmp/vmc";//Default value

                /// <summary>
                /// dir/folder
                /// </summary>
                public string emulog = "/tmp/recordings";//Default value

                /// <summary>
                /// dir/folder
                /// </summary>
                public string manual = "";//no value on wiki

                /// <summary>
                /// Path to patches folder
                /// dir/folder
                /// </summary>
                public string patches = "/app0/patches";//Default value

                /// <summary>
                /// dir/folder
                /// </summary>
                public string trophydata = "/app0/trophy_data";

                /// <summary>
                /// Path to folder with XXXX-YYYYY_features.lua file
                /// dir/folder
                /// </summary>
                public string featuredata = "/app0/feature_data";

                /// <summary>
                /// Post-processing (shaders?)
                /// dir/folder
                /// </summary>
                public string postproc = "";

                /// <summary>
                /// dir/folder
                /// </summary>
                public string toolingscript = "";

            }

            /// <summary>
            /// SnapShot
            /// </summary>
            public class SnapShot
            {
                public string Name = "";
                public string DataFile = "";
                public string Resotore = "";
                public string Save = "";
                public string Mcd_Files = "";
                public string Repeat = "";
                public string Modulo = "";
            }

            /// <summary>
            /// Dual Shock 4
            /// </summary>
            public class DS4
            {
                public string deadzone_adjust = "";
                public string diagonal_adjust = "";
            }

            /// <summary>
            /// Host
            /// </summary>
            public class Host
            {
                /// <summary>
                /// --host-pad-loses-focus=1
                /// </summary>
                public string pad_loses_focus = "";
                /// <summary>
                /// --host-keyboard=4
                /// </summary>
                public string gamepads = "";
                /// <summary>
                /// slot [0-7]
                /// </summary>
                public string keyboard = "";
                /// <summary>
                /// 1,0,on,off,mono
                /// --host-audio=1
                /// </summary>
                public string audio = "";
                /// <summary>
                /// msec/float
                /// --host-audio-latency=1.5
                /// </summary>
                public string audio_latency = "";
                /// <summary>
                /// msec/float
                /// --host-window-scale=0.5
                /// </summary>
                public string window_scale = "";
                /// <summary>
                /// x,y
                /// </summary>
                public string window_pos = "";
                /// <summary>
                /// normal,full,4:3,16:9
                /// --host-display-mode=full
                /// </summary>
                public string display_mode = "";
                /// <summary>
                /// 0,off,minimal,verbose
                /// --host-osd=0
                /// </summary>
                public string osd = "";
                /// <summary>
                /// 
                /// </summary>
                public string vsync = "";
                /// <summary>
                /// 
                /// </summary>
                public string trophy_support = "";
            }

            /// <summary>
            /// unix_time (seconds since epoch)
            /// --rtc-epoch=1523776362
            /// </summary>
            public string rtc_epoch = "";

            public string framelimiter = "";

            /// <summary>
            /// FPS/float
            /// --framelimit-fps=0.8
            /// </summary>
            public string framelimit_fps = "";

            /// <summary>
            /// scalar/float
            /// --framelimit-scalar=3.2
            /// </summary>
            public string framelimit_scalar = "";

            /// <summary>
            /// normal,fast,fastest,slow,slower,slowest
            /// --framelimit-mode=fast
            /// </summary>
            public Framelimit_Mode framelimit_mode = Framelimit_Mode.Disabled;

            /// <summary>
            /// 
            /// </summary>
            public string audio_stretching = "";

            /// <summary>
            /// --ps2-lang=system
            /// </summary>
            public string ps2_lang = "system";

            /// <summary>
            /// 
            /// </summary>
            public string pad_record = "";

            /// <summary>
            /// 1-5
            /// numbers of discs in package (maximum=5)
            /// --max-disc-num=1
            /// </summary>
            public string max_disc_num = "1";

            /// <summary>
            /// sets title-id for patches	
            /// --ps2-title-id=SLES-50366
            /// </summary>
            public string ps2_title_id = "SLES-50366";

            /// <summary>
            /// 1-5
            /// sets boot disc for multi-disc pkg
            /// --boot-disc-id=0
            /// </summary>
            public string boot_disc_id = "1";

            /// <summary>
            /// all,none,main,bgm
            /// --mute-audio=all
            /// </summary>
            public Mute_Audio mute_audio = Mute_Audio.Disabled;

            /// <summary>
            /// all,none,main,bgm
            /// --mute-streaming-audio=all
            /// </summary>
            public Mute_Streaming_Audio mute_streaming_audio = Mute_Streaming_Audio.Disabled;

            #endregion << PS4 Config Emu >>

            /// <summary>
            /// Loads PS2 Config Settings from File
            /// </summary>
            public void LoadFromFile()
            {

            }

            /// <summary>
            /// Resets PS2 Config File to Defaults
            /// </summary>
            public void Reset()
            {
                Constants.ConfigEditorUsed = false;
                gs_uprender = Gs_Uprender.Disabled;
                gs_upscale = Gs_Upscale.Disabled;
                config_local_lua = "";
                load_tooling_lua = "";            
                max_console_spam = "";              
            }
        }

        public class ConfigEmuModel
        {
            public Gs_Uprender gs_uprender = Gs_Uprender.Disabled;

            /// <summary>
            /// Overide this value if the user wants to use anything in here
            /// </summary>
            public Gs_Upscale gs_upscale = Gs_Upscale.Disabled;

            /// <summary>
            /// No Information Provided on Wiki
            /// </summary>
            public string config_local_lua = "";

            /// <summary>
            /// No Information Provided on Wiki
            /// </summary>
            public string load_tooling_lua = "";

            public Record recod = new Record();

            public Path path = new Path();

            public SnapShot snap = new SnapShot();


            public DS4 DS4 = new DS4();

            public Host Host = new Host();
            /// <summary>
            /// No Information Provided on Wiki
            /// </summary>
            public string max_console_spam = "";

            /// <summary>
            /// unix_time (seconds since epoch)
            /// --rtc-epoch=1523776362
            /// </summary>
            public string rtc_epoch = "";

            public string framelimiter = "";

            /// <summary>
            /// FPS/float
            /// --framelimit-fps=0.8
            /// </summary>
            public string framelimit_fps = "";

            /// <summary>
            /// scalar/float
            /// --framelimit-scalar=3.2
            /// </summary>
            public string framelimit_scalar = "";

            /// <summary>
            /// normal,fast,fastest,slow,slower,slowest
            /// --framelimit-mode=fast
            /// </summary>
            public Framelimit_Mode framelimit_mode = Framelimit_Mode.Disabled;

            /// <summary>
            /// 
            /// </summary>
            public string audio_stretching = "";

            /// <summary>
            /// --ps2-lang=system
            /// </summary>
            public string ps2_lang = "system";

            /// <summary>
            /// 
            /// </summary>
            public string pad_record = "";

            /// <summary>
            /// 1-5
            /// numbers of discs in package (maximum=5)
            /// --max-disc-num=1
            /// </summary>
            public string max_disc_num = "1";

            /// <summary>
            /// sets title-id for patches	
            /// --ps2-title-id=SLES-50366
            /// </summary>
            public string ps2_title_id = "SLES-50366";

            /// <summary>
            /// 1-5
            /// sets boot disc for multi-disc pkg
            /// --boot-disc-id=0
            /// </summary>
            public string boot_disc_id = "1";

            /// <summary>
            /// all,none,main,bgm
            /// --mute-audio=all
            /// </summary>
            public Mute_Audio mute_audio = Mute_Audio.Disabled;

            /// <summary>
            /// all,none,main,bgm
            /// --mute-streaming-audio=all
            /// </summary>
            public Mute_Streaming_Audio mute_streaming_audio = Mute_Streaming_Audio.Disabled;
        }
    }
}
