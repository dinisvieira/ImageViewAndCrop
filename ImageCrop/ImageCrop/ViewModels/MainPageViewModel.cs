using System.Diagnostics;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace ImageCrop.ViewModels
{
    class MainPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            GoToCropPageCommand = new DelegateCommand(OnGoToCropPageCommandExecuted);
            _navigationService = navigationService;
        }

        public DelegateCommand GoToCropPageCommand { get; }

        private async void OnGoToCropPageCommandExecuted()
        {
            var result = await _navigationService.NavigateAsync("PhotoCropPage");
            if (!result.Success)
            {
                Debug.WriteLine(result.Exception?.Message);
            }
        }
    }
}
