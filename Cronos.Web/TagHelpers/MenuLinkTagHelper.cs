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
                var targetUrl = string.Empty;

                    targetUrl = helper.Action(Action, Controller);

                                    output.TagName = "a";
                output.Attributes.Add("href", targetUrl);

                var classes = "btn btn-progress";

                if (LinkTargetState == CurrentState)
                {
                    classes += " btn-progress-active";
                }

                output.Attributes.Add("class", classes);
            }
            else
            {
                output.TagName = "span";
                output.Attributes.Add("class", "btn btn-progress");
            }


            output.Content.SetContent(DisplayText);

        }
    }
}
