using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.Ports;
using System.Timers;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;
using UGS.Helpers;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Windows;

namespace UGS.Models
{
    public class UGS : NotificationObject
    {
        // public static UGS ugs;

        public static string ConvertAttenuationToDb(int? Volume)
        {
            if (Volume == null) return null;

            double v;
            if (Volume == 0) v = -120;
            else v = ((double)(Volume - VolumeSteps)) / 2;

            return string.Format("{0:00.0} dB", v);
        }

        public static string ConvertValueToDb(int? Value)
        {
            if (Value == null) return null;

            double v;
            if (Value == 0) v = 0;
            else v = ((double)(Value)) / 2;

            return string.Format("{0:00.0} dB", v);
        }

        public static int VolumeSteps { get { return 224; } }


        public UGS()
        {
            var SerialPorts = System.IO.Ports.SerialPort.GetPortNames().ToList();
            if (Properties.Settings.Default.ComPort == "")
                Properties.Settings.Default.ComPort = SerialPorts.OrderBy(x => int.Parse(x.Replace("COM", ""))).First();

            SerialPortName = Properties.Settings.Default.ComPort;
        }

        ~UGS()
        {
            CloseSerial();
            Properties.Settings.Default.Save();
        }



        public void SetValue(string Key, string value)
        {
            if (!UgsIsReadForNewCommands) return;
            if (!IsPortOpen) OpenSerialPort(SerialPortName);
            lock (SerialPortName)
            {
                UgsIsReadForNewCommands = false;
                this.SerialLog += "Setting " + Key + " to " + value + "\n";
                _serialPort.WriteLine(Key + "=" + value);
            }
        }
        public void SetValue(string Key, int value)
        {
            SetValue(Key, value.ToString());
        }
        public void SetValue(string Key, bool value)
        {
            SetValue(Key, (value ? "1" : "0"));
        }
        private void SendCommand(string Key)
        {
            if (IsPortOpen)
            {
                lock (SerialPortName)
                {
                    _serialPort.WriteLine(Key);
                    this.SerialLog += "request --> " + Key + "=?\n";
                }
            }
        }

        public void Reset()
        {
            SendCommand("RESET");
        }
        public void GetStatus()
        {
            SendCommand("GET_STATUS");
        }
        public void GetPresets()
        {
            SendCommand("GET_PRESETS");
        }
        public void GetAllPresets()
        {
            SendCommand("GET_ALL_PRESETS");
        }
        public void ResetFactoryCurrentPreset()
        {
            SendCommand("CUR_FACTORY");
        }
        public void ResetFactoryAllPresets()
        {
            SendCommand("ALL_FACTORY");
        }
        public void GetIcons()
        {
            SendCommand("GET_ICONS");
        }
        public void SetPreset(int num)
        {
            SetValue("PRESET", num);
            GetIcons();
        }
        public void ResetIcons()
        {
            SendCommand("RST_ICONS");
        }

        private int? _illumTft;
        public int? IllumTft
        {
            get
            {
                return _illumTft;
            }
            set
            {
                if (this._illumTft != value && value != null)
                {
                    this._illumTft = value;
                    if (!ignoreNextPreampSet_IllumTft)
                    {
                        if (value != null)
                            SetValue("ILLUM_TFT", (int)value);
                    }
                    else
                        ignoreNextPreampSet_IllumTft = false;

                    RaisePropertyChanged(() => IllumTft);

                }


            }
        }

        private int? _illumVol1;
        public int? IllumVol1
        {
            get
            {
                return _illumVol1;
            }
            set
            {
                if (this._illumVol1 != value && value != null)
                {
                    this._illumVol1 = value;
                    if (!ignoreNextPreampSet_IllumVol1)
                    {
                        if (value != null)
                            SetValue("ILLUM_VOL1", (int)value);
                    }
                    else
                        ignoreNextPreampSet_IllumVol1 = false;

                    RaisePropertyChanged(() => IllumVol1);

                }


            }
        }

