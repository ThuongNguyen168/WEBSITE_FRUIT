using WebQL_TraiCay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_TraiCay.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/

        public ActionResult ChonMua(string id)
        {
            Cartitem.GioHang gh = (Cartitem.GioHang)Session["gh"];

            if (gh == null)
                gh = new Cartitem.GioHang();
            int kq = gh.Them(id);
            Session["gh"] = gh;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult XemGioHang()
        {
            Cartitem.GioHang gh = (Cartitem.GioHang)Session["gh"];

            return View(gh);
        }
        public ActionResult Xoa(string id)
        {

            Cartitem.GioHang gh = (Cartitem.GioHang)Session["gh"];
            int kq = gh.Xoa(id);

            Session["gh"] = gh;

            return RedirectToAction("XemGioHang", "GioHang");
        }

        public ActionResult AddSL(string id)
        {

            Cartitem.GioHang gh = (Cartitem.GioHang)Session["gh"];
            int kq = gh.Them(id);
            Session["gh"] = gh;

            return RedirectToAction("XemGioHang", "GioHang");
        }
        public ActionResult XoaGio()
        {
            Cartitem.GioHang gh = (Cartitem.GioHang)Session["gh"];
            gh.XoaGioHang();
            Session["gh"] = gh;
            return RedirectToAction("xemgiohang", "GioHang");
        }
    }
}
