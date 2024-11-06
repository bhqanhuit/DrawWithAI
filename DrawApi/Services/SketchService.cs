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

namespace DrawApi.Services
{
    public interface ISketchService
    {
        Task<int> GetLatestSketch();
        Task<Sketch?> InsertSketchToDatabase(Sketch sketch);
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

        public async Task<Sketch?> InsertSketchToDatabase(Sketch newSketch)
        {
            newSketch.UploadAt = DateTime.UtcNow;
            _context.Sketches.Add(newSketch);
            await _context.SaveChangesAsync();

            return newSketch;
        }

    }
}
