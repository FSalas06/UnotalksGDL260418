
# UnotalksGDL260418

Example Integration of Microsoft Cognitive Services with Xamarin.Forms displayed in the Meetup Unotalk GDL 26/04/18 in Unosquare Guadalajara

## Getting Started

Create a free account of [Azure](https://azure.microsoft.com/es-mx/). and creat the services of:

- Bing Spell Check API
- Text Translation API
- Face API
- Vision API

Save the **URL** and **APIKEY**

or if you don't want to create an *Azure Account* Try in this [link](https://azure.microsoft.com/en-us/try/cognitive-services/) and only get and save **URL** and **APIKEY**

Once you get the **URL** and **APIKEY** 

Replace this into *Constants.cs* file

```
//Spell Check API
public static readonly string BingSpellCheckApiKey = "<Here your Key>";
public static readonly string BingSpellCheckEndpoint = "<Here your endpoint>";

//Text Translation API
public static readonly string TextTranslatorApiKey = "<Here your key>";
public static readonly string TextTranslatorEndpoint = "<Here your endpoint>";

// Face API
public static readonly string FaceApiKey = "<Here your key>";
public static readonly string FaceEndpoint = "<Here your endpoint>";

// Vision API
public static readonly string ComputerVisionApiKey = "<Here your key>";
public static readonly string ComputerVisionEndpoint = "<Here your endpoint>";
```

### Packages used

* [Microsoft.ProjectOxford.Face](https://www.nuget.org/packages/Microsoft.ProjectOxford.Face/) - To Face API integration
* [Plugin.Permissions](https://www.nuget.org/packages/Plugin.Permissions/) - To get Camera permissions
* [Xam.Plugin.Media](https://www.nuget.org/packages/Xam.Plugin.Media/3.1.3) - To get and set an image from camera or gallery
 

## Authors

* **Francisco Salas** - *Initial work* - [FSalas06](https://github.com/FSalas06)

## TODO

- [x] Create a funtional project Xamarin.Forms
- [x] Create a Git Repo
- [x] Documentation Code
- [ ] Clean Code
- [ ] Integrate Speech API
- [ ] Integrate Bing Search API
- [ ] Work into intuitive examples
- [ ] Splash Screen Android/IOS


