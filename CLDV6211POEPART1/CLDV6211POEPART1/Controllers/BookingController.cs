using CLDV6211POEPART1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEPART1.Controllers
{
    public class BookingController : Controller
    {
        private readonly POEDBcontext _context;
        public BookingController(POEDBcontext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)

        {
            var bookings = _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString)) {
                bookings = bookings.Where(b=> b.Venue.VenueName.Contains(searchString) || b.Event.EventName.Contains(searchString));
            }

            return View(await bookings.ToListAsync()); 
            //var booking = await _context.Booking.ToListAsync();
            //return View(booking);
           
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var selectedEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventID == booking.EventID);

            if (selectedEvent == null)
            {
                ModelState.AddModelError("", "Selected event not found.");
                ViewData["Events"] = _context.Event.ToList();
                ViewData["Venues"] = _context.Venue.ToList();
                return View(booking);
            }


            var conflict = await _context.Booking
                .Include(b => b.Event)
                .AnyAsync(b => b.VenueID == booking.VenueID &&
                               b.Event.EventDate.Date == selectedEvent.EventDate.Date);

            if (conflict)
            {
                ModelState.AddModelError("", "This venue is already booked for that date.");
                ViewData["Events"] = _context.Event.ToList();
                ViewData["Venues"] = _context.Venue.ToList();
                return View(booking);
            }
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);

        }

        public async Task<IActionResult> Details(int? id)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingID == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        //Actions taken when interacting with edit

        [HttpPost]


        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        //Actions taken when interacting with delete
        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
