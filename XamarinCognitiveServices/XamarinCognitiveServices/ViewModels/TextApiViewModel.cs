using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinCognitiveServices.Interfaces;
using XamarinCognitiveServices.Services;

namespace XamarinCognitiveServices.ViewModels
{
    public class TextApiViewModel : BaseViewModel
    {
        #region private properties
        readonly IBingSpellCheckService _bingSpellCheckService;
        readonly ITextTranslationService _textTranslationService;
        bool _isBusy;
        string _textToValidate;
        #endregion

        #region public properties
        /// <summary>
        /// Gets the spell check command.
        /// </summary>
        /// <value>The spell check command.</value>
        public ICommand SpellCheckCommand
        {
            private set;
            get;
        }

        /// <summary>
        /// Gets the translate command.
        /// </summary>
        /// <value>The translate command.</value>
        public ICommand TranslateCommand
        {
            private set;
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:XamarinCognitiveServices.ViewModels.TextApiViewModel"/> is busy.
        /// </summary>
        /// <value><c>true</c> if is busy; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets or sets the text to validate.
        /// </summary>
        /// <value>The text to validate.</value>
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
        #endregion

        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="T:XamarinCognitiveServices.ViewModels.TextApiViewModel"/> class.
        /// </summary>
        public TextApiViewModel()
        {
            _bingSpellCheckService = new BingSpellCheckService();
            _textTranslationService = new TextTranslationService(new AuthenticationService(Constants.TextTranslatorApiKey));
            SpellCheckCommand = new Command(OnSpellCheck);
            TranslateCommand = new Command(OnTranslate);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Ons the translate.
        /// </summary>
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

        /// <summary>
        /// Ons the spell check.
        /// </summary>
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
        #endregion
    }
}
