using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQL_TraiCay.Models
{
    public class Cartitem
    {
        public string iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnh { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public int itags { get; set; }
        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        QL_TraiCayDataContext dt = new QL_TraiCayDataContext();
        public Cartitem(string id)
        {
            TRAICAY sanpham = dt.TRAICAYs.Single(n => n.MATC == id);
            if (id != null)
            {
                iMaSP = sanpham.MATC;
                sTenSP = sanpham.TENTC;
                sAnh = sanpham.LIST_HINHANH;
                dDonGia = double.Parse(sanpham.GIAMOI.ToString());
                //itags = int.Parse(sanpham..ToString());
                iSoLuong = 1;
            }
        }
        public class GioHang
        {
            public List<Cartitem> ds;
            public GioHang()
            {
                ds = new List<Cartitem>();
            }
            public GioHang(List<Cartitem> dsGH)
            {
                if (dsGH == null)
                    ds = new List<Cartitem>();
                else
                    ds = dsGH;
            }
            public int SoMatHang()
            {
                if (ds == null)
                    return 0;
                return ds.Count;
            }
            public int TongSLHang()
            {
                int tong = 0;
                if (ds != null)
                {
                    tong = ds.Sum(n => n.iSoLuong);
                    return tong;
                }
                return 0;
            }
            public double TongThanhTien()
            {
                double tong = 0;
                if (ds != null)
                {
                    tong = ds.Sum(n => n.ThanhTien);
                    return tong;
                }
                return 0;
            }
            public int Them(string iMa)
            {
                Cartitem sp = ds.Find(n => n.iMaSP == iMa);
                if (sp == null)
                {
                    Cartitem sanpham = new Cartitem(iMa);
                    if (sanpham == null)
                    {
                        return -1;
                    }
                    ds.Add(sanpham);
                }
                else
                {
                    sp.iSoLuong++;
                }
                return 1;
            }


            public int Xoa(string iMa)
            {
                Cartitem sp = ds.Find(n => n.iMaSP == iMa);


                Cartitem sanpham = new Cartitem(iMa);
                ds.Remove(sanpham);
                sp.iSoLuong--;

                return 1;

            }

            public int XoaGioHang()
            {
                if (ds == null)
                    return 1;
                else
                {
                    ds.Clear();
                    return 1;
                }
            }

        }
    }
}