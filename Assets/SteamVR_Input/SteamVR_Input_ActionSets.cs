//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Input_ActionSet_default p__default;
        
        private static SteamVR_Input_ActionSet_buggy p_buggy;
        
        private static SteamVR_Input_ActionSet_NewSet p_NewSet;
        
        private static SteamVR_Input_ActionSet_control p_control;
        
        public static SteamVR_Input_ActionSet_default _default
        {
            get
            {
                return SteamVR_Actions.p__default.GetCopy<SteamVR_Input_ActionSet_default>();
            }
        }
        
        public static SteamVR_Input_ActionSet_buggy buggy
        {
            get
            {
                return SteamVR_Actions.p_buggy.GetCopy<SteamVR_Input_ActionSet_buggy>();
            }
        }
        
        public static SteamVR_Input_ActionSet_NewSet NewSet
        {
            get
            {
                return SteamVR_Actions.p_NewSet.GetCopy<SteamVR_Input_ActionSet_NewSet>();
            }
        }
        
        public static SteamVR_Input_ActionSet_control control
        {
            get
            {
                return SteamVR_Actions.p_control.GetCopy<SteamVR_Input_ActionSet_control>();
            }
        }
        
        private static void StartPreInitActionSets()
        {
            SteamVR_Actions.p__default = ((SteamVR_Input_ActionSet_default)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_default>("/actions/default")));
            SteamVR_Actions.p_buggy = ((SteamVR_Input_ActionSet_buggy)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_buggy>("/actions/buggy")));
            SteamVR_Actions.p_NewSet = ((SteamVR_Input_ActionSet_NewSet)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_NewSet>("/actions/NewSet")));
            SteamVR_Actions.p_control = ((SteamVR_Input_ActionSet_control)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_control>("/actions/control")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[] {
                    SteamVR_Actions._default,
                    SteamVR_Actions.buggy,
                    SteamVR_Actions.NewSet,
                    SteamVR_Actions.control};
        }
    }
}
