using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using Project_OOD.Models;
namespace Project_OOD.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        QL_ThueXeEntities db = new QL_ThueXeEntities();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return RedirectToAction("Login","Admin");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Header_Admin()
        {
            return View();
        }
        public ActionResult Menu_Admin()
        {
            return View();
        }
        //GET: Admin/Login
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["Username"];
            var sMatKhau = f["Password"];
            if (!ModelState.IsValid)
            {
                ViewBag.ThongBao = "Mã xác nhận Captcha nhập vào không đúng!";
                return View("Login");
            }else if (db.NGUOIDUNG.SingleOrDefault( n=> n.UserName == sTenDN && n.Pass == sMatKhau) == null)
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
            }
            else
            {
                NGUOIDUNG ng = db.NGUOIDUNG.SingleOrDefault(n => n.UserName == sTenDN && n.Pass == sMatKhau);
                if(ng.QUYENTRUYCAP.TenQTC == "Admin")
                {
                    Session["Admin"] = ng;
                    return RedirectToAction("Index","Admin");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không chính xác";
                    return View();
                }
            }
            return View("Login");
        }
        
    }
}