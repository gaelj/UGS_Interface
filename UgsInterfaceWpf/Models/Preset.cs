using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Media;
using UGS.Helpers;

namespace UGS.Models
{
    public class Preset : NotificationObject
    {
        public Preset()
        {
            IsLoaded = false;
        }
        public static string kwe2p = "EEPROM ";

        public static UGS _ugs;
        public static void SetUgs(UGS ugs)
        {
            _ugs = ugs;
        }
        public static int GetNumberFromSerialString(string key, string keyword)
        {
            if (!key.StartsWith(keyword)) return -1;
            key = key.Substring(keyword.Length);
            int n = int.Parse(key.Split('=')[0].Trim());
            return n;
        }

        public Dictionary<string, string> SerializeData()
        {
            var v = new List<int>() {
                CurrentInput & 0xFF,
                OutAssym?1:0,
                Mute?1:0,
                Bypass?1:0,
                VolIncrement & 0xFF,

                Illum_TFT_H & 0xFF,
                IllumVol1_H & 0xFF,
                IllumVol2_H & 0xFF,
                IllumRly1_H & 0xFF,
                IllumRly2_H & 0xFF,

                Illum_TFT_L & 0xFF,
                IllumVol1_L & 0xFF,
                IllumVol2_L & 0xFF,
                IllumRly1_L & 0xFF,
                IllumRly2_L & 0xFF,

                // 2 bytes
                ((TouchTimeout >> 8) & 0xFF),
                (TouchTimeout & 0xFF),
                ((HoldTimeout >> 8) & 0xFF),
                (HoldTimeout & 0xFF),
                ((TftSleepTimeout >> 8) & 0xFF),
                (TftSleepTimeout & 0xFF),
                /**************************************************************/
                _Inputs[0].VolumeIn & 0xFF,
                _Inputs[1].VolumeIn & 0xFF,
                _Inputs[2].VolumeIn & 0xFF,
                _Inputs[3].VolumeIn & 0xFF,
                _Inputs[4].VolumeIn & 0xFF,
                _Inputs[5].VolumeIn & 0xFF,
                /**************************************************************/
                // 2 bytes                    
                ((((Int16)BalanceLR) >> 8) & 0xFF),
                (((Int16)BalanceLR) & 0xFF),

                ((((Int16)Offset1) >> 8) & 0xFF),
                (((Int16)Offset1) & 0xFF),
                ((((Int16)Offset2) >> 8) & 0xFF),
                (((Int16)Offset2) & 0xFF),
                ((((Int16)Offset3) >> 8) & 0xFF),
                (((Int16)Offset3) & 0xFF),

                DefaultInput & 0xFF,
                UseDefOrLastInput?1:0,
                /**************************************************************/               
                _Inputs[0].DefaultVolumeIn & 0xFF,
                _Inputs[1].DefaultVolumeIn & 0xFF,
                _Inputs[2].DefaultVolumeIn & 0xFF,
                _Inputs[3].DefaultVolumeIn & 0xFF,
                _Inputs[4].DefaultVolumeIn & 0xFF,
                _Inputs[5].DefaultVolumeIn & 0xFF,
                /**************************************************************/               
                UseDefOrLastVolume?1:0,
                /**************************************************************/               
                (int) _Inputs[0].InputIcon & 0xFF,
                (int) _Inputs[1].InputIcon & 0xFF,
                (int) _Inputs[2].InputIcon & 0xFF,
                (int) _Inputs[3].InputIcon & 0xFF,
                (int) _Inputs[4].InputIcon & 0xFF,
                (int) _Inputs[5].InputIcon & 0xFF,

                _Inputs[0].InputSource & 0xFF,
                _Inputs[1].InputSource & 0xFF,
                _Inputs[2].InputSource & 0xFF,
                _Inputs[3].InputSource & 0xFF,
                _Inputs[4].InputSource & 0xFF,
                _Inputs[5].InputSource & 0xFF,

                _Inputs[0].InputTrigger ?1:0,
                _Inputs[1].InputTrigger?1:0,
                _Inputs[2].InputTrigger?1:0,
                _Inputs[3].InputTrigger?1:0,
                _Inputs[4].InputTrigger?1:0,
                _Inputs[5].InputTrigger?1:0,
                /**************************************************************/               
                    
                // 2 bytes                                 
                (FrontColor >> 8) & 0xFF,
                (FrontColor & 0xFF),
                (BackColor >> 8) & 0xFF,
                (BackColor & 0xFF),
                (BackColor2 >> 8) & 0xFF,
                (BackColor2 & 0xFF),
                (SelectedFrontColor >> 8) & 0xFF,
                (SelectedFrontColor & 0xFF),
                (TouchFrontColor >> 8) & 0xFF,
                (TouchFrontColor & 0xFF),
                (InactiveColor >> 8) & 0xFF,
                (InactiveColor & 0xFF),

                PowerToggleDelay & 0xFF,
                VolumeSweepSpeed & 0xFF,
                TftSweepSpeed & 0xFF,
                CommonVolume?1:0,

                /**************************************************************/               
                _Inputs[0].Psu2Trigger?1:0,
                _Inputs[1].Psu2Trigger?1:0,
                _Inputs[2].Psu2Trigger?1:0,
                _Inputs[3].Psu2Trigger?1:0,
                _Inputs[4].Psu2Trigger?1:0,
                _Inputs[5].Psu2Trigger?1:0,

                _Inputs[0].Ext1Trigger?1:0,
                _Inputs[1].Ext1Trigger?1:0,
                _Inputs[2].Ext1Trigger?1:0,
                _Inputs[3].Ext1Trigger?1:0,
                _Inputs[4].Ext1Trigger?1:0,
                _Inputs[5].Ext1Trigger?1:0,

                _Inputs[0].Ext2Trigger?1:0,
                _Inputs[1].Ext2Trigger?1:0,
                _Inputs[2].Ext2Trigger?1:0,
                _Inputs[3].Ext2Trigger?1:0,
                _Inputs[4].Ext2Trigger?1:0,
                _Inputs[5].Ext2Trigger?1:0,

                _Inputs[0].UseInput?1:0,
                _Inputs[1].UseInput?1:0,
                _Inputs[2].UseInput?1:0,
                _Inputs[3].UseInput?1:0,
                _Inputs[4].UseInput?1:0,
                _Inputs[5].UseInput?1:0,
                /**************************************************************/               

                OutExt?1:0,

                // v puis r pour chaque ligne
                LedAnimations[0].AnimationsVolumeValue & 0xFF,
                LedAnimations[0].AnimationsRearValue & 0xFF,
                LedAnimations[1].AnimationsVolumeValue & 0xFF,
                LedAnimations[1].AnimationsRearValue & 0xFF,
                LedAnimations[2].AnimationsVolumeValue & 0xFF,
                LedAnimations[2].AnimationsRearValue & 0xFF,
                LedAnimations[3].AnimationsVolumeValue & 0xFF,
                LedAnimations[3].AnimationsRearValue & 0xFF,
                LedAnimations[4].AnimationsVolumeValue & 0xFF,
                LedAnimations[4].AnimationsRearValue & 0xFF,
                LedAnimations[5].AnimationsVolumeValue & 0xFF,
                LedAnimations[5].AnimationsRearValue & 0xFF,
                LedAnimations[6].AnimationsVolumeValue & 0xFF,
                LedAnimations[6].AnimationsRearValue & 0xFF,
                LedAnimations[7].AnimationsVolumeValue & 0xFF,
                LedAnimations[7].AnimationsRearValue & 0xFF,
                LedAnimations[8].AnimationsVolumeValue & 0xFF,
                LedAnimations[8].AnimationsRearValue & 0xFF,
                LedAnimations[9].AnimationsVolumeValue & 0xFF,
                LedAnimations[9].AnimationsRearValue & 0xFF,
                LedAnimations[10].AnimationsVolumeValue & 0xFF,
                LedAnimations[10].AnimationsRearValue & 0xFF,
                LedAnimations[11].AnimationsVolumeValue & 0xFF,
                LedAnimations[11].AnimationsRearValue & 0xFF,
                LedAnimations[12].AnimationsVolumeValue & 0xFF,
                LedAnimations[12].AnimationsRearValue & 0xFF,

                ActionIdRotary & 0xFF,
                ActionIdPush & 0xFF,

                PowerOnMuteDelay & 0xFF,

                /**************************************************************/               
                _Inputs[0].MinVolume & 0xFF,
                _Inputs[1].MinVolume & 0xFF,
                _Inputs[2].MinVolume & 0xFF,
                _Inputs[3].MinVolume & 0xFF,
                _Inputs[4].MinVolume & 0xFF,
                _Inputs[5].MinVolume & 0xFF,

                _Inputs[0].MaxVolume & 0xFF,
                _Inputs[1].MaxVolume & 0xFF,
                _Inputs[2].MaxVolume & 0xFF,
                _Inputs[3].MaxVolume & 0xFF,
                _Inputs[4].MaxVolume & 0xFF,
                _Inputs[5].MaxVolume & 0xFF,
                /**************************************************************/               

                // 2 bytes
                
                LedAnimations[0].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[1].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[2].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[3].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[4].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[5].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[6].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[7].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[9].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[9].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[10].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[11].AnimationStateDelay_vValue & 0xFF,
                LedAnimations[12].AnimationStateDelay_vValue & 0xFF,

                LedAnimations[0].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[1].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[2].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[3].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[4].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[5].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[6].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[7].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[9].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[9].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[10].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[11].AnimationStateDelay_rValue & 0xFF,
                LedAnimations[12].AnimationStateDelay_rValue & 0xFF,


                LedAnimations[0].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[1].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[2].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[3].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[4].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[5].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[6].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[7].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[9].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[9].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[10].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[11].AnimationStateRepeat_vValue & 0xFF,
                LedAnimations[12].AnimationStateRepeat_vValue & 0xFF,

                LedAnimations[0].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[1].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[2].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[3].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[4].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[5].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[6].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[7].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[9].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[9].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[10].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[11].AnimationStateRepeat_rValue & 0xFF,
                LedAnimations[12].AnimationStateRepeat_rValue & 0xFF,

                /**************************************************************/               
                _Inputs[0].VuOffset & 0xFF,
                _Inputs[1].VuOffset & 0xFF,
                _Inputs[2].VuOffset & 0xFF,
                _Inputs[3].VuOffset & 0xFF,
                _Inputs[4].VuOffset & 0xFF,
                _Inputs[5].VuOffset & 0xFF,

                _Inputs[0].VuMultiplier & 0xFF,
                _Inputs[1].VuMultiplier & 0xFF,
                _Inputs[2].VuMultiplier & 0xFF,
                _Inputs[3].VuMultiplier & 0xFF,
                _Inputs[4].VuMultiplier & 0xFF,
                _Inputs[5].VuMultiplier & 0xFF
                /**************************************************************/               

            }.ToArray();

            StringBuilder hex = new StringBuilder(v.Length * 2);

            var keyvalues = new Dictionary<string, string>();
            int c = 0;
            byte i = 0;
            foreach (var b in v)
            {
                hex.AppendFormat("{0:x2}", b);
                c++;
                if (c == 20) // work with 20 byte long words to prevent arduino serial port overflow
                {
                    c = 0;
                    keyvalues.Add(
                        string.Format("{0}{1}", kwe2p, Number.ToString() + ASCIIEncoding.ASCII.GetChars(ASCIIEncoding.ASCII.GetBytes("A").Select(x => (byte)(x + i)).ToArray()).First()),
                        hex.ToString()
                    );
                    hex = new StringBuilder(v.Length * 2);
                    i++;
                }
            }

            // last bytes
            if (c != 0)
            {
                keyvalues.Add(
                    string.Format("{0}{1}", kwe2p, Number.ToString() + ASCIIEncoding.ASCII.GetChars(ASCIIEncoding.ASCII.GetBytes("A").Select(x => (byte)(x + i)).ToArray()).First()),
                    hex.ToString()
                );
            }

            return keyvalues;
        }

