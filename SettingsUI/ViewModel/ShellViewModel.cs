using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using SettingsUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace SettingsUI.ViewModel
{
    public class ShellViewModel : Observable
    {
        private readonly KeyboardAccelerator altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

        private readonly KeyboardAccelerator backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool isBackEnabled;
        private IList<KeyboardAccelerator> keyboardAccelerators;
        private NavigationView navigationView;
        private Type settingType;
        private NavigationViewItem selected;
        private AutoSuggestBox autoSuggestBox;
        private ICommand loadedCommand;
        private ICommand itemInvokedCommand;
        private ICommand autoSuggestBoxTextChangedCommand;
        private ICommand autoSuggestBoxQuerySubmittedCommand;

        public bool IsBackEnabled
        {
            get { return isBackEnabled; }
            set { Set(ref isBackEnabled, value); }
        }

        public NavigationViewItem Selected
        {
            get { return selected; }
            set { Set(ref selected, value); }
        }

        public ICommand LoadedCommand => loadedCommand ?? (loadedCommand = new RelayCommand(OnLoaded));
        public ICommand ItemInvokedCommand => itemInvokedCommand ?? (itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));
        public ICommand AutoSuggestBoxTextChangedCommand => autoSuggestBoxTextChangedCommand ?? (autoSuggestBoxTextChangedCommand = new RelayCommand<AutoSuggestBoxTextChangedEventArgs>(OnAutoSuggestBoxTextChanged));
        public ICommand AutoSuggestBoxQuerySubmittedCommand => autoSuggestBoxQuerySubmittedCommand ?? (autoSuggestBoxQuerySubmittedCommand = new RelayCommand<AutoSuggestBoxQuerySubmittedEventArgs>(OnAutoSuggestBoxQuerySubmitted));

        private void InternalInitialize(Frame frame, NavigationView navigationView)
        {
            this.navigationView = navigationView;
            NavigationService.Frame = frame;
            NavigationService.NavigationFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            this.navigationView.BackRequested += OnBackRequested;
        }
        public void Initialize(Frame frame, NavigationView navigationView)
        {
            InternalInitialize(frame, navigationView);
        }
        public void Initialize(Frame frame, NavigationView navigationView, Type settingType)
        {
            this.settingType = settingType;
            InternalInitialize(frame, navigationView);
        }
        public void Initialize(Frame frame, NavigationView navigationView, AutoSuggestBox autoSuggestBox)
        {
            InternalInitialize(frame, navigationView);
            this.autoSuggestBox = autoSuggestBox;
        }
        public void Initialize(Frame frame, NavigationView navigationView, Type settingType, AutoSuggestBox autoSuggestBox)
        {
            this.settingType = settingType;
            this.autoSuggestBox = autoSuggestBox;
            InternalInitialize(frame, navigationView);
        }
        public void Initialize(Frame frame, NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            this.keyboardAccelerators = keyboardAccelerators;
            InternalInitialize(frame, navigationView);
        }
        public void Initialize(Frame frame, NavigationView navigationView, Type settingType, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            this.settingType = settingType;
            this.keyboardAccelerators = keyboardAccelerators;
            InternalInitialize(frame, navigationView);
        }
        public void Initialize(Frame frame, NavigationView navigationView, AutoSuggestBox autoSuggestBox, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            this.keyboardAccelerators = keyboardAccelerators;
            this.autoSuggestBox = autoSuggestBox;
            InternalInitialize(frame, navigationView);
        }
        public void Initialize(Frame frame, NavigationView navigationView, Type settingType, AutoSuggestBox autoSuggestBox, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            this.settingType = settingType;
            this.keyboardAccelerators = keyboardAccelerators;
            this.autoSuggestBox = autoSuggestBox;
            InternalInitialize(frame, navigationView);
        }

        public ShellViewModel Builder(Frame frame, NavigationView navigationView)
        {
            InternalInitialize(frame, navigationView);
            return this;
        }
        public ShellViewModel WithSettingPage(Type settingPage)
        {
            this.settingType = settingPage;
            return this;
        }
        public ShellViewModel WithAutoSuggestBox(AutoSuggestBox autoSuggestBox)
        {
            this.autoSuggestBox = autoSuggestBox;
            return this;
        }

        public ShellViewModel WithKeyboardAccelerator(IList<KeyboardAccelerator> keyboardAccelerators)
        {
            this.keyboardAccelerators = keyboardAccelerators;
            return this;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }

        public async void OnLoaded()
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            if (keyboardAccelerators != null)
            {
                keyboardAccelerators.Add(altLeftKeyboardAccelerator);
                keyboardAccelerators.Add(backKeyboardAccelerator);
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true && settingType != null)
            {
                NavigationService.Navigate(settingType);
            }
            else if (args.InvokedItemContainer != null)
            {
                var item = navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .FirstOrDefault(menuItem => (string)menuItem.Content == (string)args.InvokedItem);

                if (item!= null)
                {
                    var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
                    NavigationService.Navigate(pageType);
                }
            }
        }

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == settingType)
            {
                Selected = (NavigationViewItem)navigationView.SettingsItem;
            }
            else if (e.SourcePageType != null)
            {
                Selected = navigationView.MenuItems
                    .OfType<NavigationViewItem>()
                    .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
            }
        }

        private static bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        public void OnAutoSuggestBoxTextChanged(AutoSuggestBoxTextChangedEventArgs args)
        {
            if (autoSuggestBox == null)
                throw new NullReferenceException("AutoSuggestBox is null, please initialize ShellViewModel with a AutoSuggestBox.");

            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suggestions = new List<string>();
                var history = navigationView.MenuItems.OfType<NavigationViewItem>().ToList();

                var querySplit = autoSuggestBox.Text.Split(' ');
                var matchingItems = history.Where(
                    item =>
                    {
                        bool flag = true;
                        foreach (string queryToken in querySplit)
                        {
                            if (item.Content.ToString().IndexOf(queryToken, StringComparison.CurrentCultureIgnoreCase) < 0)
                            {
                                flag = false;
                            }

                        }
                        return flag;
                    });

                foreach (var item in matchingItems)
                {
                    suggestions.Add(item.Content.ToString());
                }
                if (suggestions.Count > 0)
                {
                    autoSuggestBox.ItemsSource = suggestions;
                }
                else
                {
                    autoSuggestBox.ItemsSource = new string[] { "No result found" };
                }
            }
        }

        public void OnAutoSuggestBoxQuerySubmitted(AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                var item = args.ChosenSuggestion as string;
                Selected = navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .FirstOrDefault(menuItem => (string)menuItem.Content == item);

                var pageType = Selected.GetValue(NavHelper.NavigateToProperty) as Type;
                NavigationService.Navigate(pageType);
            }
        }
    }
}
