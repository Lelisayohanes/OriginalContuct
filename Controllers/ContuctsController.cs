using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/contacts")] // Route for this controller
    public class ContuctsController : Controller
    {
        private readonly CRUDDBContext dbContext;

        // Constructor to inject the database context
        public ContuctsController(CRUDDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> getContucts()
        {
            // Get all contacts from the database and return them
            return Ok(await dbContext.Contucts.ToListAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> getSingleContuct([FromRoute] Guid id)
        {
            // Find a single contact by its unique identifier (id)
            var contuct = await dbContext.Contucts.FindAsync(id);

            if (contuct == null)
            {
                // Contact not found, return a 404 (Not Found) response
                return NotFound();
            }

            // Return the found contact as a JSON response
            return Ok(contuct);
        }

        [HttpPost]
        public async Task<IActionResult> AddContuct(AddContuct addContuct)
        {
            // Create a new contact from the request data and add it to the database
            var contuct = new Contuct()
            {
                Id = Guid.NewGuid(),
                Address = addContuct.Address,
                Email = addContuct.Email,
                FullName = addContuct.FullName,
                Phone = addContuct.Phone
            };

            await dbContext.Contucts.AddAsync(contuct);
            await dbContext.SaveChangesAsync();

            // Return the newly added contact as a JSON response
            return Ok(contuct);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateContuct(Guid id, [FromBody] UpdateContuct updateContuct)
        {
            if (updateContuct == null)
            {
                // Invalid input data, return a 400 (Bad Request) response
                return BadRequest("Invalid input data.");
            }

            // Find the contact to update by its unique identifier (id)
            var contuct = await dbContext.Contucts.FindAsync(id);

            if (contuct == null)
            {
                // Contact not found, return a 404 (Not Found) response
                return NotFound("Contact not found.");
            }

            // Update the contact properties with the provided data
            contuct.FullName = updateContuct.FullName;
            contuct.Phone = updateContuct.Phone;
            contuct.Address = updateContuct.Address;
            contuct.Email = updateContuct.Email;

            try
            {
                // Save changes to the database
                await dbContext.SaveChangesAsync();

                // Return the updated contact as a JSON response
                return Ok(contuct);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a 500 (Internal Server Error) response
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> deleteContuct([FromRoute] Guid id)
        {
            // Find the contact to delete by its unique identifier (id)
            var contuct = await dbContext.Contucts.FindAsync(id);

            if (contuct == null)
            {
                // Contact not found, return a 404 (Not Found) response
                return NotFound("Contact not found.");
            }

            // Remove the contact from the database and save changes
            dbContext.Remove(contuct);
            await dbContext.SaveChangesAsync();

            // Return the deleted contact as a JSON response
            return Ok(contuct);
        }
    }
}
