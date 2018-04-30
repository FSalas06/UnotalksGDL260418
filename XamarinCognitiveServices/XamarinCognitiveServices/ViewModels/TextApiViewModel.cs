using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinCognitiveServices.Interfaces;
using XamarinCognitiveServices.Services;

namespace XamarinCognitiveServices.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class TextApiViewModel : BaseViewModel
    {
        readonly IBingSpellCheckService _bingSpellCheckService;
        readonly ITextTranslationService _textTranslationService;
        bool _isBusy;
        string _textToValidate;

        public ICommand SpellCheckCommand
        {
            private set;
            get;
        }

        public ICommand TranslateCommand
        {
            private set;
            get;
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                SetObservableProperty(ref _isBusy, value);
            }
        }

        public string TextToValidate
        {
            get
            {
                return _textToValidate;
            }
            set
            {
                SetObservableProperty(ref _textToValidate, value);
            }
        }

        public TextApiViewModel()
        {
            _bingSpellCheckService = new BingSpellCheckService();
            _textTranslationService = new TextTranslationService(new AuthenticationService(Constants.TextTranslatorApiKey));
            SpellCheckCommand = new Command(OnSpellCheck);
            TranslateCommand = new Command(OnTranslate);
        }

        async void OnTranslate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TextToValidate))
                {
                    IsBusy = true;

                    TextToValidate = await _textTranslationService.TranslateTextAsync(TextToValidate);

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void OnSpellCheck()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(TextToValidate))
                {
                    IsBusy = true;

                    var spellCheckResult = await _bingSpellCheckService.SpellCheckTextAsync(TextToValidate);
                    foreach (var flaggedToken in spellCheckResult.FlaggedTokens)
                    {
                        TextToValidate = 
                            TextToValidate.Replace(flaggedToken.Token, flaggedToken.Suggestions.FirstOrDefault().Suggestion);
                    }

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
