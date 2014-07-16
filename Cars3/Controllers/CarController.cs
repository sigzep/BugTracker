using System;
using Cars3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cars3.Controllers
{
    public class CarController : Controller
    {
        //
        // GET: /Car/
        public ActionResult CarLister()
        {
            var cars = Session["Cars"];

            if (null == cars)
            {
                cars = new List<Car>();
                Session["Cars"] = cars;
            }
            return View(cars);
        }
       
        public ActionResult AddCar()
        {
            return View();
        }
        
        [HttpPost]    /* This is needed from using "Create" template code inserted for Add */
        [ValidateAntiForgeryToken]  /* same as code above */
        public ActionResult AddCar(Car car)
        {
            var cars = Session["Cars"] as List<Car>;    /* Car is going to be my Class */
            cars.Add(new Car() { Make = car.Make, Model = car.Model, Year = car.Year, InteriorColor = car.InteriorColor, ExternalColor = car.ExternalColor });
            /*  Make, Model know it's a property in class Car  */
            Session["Cars"] = cars;
            return RedirectToAction ("CarLister");
        }
	}
}