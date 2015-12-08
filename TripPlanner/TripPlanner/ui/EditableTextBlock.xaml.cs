using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TripPlanner.Model;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TripPlanner.ui
{
    public sealed partial class EditableTextBlock : UserControl, INotifyPropertyChanged
    {

        #region Properties

        public static readonly DependencyProperty EditableTextProperty = DependencyProperty.Register(
            nameof(EditableText), typeof (string), typeof (EditableTextBlock), null);

        public string EditableText
        {
            get { return GetValue(EditableTextProperty)?.ToString() ?? ""; }
            set { SetValue(EditableTextProperty, value); }
        }

        public static readonly DependencyProperty ItemModeProperty = DependencyProperty.Register(nameof(ItemMode), typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(null,
            (o, args) =>
            {
                var etb = o as EditableTextBlock;
                etb._OnIsEditingChanged(etb.IsEditing, null);
            }));

        public bool ItemMode
        {
            get { return (bool) GetValue(ItemModeProperty); }
            set { SetValue(ItemModeProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(Item), typeof(EditableTextBlock), null);
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(false,
            (o, args) =>
            {
                var etb = o as EditableTextBlock;
                etb?.OnIsEditableChanged?.Invoke(etb.IsEditable);
                etb?.NotifyPropertyChanged(nameof(IsEditable));
            }));

        public static readonly DependencyProperty IsRemovingProperty = DependencyProperty.Register("IsRemoving", typeof (bool),
            typeof (EditableTextBlock), new PropertyMetadata(false, (o, args) =>
            {
                var etb = o as EditableTextBlock;
                etb?.RemovingChanged?.Invoke(etb.IsRemoving, etb.Item);
                etb?.NotifyPropertyChanged(nameof(IsRemoving));
            }));


        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(false,
            (o, args) =>
            {
                var etb = o as EditableTextBlock;
                etb?.OnIsEditingChanged?.Invoke(etb.IsEditing, etb.Item);
                etb?.NotifyPropertyChanged(nameof(IsEditing));
            }));

        public Item Item
        {
            get { return GetValue(ItemProperty) as Item; }
            set { SetValue(ItemProperty, value); }
        }

        public bool IsEditable
        {
            get { return (bool) GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        public bool IsRemoving
        {
            get { return (bool) GetValue(IsRemovingProperty); }
            set { SetValue(IsRemovingProperty, value); }
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
        private readonly ColoredGlyph _closeGlyph = new ColoredGlyph() { Glyph = "\uE10B", Color = Colors.LightGreen };
        private readonly ColoredGlyph _deleteGlyph = new ColoredGlyph() { Glyph = "\uE107", Color = Colors.White };
        #endregion

        #region Events
        public delegate void IsEditableChangedDelegate(bool isEditable);

        public delegate void IsEditingChangedDelegate(bool isEditing, Item item);

        public delegate void IsRemovedDelegate(Item item);

        public delegate void OnTextChangedDelegate();

        public delegate void IsRemovableChangedDelegate(bool newValue, Item item);

        public event IsEditableChangedDelegate OnIsEditableChanged;
        public event IsEditingChangedDelegate OnIsEditingChanged;

        public event OnTextChangedDelegate OnTextChanged;

        public event IsRemovedDelegate OnRemoved;

        public event IsRemovableChangedDelegate RemovingChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private readonly HyperlinkButton _actionButton = new HyperlinkButton();

        public void Focus()
        {
            if (_leftSide is TextBox)
            {
                (_leftSide as TextBox).Focus(FocusState.Programmatic);
            }
            else
            {
                Focus(FocusState.Programmatic);
            }
        }

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
            RemovingChanged += _OnIsRemovableChanged;

            //Add components to page
            MainGrid.Children.Add(_actionButton);
            
            //Warm up events
            OnIsEditingChanged?.Invoke(IsEditing, GetValue(ItemProperty) as Item);
            OnIsEditableChanged?.Invoke(IsEditable);
            RemovingChanged?.Invoke(IsRemoving, GetValue(ItemProperty) as Item);
        } 
        #endregion

        private void _OnIsRemovableChanged(bool newValue, Item item)
        {
            if (IsEditable)
            {
                return;
            }

            _actionButton.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
            UpdateGlyph();
        }
        

        private void _OnIsEditableChanged(bool isEditable)
        {
            if (IsRemoving)
            {
                return;
            }

            _actionButton.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
            UpdateGlyph();
        }

        private void _OnIsEditingChanged(bool isEditing, Item item)
        {
            UpdateGlyph();
            if (_leftSide != null) MainGrid.Children.Remove(_leftSide);

            if (isEditing)
            {
                TextBox box = new TextBox() {HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Center };
                box.SetValue(Grid.ColumnProperty, 0);
                box.FontSize = FontSize;
                box.SetBinding(TextBox.TextProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(ItemMode ? "Item.Name" : "EditableText"),
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
                    Path = new PropertyPath(ItemMode ? "Item.Name" : "EditableText"),
                    Mode = BindingMode.OneWay
                });
                _leftSide = block;
            }

            MainGrid.Children.Add(_leftSide);
        }


        private void UpdateGlyph()
        {
            if (IsRemoving)
            {
                CurrentGlyph = _deleteGlyph;
            } else if (IsEditable && !IsEditing)
            {
                CurrentGlyph = _editGlyph;
            } else if (IsEditable && IsEditing)
            {
                CurrentGlyph = _closeGlyph;
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
            if (IsRemoving)
            {
                IsRemoving = false;
                IsEditing = false;
                IsEditable = false;
                OnRemoved?.Invoke(GetValue(ItemProperty) as Item);
            }
            else
            {
                IsEditing = !IsEditing;
            }
        }
    }
}
