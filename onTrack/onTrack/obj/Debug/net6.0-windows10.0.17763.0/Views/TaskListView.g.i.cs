﻿#pragma checksum "..\..\..\..\Views\TaskListView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A9391B6FDA302C63C2A8293B729E7DD79E9C295D"
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
using onTrack.Components;


namespace onTrack.Views {
    
    
    /// <summary>
    /// TaskListView
    /// </summary>
    public partial class TaskListView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 250 "..\..\..\..\Views\TaskListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scroll_viewer;
        
        #line default
        #line hidden
        
        
        #line 251 "..\..\..\..\Views\TaskListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView tree;
        
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
            System.Uri resourceLocater = new System.Uri("/onTrack;component/views/tasklistview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\TaskListView.xaml"
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
            
            #line 10 "..\..\..\..\Views\TaskListView.xaml"
            ((onTrack.Views.TaskListView)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseDown);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\Views\TaskListView.xaml"
            ((onTrack.Views.TaskListView)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.UserControl_GotFocus);
            
            #line default
            #line hidden
            
            #line 13 "..\..\..\..\Views\TaskListView.xaml"
            ((onTrack.Views.TaskListView)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.UserControl_KeyDown);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\..\Views\TaskListView.xaml"
            ((onTrack.Views.TaskListView)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.scroll_viewer = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 250 "..\..\..\..\Views\TaskListView.xaml"
            this.scroll_viewer.PreviewMouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.ScrollViewer_PreviewMouseWheel);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tree = ((System.Windows.Controls.TreeView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

