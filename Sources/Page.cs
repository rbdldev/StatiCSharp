﻿using StatiCSharp.Interfaces;
using System;
using System.Collections.Generic;

namespace StatiCSharp;

/// <summary>
/// Represenation of a page.
/// </summary>
internal class Page : IPage
{
    public bool Published { get; set; } = true;
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public DateOnly DateLastModified { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string Path { get; set; } = string.Empty;

    public string Hierarchy { get; set; } = string.Empty;

    public string Url
    {
        get
        {
            string x = string.Empty;
            if (Path == string.Empty)
            {
                x = MarkdownFileName.Substring(0, MarkdownFileName.LastIndexOf(".md")).Replace(" ", "-").Trim();
            }
            else
            {
                x = Path;
            }
            return $"/{Hierarchy}/{x}";
        }
    }

    public string MarkdownFileName { get; set; } = string.Empty;

    public string MarkdownFilePath { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new List<string>();

    public string Content { get; set; } = string.Empty;
}
