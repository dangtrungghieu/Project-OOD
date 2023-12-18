using Project_OOD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;

namespace Project_OOD.Areas.User.Controllers
{
    public class QuanLyController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        // GET: User/QuanLy
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];

            var sTen = f["sTenNguoiDung"];
            var sGioiTinh = f["sGioiTinh"];
            var sNgaySinh = f["sNgaySinh"];
            var sEmail = f["sEmail"];
            var sDiaChi = f["sDiaChi"];

            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung ==  nGUOIDUNG.MaNguoiDung);

            ng.TenNguoiDung = sTen;
            ng.GioiTinh = sGioiTinh;
            ng.Email = sEmail;
            ng.DiaChi = sDiaChi;
            db.SaveChanges();

            Session.Clear();
            Session["TaiKhoan"] = nGUOIDUNG;
            return RedirectToAction("Index");
        }

        public ActionResult MuonXe(int? page, int? id)
        {
            int iSize = 10;
            int iPageNum = (page ?? 1);

            // Kiểm tra xem id có giá trị không
            if (id.HasValue)
            {
                var dsXe = db.XE.Where(x => x.MaLoai == id);

                ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;

                return View(dsXe.OrderBy(s => s.MaXe).ToPagedList(iPageNum, iSize));
            }
            else
            {
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult XacNhan(int? id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult XacNhan(FormCollection f)
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            var ngaymuon = f["sNgayMuon"];
            var giomuon = f["sGioMuon"];
            var maxe = f["sMaXe"];
            var sMaND = nGUOIDUNG.MaNguoiDung;
            
            //if(Convert.ToDateTime(ngaymuon) <= DateTime.Now)
            //{
            //    ViewBag.ThongBao = "Ngày mượn không hợp lệ!";
            //}
            //else
            //{
                MUONXE mx = new MUONXE();
                mx.MaXeMuon = int.Parse(maxe);
                mx.MaTheMuon = nGUOIDUNG.MaNguoiDung;
                mx.ThoiGianMuon = Convert.ToDateTime(f["sNgayMuon"]);
                db.MUONXE.Add(mx);

            XE xE = db.XE.Find(int.Parse(maxe));
            xE.TinhTrang = "Đang được mượn";
                db.SaveChanges();
            //}
            return RedirectToAction("Index", "QuanLy");
        }
        public ActionResult XeMuon ()
        {
            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            var ds = from n in db.MUONXE where n.MaTheMuon == nGUOIDUNG.MaNguoiDung select n;

            return View(ds.ToList());
        }
        public ActionResult HuyMuon(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            NGUOIDUNG nGUOIDUNG = (NGUOIDUNG)Session["TaiKhoan"];
            NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.MaNguoiDung == nGUOIDUNG.MaNguoiDung);
            MUONXE muonXe = db.MUONXE.SingleOrDefault(m => m.MaMuonXe == id);
            XE xe = db.XE.SingleOrDefault(x => x.MaXe == muonXe.MaXeMuon);
            if (muonXe == null)
            {
                return HttpNotFound();
            }
            db.MUONXE.Remove(muonXe);

            
            if (xe != null)
            {
                xe.TinhTrang = "Chưa được mượn";
                db.SaveChanges();
            }
            db.SaveChanges ();
            return RedirectToAction("Index", "QuanLy");
        }
    }
}