        public void DeserializeData(int number, string str2)
        {
            IsLoaded = false;
            int i;
            List<int> v = new List<int>();

            for (var j = 0; j < str2.Length; j += 2)
            {
                string c = str2.Substring(j, 2);
                v.Add(int.Parse(c, System.Globalization.NumberStyles.HexNumber));
            }
            i = 0;

            _CurrentInput = v[i++];
            _OutAssym = (v[i++] == 1);
            _Mute = (v[i++] == 1);
            _Bypass = (v[i++] == 1);
            _VolIncrement = v[i++];

            _Illum_TFT_H = v[i++];
            _IllumVol1_H = v[i++];
            _IllumVol2_H = v[i++];
            _IllumRly1_H = v[i++];
            _IllumRly2_H = v[i++];
            _Illum_TFT_L = v[i++];
            _IllumVol1_L = v[i++];
            _IllumVol2_L = v[i++];
            _IllumRly1_L = v[i++];
            _IllumRly2_L = v[i++];

            // 2 bytes
            _TouchTimeout = v[i++] << 8 | v[i++];
            _HoldTimeout = v[i++] << 8 | v[i++];
            _TftSleepTimeout = v[i++] << 8 | v[i++];

            for (var j = 0; j < 6; j++)
                _Inputs[j]._VolumeIn = v[i++];

            // 2 bytes
            _BalanceLR = ((Int16)(v[i++] << 8 | v[i++]));
            _Offset1 = ((Int16)(v[i++] << 8 | v[i++]));
            _Offset2 = ((Int16)(v[i++] << 8 | v[i++]));
            _Offset3 = ((Int16)(v[i++] << 8 | v[i++]));

            _DefaultInput = v[i++];
            _UseDefOrLastInput = (v[i++] == 1);

            for (var j = 0; j < 6; j++)
                _Inputs[j]._DefaultVolumeIn = v[i++];

            _UseDefOrLastVolume = (v[i++] == 1);

            for (var j = 0; j < 6; j++)
                _Inputs[j]._InputIcon = (Ic)v[i++];
            for (var j = 0; j < 6; j++)
                _Inputs[j]._InputSource = v[i++];
            for (var j = 0; j < 6; j++)
                _Inputs[j]._InputTrigger = (v[i++] == 1);

            // 2 bytes
            _FrontColor = v[i++] << 8 | v[i++];
            _BackColor = v[i++] << 8 | v[i++];
            _BackColor2 = v[i++] << 8 | v[i++];
            _SelectedFrontColor = v[i++] << 8 | v[i++];
            _TouchFrontColor = v[i++] << 8 | v[i++];
            _InactiveColor = v[i++] << 8 | v[i++];

            _SoftStartDelay = v[i++];
            _VolumeSweepSpeed = v[i++];
            _TftSweepSpeed = v[i++];
            _CommonVolume = (v[i++] == 1);

            for (var j = 0; j < 6; j++)
                _Inputs[j]._Psu2Trigger = (v[i++] == 1);
            for (var j = 0; j < 6; j++)
                _Inputs[j]._Ext1Trigger = (v[i++] == 1);
            for (var j = 0; j < 6; j++)
                _Inputs[j]._Ext2Trigger = (v[i++] == 1);
            for (var j = 0; j < 6; j++)
                _Inputs[j]._UseInput = (v[i++] == 1);

            _OutExt = (v[i++] == 1);

            for (int j = 0; j < 13; j++)
            {
                LedAnimations[j]._AnimationsVolumeValue = v[i++];
                LedAnimations[j]._AnimationsRearValue = v[i++];
            }

            _ActionIdRotary = v[i++];
            _ActionIdPush = v[i++];

            _PowerOnMuteDelay = v[i++];


            for (var j = 0; j < 6; j++)
                _Inputs[j]._MinVolume = v[i++];
            for (var j = 0; j < 6; j++)
                _Inputs[j]._MaxVolume = v[i++];


            // 2 bytes    
            for (int j = 0; j < 13; j++)
                LedAnimations[j]._AnimationStateDelay_vValue = v[i++];

            for (int j = 0; j < 13; j++)
                LedAnimations[j]._AnimationStateDelay_rValue = v[i++];

            for (int j = 0; j < 13; j++)
                LedAnimations[j]._AnimationStateRepeat_vValue = v[i++];

            for (int j = 0; j < 13; j++)
                LedAnimations[j]._AnimationStateRepeat_rValue = v[i++];

            for (var j = 0; j < 6; j++)
                _Inputs[j]._VuOffset = v[i++];
            for (var j = 0; j < 6; j++)
                _Inputs[j]._VuMultiplier = v[i++];




            _StandbyIcon = Ic.ICONS_Standby;
            _MuteIcon = Ic.ICONS_Mute;
            _BypassIcon = Ic.ICONS_Bypass;
            _SymmoutIcon = Ic.ICONS_Symmout;
            _SpeakerIcon = Ic.ICONS_Speaker;

            RaisePropertyChanged(() => CurrentInput);
            RaisePropertyChanged(() => OutAssym);
            RaisePropertyChanged(() => Mute);
            RaisePropertyChanged(() => Bypass);
            RaisePropertyChanged(() => VolIncrement);
            RaisePropertyChanged(() => Illum_TFT_H);
            RaisePropertyChanged(() => IllumVol1_H);
            RaisePropertyChanged(() => IllumVol2_H);
            RaisePropertyChanged(() => IllumRly1_H);
            RaisePropertyChanged(() => IllumRly2_H);
            RaisePropertyChanged(() => Illum_TFT_L);
            RaisePropertyChanged(() => IllumVol1_L);
            RaisePropertyChanged(() => IllumVol2_L);
            RaisePropertyChanged(() => IllumRly1_L);
            RaisePropertyChanged(() => IllumRly2_L);
            RaisePropertyChanged(() => TouchTimeout);
            RaisePropertyChanged(() => HoldTimeout);
            RaisePropertyChanged(() => TftSleepTimeout);

            RaisePropertyChanged(() => BalanceLR);
            RaisePropertyChanged(() => Offset1);
            RaisePropertyChanged(() => Offset2);
            RaisePropertyChanged(() => Offset3);

            RaisePropertyChanged(() => DefaultInput);
            RaisePropertyChanged(() => UseDefOrLastInput);
            RaisePropertyChanged(() => UseDefOrLastVolume);

            RaisePropertyChanged(() => FrontColor);
            RaisePropertyChanged(() => BackColor);
            RaisePropertyChanged(() => BackColor2);
            RaisePropertyChanged(() => SelectedFrontColor);
            RaisePropertyChanged(() => TouchFrontColor);
            RaisePropertyChanged(() => InactiveColor);

            RaisePropertyChanged(() => StandbyIcon);
            RaisePropertyChanged(() => MuteIcon);
            RaisePropertyChanged(() => BypassIcon);
            RaisePropertyChanged(() => SymmoutIcon);
            RaisePropertyChanged(() => SpeakerIcon);

            RaisePropertyChanged(() => PowerToggleDelay);
            RaisePropertyChanged(() => VolumeSweepSpeed);
            RaisePropertyChanged(() => TftSweepSpeed);
            RaisePropertyChanged(() => CommonVolume);

            RaisePropertyChanged(() => UseAtLeastOneInput);

            RaisePropertyChanged(() => OutExt);
            RaisePropertyChanged(() => ActionIdRotary);
            RaisePropertyChanged(() => ActionIdPush);

            RaisePropertyChanged(() => PowerOnMuteDelay);

            foreach (var p in _Inputs)
            {
                RaisePropertyChanged(() => p.VolumeIn);
                RaisePropertyChanged(() => p.DefaultVolumeIn);
                RaisePropertyChanged(() => p.InputSource);
                RaisePropertyChanged(() => p.InputTrigger);
                RaisePropertyChanged(() => p.InputIcon);
                RaisePropertyChanged(() => p.Psu2Trigger);
                RaisePropertyChanged(() => p.Ext1Trigger);
                RaisePropertyChanged(() => p.Ext2Trigger);
                RaisePropertyChanged(() => p.UseInput);
                RaisePropertyChanged(() => p.MinVolume);
                RaisePropertyChanged(() => p.MaxVolume);
                RaisePropertyChanged(() => p.VuOffset);
                RaisePropertyChanged(() => p.VuMultiplier);
            }

            foreach (var l in LedAnimations)
            {
                RaisePropertyChanged(() => l.AnimationsName);
                RaisePropertyChanged(() => l.AnimationsVolumeValue);
                RaisePropertyChanged(() => l.AnimationStateDelay_vValue);
                RaisePropertyChanged(() => l.AnimationStateRepeat_vValue);
                RaisePropertyChanged(() => l.AnimationsRearValue);
                RaisePropertyChanged(() => l.AnimationStateDelay_rValue);
                RaisePropertyChanged(() => l.AnimationStateRepeat_rValue);
            }
            
            IsLoaded = true;
        }
        public static void RequestDelayedSaveSettingsToUgs()
        {
            if (TimerE2pWrite == null)
            {
                TimerE2pWrite = new System.Timers.Timer(1000);
                TimerE2pWrite.AutoReset = false;
                TimerE2pWrite.Elapsed += new ElapsedEventHandler(SaveSettingsToUgs);
            }
            TimerE2pWrite.Enabled = true;
            TimerE2pWrite.Start();
        }
        private static void SaveSettingsToUgs(object sender, ElapsedEventArgs e)
        {
            if (_ugs == null)
                return;

            foreach (var kv in _ugs.CurrentPreset.SerializeData())
            {
                while (!_ugs.UgsIsReadForNewCommands) Thread.Sleep(50);
                _ugs.SetValue(kv.Key, kv.Value);
            }
            TimerE2pWrite.Enabled = false;
        }

