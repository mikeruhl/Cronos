using System;
using Cronos.Web.Models;
using Cronos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cronos.Web.TagHelpers
{
    public class MenuLinkTagHelper : TagHelper
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string DisplayText { get; set; }
        public int LinkTargetState { get; set; }
        public int MinRequiredState { get; set; }

        public int CurrentState { get; set; }
        public int HighestState { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (HighestState >= MinRequiredState)
            {
                var helper = new UrlHelper(ViewContext);
                var link = new TagBuilder("a");

                var targetUrl = string.Empty;

                    targetUrl = helper.Action(Action, Controller);

                link.Attributes.Add("href", targetUrl);

                var classes = "btn menu-nav btn-progress";

                if (LinkTargetState == CurrentState)
                {
                    classes += "-active";
                }

                link.Attributes.Add("class", classes);
                link.InnerHtml.AppendHtml(DisplayText);
                output.Content.AppendHtml(link);
                output.Attributes.Add("class", "col-md-4 link link-complete");
            }
            else
            {
                var spanner = new TagBuilder("span");
                spanner.Attributes.Add("class", "btn btn-progress  menu-stub");
                spanner.InnerHtml.AppendHtml(DisplayText);
                output.Attributes.Add("class", "col-md-4 link");
                output.Content.AppendHtml(spanner);
            }
        }
    }
}
