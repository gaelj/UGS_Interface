using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGS.Helpers;

namespace UGS.Models
{
    public class InputParameterSet : NotificationObject
    {
        public InputParameterSet() { }
        public InputParameterSet(int inputNumber)
        {
            _InputNumber = inputNumber;
        }

        public int _InputNumber;
        public int InputNumber
        {
            get
            {
                return _InputNumber;
            }
            set
            {
                _InputNumber = value;
                RaisePropertyChanged(() => InputNumber);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _VolumeIn;
        public int VolumeIn
        {
            get
            {
                return _VolumeIn;
            }
            set
            {
                _VolumeIn = value;
                RaisePropertyChanged(() => VolumeIn);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _DefaultVolumeIn;
        public int DefaultVolumeIn
        {
            get
            {
                return _DefaultVolumeIn;
            }
            set
            {
                _DefaultVolumeIn = value;
                RaisePropertyChanged(() => DefaultVolumeIn);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public Ic _InputIcon;
        public Ic InputIcon
        {
            get
            {
                return _InputIcon;
            }
            set
            {
                _InputIcon = value;
                RaisePropertyChanged(() => InputIcon);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _InputSource;
        public int InputSource
        {
            get
            {
                return _InputSource;
            }
            set
            {
                _InputSource = value;
                RaisePropertyChanged(() => InputSource);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public bool _InputTrigger;
        public bool InputTrigger
        {
            get
            {
                return _InputTrigger;
            }
            set
            {
                _InputTrigger = value;
                RaisePropertyChanged(() => InputTrigger);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public bool _Psu2Trigger;
        public bool Psu2Trigger
        {
            get
            {
                return _Psu2Trigger;
            }
            set
            {
                _Psu2Trigger = value;
                RaisePropertyChanged(() => Psu2Trigger);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public bool _Ext1Trigger;
        public bool Ext1Trigger
        {
            get
            {
                return _Ext1Trigger;
            }
            set
            {
                _Ext1Trigger = value;
                RaisePropertyChanged(() => Ext1Trigger);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public bool _Ext2Trigger;
        public bool Ext2Trigger
        {
            get
            {
                return _Ext2Trigger;
            }
            set
            {
                _Ext2Trigger = value;
                RaisePropertyChanged(() => Ext2Trigger);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public bool _UseInput;
        public bool UseInput
        {
            get
            {
                return _UseInput;
            }
            set
            {
                _UseInput = value;
                RaisePropertyChanged(() => UseInput);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public bool _InputIsActive;
        public bool InputIsActive
        {
            get
            {
                return _InputIsActive;
            }
            set
            {
                if (_InputIsActive != value)
                {
                    _InputIsActive = value;
                    if (value && Preset._ugs.Input != InputNumber)
                        Preset._ugs.Input = InputNumber;
                    RaisePropertyChanged(() => InputIsActive);
                }
            }
        }
        public int _MinVolume;
        public int MinVolume
        {
            get
            {
                return _MinVolume;
            }
            set
            {
                _MinVolume = value;
                RaisePropertyChanged(() => MinVolume);
                RaisePropertyChanged(() => Preset._ugs.CurrentPreset.MinVolume);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _MaxVolume;
        public int MaxVolume
        {
            get
            {
                return _MaxVolume;
            }
            set
            {
                _MaxVolume = value;
                RaisePropertyChanged(() => MaxVolume);
                RaisePropertyChanged(() => Preset._ugs.CurrentPreset.MaxVolume);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _VuOffset;
        public int VuOffset
        {
            get
            {
                return _VuOffset;
            }
            set
            {
                _VuOffset = value;
                RaisePropertyChanged(() => VuOffset);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _VuMultiplier;
        public int VuMultiplier
        {
            get
            {
                return _VuMultiplier;
            }
            set
            {
                _VuMultiplier = value;
                RaisePropertyChanged(() => VuMultiplier);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
    }
}
