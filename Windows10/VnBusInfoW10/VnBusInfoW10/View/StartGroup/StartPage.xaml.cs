using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VnBusInfoW10.Model;
using VnBusInfoW10.ViewModel.StartGroup;

namespace VnBusInfoW10.View.StartGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage
    {
        private StartViewModel _vm;
        public StartPage()
        {
            InitializeComponent();
            _vm = DataContext as StartViewModel;
            Loaded += StartPage_Loaded;
        }

        private void StartPage_Loaded(object sender, RoutedEventArgs e)
        {

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            //TitleBar.Height = coreTitleBar.Height;
            Window.Current.SetTitleBar(TitleGrid);

            FunctionsListView.SelectedIndex = 0;
            //BottomListView.SelectedIndex = 0;
        }

        private void HamburgerButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void FunctionsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FunctionsListView.SelectedIndex != -1)
            {
                BottomListView.SelectedIndex = -1;
                MenuListItem m = FunctionsListView.SelectedItem as MenuListItem;
                Debug.Assert(m != null, "m != null");
                _vm.NavigateToFunction(MainFrame, m.MenuF);
            }
        }

        private void BottomFunctionsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BottomListView.SelectedIndex != -1)
            {
                FunctionsListView.SelectedIndex = -1;
                MenuListItem m = BottomListView.SelectedItem as MenuListItem;
                Debug.Assert(m != null, "m != null");
                _vm.NavigateToFunction(MainFrame, m.MenuF);
            }
        }
    }
}
