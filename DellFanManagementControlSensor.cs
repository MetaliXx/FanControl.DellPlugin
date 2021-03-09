﻿using DellFanManagement.Interop;
using FanControl.Plugins;

namespace FanControl.DellPlugin
{
    public class DellFanManagementControlSensor: IPluginControlSensor
    {
        private readonly FanIndex _fanIndex;
        private bool _isSet = false;
        private float? _val;

        public DellFanManagementControlSensor(FanIndex fanIndex) => _fanIndex = fanIndex;

        public string Identifier => $"Dell/Control/{(int)_fanIndex}";

        public float? Value { get; private set; }

        public string Name => $"Dell Control {(int)_fanIndex + 1}";

        public string Origin => $"DellFanLib {DellFanLib.Version}";

        public void Reset()
        {
            DellFanLib.EnableEcFanControl(_fanIndex == FanIndex.Fan1 ? false : true);
            _isSet = false;
        }

        public void Set(float val)
        {
            if (!_isSet)
            {
                DellFanLib.DisableEcFanControl(_fanIndex == FanIndex.Fan1 ? false : true);
                _isSet = true;
            }

            _val = val;
            FanLevel fanLevel = GetFanLevel(val);
            DellFanLib.SetFanLevel(_fanIndex, fanLevel);
        }

        public void Update() => Value = _val;

        private FanLevel GetFanLevel(float val)
        {
            if (val < 1)
                return FanLevel.Level0;
            else if (val < 100)
                return FanLevel.Level1;
            else
                return FanLevel.Level2;
        }
    }
}