using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;

using Windows.UI.Xaml;

using AppStudio.Services;
using AppStudio.Data;

namespace AppStudio.ViewModels
{
    public class MainViewModel : BindableBase
    {
       private ElheddafViewModel _elheddafModel;
       private EchoroukViewModel _echoroukModel;
       private ElwatanViewModel _elwatanModel;
       private LexpressionViewModel _lexpressionModel;
       private LiberteViewModel _liberteModel;
        private PrivacyViewModel _privacyModel;

        private ViewModelBase _selectedItem = null;

        public MainViewModel()
        {
            _selectedItem = ElheddafModel;
            _privacyModel = new PrivacyViewModel();

        }
 
        public ElheddafViewModel ElheddafModel
        {
            get { return _elheddafModel ?? (_elheddafModel = new ElheddafViewModel()); }
        }
 
        public EchoroukViewModel EchoroukModel
        {
            get { return _echoroukModel ?? (_echoroukModel = new EchoroukViewModel()); }
        }
 
        public ElwatanViewModel ElwatanModel
        {
            get { return _elwatanModel ?? (_elwatanModel = new ElwatanViewModel()); }
        }
 
        public LexpressionViewModel LexpressionModel
        {
            get { return _lexpressionModel ?? (_lexpressionModel = new LexpressionViewModel()); }
        }
 
        public LiberteViewModel LiberteModel
        {
            get { return _liberteModel ?? (_liberteModel = new LiberteViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            ElheddafModel.ViewType = viewType;
            EchoroukModel.ViewType = viewType;
            ElwatanModel.ViewType = viewType;
            LexpressionModel.ViewType = viewType;
            LiberteModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public Visibility AppBarVisibility
        {
            get
            {
                return SelectedItem == null ? AboutVisibility : SelectedItem.AppBarVisibility;
            }
        }

        public Visibility AboutVisibility
        {

      get { return Visibility.Collapsed; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("AppBarVisibility");
            OnPropertyChanged("AboutVisibility");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadDataAsync(bool forceRefresh = false)
        {
            var loadTasks = new Task[]
            { 
                ElheddafModel.LoadItemsAsync(forceRefresh),
                EchoroukModel.LoadItemsAsync(forceRefresh),
                ElwatanModel.LoadItemsAsync(forceRefresh),
                LexpressionModel.LoadItemsAsync(forceRefresh),
                LiberteModel.LoadItemsAsync(forceRefresh),
            };
            await Task.WhenAll(loadTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await LoadDataAsync(true);
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand PrivacyCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateTo(_privacyModel.Url);
                });
            }
        }
    }
}
