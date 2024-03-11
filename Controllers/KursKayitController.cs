using KursKayitApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KursKayitApp.Controllers
{
    public class KursKayitController : Controller
    {
        private readonly DataContext _context;
        public KursKayitController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var kursKayitlari = await _context.KursKayitlari
                        .Include(x => x.Ogrenci) // Include aracılığı ile KursKayit tablosunda prop olarak bulunan Ogrenci Tablosundaki tüm bilgilere erişimi sağlanır
                        .Include(x => x.Kurs)
                        .ToListAsync();
            return View(kursKayitlari);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Ogrenciler = new SelectList(await _context.Ogrenciler.ToListAsync(), "OgrenciId", "OgrenciAdSoyad"); // value bilgisi, Text bilgisi
            ViewBag.Kurslar = new SelectList(await _context.Kurslar.ToListAsync(), "KursId", "Baslik");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(KursKayit model)
        {
            model.KayitTarihi = DateTime.Now;
            _context.KursKayitlari.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kayit = await _context
            .KursKayitlari
            .Include(m => m.Kurs)
            .Include(m => m.Ogrenci)
            .FirstOrDefaultAsync(m => m.KayitId == id);
            if (kayit == null)
            {
                return NotFound();
            }
            return View(kayit);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(KursKayit model)
        {
            if (model == null)
            {
                return NotFound();
            }

            var kayit = await _context.KursKayitlari.FirstOrDefaultAsync(m => m.KayitId == model.KayitId);
            if (kayit == null)
            {
                return NotFound();
            }

            _context.KursKayitlari.Remove(kayit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Ogrenciler = new SelectList(await _context.Ogrenciler.ToListAsync(), "OgrenciId", "OgrenciAdSoyad");
            ViewBag.Kurslar = new SelectList(await _context.Kurslar.ToListAsync(), "KursId", "Baslik");
            if (id == null)
            {
                return NotFound();
            }
            var kayit = await _context.KursKayitlari.FirstOrDefaultAsync(m => m.KayitId == id);
            if (kayit == null)
            {

                return NotFound();
            }
            return View(kayit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursKayit model)
        {
            if (id != model.KayitId )
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}