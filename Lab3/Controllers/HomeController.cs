using Lab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Lab3.Models.ViewModels;
using System.Web;


namespace Lab3.Controllers
{
    public class HomeController : Controller
    {
        INodeRepository repository;
        private AVL tree;
        const int pageSize = 1000;
        public HomeController(INodeRepository rep ,AVL avl)
        {
            tree = avl;
            repository = rep;
        }
        public IActionResult Index(int currentPage = 1)
        {
            var list = tree.ToList();
            return View(new PagingInfoViewModel
                (
                list.Skip(pageSize*(currentPage - 1)).Take(pageSize),
                new PageInfo (currentPage, (list.Count-1)/pageSize+1, pageSize)
                ));
        }
        public IActionResult Add() => View(new Row());
        [HttpPost]
        public RedirectToActionResult Add(Row row)
        {
            if (row.RowId != 0)
            {
                tree.Add(new Node { Row = row });
            }
            return RedirectToAction(nameof(Index));            
        }
        public IActionResult Complete(Row row)
        {            
            return View(row.RowId == 0 ? null : row);
        }
        public IActionResult Find()
        {
            ViewBag.Action = "Find";
            return View("FD");
        }
        [HttpPost]
        public RedirectToActionResult Find(IdentityViewModel identityViewModel)
        {
            int i = 0;
            Node result = tree.Find(identityViewModel.Id, ref i);
            var tmp = result != null ? $"seccessful in {i} comparers " : $"failed in {i} comparers";
            TempData["result"] = string.Format($"Find was {tmp}");
            return RedirectToAction(nameof(Complete), result?.Row);
        }
        public IActionResult Remove()
        {
            ViewBag.Action = "Remove";
            return View("FD",new IdentityViewModel());
        }
        [HttpPost]
        public RedirectToActionResult Remove(IdentityViewModel identityViewModel)
        {
            tree.Delete(identityViewModel.Id);            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            int i = 0;
            var nodeForEdit = tree.Find(id, ref i);
            return View(nodeForEdit.Row);
        }
        [HttpPost]
        public RedirectToActionResult Edit(Row node)
        {
            tree.Edit(node);
            return RedirectToAction("Index");
        }
        public RedirectToActionResult Save()
        {
            string result;
            try
            {
                repository.Build(tree.ToList());
                result = "successful";
            }
            catch (Exception)
            {

                result = "failed";
            }
            TempData["result"] = string.Format($"Save was {result}");
            return RedirectToAction("Complete");
        }
    }
}
