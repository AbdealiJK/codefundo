﻿#pragma checksum "F:\codefundo\winfinityClient\winfinityClient\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F88F854EB8C00973DD4982E17B6DBAFF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace winfinityClient {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock DeviceUid;
        
        internal System.Windows.Controls.Image RefreshUid;
        
        internal System.Windows.Controls.Button CreateRoom;
        
        internal System.Windows.Controls.Button JoinRoom;
        
        internal System.Windows.Controls.Button Speak;
        
        internal System.Windows.Controls.Button NFCSend;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/winfinityClient;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.DeviceUid = ((System.Windows.Controls.TextBlock)(this.FindName("DeviceUid")));
            this.RefreshUid = ((System.Windows.Controls.Image)(this.FindName("RefreshUid")));
            this.CreateRoom = ((System.Windows.Controls.Button)(this.FindName("CreateRoom")));
            this.JoinRoom = ((System.Windows.Controls.Button)(this.FindName("JoinRoom")));
            this.Speak = ((System.Windows.Controls.Button)(this.FindName("Speak")));
            this.NFCSend = ((System.Windows.Controls.Button)(this.FindName("NFCSend")));
        }
    }
}