        private int? _illumVol2;
        public int? IllumVol2
        {
            get
            {
                return _illumVol2;
            }
            set
            {
                if (this._illumVol2 != value && value != null)
                {
                    this._illumVol2 = value;
                    if (!ignoreNextPreampSet_IllumVol2)
                    {
                        if (value != null)
                            SetValue("ILLUM_VOL2", (int)value);
                    }
                    else
                        ignoreNextPreampSet_IllumVol2 = false;

                    RaisePropertyChanged(() => IllumVol2);

                }


            }
        }

        private int? _illumRly1;
        public int? IllumRly1
        {
            get
            {
                return _illumRly1;
            }
            set
            {
                if (this._illumRly1 != value && value != null)
                {
                    this._illumRly1 = value;
                    if (!ignoreNextPreampSet_IllumRly1)
                    {
                        if (value != null)
                            SetValue("ILLUM_RLY1", (int)value);
                    }
                    else
                        ignoreNextPreampSet_IllumRly1 = false;

                    RaisePropertyChanged(() => IllumRly1);

                }


            }
        }

        private int? _illumRly2;
        public int? IllumRly2
        {
            get
            {
                return _illumRly2;
            }
            set
            {
                if (this._illumRly2 != value && value != null)
                {
                    this._illumRly2 = value;
                    if (!ignoreNextPreampSet_IllumRly2)
                    {
                        if (value != null)
                            SetValue("ILLUM_RLY2", (int)value);
                    }
                    else
                        ignoreNextPreampSet_IllumRly2 = false;

                    RaisePropertyChanged(() => IllumRly2);

                }


            }
        }

