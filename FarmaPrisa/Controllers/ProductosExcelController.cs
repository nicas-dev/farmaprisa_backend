using ClosedXML.Excel;
using FarmaPrisa.Data;
using FarmaPrisa.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FarmaPrisa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosExcelController : ControllerBase
    {
        private readonly FarmaPrisaContext _context;

        public ProductosExcelController(FarmaPrisaContext context)
        {
            _context = context;
        }

        [HttpPost("importar-excel")]
        public async Task<IActionResult> ImportarExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Debe seleccionar un archivo Excel para importar.");

            if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Solo se permiten archivos con extensión .xlsx");

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1); // Primera hoja
                        var rows = worksheet.RowsUsed().Skip(1); // Saltar encabezado

                        var productos = new List<PlantillaProductos>();

                        foreach (var row in rows)
                        {
                            var cells = row.CellsUsed().Select(c => c.GetValue<string>().Trim()).ToList();

                            // Validar que tenga 4 columnas
                            if (cells.Count != 4)
                                throw new Exception($"Fila incompleta: Se esperaban 4 columnas, se encontraron {cells.Count}. Los campos son: {string.Join(",", cells)}");

                            var producto = new PlantillaProductos
                            {
                                Codigo = cells[0],
                                Nombre = cells[1],
                                Precio = int.Parse(cells[2]),
                                Stock = int.Parse(cells[3])
                            };

                            productos.Add(producto);
                        }

                        // Guardar en la base de datos
                        _context.PlantillaProductos.AddRange(productos);
                        await _context.SaveChangesAsync();
                    }
                }

                return Ok("Importación de productos desde Excel completada exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error durante la importación: " + ex.Message);
            }
        }
    }
}
