using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UGS.Helpers;
using UGS.Models;
using System.ComponentModel;
using System.Linq;
using System.IO.Ports;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.IO;
using System.Threading;

namespace UGS.ViewModels
{
    public class UGSViewModel : BaseViewModel
    {
        #region Ctor
        public UGSViewModel()
        {
            try
            {
                if (ugs == null)
                {
                    ugs = new Models.UGS();
                    Preset.SetUgs(ugs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Properties        
        private const int NbPresetGroups = 4;

        private static Models.UGS _ugs;
        public Models.UGS ugs
        {
            get { return _ugs; }
            set
            {
                if (_ugs != value)
                {
                    _ugs = value;
                    RaisePropertyChanged(() => ugs);
                }
            }
        }

        public BindingList<string> SerialPorts
        {
            get
            {
                var bl = new BindingList<string>(System.IO.Ports.SerialPort
                    .GetPortNames()
                    .ToList());
                return bl;
            }
        }
        
        private BindingList<Icon> _InputIcons;
        public BindingList<Icon> InputIcons
        {
            get
            {
                if (Icon.Icons == null && _InputIcons == null) return null;
                //  if (_InputIcons == null)
                _InputIcons = new BindingList<Icon>(Icon.Icons
                    .Values
                    .Where(i => i.Item1.IconId >= Ic.ICONS_PC && i.Item1.IconId <= Ic.ICONS_Aux2)
                    .Select(x => x.Item1)
                    .ToList());
                return _InputIcons;
            }
        }
        

        public BindingList<Cl> ColourList
        {
            get
            {
                return new BindingList<Cl>(new List<Cl>() { Cl.IC_BC, Cl.IC_FC });
            }
        }

        public BindingList<Icon> CommandList
        {
            get
            {
                /*
                IC_CM_FRR = 1,// Fill round rectangle 	(5)
                IC_CM_FR = 2,// Fill rectangle			(4)
                IC_CM_FR2 = 3,// Fill rectangle 2		(6)
                IC_CM_FT = 4,// Fill triangle			(6)
                IC_CM_FC = 5,// Fill cirle				(3)
                IC_CM_FCV = 6,// Fill curve				(5)
                IC_CM_CP = 32, // Copy icon				(1)
                IC_CM_NO = 48, // No icon				(0)
                IC_CM_END = 255, // End command
                */
                return new BindingList<Icon>(CommandIcons /*new List<Cm>() { Cm.IC_CM_FRR, Cm.IC_CM_FR, Cm.IC_CM_FR2,
                    Cm.IC_CM_FT, Cm.IC_CM_FC, Cm.IC_CM_FCV, Cm.IC_CM_CP, Cm.IC_CM_NO, Cm.IC_CM_END }*/);
            }
        }

        private static List<Icon> _CommandIcons;
        public static List<Icon> CommandIcons
        {
            get
            {
                if (_CommandIcons == null)
                {
                    _CommandIcons = new List<Icon>();

                    var ic = new Icon();
                    var sp = new Icon.FilledRectangle() { command = Cm.IC_CM_FR };
                    sp.data.Where(x => x.Name == "x").First().Value = 20;
                    sp.data.Where(x => x.Name == "y").First().Value = 20;
                    sp.data.Where(x => x.Name == "width").First().Value = 70;
                    sp.data.Where(x => x.Name == "height").First().Value = 40;

                    ic.Shapes = new BindingList<Icon.IconShape>();
                    ic.Shapes.Add(sp);
                    ic.IconId = Ic.ICONS_Shape;
                    _CommandIcons.Add(ic);
                    /*
                    Icon.SetColors(
                        Colors.Black,
                       // (Color)(new RGB565ToColorConverter().Convert(CurrentPreset.BackColor, null, null, null))
                       Colors.Transparent,
                       Colors.DarkGreen
                    );*/

                }
                return _CommandIcons;
            }
        }

        #endregion

        #region Commands

        public ICommand OpenSerialCommand { get { return new DelegateCommand(OnOpenSerial); } }
        public ICommand ClearSerialLogCommand { get { return new DelegateCommand(OnClearSerialLog); } }
        public ICommand WritePresetCommand { get { return new DelegateCommand(OnWritePreset); } }
        public ICommand ResetFactoryCurrentCommand { get { return new DelegateCommand(OnResetFactoryCurrent); } }
        public ICommand ResetArduinoCommand { get { return new DelegateCommand(OnResetArduino); } }
      //  public ICommand RefreshIconsCommand { get { return new DelegateCommand(OnRefreshIcons); } }
        public ICommand GetIconsCommand { get { return new DelegateCommand(OnGetIcons); } }
        public ICommand EditIconsCommand { get { return new DelegateCommand(OnEditIcons); } }
        public ICommand PreviousPresetCommand { get { return new DelegateCommand(OnPreviousPreset); } }
        public ICommand NextPresetCommand { get { return new DelegateCommand(OnNextPreset); } }
        public ICommand ResetIconsCommand { get { return new DelegateCommand(OnResetIcons); } }
        public ICommand CustomizeTaskBarCommand { get { return new DelegateCommand(OnCustomizeTaskBar); } }
        public ICommand CloseCommand { get { return new DelegateCommand(OnClose); } }
        public ICommand DoNothingCommand { get { return new DelegateCommand(OnDoNothing, CanExecuteDoNothing); } }

        #endregion

        #region Command Handlers
        private void OnOpenSerial()
        {
            try
            {
                ugs.OpenSerial(ugs.SerialPortName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void OnClearSerialLog()
        {
            ugs.SerialLog = "";
        }

        private void OnWritePreset()
        {
            Preset.RequestDelayedSaveSettingsToUgs();
        }

        private void OnResetFactoryCurrent()
        {
            ugs.ResetFactoryCurrentPreset();
            ugs.GetIcons();
        }

        private void OnResetArduino()
        {
            ugs.Reset();
        }

        private void OnGetIcons()
        {
            ugs.GetIcons();
        }
        /*
        private void OnRefreshIcons()
        {
            RaisePropertyChanged(() => InputIcons);
            //    InputIcons = null; // force refresh
        }
        */
        private void OnEditIcons()
        {
            var ic = new IconEditor();

            ic.ShowDialog();
        }

        private void OnPreviousPreset()
        {
            var newP = ugs.CurrentPreset.Number - 1;
            if (newP < 0) newP = NbPresetGroups - 1;
            ugs.SetPreset(newP);
        }

        private void OnNextPreset()
        {
            var newP = ugs.CurrentPreset.Number + 1;
            if (newP > (NbPresetGroups - 1)) newP = 0;
            ugs.SetPreset(newP);
        }
        private void OnResetIcons()
        {
            ugs.ResetIcons();
        }
        private void OnCustomizeTaskBar()
        {
            ugs.CustomizeTaskBar();
        }

        private void OnClose()
        {

        }
        private bool CanExecuteOnClose()
        {
            return true;
        }

        private void OnDoNothing()
        {
        }
        private bool CanExecuteDoNothing()
        {
            return false;
        }

        #endregion





    }
}