        private int? _volume;
        public int? Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (this._volume != value && value != null && value >= 0 && value <= VolumeSteps)
                {
                    this._volume = value;
                    if (!ignoreNextPreampSet_Volume)
                    {
                        SetValue("VOLUME", (int)value);
                    }
                    else
                        ignoreNextPreampSet_Volume = false;

                    RaisePropertyChanged(() => Volume);
                    RaisePropertyChanged(() => VolumeDb);
                }
            }
        }

        public string VolumeDb
        {
            get
            {
                return ConvertAttenuationToDb(this.Volume);
            }
        }

        private int? _input;
        public int? Input
        {
            get
            {
                return _input;
            }
            set
            {
                if (this._input != value && value != null)
                {
                    this._input = value;
                    SetValue("INPUT", (int)value);

                    RaisePropertyChanged(() => Input);
                }
                if (CurrentPreset != null)
                    foreach (var p in CurrentPreset.Inputs)
                        p.InputIsActive = ((p.InputNumber) == value);
            }
        }


        public void UseNextInput()
        {
            for (var i = 1; i <= 6; i++)
            {
                var nextInput = (int)Input + i;
                if (nextInput > 5) nextInput = 0;
                if (nextInput < 0) nextInput = 5;

                if (CurrentPreset.IsInputUsed(nextInput + 1))
                {
                    Input = nextInput;
                    return;
                }
            }
        }
        public void UsePreviousInput()
        {
            for (var i = 1; i <= 6; i++)
            {
                var prevInput = (int)Input - i;
                if (prevInput > 5) prevInput = 0;
                if (prevInput < 0) prevInput = 5;

                if (CurrentPreset.IsInputUsed(prevInput + 1))
                {
                    Input = prevInput;
                    return;
                }
            }
        }

        private bool _standby;
        public bool Standby
        {
            get
            {
                return _standby;
            }
            set
            {
                if (this._standby != value)
                {
                    SetValue("STANDBY", value);
                    this._standby = value;
                    RaisePropertyChanged(() => Standby);

                }
            }
        }

        private string _page;
        public string Page
        {
            get
            {
                return _page;
            }
            set
            {
                if (this._page != value)
                {
                    SetValue("PAGE", value);
                    this._page = value;
                    RaisePropertyChanged(() => Page);

                }
            }
        }

        private bool _mute;
        public bool Mute
        {
            get
            {
                return _mute;
            }
            set
            {
                if (this._mute != value)
                {
                    SetValue("MUTE", value);
                    this._mute = value;
                    RaisePropertyChanged(() => Mute);
                }
            }
        }



        private bool _Bypass;
        public bool Bypass
        {
            get
            {
                return _Bypass;
            }
            set
            {
                if (this._Bypass != value)
                {
                    SetValue("BYPASS", value);
                    this._Bypass = value;
                    RaisePropertyChanged(() => Bypass);
                }
            }
        }



        private bool _OutAssym;
        public bool OutAssym
        {
            get
            {
                return _OutAssym;
            }
            set
            {
                if (this._OutAssym != value)
                {
                    SetValue("OUT_ASYM", value);
                    this._OutAssym = value;
                    RaisePropertyChanged(() => OutAssym);
                }
            }
        }



        private bool _ExtOut;
        public bool ExtOut
        {
            get
            {
                return _ExtOut;
            }
            set
            {
                if (this._ExtOut != value)
                {
                    SetValue("EXTOUT", value);
                    this._ExtOut = value;
                    RaisePropertyChanged(() => ExtOut);
                }
            }
        }



        private int _NbRlyBoards;
        public int NbRlyBoards
        {
            get
            {
                return _NbRlyBoards;
            }
            private set
            {
                if (this._NbRlyBoards != value)
                {
                    this._NbRlyBoards = value;
                    RaisePropertyChanged(() => NbRlyBoards);
                }
            }
        }

        private int _NbVolBoards;
        public int NbVolBoards
        {
            get
            {
                return _NbVolBoards;
            }
            private set
            {
                if (this._NbVolBoards != value)
                {
                    this._NbVolBoards = value;
                    RaisePropertyChanged(() => NbVolBoards);
                }
            }
        }

        private float _LoopTime;
        public float LoopTime
        {
            get
            {
                return _LoopTime;
            }
            private set
            {
                if (this._LoopTime != value)
                {
                    this._LoopTime = value;
                    RaisePropertyChanged(() => LoopTime);
                }
            }
        }


        private bool _DacSrc;
        public bool DacSrc
        {
            get
            {
                return _DacSrc;
            }
            set
            {
                if (this._DacSrc != value)
                {
                    SetValue("DACSRC", value);
                    this._DacSrc = value;
                    RaisePropertyChanged(() => DacSrc);
                }
            }
        }
        private string _DacStatus;
        public string DacStatus
        {
            get
            {
                return _DacStatus;
            }
            private set
            {
                _DacStatus = value;
                RaisePropertyChanged(() => DacStatus);
            }
        }


        private Preset _CurrentPreset;
        public Preset CurrentPreset
        {
            get { return _CurrentPreset; }
            set
            {
                _CurrentPreset = value;
                RaisePropertyChanged(() => CurrentPreset);
            }
        }

        private BindingList<Icon> _Icons;
        public BindingList<Icon> Icons
        {
            get
            {
                if (Icon.Icons == null && _Icons == null) return null;
                if (_Icons == null)
                    _Icons = new BindingList<Icon>(Icon.Icons.Values.Select(x => x.Item1).ToList());
                return _Icons;
            }
            set
            {
                _Icons = value;
                RaisePropertyChanged(() => Icons);
            }
        }

        public void ReloadAllIcons()
        {
            if (CurrentPreset != null)
            {
                /*
                Icon.SetColors(
                    (Color)(new Converters.RGB565ToColorConverter().Convert(CurrentPreset.FrontColor, null, null, null)),
                    Colors.Transparent,
                    (Color)(new Converters.RGB565ToColorConverter().Convert(CurrentPreset.SelectedFrontColor, null, null, null))
                );*/
                Icon.RefreshAllIcons();
            }

            RaisePropertyChanged(() => Icons);
        }

        private string _serialLog;
        public string SerialLog
        {
            get
            {
                return _serialLog;
            }
            set
            {
                if (this._serialLog != value)
                {
                    this._serialLog = value;
                    // only keep last 200 lines
                    var nbLines = _serialLog.Split('\n').Count();
                    var i = 0;
                    if (nbLines > 200)
                        _serialLog = String.Join("\n", _serialLog.Split('\n').ToDictionary(x => i++, x => x).Where(x => x.Key > nbLines - 200).Select(x => x.Value));

                    RaisePropertyChanged(() => SerialLog);
                }
            }
        }

        private SerialPort _serialPort;
        private string _serialPortName;
        public string SerialPortName
        {
            get { return _serialPortName; }
            set
            {
                if (this._serialPortName != value)
                {
                    Properties.Settings.Default.ComPort = value;
                    _serialPortName = value;
                    OpenSerial(value);
                    RaisePropertyChanged(() => SerialPortName);
                    RaisePropertyChanged(() => IsPortOpen);
                }
            }
        }

        public bool IsPortOpen
        {
            get { return _serialPort != null && _serialPort.IsOpen; }
            set
            {
                if (value)
                    OpenSerial(SerialPortName);
                else
                    CloseSerial();
            }
        }

        public void OpenSerial(string portName)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            if (OpenSerialPort(portName))
                if (ReadUgsParameters())
                    return;

            _serialPort.Close();
            RaisePropertyChanged(() => IsPortOpen);
        }
        private bool OpenSerialPort(string portName)
        {
            if (IsPortOpen && portName == _serialPort.PortName) return true;
            if (IsPortOpen) _serialPort.Close();

            if (!SerialPort.GetPortNames().Contains(portName)) return false;

            // 8 data bits, no parity, one stop bit
            _serialPort = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
            _serialPort.ReadTimeout = 2000;
            _serialPort.WriteTimeout = 2000;
            _serialPort.NewLine = "\r";
            _serialPort.Encoding = Encoding.GetEncoding("ASCII");
            _serialPort.ReadBufferSize = 32768;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            try
            {
                _serialPort.Open();
                RaisePropertyChanged(() => IsPortOpen);
                SendCommand("");
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("COM port error: " + ex.Message);
                return false;
            }

        }

        private bool ReadUgsParameters()
        {
            try
            {
                var startReadUgsParameters = DateTime.Now;

                // GetPresets();
                GetStatus();

                while (CurrentPreset == null || CurrentPreset.IsLoaded == false)
                {
                    System.Threading.Thread.Sleep(50);
                    if (startReadUgsParameters < DateTime.Now.AddSeconds(-2))
                    {
                        // timeout
                        throw new Exception("Communication timeout");
                    }
                }

                GetIcons();
                while (Icons == null) System.Threading.Thread.Sleep(50);

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("UGS error: " + ex.Message);
                return false;
            }
        }

        public void CloseSerial()
        {
            if (_serialPort != null)
            {
                _serialPort.Close();
                RaisePropertyChanged(() => IsPortOpen);
            }
        }

        [STAThread]
        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars && _serialPort.IsOpen)
            {
                try
                {
                    lock (_serialPort)
                    {
                        while (_serialPort.BytesToRead > 1) // 1 instead of 0 because of timeouts on single \n character
                        {
                            IEnumerable<string> rs;
                            rs = _serialPort.ReadLine().Split('\n').Select(x => x.Trim()).Where(x => x != "");

                            foreach (var r in rs)
                            {
                                ProcessRxData(r);
                            }
                        }
                    }
                }
                catch (TimeoutException)
                {
                    var eg = _serialPort.ReadExisting();
                    Console.WriteLine(eg);
                }
                catch (System.IO.IOException ex)
                {
                }
                catch (Exception ex)
                {
                }
            }
        }

        bool ignoreNextPreampSet_Volume = false;
        bool ignoreNextPreampSet_IllumTft = false;
        bool ignoreNextPreampSet_IllumVol1 = false;
        bool ignoreNextPreampSet_IllumVol2 = false;
        bool ignoreNextPreampSet_IllumRly1 = false;
        bool ignoreNextPreampSet_IllumRly2 = false;

        private void ProcessRxData(string d)
        {
            if (d.Trim() == "") return;

            if (d.StartsWith("RX:")) { }
            else if (d.StartsWith("REM:")) { }
            else
            {
                var kv = GetKeyValue(d);

                if (kv.Key != "" && kv.Value != "?")
                {

                    if (kv.Key == "VOLUME")
                    {
                        ignoreNextPreampSet_Volume = true;
                        Volume = int.Parse(kv.Value);
                    }
                    else if (kv.Key == "STANDBY")
                    {
                        _standby = (int.Parse(kv.Value) == 1);
                        RaisePropertyChanged(() => Standby);
                    }
                    else if (kv.Key == "RIAA_PWR") { /* = int.Parse(kv.Value);  */ }
                    else if (kv.Key == "5V_PWR") { /* = int.Parse(kv.Value);  */ }
                    else if (kv.Key == "MUTE")
                    {
                        _mute = (int.Parse(kv.Value) == 1);
                        RaisePropertyChanged(() => Mute);
                    }
                    else if (kv.Key == "BALANCE") { /* = int.Parse(kv.Value);  */ }
                    else if (kv.Key.StartsWith("OFFSET")) { /* = int.Parse(kv.Value);  */ }
                    else if (kv.Key == "INPUT")
                    {
                        Input = int.Parse(kv.Value);
                    }
                    else if (kv.Key == "BYPASS")
                    {
                        _Bypass = (int.Parse(kv.Value) == 1);
                        RaisePropertyChanged(() => Bypass);
                    }
                    else if (kv.Key == "OUT_ASYM")
                    {
                        _OutAssym = (int.Parse(kv.Value) == 1);
                        RaisePropertyChanged(() => OutAssym);
                    }
                    else if (kv.Key == "EXTOUT")
                    {
                        _ExtOut = (int.Parse(kv.Value) == 1);
                        RaisePropertyChanged(() => ExtOut);
                    }
                    else if (kv.Key == "ILLUM_TFT")
                    {
                        ignoreNextPreampSet_IllumTft = true;
                        IllumTft = int.Parse(kv.Value);
                    }
                    else if (kv.Key == "ILLUM_VOL1")
                    {
                        ignoreNextPreampSet_IllumVol1 = true;
                        IllumVol1 = int.Parse(kv.Value);
                    }
                    else if (kv.Key == "ILLUM_VOL2")
                    {
                        ignoreNextPreampSet_IllumVol2 = true;
                        IllumVol2 = int.Parse(kv.Value);
                    }
                    else if (kv.Key == "ILLUM_RLY1")
                    {
                        ignoreNextPreampSet_IllumRly1 = true;
                        IllumRly1 = int.Parse(kv.Value);
                    }
                    else if (kv.Key == "ILLUM_RLY2")
                    {
                        ignoreNextPreampSet_IllumRly2 = true;
                        IllumRly2 = int.Parse(kv.Value);
                    }
                    else if (kv.Key == "GET_RLYBRD")
                    {
                        _NbRlyBoards = (int.Parse(kv.Value));
                        RaisePropertyChanged(() => NbRlyBoards);
                    }
                    else if (kv.Key == "GET_VOLBRD")
                    {
                        _NbVolBoards = (int.Parse(kv.Value));
                        RaisePropertyChanged(() => NbVolBoards);
                    }
                    else if (kv.Key == "GET_LOOPTIME")
                    {
                        _LoopTime = (int.Parse(kv.Value));
                        RaisePropertyChanged(() => LoopTime);
                    }
                    else if (kv.Key == "DACSRC")
                    {
                        DacSrc = (int.Parse(kv.Value) == 1);
                    }
                    else if (kv.Key == "DACSTATUS")
                    {
                        DacStatus = kv.Value;
                    }
                    else if (kv.Key.StartsWith("EEPROM"))
                    {
                        /* = int.Parse(kv.Value);  */
                        string str = kv.Value;
                        int num = Preset.GetNumberFromSerialString(kv.Key, Preset.kwe2p);

                        if (CurrentPreset == null)
                            CurrentPreset = new Preset();

                        CurrentPreset.Number = num;
                        CurrentPreset.DeserializeData(num, str);

                        // CustomizeTaskBar();
                        // ReloadAllIcons();

                    }
                    else if (kv.Key.StartsWith("ICONS"))
                    {
                        string str = kv.Value;
                        Icon.Icons = Icon.LoadAllFromBigString(str,
                            (Color)(new Converters.RGB565ToColorConverter().Convert(CurrentPreset.FrontColor, null, null, null)),
                            Colors.Transparent,
                            (Color)(new Converters.RGB565ToColorConverter().Convert(CurrentPreset.SelectedFrontColor, null, null, null))
                        );
                        ReloadAllIcons();

                    }

                    else
                    {
                        // throw new Exception("Unknown key: " + kv.Key + " (value = " + kv.Value + ")");
                        d = "Unknown key: " + kv.Key + " (value = " + kv.Value + ")";
                    }
                }

                if (kv.Key != "GET_LOOPTIME")
                {
                    this.SerialLog += d + "\n";
                    RaisePropertyChanged(() => SerialLog);
                }
            }
            if (d.StartsWith("###"))
            {
                UgsIsReadForNewCommands = true;
            }


            // UGS.ugs.Presets.ResetBindings();
            // UGS.ugs.Icons.ResetBindings();

        }

        private bool _cmdAck;
        public bool UgsIsReadForNewCommands
        {
            get
            {
                return _cmdAck;
            }
            set
            {
                _cmdAck = value; RaisePropertyChanged(() => UgsIsReadForNewCommands);
            }
        }

        private KeyValuePair<string, string> GetKeyValue(string d)
        {
            if (d.Contains("="))
                return new KeyValuePair<string, string>(d.Split('=')[0], d.Split('=')[1]);
            else
                return new KeyValuePair<string, string>("", "");
        }

        public void CustomizeTaskBar()
        {
            if (!TaskbarManager.IsPlatformSupported) return;

            var tbButtons = new List<ThumbnailToolBarButton>();

            ThumbnailToolBarButton buttonStdBy = new ThumbnailToolBarButton(IconFromBitmapHelper.GetIconFromBitmap("Images/ic_power_settings_new_black_48dp_2x.png"), "Stand-by");
            ThumbnailToolBarButton buttonVolDown = new ThumbnailToolBarButton(IconFromBitmapHelper.GetIconFromBitmap("Images/ic_volume_down_black_48dp_2x.png"), "Volume down");
            ThumbnailToolBarButton buttonVolUp = new ThumbnailToolBarButton(IconFromBitmapHelper.GetIconFromBitmap("Images/ic_volume_up_black_48dp_2x.png"), "Volume up");
            ThumbnailToolBarButton buttonMute = new ThumbnailToolBarButton(IconFromBitmapHelper.GetIconFromBitmap("Images/ic_volume_off_black_48dp_2x.png"), "Mute");

            tbButtons.AddRange(new List<ThumbnailToolBarButton>() { buttonStdBy, buttonMute, buttonVolDown, buttonVolUp });

            buttonStdBy.Click += new EventHandler<ThumbnailButtonClickedEventArgs>((sender, e) => this.Standby = !this.Standby);
            buttonVolDown.Click += new EventHandler<ThumbnailButtonClickedEventArgs>((sender, e) => this.Volume -= 3);
            buttonVolUp.Click += new EventHandler<ThumbnailButtonClickedEventArgs>((sender, e) => this.Volume += 3);
            buttonMute.Click += new EventHandler<ThumbnailButtonClickedEventArgs>((sender, e) => this.Mute = !this.Mute);

            if (this.CurrentPreset != null)
                if (this.CurrentPreset.UseAtLeastOneInput)
                {
                    ThumbnailToolBarButton buttonPrevSource = new ThumbnailToolBarButton(IconFromBitmapHelper.GetIconFromBitmap("Images/ic_expand_less_black_48dp_2x.png"), "Previous source");
                    ThumbnailToolBarButton buttonNextSource = new ThumbnailToolBarButton(IconFromBitmapHelper.GetIconFromBitmap("Images/ic_expand_more_black_48dp_2x.png"), "Next source");

                    buttonPrevSource.Click += new EventHandler<ThumbnailButtonClickedEventArgs>((sender, e) => this.UsePreviousInput());
                    buttonNextSource.Click += new EventHandler<ThumbnailButtonClickedEventArgs>((sender, e) => this.UseNextInput());

                    tbButtons.AddRange(new List<ThumbnailToolBarButton>() { buttonPrevSource, buttonNextSource });
                }

            ThumbnailToolBarManager manager = TaskbarManager.Instance.ThumbnailToolBars;
            Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager tm = TaskbarManager.Instance;

            manager.AddButtons(new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle, tbButtons.ToArray());
        }
    }
}
