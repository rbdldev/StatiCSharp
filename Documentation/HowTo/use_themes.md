# Use Themes

**StatiC#** makes it easy to use different themes for your website. This article shows how to use *Foundation*.  

## Add a template to your website project

Add the template of your choice to your website project as a project or package reference. Check out the documentation of your template for more details. Here, we implement Foundation as a package reference in the project file:

```C#
<ItemGroup>
    <PackageReference Include="StatiCsharp.Themes.Foundation" Version="0.1.0" />
</ItemGroup>
```
Your can use the NuGet package manager as well.  
Build your project to restore packages.  

Now we can import the theme in the `Program.cs` of the website project, initiate a new member and inject it to the StatiC# website generating process:

```C#
using StatiCsharp;
using Foundation;

var myAwesomeWebsite = new Website(
    url: "https://yourdomain.com",
    name: "My Awesome Website",
    description: @"Description of your website",
    language: "en-US",
    sections: "posts, about",
    source: @"/path/to/your/project"
    );

var theme = new FoundationHtmlFactory();

myAwesomeWebsite.Make(theme);
```

Build and run your project. Your website is created with the new theme in your `output` directory.

```bash
$ dotnet run
```