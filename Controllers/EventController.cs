using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Context;
using EventEase.Services;
using Azure.Storage.Blobs;

namespace EventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<EventController> _logger;

        public EventController(
            ILogger<EventController> logger
            , ApplicationDbContext context
            , BlobServiceClient blobServiceClient
            , IBlobStorageService blobStorageService
        )
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching all events from the database.");
                // var events = await _context.Events.Include(e => e.Venue).ToListAsync();
                // return View(events);
                var events = await _context.Events
                    .Include(e => e.Venue)
                    .Include(e => e.EventType)
                    .AsNoTracking()
                    .ToListAsync();
                return View(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "TypeName"); // Added this line
            ViewData["FormAction"] = "Create";
            ViewData["SubmitButtonText"] = "Create";
            return View(
                "CreateEdit", 
                new Event { 
                    EventDate = DateTime.Now.AddDays(1),  
                    EndDate = DateTime.Now.AddDays(1).AddHours(2) // Default to 2 hours later
                });
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "TypeName", @event.EventTypeId); // Added this line
            ViewData["FormAction"] = "Edit";
            ViewData["SubmitButtonText"] = "Save";
            return View("CreateEdit", @event);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("EventId,EventName,EventDate,EndDate,Description,VenueId,EventTypeId")] Event @event
            , IFormFile? imageFile
        )
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Upload the image to Azure Blob Storage
                var blobName = $"events/{Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName)}";
                _logger.LogInformation($"Blob name: {blobName}\n");
                await _blobStorageService.UploadImageAsync(imageFile, blobName);
                @event.ImageUrl = blobName;
            } else {
                _logger.LogInformation("Image file is null or empty: {imageFile}", imageFile);
            }
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "TypeName", @event.EventTypeId); // Added this line
            ViewData["FormAction"] = "Create";
            ViewData["SubmitButtonText"] = "Create";
            return View("CreateEdit", @event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, 
            [Bind("EventId,EventName,EventDate,EndDate,Description,VenueId,EventTypeId")] Event @event
            , IFormFile? imageFile
        )
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                // Upload the image to Azure Blob Storage
                var blobName = $"events/{Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName)}";
                _logger.LogInformation($"Blob name: {blobName}\n");
                await _blobStorageService.UploadImageAsync(imageFile, blobName);
                @event.ImageUrl = blobName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);

            //Include Event Type info in Event view select dropdown
            ViewData["EventTypeId"] = new SelectList(
                _context.EventTypes,
                "EventTypeId", "TypeName",
                @event.EventTypeId); // Added this line

            ViewData["FormAction"] = "Edit";
            ViewData["SubmitButtonText"] = "Save";
            return View("CreateEdit", @event);
        }

        [HttpGet("/image/events/{blobName}")]
        public async Task<IActionResult> GetImage(string blobName)
        {
            try 
            {
                _logger.LogInformation("Getting image: {blobName}\n", blobName);
                var containerClient = _blobServiceClient.GetBlobContainerClient("event-ease");
                var blobClient = containerClient.GetBlobClient($"events/{blobName}");
                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;
                
                // Detect content type from extension
                var contentType = GetContentType(blobName);
                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving image");
                return NotFound();
            }
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);
                
            if (@event == null)
            {
                return NotFound();
            }

            ViewData["ViewMode"] = "Details";
            return View("DetailDelete", @event);
        }
        
        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);
                
            if (@event == null)
            {
                return NotFound();
            }

            // Check for existing bookings by querying the Bookings table
            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.EventId == id);
                
            if (hasBookings)
            {
                ViewData["ErrorMessage"] = "This event cannot be deleted because it has existing bookings. " + 
                                        "Please cancel all bookings first.";
            }

            ViewData["ViewMode"] = "Delete";
            return View("DetailDelete", @event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // First check for bookings
            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.EventId == id);
                
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete event with existing bookings.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            try
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the event. " +
                                        "It may have associated bookings that prevent deletion.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception)
            {
                // Catch any other unexpected errors
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the event.";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}