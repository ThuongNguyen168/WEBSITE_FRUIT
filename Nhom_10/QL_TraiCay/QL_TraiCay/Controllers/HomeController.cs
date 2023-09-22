using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_TraiCay.Models;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
namespace QL_TraiCay.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        DataDataContext dl = new DataDataContext();
        //
        // GET: /Home/
        static TAIKHOAN tk = null;
        static bool is_user = true;

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public ActionResult Index(string s)
        {
            ViewBag.VN = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("VN")).ToList();
            ViewBag.NK = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("NK")).ToList();
            ViewBag.TL = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("TL")).ToList();
            return View();
        }

        public ActionResult DanhMuc_SP_2()
        {
            ViewBag.VN = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("VN")).ToList();
            ViewBag.NK = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("NK")).ToList();
            ViewBag.TL = dl.LOAITRAICAYs.Where(t => t.MALOAI.Contains("TL")).ToList();
            return PartialView();
        }
        public ActionResult carousel_Anh()
        {
            return PartialView(dl.TRAICAYs.Take(2).ToList());
        }
        public ActionResult DS_SP_TheoLoai(string id)
        {
            return View(dl.TRAICAYs.Where(t => t.MALOAI == id).ToList());
        }
        public ActionResult TimKiem(string timkiem)
        {
            return View(dl.TRAICAYs.Where(t => (t.TENTC.Contains(timkiem) || t.GIAMGIA.ToString().Contains(timkiem) || t.LUOTXEM.ToString().Contains(timkiem))).ToList());
        }
        public ActionResult ChiTietSP(string id)
        {
            string mncc = dl.NHACC_TRAICAYs.First(t => t.MATC == id).MANCC;
            ViewBag.Nhacc = dl.NHACUNGCAPs.First(t => t.MANCC == mncc).MANCC;
            string mnsx = dl.NHASX_TRAICAYs.First(t => t.MATC == id).MANSX;
            ViewBag.Nhasx = dl.NHASANXUATs.First(t => t.MANSX == mnsx).MANSX;
            return View(dl.TRAICAYs.First(t => t.MATC == id));
        }
        public ActionResult ChiTietNCC(string id)
        {
            return PartialView(dl.NHACUNGCAPs.First(t => t.MANCC == id));
        }
        public ActionResult ChiTietNSX(string id)
        {
            return PartialView(dl.NHASANXUATs.First(t => t.MANSX == id));
        }

        public ActionResult DangKi()
        {
            return PartialView();
        }

        public ActionResult SauKhiDangKi(FormCollection collection)
        {
            // Tạo ngẫu nhiên các mã tài khoản, mã giỏ hàng, mã user.
            string MATK = string.Empty;
            string MAGH = string.Empty;
            string MAUSER = string.Empty;

            while (true)
            {
                MATK = "TK" + RandomNumber(1000, 9999).ToString();
                if (dl.TAIKHOANs.FirstOrDefault(t => t.MATK == MATK) == null)
                    break;
            }

            while (true)
            {
                MAGH = "GH" + RandomNumber(1000, 9999).ToString();
                if (dl.GIOHANGs.FirstOrDefault(g => g.MAGH == MAGH) == null)
                    break;
            }

            while (true)
            {
                MAUSER = "USER" + RandomNumber(1000, 9999).ToString();
                if (dl._USERs.FirstOrDefault(u => u.MAUSER == MAUSER) == null)
                    break;
            }

            // Thêm một tài khoản
            TAIKHOAN taikhoan = new TAIKHOAN();
            taikhoan.MATK = MATK;
            taikhoan.HOTEN = collection["hoten"];
            taikhoan.USERNAME = collection["username"];
            taikhoan.MATKHAU = collection["password"];

            dl.TAIKHOANs.InsertOnSubmit(taikhoan);

            // Thêm một giỏ hàng của user mới
            GIOHANG giohang = new GIOHANG();
            giohang.MAGH = MAGH;
            giohang.THANHTIEN = 0;

            dl.GIOHANGs.InsertOnSubmit(giohang);

            // Thêm user
            _USER user = new _USER();
            user.MAUSER = MAUSER;
            user.MATK = taikhoan.MATK;
            user.MAGH = giohang.MAGH;
            user.HOTEN = collection["hoten"];
            user.SDT = collection["sdt"];
            user.AVARTA = collection["avatar"];
            user.DIACHI = collection["diachi"];
            user.EMAIL = collection["email"];

            dl._USERs.InsertOnSubmit(user);
            dl.SubmitChanges();

            return View("ThongBao", (object)"Đăng ký thành công !");
        }

        public ActionResult DangNhap(int id)
        {
            return PartialView(id);
        }

        public ActionResult DangXuat()
        {
            tk = null;
            return View("Index", dl.TRAICAYs.ToList());
        }

        [HttpPost]
        public ActionResult SauKhiDangNhap(FormCollection collection)
        {
            tk = dl.TAIKHOANs.FirstOrDefault(taikhoan =>
                taikhoan.USERNAME == collection["username"] && taikhoan.MATKHAU == collection["password"]);

            if (tk == null)
                return PartialView("DangNhap", 1);

            is_user = dl._USERs.FirstOrDefault(u => u.MATK == tk.MATK) != null;

            if (is_user)
                return View("Index", dl.TRAICAYs.ToList());
            else
                return RedirectToAction("Index", "QuanLy");
        }

        public ActionResult GioHang()
        {
            if (tk == null)
                return View("ThongBao", "Bạn chưa đăng nhập !");

            _USER user = dl._USERs.FirstOrDefault(u => u.MATK == tk.MATK);
            var listCHITIET = dl.CHITIETGIOHANGs.Where(ct => ct.MAGH == user.MAGH).ToList();

            return View();
        }

        public ActionResult ThongBao(string id)
        {
            return View(id);
        }

        public ActionResult HienThiTenNguoiDung()
        {
            ViewBag.is_sign_in = tk != null;

            if (tk == null)
                return PartialView((object)"Chưa đăng nhập");

            _USER tim_user = dl._USERs.FirstOrDefault(u => u.MATK == tk.MATK);
            return PartialView((object)tim_user.HOTEN);
        }

    }
}
