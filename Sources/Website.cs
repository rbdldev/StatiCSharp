﻿using System.Globalization; // CultureInfo
using System.IO; // GetCurrentDirectory, GetDirectories, CreateDirectory
using static System.Console;
using Markdig; // Markdown parser
using System.Reflection;
using StatiCsharp.Interfaces;
using System.Text; // StringBuilder

namespace StatiCsharp
{
    public partial class Website : IWebsite
    {
        private string url;
        public string Url
        {
            get { return url; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }

        private string description;
        public string Description
        {
            get { return description; }
        }

        private CultureInfo language;
        public CultureInfo Language
        {
            get { return language; }
        }

        private string output;
        public string? Output
        {
            get { return output; }
        }

        private ISite? index = new Index();
        public ISite? Index { get { return this.index; } }

        private List<ISite> pages = new List<ISite>();
        public List<ISite> Pages
        {
            get { return this.pages; }
            set { this.pages = value; }
        }

        private List<ISection> sections = new List<ISection>();
        public List<ISection> Sections
        {
            get { return this.sections; }
            set { this.sections = value; }
        }

        private List<string> makeSectionsFor = new List<string>();
        public List<string> MakeSectionsFor
        {
            get { return this.makeSectionsFor; }
            set { this.makeSectionsFor = value; }
        }

        private string content;
        public string Content { get { return this.content; } }

        private string resources = string.Empty;
        public string Resources { 
            get { return this.resources;} 
            set { this.resources = value; }
        }

        // Init
        public Website()
        {
            url = "URL/of/my/awesome/website";
            name = "name of my site";
            description = "descripton of site";
            language = new CultureInfo("en-US");
            content = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StatiCsharpEnv", "Content");
            resources = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StatiCsharpEnv", "Resources");
            output = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StatiCsharpEnv", "Output");
            makeSectionsFor = new List<string>();
        }

        public Website(string url, string name, string description, string language)
        {
            this.url = url;
            this.name = name;
            this.description = description;
            this.language = new CultureInfo(language);
            content = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StatiCsharpEnv", "Content");
            resources = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StatiCsharpEnv", "Resources");
            output = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "StatiCsharpEnv", "Output");
        }

        public void Make()
        {
            IHtmlFactory factory = new FoundationHtmlFactory();
            factory.WithWebsite(this);
            this.Make(factory);
        }

        public void Make(IHtmlFactory HtmlFactory)
        {
            HtmlFactory.WithWebsite(this);
            WriteLine("Making your website...");

            WriteLine("Collecting markdown data...");
            GenerateSitesFromMarkdown(this);

            WriteLine("Generating index page...");
            MakeIndex(HtmlFactory);

            WriteLine("Generating pages...");
            MakePages(HtmlFactory);

            WriteLine("Generating sections...");
            MakeSections(HtmlFactory);

            WriteLine("Generating items...");
            MakeItems(HtmlFactory);

            WriteLine("Copying resources");
            CopyAll(this.Resources, output);
            File.Copy(HtmlFactory.cssPath, Path.Combine(output, "styles.css"), true);

        }

        /// <summary>
        /// Creates and writes the index/homepage of the website with the given HtmlFactory.
        /// </summary>
        /// <param name="HtmlFactory"></param>
        private void MakeIndex(IHtmlFactory HtmlFactory)
        {
            string body = HtmlFactory.MakeIndexHtml(this);
            string index = AddLeadingHtmlCode(this, this.Index, body);
            File.WriteAllText(Path.Combine(output, "index.html"), index);
        }

        private void MakePages(IHtmlFactory HtmlFactory)
        {
            foreach (IPage site in this.Pages)
            {
                string body = HtmlFactory.MakePageHtml(site);
                string page = AddLeadingHtmlCode(this, site, body);
                // Create directory, if it does not excist.
                string pathInHierachy = (site.Path == string.Empty) ? site.MarkdownFileName : site.Path;
                if (pathInHierachy == "index") { pathInHierachy = string.Empty; }
                string path = Directory.CreateDirectory(Path.Combine(output, site.Hierarchy, pathInHierachy)).ToString();
                File.WriteAllText(Path.Combine(path, "index.html"), page);
            }
        }

        private void MakeSections(IHtmlFactory HtmlFactory)
        {
            foreach(ISection site in this.Sections)
            {
                string body = HtmlFactory.MakeSectionHtml(site);
                string page = AddLeadingHtmlCode(this, site, body);
                string path = Directory.CreateDirectory(Path.Combine(output, site.SectionName)).ToString();
                File.WriteAllText(Path.Combine(path, "index.html"), page);
            }
        }

        public void MakeItems(IHtmlFactory HtmlFactory)
        {
            foreach(ISection currentSection in this.Sections)
            {
                foreach (IItem site in currentSection.Items)
                {
                    string body = HtmlFactory.MakeItemHtml(site);
                    string page = AddLeadingHtmlCode(this, site, body);
                    string itemPath = (site.Path != string.Empty) ? site.Path : site.MarkdownFileName;
                    string path = Directory.CreateDirectory(Path.Combine(output, currentSection.SectionName, itemPath)).ToString();
                    File.WriteAllText(Path.Combine(path, "index.html"), page);
                }
            }
        }

        private void CopyAll(string sourceDir, string destinationDir)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories

      
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (true)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyAll(subDir.FullName, newDestinationDir);
                }
            }
        }
    }
}