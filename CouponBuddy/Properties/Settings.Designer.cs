﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CouponBuddy.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c42f698c-7d4c-4beb-8256-6ea5e2f4d758")]
        public string LOCATION_ID {
            get {
                return ((string)(this["LOCATION_ID"]));
            }
            set {
                this["LOCATION_ID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("45")]
        public int INACTIVITY_TIMEOUT {
            get {
                return ((int)(this["INACTIVITY_TIMEOUT"]));
            }
            set {
                this["INACTIVITY_TIMEOUT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int INACTIVE_AD_DURATION {
            get {
                return ((int)(this["INACTIVE_AD_DURATION"]));
            }
            set {
                this["INACTIVE_AD_DURATION"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("cody@pg-technologies.com")]
        public string DATABASE_USERNAME {
            get {
                return ((string)(this["DATABASE_USERNAME"]));
            }
            set {
                this["DATABASE_USERNAME"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Airplane10")]
        public string DATABASE_PASSWORD {
            get {
                return ((string)(this["DATABASE_PASSWORD"]));
            }
            set {
                this["DATABASE_PASSWORD"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool FIRST_LAUNCH {
            get {
                return ((bool)(this["FIRST_LAUNCH"]));
            }
            set {
                this["FIRST_LAUNCH"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int DEVICE_ID {
            get {
                return ((int)(this["DEVICE_ID"]));
            }
            set {
                this["DEVICE_ID"] = value;
            }
        }
    }
}
