using AutoRealmProject.Backend.Data;
using AutoRealmProject.Backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoRealmProject.Frontend.Controllers
{
    public class CarAdsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        private static readonly Dictionary<string, List<string>> CarData = new Dictionary<string, List<string>>
        {
            { "Ford", new List<string> { "Fiesta", "Focus", "Mustang", "Explorer", "F-150" } },
            { "Chevrolet", new List<string> { "Spark", "Malibu", "Camaro", "Silverado", "Equinox" } },
            { "Toyota", new List<string> { "Auris", "Corolla", "Camry", "Prius", "Highlander" } },
            { "Honda", new List<string> { "Civic", "Accord", "CR-V", "Pilot", "Fit" } },
            { "Nissan", new List<string> { "Sentra", "Altima", "Maxima", "Rogue", "Frontier" } },
            { "BMW", new List<string> { "3 Series", "5 Series", "7 Series", "X3", "X5" } },
            { "Mercedes-Benz", new List<string> { "C-Class", "E-Class", "S-Class", "GLC", "GLE" } },
            { "Audi", new List<string> { "A3", "A4", "A6", "Q5", "Q7" } },
            { "Volkswagen", new List<string> { "Golf", "Passat", "Tiguan", "Jetta", "Atlas" } },
            { "Hyundai", new List<string> { "Elantra", "Sonata", "Tucson", "Santa Fe", "Kona" } },
            { "Kia", new List<string> { "Rio", "Optima", "Sorento", "Sportage", "Soul" } },
            { "Mazda", new List<string> { "Mazda3", "Mazda6", "CX-5", "CX-9", "MX-5 Miata" } },
            { "Subaru", new List<string> { "Impreza", "Outback", "Forester", "Crosstrek", "WRX" } },
            { "Lexus", new List<string> { "IS", "ES", "RX", "GX", "LS" } },
            { "Jaguar", new List<string> { "XE", "XF", "F-Pace", "E-Pace", "I-Pace" } },
            { "Land Rover", new List<string> { "Range Rover", "Discovery", "Defender", "Evoque", "Velar" } },
            { "Porsche", new List<string> { "911", "Cayenne", "Macan", "Panamera", "Taycan" } },
            { "Ferrari", new List<string> { "488", "Roma", "Portofino", "SF90", "812 Superfast" } },
            { "Lamborghini", new List<string> { "Huracan", "Aventador", "Urus", "Gallardo", "Veneno" } },
            { "Tesla", new List<string> { "Model S", "Model 3", "Model X", "Model Y", "Cybertruck" } },
            { "Volvo", new List<string> { "S60", "XC60", "V60", "XC90", "XC40" } },
            { "Acura", new List<string> { "ILX", "TLX", "RDX", "MDX", "NSX" } },
            { "Infiniti", new List<string> { "Q50", "Q60", "QX50", "QX70", "QX80" } },
            { "Cadillac", new List<string> { "CT4", "CT5", "XT4", "XT5", "Escalade" } },
            { "Buick", new List<string> { "Encore", "Enclave", "Envision", "Regal", "LaCrosse" } },
            { "Jeep", new List<string> { "Wrangler", "Cherokee", "Grand Cherokee", "Renegade", "Compass" } },
            { "Chrysler", new List<string> { "300", "Pacifica", "Voyager", "Aspen", "Town & Country" } },
            { "Dodge", new List<string> { "Charger", "Challenger", "Durango", "Journey", "Grand Caravan" } },
            { "Mitsubishi", new List<string> { "Outlander", "Lancer", "Eclipse Cross", "Mirage", "ASX" } },
            { "Alfa Romeo", new List<string> { "Giulia", "Stelvio", "4C", "8C", "Tonale" } },
            { "Fiat", new List<string> { "500", "500X", "500L", "Panda", "Tipo" } },
            { "Peugeot", new List<string> { "208", "3008", "5008", "308", "508" } },
            { "Citroën", new List<string> { "C3", "C4", "C5", "Cactus", "Berlingo" } },
            { "Renault", new List<string> { "Clio", "Megane", "Captur", "Kadjar", "Talisman" } },
            { "Opel", new List<string> { "Corsa", "Astra", "Insignia", "Mokka", "Crossland" } },
            { "Skoda", new List<string> { "Fabia", "Octavia", "Superb", "Karoq", "Kodiaq" } },
            { "Mini", new List<string> { "Cooper", "Countryman", "Clubman", "Paceman", "John Cooper Works" } },
            { "Bentley", new List<string> { "Continental", "Bentayga", "Flying Spur", "Mulsanne", "Azure" } },
            { "Rolls-Royce", new List<string> { "Phantom", "Ghost", "Wraith", "Dawn", "Cullinan" } },
            { "Maserati", new List<string> { "Ghibli", "Quattroporte", "Levante", "GranTurismo", "MC20" } },
            { "Aston Martin", new List<string> { "Vantage", "DB11", "DBX", "Rapide", "Valhalla" } },
            { "McLaren", new List<string> { "570S", "720S", "GT", "Artura", "P1" } },
            { "Bugatti", new List<string> { "Chiron", "Veyron", "Divo", "Centodieci", "Bolide" } },
            { "Pagani", new List<string> { "Huayra", "Zonda", "R", "Cinque", "Revolucion" } },
            { "Koenigsegg", new List<string> { "Agera", "Regera", "Jesko", "Gemera", "One:1" } },
        };

        public CarAdsController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CarAds.ToListAsync());
        }

        public async Task<IActionResult> Home()
        {
            return View(await _context.CarAds.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carAd = await _context.CarAds
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.AdId == id);

            if (carAd == null)
            {
                return NotFound();
            }

            return View(carAd);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(CarData.Keys);
            return View(new CarAd());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdId,Brand,Model,Year,Price,Mileage,City,Description,CarPhoto")] CarAd carAd, IFormFile photo)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                carAd.OwnerId = user.Id;

                if (string.IsNullOrEmpty(carAd.Description))
                {
                    carAd.Description = "No description";
                }

                if (photo == null || photo.Length == 0)
                {
                    ModelState.AddModelError("Photo", "Photo is required.");
                    ViewBag.Brands = new SelectList(CarData.Keys);
                    return View(carAd);
                }

                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    carAd.CarPhoto = memoryStream.ToArray();
                }

                _context.Add(carAd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Brands = new SelectList(CarData.Keys);
            return View(carAd);
        }

        [HttpGet]
        public JsonResult GetModels(string brand)
        {
            if (CarData.ContainsKey(brand))
            {
                return Json(CarData[brand]);
            }
            return Json(new List<string>());
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carAd = await _context.CarAds.FindAsync(id);
            ViewBag.Brands = new SelectList(CarData.Keys);
            if (carAd == null)
            {
                return NotFound();
            }
            return View(carAd);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdId,Brand,Model,Year,Price,Mileage,City,Description")] CarAd carAd, IFormFile newPhoto)
        {
            if (id != carAd.AdId)
            {
                return NotFound();
            }

            try
            {
                if (newPhoto != null && newPhoto.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await newPhoto.CopyToAsync(memoryStream);
                    carAd.CarPhoto = memoryStream.ToArray();
                }
                else
                {
                    var existingCarAd = await _context.CarAds.AsNoTracking().FirstOrDefaultAsync(c => c.AdId == id);
                    if (existingCarAd != null)
                    {
                        carAd.CarPhoto = existingCarAd.CarPhoto;
                    }
                }

                carAd.OwnerId = _userManager.GetUserId(User);

                _context.Update(carAd);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return View(carAd);
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carAd = await _context.CarAds
                .FirstOrDefaultAsync(m => m.AdId == id);
            if (carAd == null)
            {
                return NotFound();
            }

            return View(carAd);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carAd = await _context.CarAds.FindAsync(id);
            if (carAd != null)
            {
                _context.CarAds.Remove(carAd);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarAdExists(int id)
        {
            return _context.CarAds.Any(e => e.AdId == id);
        }
    }
}
