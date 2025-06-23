using CLDV6211POEPART1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CLDV6211POEPART1.Controllers
{
    public class EventController : Controller
    {
        private readonly POEDBcontext _context;

        public EventController(POEDBcontext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchType, int? venueId, DateTime? startDate, DateTime? endDate)
            
        {
            //var evente = await _context.Event.ToListAsync();
            var evente = _context.Event
                .Include(e => e.Venue)
                .AsQueryable();

            if (venueId.HasValue)
            {
                evente = evente.Where(e => e.VenueID == venueId);
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                evente = evente.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);
            }

            ViewData["Venues"] = _context.Venue.ToList();
            return View(evente);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //Actions taken when interacting with create
        public async Task<IActionResult> Create(Event evente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EventTypes"] = _context.EventType.ToList();
            return View(evente);

        }
//Actions taken when interacting with details
        public async Task<IActionResult> Details(int? id){
            var evente = await _context.Event.FirstOrDefaultAsync(m => m.EventID == id);
            if (evente == null){
                return NotFound();}
            return View(evente);
        }
        private bool EventExists(int id) {
            return _context.Event.Any(e => e.EventID == id);
        }
        public async Task<IActionResult> Edit(int? id){
            if (id == null){
                return NotFound();
            }
            var evente = await _context.Event.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            //ViewData["EventTypes"] = _context.EventType.ToList();
            return View(evente);
        }
        //Actions taken when interacting with edit

        [HttpPost]
        

        public async Task<IActionResult> Edit(int id, Event evente){
            if (id != evente.EventID){
                return NotFound();
            }

            if (ModelState.IsValid){
                try{
                    _context.Update(evente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!EventExists(evente.EventID)){
                        return NotFound();
                    }
                    else{
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(evente);
        }

        //Actions taken when interacting with delete
        public async Task<IActionResult> Delete(int? id)
        {
            var evente = await _context.Event.FirstOrDefaultAsync(m => m.EventID == id);
            if (evente == null){
                return NotFound();
            }
            return View(evente);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evente = await _context.Event.FindAsync(id);
            if (evente == null) return NotFound();

            var hasBookings = await _context.Booking.AnyAsync(b => b.EventID == id);
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this item because it has existing bookings";
                return RedirectToAction(nameof(Index));
            }

            _context.Event.Remove(evente);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Deleted successfully";
            return RedirectToAction(nameof(Index));
        }


        //[HttpPost]
        //public async Task<IActionResult> Delete(int id){
        //    var evente = await _context.Event.FindAsync(id);
        //    _context.Event.Remove(evente);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
