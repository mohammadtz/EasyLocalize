# EasyLocalize

I started learning programming with JavaScript, then I decided to switch to TypeScript, and finally I found my way and become a C# Developer. After while C# became my favorite language, because of the features it has, but I had a problem with the default localization system in the .NET. When I used JS or TS, I preferred use JSON as localize in my applications, because it was very simple and more functional for me. So I decided to write a simple package to handle software localization with JSON format in .NET. I hope you like it and find it valuable. This is my first published package, so it may have some issues. If you find any problems in this package, please report it in Issues part.

# Get Started

To get started with the package you have to install [EasyLocalize](https://www.nuget.org/packages/EasyLocalize) Package from NuGet, after that you can use it for any type of .Net projects.
In the source code, you can find the project name EasyLocalize.Example, this project contain a console application to show an example of how to use it in your projects.

## Pure Usage

This way is simple usage for any type of .Net applications:
```csharp
var localizeOptions = new EasyLocalizeOptions  
{  
  RegisterMessages = new List<RegisterMessage>()  
  {  
    new()  
    {  
	LocalizeKey = "en-US", // this is localization key package work with this name
	JsonFilePath = "/Localizations/en.json", // json file address
	IsExternal = false  // if is true means JsonFilePath is a external resuorce
    },
    new()  
    {  
    	LocalizeKey = "fa-IR",  
      	JsonFilePath = "https://{YOUR_WEBSITEADDRESS}/Localizations/fa.json",
      	IsExternal = true
    },
    // how many language your want just add it here
    // ...
  },
  DefaultLanguage = "en-US",  // Pick Your default 
};

// after create options object your must be pass options to Message Class and create new instanse
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


## Use simpler with Dependency Injection version for ASP.NET projects
If your use any type of Asp.Net project support Dependency Injection,  You can simply use [EasyLocalize.DependencyInjection](https://www.nuget.org/packages/EasyLocalize.DependencyInjection/) Package this package is more simpler and useful than above option, In below Example, I show you how to use the package in ASP.NET WebApi project.
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

///TODO: ADD THIS MIDDLEWARE TO READ LANGUAGE AUTOMATICLY FROM REQUEST HEADER "accept-language"
app.UseEasyLocalization();
////////////////////////////////////

app.UseHttpsRedirection();
// ....
```

Than You can use it like below:
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
