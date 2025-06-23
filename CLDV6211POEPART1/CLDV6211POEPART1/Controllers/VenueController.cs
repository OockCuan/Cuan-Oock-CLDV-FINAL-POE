using Azure.Storage.Blobs;
using CLDV6211POEPART1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEPART1.Controllers
{
    public class VenueController : Controller
    {
        private readonly POEDBcontext _context;

        public VenueController(POEDBcontext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()

        {
            var venue = await _context.Venue.ToListAsync();
            return View(venue);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                if (venue.ImageFile != null)
                {
                    var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);
                    venue.ImageURL= blobUrl;
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) {
                return NotFound();
            }

            var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueID == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var venue = await _context.Venue.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        //Actions taken when interacting with edit

        [HttpPost]


        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (venue.ImageFile != null)
                    {
                        
                        var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);

                       
                        venue.ImageURL = blobUrl;
                    }
                    else
                    {
                        
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venue updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        //Actions taken when interacting with delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) {
                return NotFound();
            }
            var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueID == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var venue = await _context.Venue.FindAsync(id);
            if (venue == null) { return NotFound(); }

            var hasBookings = await _context.Booking.AnyAsync(b => b.VenueID == id);
            if (hasBookings) {
                TempData["ErrorMessage"] = "Cannot delete this item because it has existing bookings";
                return RedirectToAction(nameof(Index));
            }

            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var venue = await _context.Venue.FindAsync(id);
        //    _context.Venue.Remove(venue);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
        {
            //connection string here
            

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = imageFile.ContentType
            };

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });
            }

            return blobClient.Uri.ToString();
        }
    }
    }

