using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGS.Helpers;

namespace UGS.Models
{
    public class LedAnimationParameterSet : NotificationObject
    {
        private int id;
        private Dictionary<int, string> AnimationNames = new Dictionary<int, string>()
        {
            { 1,"Is in Stand-by" },
            { 2,"Is in Power on" },
            { 3,"Is in Mute"},
            { 4,"Power on"},
            { 5,"Stand-by"},
            { 6,"Mute off"},
            { 7,"Mute on"},
            { 8,"Input change"},
            { 9,"Trigger change"},
            { 10, "Screen change"},
            { 11, "Action executed"},
            { 12, "Setting change"},
            { 13, "Volume change"}
        };
        public LedAnimationParameterSet(int _id)
        {
            id = _id;
        }
        public string AnimationsName
        {
            get
            {
                return AnimationNames[id];
            }
        }
        public int _AnimationsVolumeValue;
        public int AnimationsVolumeValue
        {
            get
            {
                return _AnimationsVolumeValue;
            }
            set
            {
                _AnimationsVolumeValue = value;
                RaisePropertyChanged(() => AnimationsVolumeValue);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _AnimationStateDelay_vValue;
        public int AnimationStateDelay_vValue
        {
            get
            {
                return _AnimationStateDelay_vValue;
            }
            set
            {
                _AnimationStateDelay_vValue = value;
                RaisePropertyChanged(() => AnimationStateDelay_vValue);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _AnimationStateRepeat_vValue;
        public int AnimationStateRepeat_vValue
        {
            get
            {
                return _AnimationStateRepeat_vValue;
            }
            set
            {
                _AnimationStateRepeat_vValue = value;
                RaisePropertyChanged(() => AnimationStateRepeat_vValue);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _AnimationsRearValue;
        public int AnimationsRearValue
        {
            get
            {
                return _AnimationsRearValue;
            }
            set
            {
                _AnimationsRearValue = value;
                RaisePropertyChanged(() => AnimationsRearValue);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _AnimationStateDelay_rValue;
        public int AnimationStateDelay_rValue
        {
            get
            {
                return _AnimationStateDelay_rValue;
            }
            set
            {
                _AnimationStateDelay_rValue = value;
                RaisePropertyChanged(() => AnimationStateDelay_rValue);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }
        public int _AnimationStateRepeat_rValue;
        public int AnimationStateRepeat_rValue
        {
            get
            {
                return _AnimationStateRepeat_rValue;
            }
            set
            {
                _AnimationStateRepeat_rValue = value;
                RaisePropertyChanged(() => AnimationStateRepeat_rValue);
                Preset.RequestDelayedSaveSettingsToUgs();
            }
        }


        public BindingList<string> AnimationsVolume
        {
            get
            {
                return new BindingList<string>()
                {
                    "None",
                    "Volume",
                    "All Blinking",
                    "All On",
                    "VU Meter",
                    "Chase Clockwise",
                    "Chase Counter-clockwise"
                };
            }
        }
        public BindingList<string> AnimationsRear
        {
            get
            {
                return new BindingList<string>()
                {
                    "None",
                    "Active inputs",
                    "All Blinking",
                    "All On",
                    "Chase Parallel",
                    "Chase Serial",
                };
            }
        }
    }
}
