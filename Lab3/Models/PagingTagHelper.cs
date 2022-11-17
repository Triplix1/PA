using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Lab3.Models.ViewModels;

namespace Lab3.Models
{
    [HtmlTargetElement("div", Attributes = "page-link")]
    public class PagingTagHelper : TagHelper
    {
        IUrlHelperFactory urlHelperFactory;
        public PagingTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageInfo PageLink { get; set; }
        public string PageAction { get; set; }
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            for (int i = 1; i <= PageLink.PageCount; i++)
            {
                TagBuilder a = new TagBuilder("a");
                PageUrlValues["currentPage"] = i;
                a.Attributes["href"] = urlHelper.Action(PageAction,PageUrlValues);
                a.AddCssClass("btn");
                a.AddCssClass(i == PageLink.CurrentPage ? "btn-primary" : "btn-secondary");                
                a.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(a);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
