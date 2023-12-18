using Project_OOD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_OOD.Controllers
{
    public class UsersController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        // GET: User/User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var TenDN = f["sTenDN"];
            var MatKhau = f["sMatKhau"];
            NGUOIDUNG nd = db.NGUOIDUNG.SingleOrDefault(n => n.UserName == TenDN && n.Pass == MatKhau);
            if (nd != null)
            {
                ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công!";
                Session["Taikhoan"] = nd;
                return RedirectToAction("Index", "User", new { area = "User"});
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection f, NGUOIDUNG nd)
        {
            var HoTen = f["sHoTen"];
            var Email = f["sEmail"];
            var TenDN = f["sTenDN"];
            var MatKhau = f["sMatKhau"];
            var MatKhauNL = f["sMatKhauNL"];
            var DiaChi = f["sDiaChi"];
            var GioiTinh = f["sGioiTinh"];
            if (db.NGUOIDUNG.SingleOrDefault(n => n.UserName == TenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại!";
            }
            else if (db.NGUOIDUNG.SingleOrDefault(n => n.Email == Email) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng!";
            }
            else if (MatKhauNL != MatKhau)
            {
                ViewBag.ThongBao = "Mật khẩu nhập lại chưa đúng!";
            }
            else if (ModelState.IsValid)
            {
                nd.TenNguoiDung = HoTen;
                nd.Email = Email;
                nd.UserName = TenDN;
                nd.GioiTinh = "Nam";
                nd.DiaChi = "Việt Nam";
                nd.Pass = MatKhau;

                db.NGUOIDUNG.Add(nd);
                db.SaveChanges();
                return RedirectToAction("Login", "Users");
            }
            return View();
        }

    }
}