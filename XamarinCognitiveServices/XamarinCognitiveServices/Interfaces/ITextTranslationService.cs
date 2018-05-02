﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinCognitiveServices.Interfaces
{
    interface ITextTranslationService
    {
        Task<string> TranslateTextAsync(string text);
    }
}
