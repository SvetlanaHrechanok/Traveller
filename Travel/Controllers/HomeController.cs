using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel.Models;
using System.Text;

namespace Travel.Controllers
{
    public class HomeController : Controller
    {
        private TravelDBModelEntities travelDB = new TravelDBModelEntities();

        public ActionResult Index()
        {
            ViewBag.Travels = travelDB;
            return View();
        }

        public StringBuilder GetTours(Country country)
        {
            StringBuilder result = new StringBuilder();
            try
            {                
                IEnumerable<Hotel> hotels = travelDB.Hotels.Where(h => h.IdCountry == country.Id);
                IEnumerable<Tour> tours = travelDB.Tours;
                IEnumerable<Picture> pictures = travelDB.Pictures;
                int num = 0;

                foreach (Hotel h in hotels)
                {
                    foreach (Tour t in tours)
                    {
                        if (t.IdHotel == h.Id)
                        {
                            num++;
                        }
                    }
                }

                if (num != 0)
                {
                    foreach (Hotel hotel in hotels)
                    {
                        foreach (Tour tour in tours.Where(t => t.IdHotel == hotel.Id))
                        {
                            Picture currentPictures = pictures.FirstOrDefault(p => p.Id == hotel.IdPicture);
                            result.AppendLine("" +
                            "<div class='col-sm-6 col-md-4'>" +
                                "<div class='thumbnail'>" +
                                    "<img src = '" + currentPictures.NamePicture + "' alt='" + currentPictures.NamePicture + "'>" +
                                    "<div class='caption'>" +
                                        "<h4>" + hotel.NameHotel + "</h4>" +
                                        "<h5>" + tour.DateArrival.GetValueOrDefault().ToShortDateString() + "</h5>" +
                                        "<p>" + hotel.Price + "$</p>" +
                                        "<p>" + tour.AmountDay + " days</p>" +
                                        "<p>" + tour.Cost(hotel) + "$</p>" +
                                        "<div class='buttonsTour'>" +
                                            "<a href = '/Home/Edit?id=" + tour.Id + "' class='btn btn-primary'>Edit tour</a>" +
                                            "<button class='btn btn-danger deleteTour'>Delete</button>" +
                                            "<a href = '/Home/ReadMore?id=" + tour.Id + "' > READ MORE... </a>" +
                                            "<span class='idTour' style='display: none;'>" + tour.Id + "</span>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>");
                        }
                    }
                }
                else
                {
                    result.AppendLine("" +
                            "<div class='col-sm-12'>" +
                                "<div>" +
                                    "<h4>Tours are not in this country!</h4>" +
                                "</div>" +
                            "</div>");
                }
            }
            catch(ArgumentException ex)
            {
                result.AppendLine("Error" + ex.ToString());
            }
            catch(NullReferenceException ex)
            {
                result.AppendLine("Error" + ex.ToString());
            }

            return result;
        }

        public StringBuilder GetCountries()
        {
            StringBuilder result = new StringBuilder();
            IEnumerable<Country> countries = travelDB.Countries;

            if(countries.Count() != 0)
            {
                foreach(Country country in countries)
                {
                    result.AppendLine("" +
                    "<div class='country'>" +
                        "<h3>" + country.NameCountry + "</h3>" +
                    "</div>" +
                    "<div class='aboutCountryTours' style='display: none;'>" +
                        "<div class='row tour'>" +
                            GetTours(country) +
                        "</div>" +
                    "</div>");
                }
            }
            else
            {
                result.AppendLine("Countries is not founded!");
            }
            
            return result;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Svetlana Hrechanok =)";

            return View();
        }

        [HttpGet]
        public ActionResult ReadMore(int id)
        {
            Tour currentTour = travelDB.Tours.FirstOrDefault(t => t.Id == id);
            if(currentTour != null)
            {
                Hotel currentHotel = travelDB.Hotels.FirstOrDefault(h => h.Id == currentTour.IdHotel);
                Picture currentPicture = travelDB.Pictures.FirstOrDefault(p => p.Id == currentHotel.IdPicture);


                ViewBag.Hotel = currentHotel;
                ViewBag.NamePicture = currentPicture.NamePicture;
                ViewBag.NameHotel = currentHotel.NameHotel;
                ViewBag.DateArrival = currentTour.DateArrival.GetValueOrDefault().ToShortDateString();
                ViewBag.Price = currentHotel.Price;
                ViewBag.Amountday = currentTour.AmountDay;
                ViewBag.Cost = currentTour.Cost(currentHotel);
                ViewBag.AboutHotel = currentHotel.AboutHotel;
                return View();
            }
            else
            {
                ViewBag.Message = "Page is not found";
                return View("~/Views/Home/Message.cshtml");
            }
        }

