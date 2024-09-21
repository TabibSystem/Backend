using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;

namespace TabibApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMedicalHistoryService _medicalHistoryService;
        private readonly IWebHostEnvironment _IWebHostEnvironment;

        public MedicalHistoryController(IMedicalHistoryService medicalHistoryService, IWebHostEnvironment IWebHostEnvironment)
        {
            _medicalHistoryService = medicalHistoryService;
            _IWebHostEnvironment = IWebHostEnvironment;
        }

        /// <summary>
        /// Uploads a medical history file for a patient.
        /// </summary>
        /// <param name="dto">The medical history upload data transfer object.</param>
        /// <returns>A message indicating the result of the upload.</returns>
        /// <response code="200">If the file was uploaded successfully.</response>
        /// <response code="400">If no file was uploaded or the file upload failed.</response>
        /// <response code="404">If the patient ID is not found.</response>
        [Authorize("Patient")]
        [HttpPost("upload-medical-history")]

        public async Task<IActionResult> UploadMedicalHistory([FromForm] MedicalHistoryUploadDto dto)
        {
            if (dto.File is null || dto.File.Length == 0)
                return BadRequest("No file uploaded.");

            var result = await _medicalHistoryService.GetpatientId(dto.PatientId);
            if (result == 0)
                return NotFound();

            var fileUrl = await SaveFileAsync(dto.File, "MedicalHistory");
            if (fileUrl == null)
                return BadRequest("File upload failed.");

            var medicalHistoryRecord = new MedicalHistoryRecord
            {
                FileName = dto.File.FileName,
                FileUrl = fileUrl,
                UploadDate = DateTime.Now,
                PatientId = result.Value
            };

            await _medicalHistoryService.Add(medicalHistoryRecord);

            return Ok(new { message = "File uploaded successfully", fileUrl });
        }

        /// <summary>
        /// Gets the medical history records for a patient.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <returns>A list of medical history records for the patient.</returns>
        /// <response code="200">Returns the list of medical history records.</response>
        [Authorize("Doctor,Patient")]
        [HttpGet("patient/{patientId}/medical-history")]
        [ProducesResponseType(typeof(IEnumerable<MedicalHistoryRecord>), 200)]
        public async Task<IActionResult> GetMedicalHistoryRecords(string patientId)
        {
            var records = await _medicalHistoryService.Get(patientId);
            return Ok(records);
        }

        /// <summary>
        /// Saves a file to the specified folder.
        /// </summary>
        /// <param name="file">The file to save.</param>
        /// <param name="folderName">The name of the folder to save the file in.</param>
        /// <returns>The URL of the saved file.</returns>
        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            try
            {
                var uploadsFolder = Path.Combine(_IWebHostEnvironment.WebRootPath, folderName);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var publicUrl = $"/{folderName}/{fileName}";
                return publicUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}