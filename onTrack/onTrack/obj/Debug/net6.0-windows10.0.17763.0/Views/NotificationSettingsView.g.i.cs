﻿#pragma checksum "..\..\..\..\Views\NotificationSettingsView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8EDF6BBAAB00D487EB02F2DF3BC7C1A3BAD5318F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace onTrack.Views {
    
    
    /// <summary>
    /// NotificationSettingsView
    /// </summary>
    public partial class NotificationSettingsView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 64 "..\..\..\..\Views\NotificationSettingsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel reinforcements;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\Views\NotificationSettingsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton StandardReinforcement;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\Views\NotificationSettingsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton TypeOutTheGoalReinforcement;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\Views\NotificationSettingsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton PressTheRightGoalReinforcement;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\Views\NotificationSettingsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton WhatYouGonnaDoNowReinforcement;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\Views\NotificationSettingsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RandomReinforcement;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Views\NotificationSettingsView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton NoneReinforcement;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/onTrack;V1.0.0.0;component/views/notificationsettingsview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\NotificationSettingsView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.reinforcements = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.StandardReinforcement = ((System.Windows.Controls.RadioButton)(target));
            
            #line 65 "..\..\..\..\Views\NotificationSettingsView.xaml"
            this.StandardReinforcement.Checked += new System.Windows.RoutedEventHandler(this.Reinforcement_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TypeOutTheGoalReinforcement = ((System.Windows.Controls.RadioButton)(target));
            
            #line 66 "..\..\..\..\Views\NotificationSettingsView.xaml"
            this.TypeOutTheGoalReinforcement.Checked += new System.Windows.RoutedEventHandler(this.Reinforcement_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PressTheRightGoalReinforcement = ((System.Windows.Controls.RadioButton)(target));
            
            #line 67 "..\..\..\..\Views\NotificationSettingsView.xaml"
            this.PressTheRightGoalReinforcement.Checked += new System.Windows.RoutedEventHandler(this.Reinforcement_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.WhatYouGonnaDoNowReinforcement = ((System.Windows.Controls.RadioButton)(target));
            
            #line 68 "..\..\..\..\Views\NotificationSettingsView.xaml"
            this.WhatYouGonnaDoNowReinforcement.Checked += new System.Windows.RoutedEventHandler(this.Reinforcement_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.RandomReinforcement = ((System.Windows.Controls.RadioButton)(target));
            
            #line 69 "..\..\..\..\Views\NotificationSettingsView.xaml"
            this.RandomReinforcement.Checked += new System.Windows.RoutedEventHandler(this.Reinforcement_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.NoneReinforcement = ((System.Windows.Controls.RadioButton)(target));
            
            #line 70 "..\..\..\..\Views\NotificationSettingsView.xaml"
            this.NoneReinforcement.Checked += new System.Windows.RoutedEventHandler(this.Reinforcement_Checked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