        private static System.Timers.Timer TimerE2pWrite;

        #region Properties


        public bool IsLoaded { get; set; }

        public int VolumeSteps
        {
            get { return 224; }
        }




        #region Preset number, CurrentInput, OutAssym, OutExt, Mute, Bypass, VolIncrement

        private int _Number;
        public int Number
        {
            get { return _Number; }
            set
            {
                _Number = value;
                RaisePropertyChanged(() => Number);
            }
        }

        private int _CurrentInput;
        public int CurrentInput
        {
            get { return _CurrentInput; }
            set
            {
                _CurrentInput = value;
                RaisePropertyChanged(() => CurrentInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _OutAssym;
        public bool OutAssym
        {
            get { return _OutAssym; }
            set
            {
                _OutAssym = value;
                RaisePropertyChanged(() => OutAssym);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _OutExt;
        public bool OutExt
        {
            get { return _OutExt; }
            set
            {
                _OutExt = value;
                RaisePropertyChanged(() => OutExt);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Mute;
        public bool Mute
        {
            get { return _Mute; }
            set
            {
                _Mute = value;
                RaisePropertyChanged(() => Mute);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Bypass;
        public bool Bypass
        {
            get { return _Bypass; }
            set
            {
                _Bypass = value;
                RaisePropertyChanged(() => Bypass);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _VolIncrement;
        public int VolIncrement
        {
            get { return _VolIncrement; }
            set
            {
                _VolIncrement = value;
                RaisePropertyChanged(() => VolIncrement);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        #region Illumination H / L
        private int _Illum_TFT_H;
        public int Illum_TFT_H
        {
            get { return _Illum_TFT_H; }
            set
            {
                _Illum_TFT_H = value;
                RaisePropertyChanged(() => Illum_TFT_H);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumVol1_H;
        public int IllumVol1_H
        {
            get { return _IllumVol1_H; }
            set
            {
                _IllumVol1_H = value;
                RaisePropertyChanged(() => IllumVol1_H);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumVol2_H;
        public int IllumVol2_H
        {
            get { return _IllumVol2_H; }
            set
            {
                _IllumVol2_H = value;
                RaisePropertyChanged(() => IllumVol2_H);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumRly1_H;
        public int IllumRly1_H
        {
            get { return _IllumRly1_H; }
            set
            {
                _IllumRly1_H = value;
                RaisePropertyChanged(() => IllumRly1_H);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumRly2_H;
        public int IllumRly2_H
        {
            get { return _IllumRly2_H; }
            set
            {
                _IllumRly2_H = value;
                RaisePropertyChanged(() => IllumRly2_H);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _Illum_TFT_L;
        public int Illum_TFT_L
        {
            get { return _Illum_TFT_L; }
            set
            {
                _Illum_TFT_L = value;
                RaisePropertyChanged(() => Illum_TFT_L);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumVol1_L;
        public int IllumVol1_L
        {
            get { return _IllumVol1_L; }
            set
            {
                _IllumVol1_L = value;
                RaisePropertyChanged(() => IllumVol1_L);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumVol2_L;
        public int IllumVol2_L
        {
            get { return _IllumVol2_L; }
            set
            {
                _IllumVol2_L = value;
                RaisePropertyChanged(() => IllumVol2_L);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumRly1_L;
        public int IllumRly1_L
        {
            get { return _IllumRly1_L; }
            set
            {
                _IllumRly1_L = value;
                RaisePropertyChanged(() => IllumRly1_L);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _IllumRly2_L;
        public int IllumRly2_L
        {
            get { return _IllumRly2_L; }
            set
            {
                _IllumRly2_L = value;
                RaisePropertyChanged(() => IllumRly2_L);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        #region Timeouts
        private int _TouchTimeout;
        public int TouchTimeout
        {
            get { return _TouchTimeout; }
            set
            {
                _TouchTimeout = value;
                RaisePropertyChanged(() => TouchTimeout);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _HoldTimeout;
        public int HoldTimeout
        {
            get { return _HoldTimeout; }
            set
            {
                _HoldTimeout = value;
                RaisePropertyChanged(() => HoldTimeout);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _TftSleepTimeout;
        public int TftSleepTimeout
        {
            get { return _TftSleepTimeout; }
            set
            {
                _TftSleepTimeout = value;
                RaisePropertyChanged(() => TftSleepTimeout);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        /*
        #region Volume In

        private int _VolumeIn1;
        public int VolumeIn1
        {
            get { return _VolumeIn1; }
            set
            {
                _VolumeIn1 = value;
                RaisePropertyChanged(() => VolumeIn1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _VolumeIn2;
        public int VolumeIn2
        {
            get { return _VolumeIn2; }
            set
            {
                _VolumeIn2 = value;
                RaisePropertyChanged(() => VolumeIn2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _VolumeIn3;
        public int VolumeIn3
        {
            get { return _VolumeIn3; }
            set
            {
                _VolumeIn3 = value;
                RaisePropertyChanged(() => VolumeIn3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _VolumeIn4;
        public int VolumeIn4
        {
            get { return _VolumeIn4; }
            set
            {
                _VolumeIn4 = value;
                RaisePropertyChanged(() => VolumeIn4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _VolumeIn5;
        public int VolumeIn5
        {
            get { return _VolumeIn5; }
            set
            {
                _VolumeIn5 = value;
                RaisePropertyChanged(() => VolumeIn5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _VolumeIn6;
        public int VolumeIn6
        {
            get { return _VolumeIn6; }
            set
            {
                _VolumeIn6 = value;
                RaisePropertyChanged(() => VolumeIn6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion
        */

        #region Balance / Offsets

        private int _BalanceLR;
        public int BalanceLR
        {
            get { return _BalanceLR; }
            set
            {
                _BalanceLR = value;
                RaisePropertyChanged(() => BalanceLR);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _Offset1;
        public int Offset1
        {
            get { return _Offset1; }
            set
            {
                _Offset1 = value;
                RaisePropertyChanged(() => Offset1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _Offset2;
        public int Offset2
        {
            get { return _Offset2; }
            set
            {
                _Offset2 = value;
                RaisePropertyChanged(() => Offset2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _Offset3;
        public int Offset3
        {
            get { return _Offset3; }
            set
            {
                _Offset3 = value;
                RaisePropertyChanged(() => Offset3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        #region DefaultInput, UseDefOrLastInput, DefaultVolumeIns, UseDefOrLastVolume

        private int _DefaultInput;
        public int DefaultInput
        {
            get { return _DefaultInput; }
            set
            {
                _DefaultInput = value;
                RaisePropertyChanged(() => DefaultInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _UseDefOrLastInput;
        public bool UseDefOrLastInput
        {
            get { return _UseDefOrLastInput; }
            set
            {
                _UseDefOrLastInput = value;
                RaisePropertyChanged(() => UseDefOrLastInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        /*
        private int _DefaultVolumeIn1;
        public int DefaultVolumeIn1
        {
            get { return _DefaultVolumeIn1; }
            set
            {
                _DefaultVolumeIn1 = value;
                RaisePropertyChanged(() => DefaultVolumeIn1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _DefaultVolumeIn2;
        public int DefaultVolumeIn2
        {
            get { return _DefaultVolumeIn2; }
            set
            {
                _DefaultVolumeIn2 = value;
                RaisePropertyChanged(() => DefaultVolumeIn2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _DefaultVolumeIn3;
        public int DefaultVolumeIn3
        {
            get { return _DefaultVolumeIn3; }
            set
            {
                _DefaultVolumeIn3 = value;
                RaisePropertyChanged(() => DefaultVolumeIn3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _DefaultVolumeIn4;
        public int DefaultVolumeIn4
        {
            get { return _DefaultVolumeIn4; }
            set
            {
                _DefaultVolumeIn4 = value;
                RaisePropertyChanged(() => DefaultVolumeIn4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _DefaultVolumeIn5;
        public int DefaultVolumeIn5
        {
            get { return _DefaultVolumeIn5; }
            set
            {
                _DefaultVolumeIn5 = value;
                RaisePropertyChanged(() => DefaultVolumeIn5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _DefaultVolumeIn6;
        public int DefaultVolumeIn6
        {
            get { return _DefaultVolumeIn6; }
            set
            {
                _DefaultVolumeIn6 = value;
                RaisePropertyChanged(() => DefaultVolumeIn6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        */
        private bool _UseDefOrLastVolume;
        public bool UseDefOrLastVolume
        {
            get { return _UseDefOrLastVolume; }
            set
            {
                _UseDefOrLastVolume = value;
                RaisePropertyChanged(() => UseDefOrLastVolume);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        #region Active Icons Ids
        private Ic _StandbyIcon;
        public Ic StandbyIcon
        {
            get
            {
                return _StandbyIcon;
            }
            set
            {
                _StandbyIcon = value;
                RaisePropertyChanged(() => StandbyIcon);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _MuteIcon;
        public Ic MuteIcon
        {
            get
            {
                return _MuteIcon;
            }
            set
            {
                _MuteIcon = value;
                RaisePropertyChanged(() => MuteIcon);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _BypassIcon;
        public Ic BypassIcon
        {
            get
            {
                return _BypassIcon;
            }
            set
            {
                _BypassIcon = value;
                RaisePropertyChanged(() => BypassIcon);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _SymmoutIcon;
        public Ic SymmoutIcon
        {
            get
            {
                return _SymmoutIcon;
            }
            set
            {
                _SymmoutIcon = value;
                RaisePropertyChanged(() => SymmoutIcon);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _SpeakerIcon;
        public Ic SpeakerIcon
        {
            get
            {
                return _SpeakerIcon;
            }
            set
            {
                _SpeakerIcon = value;
                RaisePropertyChanged(() => SpeakerIcon);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        /*
        private Ic _InputIcons1;
        public Ic InputIcons1
        {
            get { return _InputIcons1; }
            set
            {
                if (_InputIcons1 == value) return;

                if (value == _InputIcons2) { _InputIcons2 = _InputIcons1; RaisePropertyChanged(() => InputIcons2); }
                else if (value == _InputIcons3) { _InputIcons3 = _InputIcons1; RaisePropertyChanged(() => InputIcons3); }
                else if (value == _InputIcons4) { _InputIcons4 = _InputIcons1; RaisePropertyChanged(() => InputIcons4); }
                else if (value == _InputIcons5) { _InputIcons5 = _InputIcons1; RaisePropertyChanged(() => InputIcons5); }
                else if (value == _InputIcons6) { _InputIcons6 = _InputIcons1; RaisePropertyChanged(() => InputIcons6); }
                _InputIcons1 = value;
                RaisePropertyChanged(() => InputIcons1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _InputIcons2;
        public Ic InputIcons2
        {
            get { return _InputIcons2; }
            set
            {
                if (_InputIcons2 == value) return;

                if (value == _InputIcons1) { _InputIcons1 = _InputIcons2; RaisePropertyChanged(() => InputIcons1); }
                else if (value == _InputIcons3) { _InputIcons3 = _InputIcons2; RaisePropertyChanged(() => InputIcons3); }
                else if (value == _InputIcons4) { _InputIcons4 = _InputIcons2; RaisePropertyChanged(() => InputIcons4); }
                else if (value == _InputIcons5) { _InputIcons5 = _InputIcons2; RaisePropertyChanged(() => InputIcons5); }
                else if (value == _InputIcons6) { _InputIcons6 = _InputIcons2; RaisePropertyChanged(() => InputIcons6); }
                _InputIcons2 = value;
                RaisePropertyChanged(() => InputIcons2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _InputIcons3;
        public Ic InputIcons3
        {
            get { return _InputIcons3; }
            set
            {
                if (_InputIcons3 == value) return;

                if (value == _InputIcons1) { _InputIcons1 = _InputIcons3; RaisePropertyChanged(() => InputIcons1); }
                else if (value == _InputIcons2) { _InputIcons2 = _InputIcons3; RaisePropertyChanged(() => InputIcons2); }
                else if (value == _InputIcons4) { _InputIcons4 = _InputIcons3; RaisePropertyChanged(() => InputIcons4); }
                else if (value == _InputIcons5) { _InputIcons5 = _InputIcons3; RaisePropertyChanged(() => InputIcons5); }
                else if (value == _InputIcons6) { _InputIcons6 = _InputIcons3; RaisePropertyChanged(() => InputIcons6); }
                _InputIcons3 = value;
                RaisePropertyChanged(() => InputIcons3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _InputIcons4;
        public Ic InputIcons4
        {
            get { return _InputIcons4; }
            set
            {
                if (_InputIcons4 == value) return;

                if (value == _InputIcons1) { _InputIcons1 = _InputIcons4; RaisePropertyChanged(() => InputIcons1); }
                else if (value == _InputIcons2) { _InputIcons2 = _InputIcons4; RaisePropertyChanged(() => InputIcons2); }
                else if (value == _InputIcons3) { _InputIcons3 = _InputIcons4; RaisePropertyChanged(() => InputIcons3); }
                else if (value == _InputIcons5) { _InputIcons5 = _InputIcons4; RaisePropertyChanged(() => InputIcons5); }
                else if (value == _InputIcons6) { _InputIcons6 = _InputIcons4; RaisePropertyChanged(() => InputIcons6); }
                _InputIcons4 = value;
                RaisePropertyChanged(() => InputIcons4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _InputIcons5;
        public Ic InputIcons5
        {
            get { return _InputIcons5; }
            set
            {
                if (_InputIcons5 == value) return;

                if (value == _InputIcons1) { _InputIcons1 = _InputIcons5; RaisePropertyChanged(() => InputIcons1); }
                else if (value == _InputIcons2) { _InputIcons2 = _InputIcons5; RaisePropertyChanged(() => InputIcons2); }
                else if (value == _InputIcons3) { _InputIcons3 = _InputIcons5; RaisePropertyChanged(() => InputIcons3); }
                else if (value == _InputIcons4) { _InputIcons4 = _InputIcons5; RaisePropertyChanged(() => InputIcons4); }
                else if (value == _InputIcons6) { _InputIcons6 = _InputIcons5; RaisePropertyChanged(() => InputIcons6); }
                _InputIcons5 = value;
                RaisePropertyChanged(() => InputIcons5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private Ic _InputIcons6;
        public Ic InputIcons6
        {
            get { return _InputIcons6; }
            set
            {
                if (_InputIcons6 == value) return;

                if (value == _InputIcons1) { _InputIcons1 = _InputIcons6; RaisePropertyChanged(() => InputIcons1); }
                else if (value == _InputIcons2) { _InputIcons2 = _InputIcons6; RaisePropertyChanged(() => InputIcons2); }
                else if (value == _InputIcons3) { _InputIcons3 = _InputIcons6; RaisePropertyChanged(() => InputIcons3); }
                else if (value == _InputIcons4) { _InputIcons4 = _InputIcons6; RaisePropertyChanged(() => InputIcons4); }
                else if (value == _InputIcons5) { _InputIcons5 = _InputIcons6; RaisePropertyChanged(() => InputIcons5); }
                _InputIcons6 = value;
                RaisePropertyChanged(() => InputIcons6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }


        private int _InputSources1;
        public int InputSources1
        {
            get { return _InputSources1; }
            set
            {
                _InputSources1 = value;
                RaisePropertyChanged(() => InputSources1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _InputSources2;
        public int InputSources2
        {
            get { return _InputSources2; }
            set
            {
                _InputSources2 = value;
                RaisePropertyChanged(() => InputSources2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _InputSources3;
        public int InputSources3
        {
            get { return _InputSources3; }
            set
            {
                _InputSources3 = value;
                RaisePropertyChanged(() => InputSources3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _InputSources4;
        public int InputSources4
        {
            get { return _InputSources4; }
            set
            {
                _InputSources4 = value;
                RaisePropertyChanged(() => InputSources4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _InputSources5;
        public int InputSources5
        {
            get { return _InputSources5; }
            set
            {
                _InputSources5 = value;
                RaisePropertyChanged(() => InputSources5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _InputSources6;
        public int InputSources6
        {
            get { return _InputSources6; }
            set
            {
                _InputSources6 = value;
                RaisePropertyChanged(() => InputSources6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        
        #endregion

            
        #region Triggers

        
        private bool _InputTrigger1;
        public bool InputTrigger1
        {
            get { return _InputTrigger1; }
            set
            {
                _InputTrigger1 = value;
                RaisePropertyChanged(() => InputTrigger1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _InputTrigger2;
        public bool InputTrigger2
        {
            get { return _InputTrigger2; }
            set
            {
                _InputTrigger2 = value;
                RaisePropertyChanged(() => InputTrigger2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _InputTrigger3;
        public bool InputTrigger3
        {
            get { return _InputTrigger3; }
            set
            {
                _InputTrigger3 = value;
                RaisePropertyChanged(() => InputTrigger3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _InputTrigger4;
        public bool InputTrigger4
        {
            get { return _InputTrigger4; }
            set
            {
                _InputTrigger4 = value;
                RaisePropertyChanged(() => InputTrigger4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _InputTrigger5;
        public bool InputTrigger5
        {
            get { return _InputTrigger5; }
            set
            {
                _InputTrigger5 = value;
                RaisePropertyChanged(() => InputTrigger5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _InputTrigger6;
        public bool InputTrigger6
        {
            get { return _InputTrigger6; }
            set
            {
                _InputTrigger6 = value;
                RaisePropertyChanged(() => InputTrigger6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }









        private bool _Psu2Trigger1;
        public bool Psu2Trigger1
        {
            get { return _Psu2Trigger1; }
            set
            {
                _Psu2Trigger1 = value;
                RaisePropertyChanged(() => Psu2Trigger1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Psu2Trigger2;
        public bool Psu2Trigger2
        {
            get { return _Psu2Trigger2; }
            set
            {
                _Psu2Trigger2 = value;
                RaisePropertyChanged(() => Psu2Trigger2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Psu2Trigger3;
        public bool Psu2Trigger3
        {
            get { return _Psu2Trigger3; }
            set
            {
                _Psu2Trigger3 = value;
                RaisePropertyChanged(() => Psu2Trigger3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Psu2Trigger4;
        public bool Psu2Trigger4
        {
            get { return _Psu2Trigger4; }
            set
            {
                _Psu2Trigger4 = value;
                RaisePropertyChanged(() => Psu2Trigger4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Psu2Trigger5;
        public bool Psu2Trigger5
        {
            get { return _Psu2Trigger5; }
            set
            {
                _Psu2Trigger5 = value;
                RaisePropertyChanged(() => Psu2Trigger5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Psu2Trigger6;
        public bool Psu2Trigger6
        {
            get { return _Psu2Trigger6; }
            set
            {
                _Psu2Trigger6 = value;
                RaisePropertyChanged(() => Psu2Trigger6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }




        private bool _Ext1Trigger1;
        public bool Ext1Trigger1
        {
            get { return _Ext1Trigger1; }
            set
            {
                _Ext1Trigger1 = value;
                RaisePropertyChanged(() => Ext1Trigger1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext1Trigger2;
        public bool Ext1Trigger2
        {
            get { return _Ext1Trigger2; }
            set
            {
                _Ext1Trigger2 = value;
                RaisePropertyChanged(() => Ext1Trigger2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext1Trigger3;
        public bool Ext1Trigger3
        {
            get { return _Ext1Trigger3; }
            set
            {
                _Ext1Trigger3 = value;
                RaisePropertyChanged(() => Ext1Trigger3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext1Trigger4;
        public bool Ext1Trigger4
        {
            get { return _Ext1Trigger4; }
            set
            {
                _Ext1Trigger4 = value;
                RaisePropertyChanged(() => Ext1Trigger4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext1Trigger5;
        public bool Ext1Trigger5
        {
            get { return _Ext1Trigger5; }
            set
            {
                _Ext1Trigger5 = value;
                RaisePropertyChanged(() => Ext1Trigger5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext1Trigger6;
        public bool Ext1Trigger6
        {
            get { return _Ext1Trigger6; }
            set
            {
                _Ext1Trigger6 = value;
                RaisePropertyChanged(() => Ext1Trigger6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }






        private bool _Ext2Trigger1;
        public bool Ext2Trigger1
        {
            get { return _Ext2Trigger1; }
            set
            {
                _Ext2Trigger1 = value;
                RaisePropertyChanged(() => Ext2Trigger1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext2Trigger2;
        public bool Ext2Trigger2
        {
            get { return _Ext2Trigger2; }
            set
            {
                _Ext2Trigger2 = value;
                RaisePropertyChanged(() => Ext2Trigger2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext2Trigger3;
        public bool Ext2Trigger3
        {
            get { return _Ext2Trigger3; }
            set
            {
                _Ext2Trigger3 = value;
                RaisePropertyChanged(() => Ext2Trigger3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext2Trigger4;
        public bool Ext2Trigger4
        {
            get { return _Ext2Trigger4; }
            set
            {
                _Ext2Trigger4 = value;
                RaisePropertyChanged(() => Ext2Trigger4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext2Trigger5;
        public bool Ext2Trigger5
        {
            get { return _Ext2Trigger5; }
            set
            {
                _Ext2Trigger5 = value;
                RaisePropertyChanged(() => Ext2Trigger5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _Ext2Trigger6;
        public bool Ext2Trigger6
        {
            get { return _Ext2Trigger6; }
            set
            {
                _Ext2Trigger6 = value;
                RaisePropertyChanged(() => Ext2Trigger6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        

        #endregion
            */

        #region Colors

        private void RefreshIcons()
        {
            foreach (var p in _Inputs)
                RaisePropertyChanged(() => p.InputIcon);

            RaisePropertyChanged(() => StandbyIcon);
            RaisePropertyChanged(() => MuteIcon);
            RaisePropertyChanged(() => BypassIcon);
            RaisePropertyChanged(() => SymmoutIcon);
            RaisePropertyChanged(() => SpeakerIcon);
        }

        private int _FrontColor;
        public int FrontColor
        {
            get { return _FrontColor; }
            set
            {
                _FrontColor = value;
                RaisePropertyChanged(() => FrontColor);
                Icon.SetIconFrontColor((Color)(new Converters.RGB565ToColorConverter().Convert(FrontColor, null, null, null)));
                RefreshIcons();
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _BackColor;
        public int BackColor
        {
            get { return _BackColor; }
            set
            {
                _BackColor = value;
                RaisePropertyChanged(() => BackColor);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _BackColor2;
        public int BackColor2
        {
            get { return _BackColor2; }
            set
            {
                _BackColor2 = value;
                RaisePropertyChanged(() => BackColor2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _SelectedFrontColor;
        public int SelectedFrontColor
        {
            get { return _SelectedFrontColor; }
            set
            {
                _SelectedFrontColor = value;
                RaisePropertyChanged(() => SelectedFrontColor);
                Icon.SetIconSelectedColor((Color)(new Converters.RGB565ToColorConverter().Convert(SelectedFrontColor, null, null, null)));
                RefreshIcons();
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _TouchFrontColor;
        public int TouchFrontColor
        {
            get { return _TouchFrontColor; }
            set
            {
                _TouchFrontColor = value;
                RaisePropertyChanged(() => TouchFrontColor);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _InactiveColor;
        public int InactiveColor
        {
            get { return _InactiveColor; }
            set
            {
                _InactiveColor = value;
                RaisePropertyChanged(() => InactiveColor);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        #region Delays / Speeds

        private int _SoftStartDelay;
        public int PowerToggleDelay
        {
            get { return _SoftStartDelay; }
            set
            {
                _SoftStartDelay = value;
                RaisePropertyChanged(() => PowerToggleDelay);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _VolumeSweepSpeed;
        public int VolumeSweepSpeed
        {
            get { return _VolumeSweepSpeed; }
            set
            {
                _VolumeSweepSpeed = value;
                RaisePropertyChanged(() => VolumeSweepSpeed);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _TftSweepSpeed;
        public int TftSweepSpeed
        {
            get { return _TftSweepSpeed; }
            set
            {
                _TftSweepSpeed = value;
                RaisePropertyChanged(() => TftSweepSpeed);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        #endregion

        #region Volume Settings
        private bool _CommonVolume;
        public bool CommonVolume
        {
            get { return _CommonVolume; }
            set
            {
                _CommonVolume = value;
                RaisePropertyChanged(() => CommonVolume);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        public int MinVolume
        {
            get
            {
                if (_ugs.Input == null) return -1;
                return _ugs.CurrentPreset._Inputs[(int)_ugs.Input].MinVolume;
            }
            set
            {
                _ugs.CurrentPreset._Inputs[(int)_ugs.Input].MinVolume = value;
            }
        }

        public int MaxVolume
        {
            get
            {
                if (_ugs.Input == null) return -1;
                return _ugs.CurrentPreset._Inputs[(int)_ugs.Input].MaxVolume;
            }
            set
            {
                _ugs.CurrentPreset._Inputs[(int)_ugs.Input].MaxVolume = value;
            }
        }

        /*
        private int _MinVolume1;
        public int MinVolume1
        {
            get { return _MinVolume1; }
            set
            {
                _MinVolume1 = value;
                RaisePropertyChanged(() => MinVolume1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MinVolume2;
        public int MinVolume2
        {
            get { return _MinVolume2; }
            set
            {
                _MinVolume2 = value;
                RaisePropertyChanged(() => MinVolume2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MinVolume3;
        public int MinVolume3
        {
            get { return _MinVolume3; }
            set
            {
                _MinVolume3 = value;
                RaisePropertyChanged(() => MinVolume3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MinVolume4;
        public int MinVolume4
        {
            get { return _MinVolume4; }
            set
            {
                _MinVolume4 = value;
                RaisePropertyChanged(() => MinVolume4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MinVolume5;
        public int MinVolume5
        {
            get { return _MinVolume5; }
            set
            {
                _MinVolume5 = value;
                RaisePropertyChanged(() => MinVolume5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MinVolume6;
        public int MinVolume6
        {
            get { return _MinVolume6; }
            set
            {
                _MinVolume6 = value;
                RaisePropertyChanged(() => MinVolume6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }


        private int _MaxVolume1;
        public int MaxVolume1
        {
            get { return _MaxVolume1; }
            set
            {
                _MaxVolume1 = value;
                RaisePropertyChanged(() => MaxVolume1);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MaxVolume2;
        public int MaxVolume2
        {
            get { return _MaxVolume2; }
            set
            {
                _MaxVolume2 = value;
                RaisePropertyChanged(() => MaxVolume2);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MaxVolume3;
        public int MaxVolume3
        {
            get { return _MaxVolume3; }
            set
            {
                _MaxVolume3 = value;
                RaisePropertyChanged(() => MaxVolume3);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MaxVolume4;
        public int MaxVolume4
        {
            get { return _MaxVolume4; }
            set
            {
                _MaxVolume4 = value;
                RaisePropertyChanged(() => MaxVolume4);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MaxVolume5;
        public int MaxVolume5
        {
            get { return _MaxVolume5; }
            set
            {
                _MaxVolume5 = value;
                RaisePropertyChanged(() => MaxVolume5);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _MaxVolume6;
        public int MaxVolume6
        {
            get { return _MaxVolume6; }
            set
            {
                _MaxVolume6 = value;
                RaisePropertyChanged(() => MaxVolume6);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        */

        #endregion

        #region Rotary push action

        private int _ActionIdRotary;
        public int ActionIdRotary
        {
            get { return _ActionIdRotary; }
            set
            {
                _ActionIdRotary = value;
                RaisePropertyChanged(() => ActionIdRotary);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _ActionIdPush;
        public int ActionIdPush
        {
            get { return _ActionIdPush; }
            set
            {
                _ActionIdPush = value;
                RaisePropertyChanged(() => ActionIdPush);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        #endregion

        #region Power on mute delay
        private int _PowerOnMuteDelay;
        public int PowerOnMuteDelay
        {
            get { return _PowerOnMuteDelay; }
            set
            {
                _PowerOnMuteDelay = value;
                RaisePropertyChanged(() => PowerOnMuteDelay);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }


        #endregion
        /*
        #region Use Inputs

        
        private bool _UseInput1;
        public bool UseInput1
        {
            get { return _UseInput1; }
            set
            {
                if (!value && UseInput1 && UsedInputCount <= 2)
                {
                    // throw new Exception("At least one input must be used");
                }
                else
                    _UseInput1 = value;
                RaisePropertyChanged(() => UseInput1);
                RaisePropertyChanged(() => UseAtLeastOneInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _UseInput2;
        public bool UseInput2
        {
            get { return _UseInput2; }
            set
            {
                if (!value && UseInput2 && UsedInputCount <= 2 && !UseInput1)
                {
                    // throw new Exception("At least one input must be used");
                }
                else
                    _UseInput2 = value;
                RaisePropertyChanged(() => UseInput2);
                RaisePropertyChanged(() => UseAtLeastOneInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _UseInput3;
        public bool UseInput3
        {
            get { return _UseInput3; }
            set
            {
                if (!value && UseInput3 && UsedInputCount <= 2 && !UseInput1)
                {
                    // throw new Exception("At least one input must be used");
                }
                else
                    _UseInput3 = value;
                RaisePropertyChanged(() => UseInput3);
                RaisePropertyChanged(() => UseAtLeastOneInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _UseInput4;
        public bool UseInput4
        {
            get { return _UseInput4; }
            set
            {
                if (!value && UseInput4 && UsedInputCount <= 2 && !UseInput1)
                {
                    // throw new Exception("At least one input must be used");
                }
                else
                    _UseInput4 = value;
                RaisePropertyChanged(() => UseInput4);
                RaisePropertyChanged(() => UseAtLeastOneInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private bool _UseInput5;
        public bool UseInput5
        {
            get { return _UseInput5; }
            set
            {
                if (!value && UseInput5 && UsedInputCount <= 2 && !UseInput1)
                {
                    // throw new Exception("At least one input must be used");
                }
                else
                    _UseInput5 = value;
                RaisePropertyChanged(() => UseInput5);
                RaisePropertyChanged(() => UseAtLeastOneInput);
                Preset.RequestDelayedSaveSettingsToUgs();

            }
        }

        private bool _UseInput6;
        public bool UseInput6
        {
            get { return _UseInput6; }
            set
            {
                if (!value && UseInput6 && UsedInputCount <= 2 && !UseInput1)
                {
                    // throw new Exception("At least one input must be used");
                }
                else
                    _UseInput6 = value;
                RaisePropertyChanged(() => UseInput6);
                RaisePropertyChanged(() => UseAtLeastOneInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }


        #endregion
            */

        /*
        #region Animation Ids Volume
        
        private int _AnimationId_v_standbyOff;
        public int AnimationId_v_standbyOff
        {
            get { return _AnimationId_v_standbyOff; }
            set
            {
                _AnimationId_v_standbyOff = value;
                RaisePropertyChanged(() => AnimationId_v_standbyOff);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_standbyOn;
        public int AnimationId_v_standbyOn
        {
            get { return _AnimationId_v_standbyOn; }
            set
            {
                _AnimationId_v_standbyOn = value;
                RaisePropertyChanged(() => AnimationId_v_standbyOn);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_muteOff;
        public int AnimationId_v_muteOff
        {
            get { return _AnimationId_v_muteOff; }
            set
            {
                _AnimationId_v_muteOff = value;
                RaisePropertyChanged(() => AnimationId_v_muteOff);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_muteOn;
        public int AnimationId_v_muteOn
        {
            get { return _AnimationId_v_muteOn; }
            set
            {
                _AnimationId_v_muteOn = value;
                RaisePropertyChanged(() => AnimationId_v_muteOn);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_inputChange;
        public int AnimationId_v_inputChange
        {
            get { return _AnimationId_v_inputChange; }
            set
            {
                _AnimationId_v_inputChange = value;
                RaisePropertyChanged(() => AnimationId_v_inputChange);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_triggerChange;
        public int AnimationId_v_triggerChange
        {
            get { return _AnimationId_v_triggerChange; }
            set
            {
                _AnimationId_v_triggerChange = value;
                RaisePropertyChanged(() => AnimationId_v_triggerChange);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_isInStandby;
        public int AnimationId_v_isInStandby
        {
            get { return _AnimationId_v_isInStandby; }
            set
            {
                _AnimationId_v_isInStandby = value;
                RaisePropertyChanged(() => AnimationId_v_isInStandby);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_isInPower_on;
        public int AnimationId_v_isInPower_on
        {
            get { return _AnimationId_v_isInPower_on; }
            set
            {
                _AnimationId_v_isInPower_on = value;
                RaisePropertyChanged(() => AnimationId_v_isInPower_on);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_isInMute;
        public int AnimationId_v_isInMute
        {
            get { return _AnimationId_v_isInMute; }
            set
            {
                _AnimationId_v_isInMute = value;
                RaisePropertyChanged(() => AnimationId_v_isInMute);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_screenChange;
        public int AnimationId_v_screenChange
        {
            get { return _AnimationId_v_screenChange; }
            set
            {
                _AnimationId_v_screenChange = value;
                RaisePropertyChanged(() => AnimationId_v_screenChange);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_actionExecuted;
        public int AnimationId_v_actionExecuted
        {
            get { return _AnimationId_v_actionExecuted; }
            set
            {
                _AnimationId_v_actionExecuted = value;
                RaisePropertyChanged(() => AnimationId_v_actionExecuted);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_settingsupdated;
        public int AnimationId_v_settingsupdated
        {
            get { return _AnimationId_v_settingsupdated; }
            set
            {
                _AnimationId_v_settingsupdated = value;
                RaisePropertyChanged(() => AnimationId_v_settingsupdated);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_v_volumechanged;
        public int AnimationId_v_volumechanged
        {
            get { return _AnimationId_v_volumechanged; }
            set
            {
                _AnimationId_v_volumechanged = value;
                RaisePropertyChanged(() => AnimationId_v_volumechanged);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion
       
        #region Animation Ids Rear

        private int _AnimationId_r_standbyOff;
        public int AnimationId_r_standbyOff
        {
            get { return _AnimationId_r_standbyOff; }
            set
            {
                _AnimationId_r_standbyOff = value;
                RaisePropertyChanged(() => AnimationId_r_standbyOff);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_standbyOn;
        public int AnimationId_r_standbyOn
        {
            get { return _AnimationId_r_standbyOn; }
            set
            {
                _AnimationId_r_standbyOn = value;
                RaisePropertyChanged(() => AnimationId_r_standbyOn);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_muteOff;
        public int AnimationId_r_muteOff
        {
            get { return _AnimationId_r_muteOff; }
            set
            {
                _AnimationId_r_muteOff = value;
                RaisePropertyChanged(() => AnimationId_r_muteOff);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_muteOn;
        public int AnimationId_r_muteOn
        {
            get { return _AnimationId_r_muteOn; }
            set
            {
                _AnimationId_r_muteOn = value;
                RaisePropertyChanged(() => AnimationId_r_muteOn);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_inputChange;
        public int AnimationId_r_inputChange
        {
            get { return _AnimationId_r_inputChange; }
            set
            {
                _AnimationId_r_inputChange = value;
                RaisePropertyChanged(() => AnimationId_r_inputChange);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_triggerChange;
        public int AnimationId_r_triggerChange
        {
            get { return _AnimationId_r_triggerChange; }
            set
            {
                _AnimationId_r_triggerChange = value;
                RaisePropertyChanged(() => AnimationId_r_triggerChange);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_isInStandby;
        public int AnimationId_r_isInStandby
        {
            get { return _AnimationId_r_isInStandby; }
            set
            {
                _AnimationId_r_isInStandby = value;
                RaisePropertyChanged(() => AnimationId_r_isInStandby);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_isInPower_on;
        public int AnimationId_r_isInPower_on
        {
            get { return _AnimationId_r_isInPower_on; }
            set
            {
                _AnimationId_r_isInPower_on = value;
                RaisePropertyChanged(() => AnimationId_r_isInPower_on);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_isInMute;
        public int AnimationId_r_isInMute
        {
            get { return _AnimationId_r_isInMute; }
            set
            {
                _AnimationId_r_isInMute = value;
                RaisePropertyChanged(() => AnimationId_r_isInMute);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_screenChange;
        public int AnimationId_r_screenChange
        {
            get { return _AnimationId_r_screenChange; }
            set
            {
                _AnimationId_r_screenChange = value;
                RaisePropertyChanged(() => AnimationId_r_screenChange);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_actionExecuted;
        public int AnimationId_r_actionExecuted
        {
            get { return _AnimationId_r_actionExecuted; }
            set
            {
                _AnimationId_r_actionExecuted = value;
                RaisePropertyChanged(() => AnimationId_r_actionExecuted);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_settingsupdated;
        public int AnimationId_r_settingsupdated
        {
            get { return _AnimationId_r_settingsupdated; }
            set
            {
                _AnimationId_r_settingsupdated = value;
                RaisePropertyChanged(() => AnimationId_r_settingsupdated);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        private int _AnimationId_r_volumechanged;
        public int AnimationId_r_volumechanged
        {
            get { return _AnimationId_r_volumechanged; }
            set
            {
                _AnimationId_r_volumechanged = value;
                RaisePropertyChanged(() => AnimationId_r_volumechanged);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        #endregion

        #region Animations Delays Volume

        private int _AnimationStateDelay_v0;
        public int AnimationStateDelay_v0
        {
            get { return _AnimationStateDelay_v0; }
            set
            {
                _AnimationStateDelay_v0 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v0);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v1;
        public int AnimationStateDelay_v1
        {
            get { return _AnimationStateDelay_v1; }
            set
            {
                _AnimationStateDelay_v1 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v1);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v2;
        public int AnimationStateDelay_v2
        {
            get { return _AnimationStateDelay_v2; }
            set
            {
                _AnimationStateDelay_v2 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v2);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v3;
        public int AnimationStateDelay_v3
        {
            get { return _AnimationStateDelay_v3; }
            set
            {
                _AnimationStateDelay_v3 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v3);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v4;
        public int AnimationStateDelay_v4
        {
            get { return _AnimationStateDelay_v4; }
            set
            {
                _AnimationStateDelay_v4 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v4);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v5;
        public int AnimationStateDelay_v5
        {
            get { return _AnimationStateDelay_v5; }
            set
            {
                _AnimationStateDelay_v5 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v5);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v6;
        public int AnimationStateDelay_v6
        {
            get { return _AnimationStateDelay_v6; }
            set
            {
                _AnimationStateDelay_v6 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v6);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v7;
        public int AnimationStateDelay_v7
        {
            get { return _AnimationStateDelay_v7; }
            set
            {
                _AnimationStateDelay_v7 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v7);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v8;
        public int AnimationStateDelay_v8
        {
            get { return _AnimationStateDelay_v8; }
            set
            {
                _AnimationStateDelay_v8 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v8);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v9;
        public int AnimationStateDelay_v9
        {
            get { return _AnimationStateDelay_v9; }
            set
            {
                _AnimationStateDelay_v9 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v9);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v10;
        public int AnimationStateDelay_v10
        {
            get { return _AnimationStateDelay_v10; }
            set
            {
                _AnimationStateDelay_v10 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v10);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v11;
        public int AnimationStateDelay_v11
        {
            get { return _AnimationStateDelay_v11; }
            set
            {
                _AnimationStateDelay_v11 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v11);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v12;
        public int AnimationStateDelay_v12
        {
            get { return _AnimationStateDelay_v12; }
            set
            {
                _AnimationStateDelay_v12 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v12);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_v13;
        public int AnimationStateDelay_v13
        {
            get { return _AnimationStateDelay_v13; }
            set
            {
                _AnimationStateDelay_v13 = value;
                RaisePropertyChanged(() => AnimationStateDelay_v13);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        #endregion

        #region Animations Delays Rear

        private int _AnimationStateDelay_r0;
        public int AnimationStateDelay_r0
        {
            get { return _AnimationStateDelay_r0; }
            set
            {
                _AnimationStateDelay_r0 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r0);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r1;
        public int AnimationStateDelay_r1
        {
            get { return _AnimationStateDelay_r1; }
            set
            {
                _AnimationStateDelay_r1 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r1);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r2;
        public int AnimationStateDelay_r2
        {
            get { return _AnimationStateDelay_r2; }
            set
            {
                _AnimationStateDelay_r2 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r2);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r3;
        public int AnimationStateDelay_r3
        {
            get { return _AnimationStateDelay_r3; }
            set
            {
                _AnimationStateDelay_r3 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r3);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r4;
        public int AnimationStateDelay_r4
        {
            get { return _AnimationStateDelay_r4; }
            set
            {
                _AnimationStateDelay_r4 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r4);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r5;
        public int AnimationStateDelay_r5
        {
            get { return _AnimationStateDelay_r5; }
            set
            {
                _AnimationStateDelay_r5 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r5);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r6;
        public int AnimationStateDelay_r6
        {
            get { return _AnimationStateDelay_r6; }
            set
            {
                _AnimationStateDelay_r6 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r6);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r7;
        public int AnimationStateDelay_r7
        {
            get { return _AnimationStateDelay_r7; }
            set
            {
                _AnimationStateDelay_r7 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r7);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r8;
        public int AnimationStateDelay_r8
        {
            get { return _AnimationStateDelay_r8; }
            set
            {
                _AnimationStateDelay_r8 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r8);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r9;
        public int AnimationStateDelay_r9
        {
            get { return _AnimationStateDelay_r9; }
            set
            {
                _AnimationStateDelay_r9 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r9);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r10;
        public int AnimationStateDelay_r10
        {
            get { return _AnimationStateDelay_r10; }
            set
            {
                _AnimationStateDelay_r10 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r10);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r11;
        public int AnimationStateDelay_r11
        {
            get { return _AnimationStateDelay_r11; }
            set
            {
                _AnimationStateDelay_r11 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r11);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r12;
        public int AnimationStateDelay_r12
        {
            get { return _AnimationStateDelay_r12; }
            set
            {
                _AnimationStateDelay_r12 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r12);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateDelay_r13;
        public int AnimationStateDelay_r13
        {
            get { return _AnimationStateDelay_r13; }
            set
            {
                _AnimationStateDelay_r13 = value;
                RaisePropertyChanged(() => AnimationStateDelay_r13);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        #endregion

        #region Animations Repeat Volume
        private int _AnimationStateRepeat_v0;
        public int AnimationStateRepeat_v0
        {
            get { return _AnimationStateRepeat_v0; }
            set
            {
                _AnimationStateRepeat_v0 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v0);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v1;
        public int AnimationStateRepeat_v1
        {
            get { return _AnimationStateRepeat_v1; }
            set
            {
                _AnimationStateRepeat_v1 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v1);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v2;
        public int AnimationStateRepeat_v2
        {
            get { return _AnimationStateRepeat_v2; }
            set
            {
                _AnimationStateRepeat_v2 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v2);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v3;
        public int AnimationStateRepeat_v3
        {
            get { return _AnimationStateRepeat_v3; }
            set
            {
                _AnimationStateRepeat_v3 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v3);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v4;
        public int AnimationStateRepeat_v4
        {
            get { return _AnimationStateRepeat_v4; }
            set
            {
                _AnimationStateRepeat_v4 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v4);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v5;
        public int AnimationStateRepeat_v5
        {
            get { return _AnimationStateRepeat_v5; }
            set
            {
                _AnimationStateRepeat_v5 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v5);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v6;
        public int AnimationStateRepeat_v6
        {
            get { return _AnimationStateRepeat_v6; }
            set
            {
                _AnimationStateRepeat_v6 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v6);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v7;
        public int AnimationStateRepeat_v7
        {
            get { return _AnimationStateRepeat_v7; }
            set
            {
                _AnimationStateRepeat_v7 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v7);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v8;
        public int AnimationStateRepeat_v8
        {
            get { return _AnimationStateRepeat_v8; }
            set
            {
                _AnimationStateRepeat_v8 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v8);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v9;
        public int AnimationStateRepeat_v9
        {
            get { return _AnimationStateRepeat_v9; }
            set
            {
                _AnimationStateRepeat_v9 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v9);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v10;
        public int AnimationStateRepeat_v10
        {
            get { return _AnimationStateRepeat_v10; }
            set
            {
                _AnimationStateRepeat_v10 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v10);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v11;
        public int AnimationStateRepeat_v11
        {
            get { return _AnimationStateRepeat_v11; }
            set
            {
                _AnimationStateRepeat_v11 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v11);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v12;
        public int AnimationStateRepeat_v12
        {
            get { return _AnimationStateRepeat_v12; }
            set
            {
                _AnimationStateRepeat_v12 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v12);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_v13;
        public int AnimationStateRepeat_v13
        {
            get { return _AnimationStateRepeat_v13; }
            set
            {
                _AnimationStateRepeat_v13 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_v13);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }


        #endregion

        #region Animations Repeat Rear

        private int _AnimationStateRepeat_r0;
        public int AnimationStateRepeat_r0
        {
            get { return _AnimationStateRepeat_r0; }
            set
            {
                _AnimationStateRepeat_r0 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r0);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r1;
        public int AnimationStateRepeat_r1
        {
            get { return _AnimationStateRepeat_r1; }
            set
            {
                _AnimationStateRepeat_r1 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r1);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r2;
        public int AnimationStateRepeat_r2
        {
            get { return _AnimationStateRepeat_r2; }
            set
            {
                _AnimationStateRepeat_r2 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r2);
                foreach (var l in LedAnimations)
                    RaisePropertyChanged(() => l.AnimationStateRepeat_rValue);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r3;
        public int AnimationStateRepeat_r3
        {
            get { return _AnimationStateRepeat_r3; }
            set
            {
                _AnimationStateRepeat_r3 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r3);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r4;
        public int AnimationStateRepeat_r4
        {
            get { return _AnimationStateRepeat_r4; }
            set
            {
                _AnimationStateRepeat_r4 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r4);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r5;
        public int AnimationStateRepeat_r5
        {
            get { return _AnimationStateRepeat_r5; }
            set
            {
                _AnimationStateRepeat_r5 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r5);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r6;
        public int AnimationStateRepeat_r6
        {
            get { return _AnimationStateRepeat_r6; }
            set
            {
                _AnimationStateRepeat_r6 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r6);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r7;
        public int AnimationStateRepeat_r7
        {
            get { return _AnimationStateRepeat_r7; }
            set
            {
                _AnimationStateRepeat_r7 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r7);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r8;
        public int AnimationStateRepeat_r8
        {
            get { return _AnimationStateRepeat_r8; }
            set
            {
                _AnimationStateRepeat_r8 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r8);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r9;
        public int AnimationStateRepeat_r9
        {
            get { return _AnimationStateRepeat_r9; }
            set
            {
                _AnimationStateRepeat_r9 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r9);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r10;
        public int AnimationStateRepeat_r10
        {
            get { return _AnimationStateRepeat_r10; }
            set
            {
                _AnimationStateRepeat_r10 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r10);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r11;
        public int AnimationStateRepeat_r11
        {
            get { return _AnimationStateRepeat_r11; }
            set
            {
                _AnimationStateRepeat_r11 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r11);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r12;
        public int AnimationStateRepeat_r12
        {
            get { return _AnimationStateRepeat_r12; }
            set
            {
                _AnimationStateRepeat_r12 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r12);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        private int _AnimationStateRepeat_r13;
        public int AnimationStateRepeat_r13
        {
            get { return _AnimationStateRepeat_r13; }
            set
            {
                _AnimationStateRepeat_r13 = value;
                RaisePropertyChanged(() => AnimationStateRepeat_r13);
                RaisePropertyChanged(() => LedAnimations);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }

        #endregion
        */
        #region Animations

        List<LedAnimationParameterSet> _LedAnimations = new List<LedAnimationParameterSet>()
                {
                    new LedAnimationParameterSet(1),
                    new LedAnimationParameterSet(2),
                    new LedAnimationParameterSet(3),
                    new LedAnimationParameterSet(4),
                    new LedAnimationParameterSet(5),
                    new LedAnimationParameterSet(6),
                    new LedAnimationParameterSet(7),
                    new LedAnimationParameterSet(8),
                    new LedAnimationParameterSet(9),
                    new LedAnimationParameterSet(10),
                    new LedAnimationParameterSet(11),
                    new LedAnimationParameterSet(12),
                    new LedAnimationParameterSet(13),
                };
        public BindingList<LedAnimationParameterSet> LedAnimations
        {
            get
            {
                return new BindingList<LedAnimationParameterSet>(_LedAnimations);
            }
        }

        #endregion

        #region Inputs
        List<InputParameterSet> _Inputs = new List<InputParameterSet>()
        {
            new InputParameterSet(0),
            new InputParameterSet(1),
            new InputParameterSet(2),
            new InputParameterSet(3),
            new InputParameterSet(4),
            new InputParameterSet(5)            
        };
        public BindingList<InputParameterSet> Inputs
        {
            get
            {
                return new BindingList<InputParameterSet>(_Inputs);
            }
            set
            {
                return;
            }
        }

        public bool UseAtLeastOneInput
        {
            get
            {
                return (UsedInputCount > 1);
            }
        }
        public int UsedInputCount
        {
            get
            {
                var c = 0;

                foreach (var p in _Inputs)
                    if (p.UseInput) c++;

                return c;
            }
        }

        public bool IsInputUsed(int input)
        {
            return _Inputs[input - 1].UseInput;
        }

        #endregion

        #endregion
    }
}
