using AppStudio.Services;
using AppStudio.ViewModels;

using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Views
{
    public sealed partial class EchoroukDetail : Page
    {
        private NavigationHelper _navigationHelper;

        private DataTransferManager _dataTransferManager;

        public EchoroukDetail()
        {
            this.InitializeComponent();
            _navigationHelper = new NavigationHelper(this);

            EchoroukModel = new EchoroukViewModel();
        }

        public EchoroukViewModel EchoroukModel { get; private set; }

        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;

            _navigationHelper.OnNavigatedTo(e);

            if (EchoroukModel != null)
            {
                await EchoroukModel.LoadItemsAsync();
                EchoroukModel.SelectItem(e.Parameter);

                EchoroukModel.ViewType = ViewTypes.Detail;
            }
            DataContext = this;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
            _dataTransferManager.DataRequested -= OnDataRequested;
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (EchoroukModel != null)
            {
                EchoroukModel.GetShareContent(args.Request);
            }
        }
    }
}
