using Microsoft.EntityFrameworkCore;
using softire.api.Models;
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
  
namespace softire.api
{  
    public class SnModeloNeumaticoRepository : ISnModeloNeumaticoRepository
    {  
        private readonly NeumaticosContext _context;  
        public SnModeloNeumaticoRepository(NeumaticosContext context)  
        {  
            _context = context;  
        }

        public Task<List<SnModeloNeumatico>> GetModeloNeumaticoAll()
        {
            return _context.SnModeloNeumatico.ToListAsync();
        }


        // public Task<SnNeumaticosDet> GetNeumaticosDet(string id)  
        // {  
        //     return _context.SnNeumaticosDet.FirstOrDefault(x => x. == id));  
        // }  
        // public Task<SnNeumaticosDet> GetNeumaticosDetById(string id)
        // {
        //     return _context.SnNeumaticosDet.SingleAsync(x => x.CodNeumatico == id);
        // }
    }  
}