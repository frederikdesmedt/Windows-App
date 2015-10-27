﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class EditableTextBlock : UserControl
    {

        #region Properties

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditableTextBlock), null);
        public static DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableTextBlock), null);
        public static DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(EditableTextBlock), null);

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        public bool IsEditable
        {
            get { return (bool) GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); OnIsEditableChanged?.Invoke(IsEditable); }
        }

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); OnIsEditingChanged?.Invoke(IsEditing); }
        }

        private ColoredGlyph CurrentGlyph { get; set; }
        #endregion

        #region Glyphs
        struct ColoredGlyph
        {
            public string Glyph { get; set; }
            public Color Color { get; set; }
        }

        private readonly ColoredGlyph _editGlyph = new ColoredGlyph() { Glyph = "\uE104", Color = Colors.White };
        private readonly ColoredGlyph _closeGlyph = new ColoredGlyph() { Glyph = "\uE106", Color = Colors.Red };
        #endregion

        #region Events
        public delegate void OnIsEditableChangedHandler(bool isEditable);

        public delegate void OnIsEditingChangedHandler(bool isEditing);

        public delegate void OnTextChangedHandler();

        public event OnIsEditableChangedHandler OnIsEditableChanged;
        public event OnIsEditingChangedHandler OnIsEditingChanged;
        public event OnTextChangedHandler OnTextChanged;
        #endregion

        private readonly HyperlinkButton _actionButton = new HyperlinkButton();

        private UIElement _leftSide;

        #region Constructor
        public EditableTextBlock()
        {
            InitializeComponent();

            //Set up components
            CurrentGlyph = _editGlyph;
            _actionButton.Content = new FontIcon() { Glyph = CurrentGlyph.Glyph, Foreground = new SolidColorBrush(CurrentGlyph.Color)};
            _actionButton.SetValue(Grid.ColumnProperty, 1);
            _actionButton.Margin = new Thickness(10, 0, 0, 0);

            //Add event handlers
            _actionButton.Click += ButtonBase_OnClick;
            OnIsEditingChanged += _OnIsEditingChanged;
            OnIsEditableChanged += _OnIsEditableChanged;

            //Add components to page
            MainGrid.Children.Add(_actionButton);

            //Warm up events
            OnIsEditingChanged?.Invoke(IsEditing);
            OnIsEditableChanged?.Invoke(IsEditable);
        } 
        #endregion

        

        private void _OnIsEditableChanged(bool isEditable)
        {
            _actionButton.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _OnIsEditingChanged(bool isEditing)
        {
            UpdateGlyph();
            if (_leftSide != null) MainGrid.Children.Remove(_leftSide);

            if (isEditing)
            {
                TextBox box = new TextBox() {HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center };
                box.SetValue(Grid.ColumnProperty, 0);
                box.SetBinding(TextBox.TextProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("Text"),
                    Mode = BindingMode.TwoWay
                });
                _leftSide = box;
            }
            else
            {
                TextBlock block = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center };
                block.SetValue(Grid.ColumnProperty, 0);
                block.SetBinding(TextBlock.TextProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("Text"),
                    Mode = BindingMode.OneWay
                });
                _leftSide = block;
            }

            MainGrid.Children.Add(_leftSide);
        }

        private void UpdateGlyph()
        {
            if (IsEditing)
            {
                CurrentGlyph = _closeGlyph;
            }
            else
            {
                CurrentGlyph = _editGlyph;
            }

            var fontIcon = _actionButton.Content as FontIcon;
            if (fontIcon != null)
            {
                fontIcon.Glyph = CurrentGlyph.Glyph;
                fontIcon.Foreground = new SolidColorBrush(CurrentGlyph.Color);
            }
        }

       
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            IsEditing = !IsEditing;
            OnIsEditingChanged?.Invoke(IsEditing);
        }
    }
}