        public StringBuilder GetIndicator(Hotel hotel)
        {
            StringBuilder result = new StringBuilder();
            int number = 0;
            IEnumerable<Picture> pictures = travelDB.Pictures.Where(p => p.IdHotel == hotel.Id);
            string classPicture = "active";

            if (pictures.Count() == 1)
            {
                result.AppendLine("<li data-target='#myCarousel' data-slide-to='" + number + "' class='" + classPicture + "'></ li > ");
            }
            else
            {
                foreach (Picture picture in pictures)
                {
                    if (picture.Id == hotel.IdPicture)
                    {
                        classPicture = "active";
                    }
                    else
                    {
                        classPicture = "";
                    }
                
                    result.AppendLine("<li data-target='#myCarousel' data-slide-to='" + number +"' class='" + classPicture + "'></ li > ");
                    number++;
                }
            }
            
            return result;
        }

        public StringBuilder GetCarousel(Hotel hotel)
        {
            StringBuilder result = new StringBuilder();
            IEnumerable<Picture> pictures = travelDB.Pictures.Where(p => p.IdHotel == hotel.Id);
            string classPicture = "active";

            if(pictures.Count() == 1)
            {
                result.AppendLine("" +
                            "<div class='item " + classPicture + " slide'>" +
                                 "<img src = '" + pictures.FirstOrDefault().NamePicture + "' class='img_slide'>" +
                            "</div>");
            }
            else
            {
                foreach (Picture picture in pictures)
                {
                    if ( picture.Id == hotel.IdPicture )
                    {
                        classPicture = "active";
                    }
                    else
                    {
                        classPicture = "";
                    }

                    result.AppendLine("" +
                            "<div class='item " + classPicture + " slide'>" +
                                 "<img src = '" + picture.NamePicture + "' class='img_slide'>" +
                            "</div>");
                }
            }

            return result;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            Tour currentTour = travelDB.Tours.FirstOrDefault(t => t.Id == id);
            if (currentTour != null)
            {
                Hotel currentHotel = travelDB.Hotels.FirstOrDefault(h => h.Id == currentTour.IdHotel);
                Picture currentPictures = travelDB.Pictures.FirstOrDefault(p => p.Id == currentHotel.IdPicture);

                ViewBag.NamePicture = currentPictures.NamePicture;
                ViewBag.NameHotel = currentHotel.NameHotel;
                ViewBag.DateArrival = currentTour.GetDateToString();
                ViewBag.Price = currentHotel.Price;
                ViewBag.Amountday = currentTour.AmountDay;
                ViewBag.AboutHotel = currentHotel.AboutHotel;
                ViewBag.Cost = currentTour.Cost(currentHotel);
                return View();
            }
            else
            {
                ViewBag.Message = "Page is not found";
                return View("~/Views/Home/Message.cshtml");
            }
        }

        [HttpPost]
        public string Upload(HttpPostedFileBase upload, int idTour)
        {
            string result = "";
            try
            {
                if (upload != null)
                {
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    upload.SaveAs(Server.MapPath("~/Content/images/" + fileName));

                    Tour tour = travelDB.Tours.FirstOrDefault(t => t.Id == idTour);
                    Hotel hotel = travelDB.Hotels.FirstOrDefault(h => h.Id == tour.IdHotel);

                    Picture newPicture = new Picture
                    {
                        NamePicture = "/Content/images/" + fileName,
                        IdHotel = hotel.Id
                    };
                    travelDB.Pictures.Add(newPicture);
                    travelDB.SaveChanges();

                    hotel.IdPicture = newPicture.Id;
                    travelDB.SaveChanges();

                    result = "The picture is added!";
                }
                else
                {
                    result = "The picture is  NOT added!!";
                }
            }
            catch(HttpException ex)
            {
                result = "Error!" + ex.ToString();
            }

            return result;
        }

