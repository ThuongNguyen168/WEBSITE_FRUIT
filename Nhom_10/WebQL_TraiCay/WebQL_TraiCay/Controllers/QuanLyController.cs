using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQL_TraiCay.Models;
using System.IO;
namespace QL_TraiCay.Controllers
{
    public class QuanLyController : Controller
    {
        //
        // GET: /QuanLy/
        QL_TraiCayDataContext dl = new QL_TraiCayDataContext();

        public ActionResult Index()
        {
            return View(dl.TRAICAYs.ToList());
        }
        public ActionResult DS_TraiCay()
        {
            return Index();
        }
        public ActionResult Them_TC()
        {
            ViewBag.MaLoai = new SelectList(dl.LOAITRAICAYs.ToList(), "MALOAI", "TENLOAI");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XLThem(TRAICAY a, HttpPostedFileBase fupload)
        {
            try
            {
                if (fupload == null)
                {
                    ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                else
                {
                   
                        string filename = Path.GetFileName(fupload.FileName);
                        string path = Path.Combine(Server.MapPath("/Images/" + filename));
                        fupload.SaveAs(path);
                        a.DUONGDAN = filename;
                        dl.TRAICAYs.InsertOnSubmit(a);
                        dl.SubmitChanges();
                        return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ViewBag.Loi = ex.Message;
                return RedirectToAction("Tao_TC");
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            ViewBag.MaTC = tc.MATC;
            if(tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tc);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhanXoa(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            ViewBag.MaTC = tc.MATC;
            if (tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            dl.TRAICAYs.DeleteOnSubmit(tc);
            dl.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult chitiet(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            ViewBag.MaTC = tc.MATC;
            if (tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tc);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            TRAICAY tc = dl.TRAICAYs.SingleOrDefault(t => t.MATC == id);
            if(tc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tc);
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Sua(TRAICAY tc, HttpPostedFileBase fUpLoad)
        {
            ViewBag.MANSX = new SelectList(dl.NHASANXUATs.ToList().OrderBy(t => t.TENNSX), "MANSX", "TENNSX");
            if(fUpLoad == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fUpLoad.FileName);
                    var path = Path.Combine(Server.MapPath("/Images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tai";
                    else
                    {
                        fUpLoad.SaveAs(path);
                    }
                    tc.DUONGDAN = fileName;
                    UpdateModel(tc);
                    dl.SubmitChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
