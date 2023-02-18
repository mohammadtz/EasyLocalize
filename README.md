# EasyLocalize

Hi! When I started programming with Javascript Programming Language after switching to Typescript and finally I go to learn C# Programming Language and I Love It, but I have a problem with the default Localization system in the .Net ecosystem because when I use JS and TS I Use JSON format to localize my application, because is very simple and more functional. Then I decide to write a simple package to handle Software Localize with JSON format in .Net, I hope you Like it and valuable for you.
This is my first package published it, maybe has some issues, please if you got any issues with this package please share the problem with us

# Get Started

To get started with this package you need first install [EasyLocalize](https://www.nuget.org/packages/EasyLocalize) Package from NuGet to your project, after that you can use it for any type of .Net project.
In the source code, you can see the project name EasyLocalize.Example, this project is a console app to show you an example of how to use it in your project

## Pure Usage

A simple way to use it in any type of .Net project is after installing a package used like this:
```csharp
var localizeOptions = new EasyLocalizeOptions  
{  
  RegisterMessages = new List<RegisterMessage>()  
  {  
    new()  
    {  
	LocalizeKey = "en-US", // this is localization key package work with this name
	JsonFilePath = "\\Localizations\\en.json", // json file address
	IsExternal = false  // if is true means JsonFilePath is a external resuorce
    },
    new()  
    {  
    	LocalizeKey = "fa-IR",  
      	JsonFilePath = "https:\\{YOUR_WEBSITEADDRESS}\\Localizations\\fa.json",
      	IsExternal = true
    },
    // how many language your want just add it here
    // ...
  },
  DefaultLanguage = "en-US",  // Pick Your default 
};

// after create options object your must be pass to to Message Class and create new instanse
var easyLocalize = new Message(localizeOptions);

// just call Get method and put json localize key to get result
// and get result of defulat language
var successMessage = easyLocalize.Get("Success");
/* 
    successMessage Response Model:
    {
	Value: string, // if key in not valid return localize key your pass it, in Example: "Success"
	IsValid: boolean // show use value is valid or not
    }
*/

// for example if pass key is valid write on the console like this: "Operation done successfully"
// else write key itself, like this: "Success"
Console.WriteLine(successMessage.Value);

// the SetLanguage method change default language
// in this example I switch from "en-US" to "fa-IR"
easyLocalize.SetLanguage("fa-IR");

// after if I call Get method again
// I Got another language result
var newSuccessMessage = easyLocalize.Get("Success");

// example: "عملیات با موفقیت انجام شد"
Console.WriteLine(newSuccessMessage.Value);
```


## Use simpler with Dependency Injection version
If your use any type of Asp.Net project support Dependency Injection,  You can simply use [EasyLocalize.DependencyInjection](https://www.nuget.org/packages/EasyLocalize.DependencyInjection/) Package this package is more simpler and useful than above option, In below Example, I show you how to use this package in Asp.Net WebApi project.
after install package, open Your `Program.cs` file and put below code to your project
```csharp
// Add services to the container.  
builder.Services.AddEasyLocalizer(options =>  
{  
  options.RegisterMessages = new List<RegisterMessage>()  
    {  
      new()  
      {  
        LocalizeKey = "en-US",  
        JsonFilePath = "/Localizations/En.json"  
      },  
      new()  
      {  
        LocalizeKey = "fa-IR",  
        JsonFilePath = "/Localizations/Fa.json"  
      }  
   };  
   options.DefaultLanguage = "en-US";
});
```
After add service, your must add package middleware to:
```csharp
var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())  
{  
   app.UseSwagger();  
   app.UseSwaggerUI();  
}

///TODO: ADD THIS LINE TO YOUR SOURCE
app.UseEasyLocalization();
////////////////////////////////////

app.UseHttpsRedirection();
// ....
```

Than You can use It like this:
```csharp
[ApiController]  
[Route("api/[Controller]")]  
public class TestController : ControllerBase  
{  
  // get instance of IMessage from DI
  private readonly IMessage _message;  
  
  public TestController(IMessage message)  
  {  
     _message = message;  
  }
  
  [HttpGet]  
  public ActionResult GetTest()  
  {  
    return Ok(_message.Get("Success"));
    /* Response Model Example:
    {
	Value: string,
	IsValid: boolean
    }*/
  }
}
```
