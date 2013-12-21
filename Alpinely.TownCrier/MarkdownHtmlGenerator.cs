using MarkdownSharp;

namespace Alpinely.TownCrier
{
    public class MarkdownHtmlGenerator
    {
        public static string MarkdownToHtml(string template)
        {
            const string boilerPlate = @"<html><head><title></title></head><body>{0}</body></html>";

            var markdownSharp = new Markdown(
                new MarkdownOptions
                    {
                        AutoHyperlink = false,
                        LinkEmails = false,
                        AutoNewLines = true
                    }
                );
            var templateHtml = markdownSharp.Transform(template);
            var html = string.Format(boilerPlate, templateHtml);
            return html;
        }
    }
}