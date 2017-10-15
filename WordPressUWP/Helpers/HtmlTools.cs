﻿using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using WordPressPCL.Models;
using WordPressUWP.Services;

namespace WordPressUWP.Helpers
{

    public static class HtmlTools

    {
        public static string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public static string WrapContent(Post post)
        {
            var sb = new StringBuilder();
            var isDark = ThemeSelectorService.Theme == ElementTheme.Dark;
            var content = post.Content.Rendered;


            // remove first img from post if there's one
            content = Regex.Replace(content, "^<p><img.*?</p>", "");

            sb.Append("<html><head>");
            sb.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, user-scalable=no\">");
            sb.Append("<link rel=\"stylesheet\" href=\"ms-appx-web:///Assets/Web/Style.css\" type=\"text/css\" media=\"screen\" />");
            if (isDark)
            {
                sb.Append("<link rel=\"stylesheet\" href=\"ms-appx-web:///Assets/Web/Dark.css\" type=\"text/css\" media=\"screen\" />");
            }
            else
            {
                sb.Append("<link rel=\"stylesheet\" href=\"ms-appx-web:///Assets/Web/Light.css\" type=\"text/css\" media=\"screen\" />");
            }
            sb.Append("</head><body>");

            sb.Append(FeaturedImage(post));
            sb.Append(content);
            sb.Append("</body></html>");

            return sb.ToString();
        }

        public static string FeaturedImage(Post post)
        {
            var images = new List<MediaItem>(post.Embedded.WpFeaturedmedia);
            if (images == null)
            {
                return string.Empty;
            }

            var img = images[0];
            var imgSrc = img.SourceUrl;

            var sb = new StringBuilder();
            sb.Append("<img class=\"alignnone size-full\" ");
            sb.Append($"src=\"{imgSrc}\" width=\"{img.MediaDetails.Width}\" height=\"{img.MediaDetails.Height}\" ");

            sb.Append("srcset=\"");
            foreach (var size in img.MediaDetails.Sizes)
            {
                sb.Append($"{size.Value.SourceUrl} {size.Value.Width}w ");
            }
            sb.Append("\" ");

            sb.Append($"sizes =\"(max-width: {img.MediaDetails.Width}px) 100vw, {img.MediaDetails.Width}px\" />");

            return sb.ToString();
        }
    }
}
