using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_OOD.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;

namespace Project_OOD.Areas.Admin.Controllers
{
    public class XeController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        // GET: Admin/Xe
        [HttpGet]
        public ActionResult Index(int ?page)
        {
            int iSize = 10;
            int iPageNum = (page ?? 1);
            var dsXe = from lx in db.XE select lx;
            ViewBag.StartingIndex = (iPageNum - 1) * iSize + 1;
            return View(dsXe.OrderBy(s => s.MaXe).ToPagedList(iPageNum, iSize));
        }
        [HttpGet]
        public ActionResult AddXe()
        {
            return View(db.LOAIXE.ToList());
        }
        [HttpPost]
        public ActionResult AddXe(FormCollection f, XE xe)
        {
            var sSoLuongXe = int.Parse(f["sSoLuong"]);
            var sLoaiXe =int.Parse(f["sLoaiXe"]);
            var sTinhTrang = f["sTinhTrang"];
            var sNgayThem = DateTime.Parse(f["sNgayThem"]);
            var sKiemTra = int.Parse(f["radio1"]);
            if (sTinhTrang == "")
            {
                ViewBag.ThongBao0 = "Tình trạng không hợp lệ!";
                ViewBag.sSoLuong = sSoLuongXe;
                ViewBag.NgayThem = sNgayThem;
            }
            else if (sLoaiXe == 0)
            {
                ViewBag.ThongBao2 = "Loại xe không hợp lệ!";
                ViewBag.sSoLuong = sSoLuongXe;
                ViewBag.NgayThem = sNgayThem;
            }
            else if (sKiemTra == 0)
            {
                ViewBag.ThongBao3 = "Thêm xe mới không thành công!";
                ViewBag.sSoLuong = sSoLuongXe;
                ViewBag.NgayThem = sNgayThem;
            }
            else
            {
                for (int i = 0; i < sSoLuongXe; i++)
                {
                    xe.NgayThem = sNgayThem;
                    xe.MaLoai = sLoaiXe;
                    xe.TinhTrang = sTinhTrang;
                    db.XE.Add(xe);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Xe");
            }
            return View(db.LOAIXE.ToList());
        }
        [HttpPost]
        public ActionResult UpdateSessionStatus(int vehicleId)
        {
            var xe = db.XE.Find(vehicleId);
            if (xe != null)
            {
                if (xe.TinhTrang == "Chưa được mượn")
                {
                    xe.TinhTrang = "Đang được mượn";
                }
                else if (xe.TinhTrang == "Đang được mượn")
                {
                    xe.TinhTrang = "Đang sửa chữa";
                }
                else if (xe.TinhTrang == "Đang sửa chữa")
                {
                    xe.TinhTrang = "Chưa được mượn";
                }

                db.SaveChanges(); 
                return Content(xe.TinhTrang); 
            }

            return Content("Không tìm thấy xe");
        }
        [HttpGet]
        public ActionResult LoaiXe(int ?page)
        {
            int iSize = 10;
            int iPageNum = (page ?? 1);
            var dsLoaiXe = from lx in db.LOAIXE select lx;
            return View(dsLoaiXe.OrderBy(s => s.MaLoai).ToPagedList(iPageNum, iSize));
        }
        //Add Loai Xe
        [HttpGet]
        public ActionResult AddLoaiXe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddLoaiXe(FormCollection f, LOAIXE lx)
        {
            var sTenLoaiXe = f["sName"];
            var sDonGia = int.Parse(f["sDonGia"]);
            var sKiemTra =int.Parse(f["radio1"]);
            if(db.LOAIXE.SingleOrDefault(n => n.TenLoai == sTenLoaiXe) != null)
            {
                ViewBag.ThongBao = "Loại xe đã có trong cửa hàng!";
                ViewBag.sName = sTenLoaiXe;
                ViewBag.sDonGia = sDonGia;
            }
            else if(sKiemTra == 0)
            {
                ViewBag.ThongBao1 = "Thêm loại xe mới không thành công!";
                ViewBag.sName = sTenLoaiXe;
                ViewBag.sDonGia = sDonGia;
            }
            else
            {
                lx.TenLoai = sTenLoaiXe;
                lx.DonGia = sDonGia;
                db.LOAIXE.Add(lx);
                db.SaveChanges();
                return RedirectToAction("LoaiXe");
            }
            return View("AddLoaiXe");
        }
        public JsonResult Details_Xe(int id)
        {
            var xe = db.XE
                .Where(n => n.MaXe == id)
                .Select(n => new {
                    MaXe = n.MaXe,
                    TenLoai = n.LOAIXE.TenLoai, 
                    TinhTrang = n.TinhTrang
                })
                .SingleOrDefault();

            if (xe == null)
            {
                return Json(new { code = 404, message = "Xe không tồn tại" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = 200, xe = xe }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Details_LoaiXe(int id)
        {
            var loaiXe = db.LOAIXE
                .Where(l => l.MaLoai == id) 
                .Select(l => new
                {
                    Id = l.MaLoai,
                    TenLoai = l.TenLoai,
                    DonGia = l.DonGia,
                    SoLuong = db.XE.Count( n=>n.MaLoai == l.MaLoai)
        })
                .SingleOrDefault();

            if (loaiXe == null)
            {
                return Json(new { code = 404, message = "Loại xe không tồn tại" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = 200, loaiXe = loaiXe }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update_LoaiXe(int maLoai, string tenLoai, int donGia)
        {
            try
            {
                var loaiXe = db.LOAIXE.SingleOrDefault(l => l.MaLoai == maLoai);
                if (loaiXe != null)
                {
                    loaiXe.TenLoai = tenLoai;
                    loaiXe.DonGia = donGia;
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy loại xe" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Có lỗi xảy ra: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}