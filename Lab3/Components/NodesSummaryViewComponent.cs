using Microsoft.AspNetCore.Mvc;
using Lab3.Models;

namespace Lab3.Components
{
    public class NodesSummaryViewComponent : ViewComponent
    {
        AVL tree;
        public NodesSummaryViewComponent(AVL avl)
        {
            tree = avl;
        }
        public IViewComponentResult Invoke()
        {
            return View(tree.ToList());
        }
    }
}
