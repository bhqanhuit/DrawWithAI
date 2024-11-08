using System;
using System.Linq;
using System.Threading.Tasks;
using DrawApi.Models;
using Microsoft.EntityFrameworkCore;
using DrawApi.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DrawApi.Services
{
    public interface ISketchService
    {
        Task<int> GetLatestSketch();
        Task<Sketch?> InsertSketchToDatabase(Sketch sketch);
        Task<int> GetLastedGeneratedImage();
        Task<GeneratedImage?> InsertGeneratedImageToDatabase(GeneratedImage generatedImage);
    }
    public class SketchService : ISketchService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public SketchService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int> GetLatestSketch()
        {
            return await _context.Sketches.MaxAsync(u => u.SketchId);
        }

        public async Task<int> GetLastedGeneratedImage()
        {   
            return await _context.GeneratedImages.MaxAsync(u => u.ImageId);
        }

        public async Task<Sketch?> InsertSketchToDatabase(Sketch newSketch)
        {
            newSketch.UploadAt = DateTime.UtcNow;
            _context.Sketches.Add(newSketch);
            await _context.SaveChangesAsync();
            return newSketch;
        }

        public async Task<GeneratedImage?> InsertGeneratedImageToDatabase(GeneratedImage generatedImage)
        {
            _context.GeneratedImages.Add(generatedImage);
            await _context.SaveChangesAsync();
            return generatedImage;

        }


    }
}