        [HttpPost]
        public ActionResult Edit(int id, string nameHotel, DateTime dateArrival, int price, string aboutHotel, int amountDay, HttpPostedFileBase upload)
        {

            Tour tour = travelDB.Tours.FirstOrDefault(t => t.Id == id);
            tour.DateArrival = dateArrival;
            tour.AmountDay = amountDay;
            travelDB.SaveChanges();

            Hotel hotel = travelDB.Hotels.FirstOrDefault(h => h.Id == tour.IdHotel);
            hotel.NameHotel = nameHotel;
            hotel.Price = price;
            hotel.AboutHotel = aboutHotel;
            travelDB.SaveChanges();

            ViewBag.Message = "The tour edit! " + Upload(upload, id);

            return View("~/Views/Home/Message.cshtml");
        }

        public StringBuilder SelectImg(int idTour)
        {
            StringBuilder result = new StringBuilder();
            Tour tour = travelDB.Tours.FirstOrDefault(t => t.Id == idTour);
            Hotel hotel = travelDB.Hotels.FirstOrDefault(h => h.Id == tour.IdHotel);
            IEnumerable<Picture> pictures = travelDB.Pictures.Where(p => p.IdHotel == hotel.Id);

            if(pictures.Count() != 0)
            {
                foreach(Picture picture in pictures)
                {
                    result.AppendLine("" +
                        "<div class='img_select'>" +
                                 "<img src = '" + picture.NamePicture + "' class='img_slide'>" +
                            "</div>");
                }
            }
            else
            {
                result.AppendLine("Pictures are not founded!");
            }
            return result;
        }

        [HttpGet]
        public string Delet(int id = 0)
        {
            string result = "";

            Tour tour = travelDB.Tours.FirstOrDefault(t => t.Id == id);
            if(tour != null)
            {
                travelDB.Tours.Remove(tour);
                travelDB.SaveChanges();
            }
            else
            {
                result = "Page is not found";
            }

            result = "This tour is delete";

            return result;
        }

        public ActionResult AddTour()
        {
            return View();
        }

        [HttpPost]
        public string AddTour(string nameCountry, string nameHotel, string dateArrival, int price, string aboutHotel, int amountDay, HttpPostedFileBase upload)
        {
            string result = "";
            try
            {
                int numCountry = travelDB.Countries.Count(c => c.NameCountry == nameCountry);

                if (numCountry == 0)
                {
                    Country newCountry = new Country
                    {
                        NameCountry = nameCountry
                    };
                    travelDB.Countries.Add(newCountry);
                    travelDB.SaveChanges();
                }

                int numHotel = travelDB.Hotels.Count(h => h.NameHotel == nameHotel);
                Country country = travelDB.Countries.FirstOrDefault(c => c.NameCountry == nameCountry);
                int numIdCountry = travelDB.Hotels.Count(h => h.IdCountry == country.Id);

                if ( numHotel == 0 || numIdCountry == 0)
                {
                    Hotel newHotel = new Hotel
                    {
                        IdCountry = country.Id,
                        IdPicture = 1,
                        NameHotel = nameHotel,
                        AboutHotel = aboutHotel,
                        Price = price
                    };
                    travelDB.Hotels.Add(newHotel);
                    travelDB.SaveChanges();
                }

                IEnumerable<Hotel> hotels = travelDB.Hotels.Where(h => h.IdCountry == country.Id);
                Hotel hotelForTour = hotels.FirstOrDefault(h => h.NameHotel == nameHotel);

                Tour newTour = new Tour
                {
                    IdHotel = hotelForTour.Id,
                    DateArrival = DateTime.Parse(dateArrival),
                    AmountDay = amountDay
                };
                travelDB.Tours.Add(newTour);
                travelDB.SaveChanges();

                result = "The tour add! " + Upload(upload, newTour.Id);
            }
            catch (ArgumentException ex)
            {
                result = ex.ToString();
            }
            catch(NullReferenceException ex)
            {
                result = ex.ToString();
            }

            return result;
        }
    }
} 