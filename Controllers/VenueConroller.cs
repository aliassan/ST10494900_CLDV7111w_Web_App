using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Context;
using EventEase.Models;
using EventEase.Services;
using Azure.Storage.Blobs;

namespace EventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;
        private readonly BlobServiceClient _blobServiceClient;

        public VenueController(
            ApplicationDbContext context, 
            BlobServiceClient blobServiceClient, 
            IBlobStorageService blobStorageService
        )
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _blobServiceClient = blobServiceClient;
        }

        // GET: Venues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            ViewData["ViewMode"] = "Details";
            return View("DetailDelete", venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            ViewData["FormAction"] = "Create";
            ViewData["SubmitButtonText"] = "Create";
            return View("CreateEdit", new Venue());
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            ViewData["FormAction"] = "Edit";
            ViewData["SubmitButtonText"] = "Save";
            return View("CreateEdit", venue);
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue
            , IFormFile? imageFile
        )
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Upload the image to Azure Blob Storage
                var blobName = $"events/{Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName)}";
                await _blobStorageService.UploadImageAsync(imageFile, blobName);
                @venue.ImageUrl = blobName;
            }
            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["FormAction"] = "Create";
            ViewData["SubmitButtonText"] = "Create";
            return View("CreateEdit", venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, 
            [Bind("VenueId,VenueName,Location,Capacity,ImageUrl")] Venue venue,
            IFormFile? imageFile
        )
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                // Upload the image to Azure Blob Storage
                var blobName = $"events/{Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName)}";
                await _blobStorageService.UploadImageAsync(imageFile, blobName);
                @venue.ImageUrl = blobName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["FormAction"] = "Edit";
            ViewData["SubmitButtonText"] = "Save";
            return View("CreateEdit", venue);
        }

       // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            ViewData["ViewMode"] = "Delete";
            return View("DetailDelete", venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}