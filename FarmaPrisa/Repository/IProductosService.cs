//using FarmaPrisa.Data;
//using FarmaPrisa.Models.Dtos;
//using FarmaPrisa.Models.Dtos.OpinionProducto;
//using FarmaPrisa.Models.Dtos.Producto;
//using FarmaPrisa.Models.Dtos.Proveedor;
//using FarmaPrisa.Models.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using System.Globalization;

//namespace FarmaPrisa.Services
//{
//    public interface IProductosService
//    {
//        Task<IEnumerable<ProductoDto>> GetProductosAsync(int? categoriaId, int? proveedorId, int? sintomaId);
//        Task<ProductoDetalleDto?> GetProductoDetalleAsync(int id, string idioma);
//        Task<int> CreateProductoAsync(ProductoCreateDto dto);
//        Task<bool> UpdateProductoAsync(int id, ProductoUpdateDto dto);
//        Task<bool> ToggleProductoStatusAsync(int id);
//        Task<bool> SetImagenPrincipalAsync(int productoId, int imagenId);
//        Task<ImportResultDto> ImportarProductosAsync(Stream fileStream, string fileType);
//        Task<bool> AddImagenesGaleriaAsync(int productoId, ICollection<IFormFile> archivos);
//        Task<IEnumerable<ProveedorDto>> GetProveedoresAsync();
//        Task<bool> DeleteImagenGaleriaAsync(int imagenId);
//        Task<string> UploadFileAsync(IFormFile file);
//        Task<PagedResultDto<ProductoVencimientoDto>> GetInventarioPaginadoAsync(int page, int pageSize, string? searchTerm);
//        Task<bool> CreateOpinionAsync(int productoId, int userId, OpinionCreateDto dto);
//        Task<IEnumerable<OpinionDto>> GetOpinionesAdminAsync();
//        Task<IEnumerable<OpinionDto>> GetOpinionesByProductoIdAsync(int productoId);
//    }

//    // Services/ProductosService.cs
//    public class ProductosService : IProductosService
//    {
//        // Inyectamos directamente el contexto de la BD
//        private readonly FarmaPrisaContext _context;
//        private readonly ILogger<ProductosService> _logger;

//        public ProductosService(FarmaPrisaContext context, ILogger<ProductosService> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public async Task<IEnumerable<ProductoDto>> GetProductosAsync(int? categoriaId, int? proveedorId, int? sintomaId)
//        {
//            var query = _context.Productos.AsQueryable();

//            // 1. Aplicar filtros condicionales
//            if (categoriaId.HasValue)
//            {
//                query = query.Where(p => p.CategoriaId == categoriaId.Value);
//            }

//            if (proveedorId.HasValue)
//            {
//                query = query.Where(p => p.ProveedorId == proveedorId.Value);
//            }

//            // El filtro por síntoma requiere unir con la tabla pivote ProductoSintoma
//            if (sintomaId.HasValue)
//            {
//                query = query.Where(p => p.ProductoSintomas.Any(ps => ps.SintomaId == sintomaId.Value));
//            }

//            // 2. Incluir entidades relacionadas para el DTO
//            var productos = await query
//                .Include(p => p.Proveedor)
//                .Include(p => p.ImagenesGaleria)
//                .ToListAsync();

//            // 3. Mapeo (transformación) de la Entidad a DTO
//            return productos.Select(p =>
//            {
//                // Lógica para encontrar la ÚNICA imagen principal (EsPrincipal=true > Orden=1)
//                var imagenPrincipalEntity = p.ImagenesGaleria
//                    .FirstOrDefault(img => img.EsPrincipal == true) ??
//                    p.ImagenesGaleria.OrderBy(img => img.Orden).FirstOrDefault();

//                // Creamos una lista que SOLO contiene el DTO de la imagen principal (si existe)
//                var listaPrincipal = new List<ProductoImagenDto>();

//                if (imagenPrincipalEntity != null)
//                {
//                    listaPrincipal.Add(new ProductoImagenDto
//                    {
//                        Id = imagenPrincipalEntity.Id,
//                        UrlImagen = imagenPrincipalEntity.UrlImagen,
//                        Orden = imagenPrincipalEntity.Orden,
//                        EsPrincipal = imagenPrincipalEntity.EsPrincipal
//                    });
//                }

//                return new ProductoDto
//                {
//                    Id = p.Id,
//                    Nombre = p.Nombre,
//                    Precio = p.Precio,

//                    ImagenesGaleria = listaPrincipal,

//                    RequiereReceta = p.RequiereReceta ?? false,
//                    ProveedorNombre = p.Proveedor?.Nombre,
//                    EstaActivo = p.EstaActivo
//                };
//            }).ToList();
//        }

//        public async Task<ProductoDetalleDto?> GetProductoDetalleAsync(int id, string idioma)
//        {
//            // Carga el producto por ID e incluye todas las relaciones necesarias
//            var producto = await _context.Productos
//                .Include(p => p.Proveedor)
//                .Include(p => p.Detalles) // Incluye la tabla producto_detalle
//                    .ThenInclude(d => d.TipoDetalle) // Asegura cargar el tipo de detalle
//                .Include(p => p.OpinionesProductos) // Incluye las opiniones
//                    .ThenInclude(op => op.Usuario) // Asegura cargar el nombre del usuario que opina
//                .Include(p => p.ImagenesGaleria.OrderBy(img => img.Orden))
//                .FirstOrDefaultAsync(p => p.Id == id);

//            if (producto == null)
//            {
//                return null;
//            }

//            // Mapeo de la Entidad a DTO
//            return new ProductoDetalleDto
//            {
//                Id = producto.Id,
//                Nombre = producto.Nombre,
//                Descripcion = producto.Detalles
//                                .Where(d => d.TipoDetalle.IdentificadorUnico == "DESCRIPTION" && d.Idioma == idioma)
//                                .Select(d => d.valor_detalle)
//                                .FirstOrDefault() ?? "No disponible en este idioma.",
//                Precio = producto.Precio,
//                PrecioAnterior = producto.PrecioAnterior,                
//                Sku = producto.Sku,
//                RequiereReceta = producto.RequiereReceta ?? false,
//                ProveedorNombre = producto.Proveedor?.Nombre ?? "N/A",
//                EstaActivo = producto.EstaActivo,

//                ImagenesGaleria = producto.ImagenesGaleria
//                    .OrderBy(img => img.Orden) // Se ordenan para mostrar la miniatura primero
//                    .Select(img => new ProductoImagenDto
//                    {
//                        Id = img.Id,
//                        UrlImagen = img.UrlImagen,
//                        Orden = img.Orden,
//                        EsPrincipal = img.EsPrincipal
//                    })
//                    .ToList(),

//                // Mapeo de Colecciones (producto_detalle)
//                Especificaciones = producto.Detalles
//                .Where(d => d.Idioma == idioma && d.TipoDetalle.IdentificadorUnico != "DESCRIPTION")
//                .Select(d => new ProductoDetalleItemDto
//                {
//                    TipoDetalle = d.TipoDetalle.IdentificadorUnico,
//                    ValorDetalle = d.valor_detalle,
//                    MediaUrl = d.MediaUrl
//                }).ToList(),

//                // Mapeo de Opiniones
//                Opiniones = producto.OpinionesProductos.Select(op => new OpinionDto
//                {
//                    Calificacion = op.Calificacion,
//                    Comentario = op.Comentario,
//                    FechaOpinion = op.FechaOpinion,
//                    UsuarioNombre = op.Usuario?.NombreCompleto ?? "Usuario Anónimo"
//                }).ToList(),

//                // Cálculo de la Calificación Promedio (Lógica de Negocio)
//                CalificacionPromedio = producto.OpinionesProductos.Any()
//                                       ? producto.OpinionesProductos.Average(op => (double)op.Calificacion)
//                                       : 0.0
//            };
//        }

//        public async Task<int> CreateProductoAsync(ProductoCreateDto dto)
//        {
//            // 1. Obtener los IDs de los Tipos de Detalle
//            // Carga todos los tipos activos para mapear los identificadores a IDs numéricos (FKs)
//            var tiposDetalle = await _context.TipoDetalles
//                .ToDictionaryAsync(td => td.IdentificadorUnico, td => td.Id);

//            // Obtener el ID para la descripción principal
//            if (!tiposDetalle.TryGetValue("DESCRIPTION", out int mainDescriptionId))
//            {
//                // Esto es una validación crítica: el Tipo Detalle debe existir
//                throw new KeyNotFoundException("El identificador de la descripción principal ('DESCRIPTION') no fue encontrado.");
//            }

//            // Verificar si el SKU ya existe
//            if (await _context.Productos.AnyAsync(p => p.Sku == dto.Sku))
//            {
//                // Lanza una excepción controlada
//                throw new InvalidOperationException($"El SKU '{dto.Sku}' ya está registrado y debe ser único.");
//            }

//            // 2. Mapeo a la Entidad Producto principal
//            var nuevoProducto = new Producto
//            {
//                Nombre = dto.Nombre,
//                //Descripcion = dto.Descripcion,
//                Precio = dto.Precio,
//                PrecioAnterior = dto.PrecioAnterior,
//                Sku = dto.Sku,
//                RequiereReceta = dto.RequiereReceta,
//                CategoriaId = dto.CategoriaId,
//                ProveedorId = dto.ProveedorId,
//                EstaActivo = dto.EstaActivo,
//                TipoProducto = "fisico", // Asumimos 'fisico' por defecto
//                Detalles = new List<ProductoDetalle>()
//            };

//            // Buscamos la Categoría para validar su existencia.
//            if (dto.CategoriaId > 0)
//            {
//                var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
//                if (!categoriaExiste)
//                {
//                    throw new KeyNotFoundException($"La Categoría con ID '{dto.CategoriaId}' no existe o es inválida.");
//                }
//            }
//            else
//            {
//                // Esto asume que CategoriaId es obligatorio y no puede ser 0 o nulo.
//                // Si CategoriaId puede ser NULL en la BD, se necesitaría otra lógica.
//                throw new ArgumentException("El ID de Categoría es requerido.");
//            }

//            // 3. Mover la Descripción (dto.Descripcion) a la tabla producto_detalle
//            if (!string.IsNullOrEmpty(dto.Descripcion))
//            {
//                // Aquí necesitamos decidir qué idioma aplicar a la descripción principal.
//                // Usaremos el idioma del primer detalle o un valor por defecto ('ES').
//                string idiomaDescripcion;

//                if (dto.Detalles != null && dto.Detalles.Any())
//                {
//                    // Si hay detalles, usamos el idioma del primero (con chequeo de nulidad seguro).
//                    idiomaDescripcion = dto.Detalles.FirstOrDefault()?.Idioma?.ToUpper() ?? "ES";
//                }
//                else
//                {
//                    // Si no hay detalles enviados en el DTO, asumimos el idioma por defecto.
//                    idiomaDescripcion = "ES";
//                }

//                nuevoProducto.Detalles.Add(new ProductoDetalle
//                {
//                    TipoDetalleId = mainDescriptionId, // Usamos el ID de MAIN_DESCRIPTION
//                    valor_detalle = dto.Descripcion,   // Usamos el texto de la descripción
//                    MediaUrl = null,
//                    Idioma = idiomaDescripcion
//                });
//            }

//            // 4. Mapeo de los Otros Detalles Enviados (producto_detalle)
//            if (dto.Detalles != null && dto.Detalles.Any())
//            {
//                foreach (var detalleDto in dto.Detalles)
//                {
//                    // El idioma siempre debe ser estandarizado a mayúsculas para la BD
//                    string idiomaDetalle = detalleDto.Idioma.ToUpper();

//                    if (tiposDetalle.TryGetValue(detalleDto.TipoDetalleIdentificador, out int tipoId))
//                    {
//                        nuevoProducto.Detalles.Add(new ProductoDetalle
//                        {
//                            TipoDetalleId = tipoId,
//                            valor_detalle = detalleDto.ValorDetalle,
//                            MediaUrl = detalleDto.MediaUrl,
//                            Idioma = idiomaDetalle
//                        });
//                    }
//                    else
//                    {
//                        // Manejar el caso de un TipoDetalleIdentificador inválido (opcional)
//                        throw new KeyNotFoundException($"El tipo de detalle '{detalleDto.TipoDetalleIdentificador}' no es válido.");
//                    }
//                }
//            }

//            // 5. Guardar en la Base de Datos
//            _context.Productos.Add(nuevoProducto);
//            await _context.SaveChangesAsync();

//            return nuevoProducto.Id; // Devolvemos el ID del producto creado
//        }

//        public async Task<bool> UpdateProductoAsync(int id, ProductoUpdateDto dto)
//        {
//            var productoAActualizar = await _context.Productos
//                .Include(p => p.Detalles) // CARGA LA COLECCIÓN DE DETALLES EXISTENTES
//                .FirstOrDefaultAsync(p => p.Id == id);

//            if (productoAActualizar == null)
//            {
//                return false; // Producto no encontrado
//            }

//            // 1. OBTENER MAPEO DE TIPOS DE DETALLE (Necesario para las FKs)
//            var tiposDetalle = await _context.TipoDetalles
//                .ToDictionaryAsync(td => td.IdentificadorUnico, td => td.Id);

//            // Obtener el ID para la descripción principal
//            if (!tiposDetalle.TryGetValue("DESCRIPTION", out int mainDescriptionId))
//            {
//                throw new KeyNotFoundException("El identificador de la descripción principal ('DESCRIPTION') no fue encontrado.");
//            }

//            // 2. ACTUALIZACIÓN DE PROPIEDADES BÁSICAS
//            // Se usa reflexión o mapeo manual para actualizar solo lo que no es null en el DTO
//            if (dto.Nombre != null) productoAActualizar.Nombre = dto.Nombre;
//            //if (dto.Descripcion != null) productoAActualizar.Descripcion = dto.Descripcion;
//            if (dto.Precio.HasValue) productoAActualizar.Precio = dto.Precio.Value;
//            if (dto.PrecioAnterior.HasValue) productoAActualizar.PrecioAnterior = dto.PrecioAnterior.Value;
//            if (dto.Sku != null) productoAActualizar.Sku = dto.Sku;
//            if (dto.RequiereReceta.HasValue) productoAActualizar.RequiereReceta = dto.RequiereReceta.Value;
//            if (dto.CategoriaId.HasValue) productoAActualizar.CategoriaId = dto.CategoriaId.Value;
//            if (dto.ProveedorId.HasValue) productoAActualizar.ProveedorId = dto.ProveedorId.Value;
//            if (dto.EstaActivo.HasValue) productoAActualizar.EstaActivo = dto.EstaActivo.Value;

//            // La Descripción (dto.Descripcion) se maneja como un Detalle (DESCRIPTION).
//            if (dto.Descripcion != null)
//            {
//                // 1. Decidir el idioma de la actualización. Usamos el idioma del primer detalle enviado o 'ES'.
//                string idiomaUpdate = dto.Detalles?.FirstOrDefault(d => d.Idioma != null)?.Idioma.ToUpper() ?? "ES";

//                // 2. Buscar si ya existe una entrada de DESCRIPTION para ese idioma
//                var descripcionExistente = productoAActualizar.Detalles
//                    .FirstOrDefault(d => d.TipoDetalleId == mainDescriptionId && d.Idioma == idiomaUpdate);

//                if (descripcionExistente != null)
//                {
//                    // ACTUALIZAR: Si ya existe, actualizamos su valor.
//                    descripcionExistente.valor_detalle = dto.Descripcion;
//                }
//                else
//                {
//                    // AÑADIR: Si no existe, creamos el registro de la descripción para el idioma
//                    productoAActualizar.Detalles.Add(new ProductoDetalle
//                    {
//                        TipoDetalleId = mainDescriptionId,
//                        valor_detalle = dto.Descripcion,
//                        MediaUrl = null,
//                        Idioma = idiomaUpdate
//                    });
//                }
//            }

//            // 3. SINCRONIZACIÓN DE LA COLECCIÓN DE DETALLES (El corazón del PUT)
//            if (dto.Detalles != null)
//            {
//                var detallesActuales = productoAActualizar.Detalles.ToList();
//                var detallesNuevos = dto.Detalles.ToList();

//                // 3a. BORRAR DETALLES FALTANTES (Detalles existentes que no están en el nuevo DTO)
//                var idsDetallesEnviados = detallesNuevos.Where(d => d.Id.HasValue).Select(d => d.Id.Value).ToHashSet();
//                var detallesABorrar = detallesActuales
//                    .Where(d => !idsDetallesEnviados.Contains(d.id))
//                    .ToList();

//                foreach (var detalle in detallesABorrar)
//                {
//                    // Omitimos borrar la descripción principal si fue manejada arriba y no enviada con ID
//                    if (detalle.TipoDetalleId != mainDescriptionId)
//                    {
//                        _context.Remove(detalle);
//                    }
//                }

//                // 3b. AÑADIR/ACTUALIZAR DETALLES
//                foreach (var detalleDto in detallesNuevos)
//                {
//                    if (detalleDto.Id.HasValue)
//                    {
//                        // ACTUALIZAR (El detalle existe)
//                        var detalleExistente = detallesActuales.FirstOrDefault(d => d.id == detalleDto.Id.Value);
//                        if (detalleExistente != null)
//                        {
//                            detalleExistente.valor_detalle = detalleDto.ValorDetalle;
//                            detalleExistente.MediaUrl = detalleDto.MediaUrl;
//                            // Actualiza TipoDetalleId y Idioma
//                            if (tiposDetalle.ContainsKey(detalleDto.TipoDetalleIdentificador))
//                            {
//                                detalleExistente.TipoDetalleId = tiposDetalle[detalleDto.TipoDetalleIdentificador];
//                            }
//                            detalleExistente.Idioma = detalleDto.Idioma.ToUpper();
//                        }
//                    }
//                    else if (tiposDetalle.ContainsKey(detalleDto.TipoDetalleIdentificador))
//                    {
//                        // AÑADIR (Es un detalle nuevo, Id es null)
//                        productoAActualizar.Detalles.Add(new ProductoDetalle
//                        {
//                            TipoDetalleId = tiposDetalle[detalleDto.TipoDetalleIdentificador],
//                            valor_detalle = detalleDto.ValorDetalle,
//                            MediaUrl = detalleDto.MediaUrl,
//                            Idioma = detalleDto.Idioma.ToUpper()
//                        });
//                    }
//                }
//            }

//            // 4. GUARDAR CAMBIOS
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> ToggleProductoStatusAsync(int id)
//        {
//            // 1. Buscamos el producto por su ID (e ignoramos el filtro si existe)
//            var producto = await _context.Productos
//                .IgnoreQueryFilters()
//                .FirstOrDefaultAsync(p => p.Id == id);

//            if (producto == null)
//            {
//                return false; // Producto no encontrado
//            }

//            // 2. Lógica de Alternancia (Toggle): Invertir el valor actual de EstaActivo
//            producto.EstaActivo = !producto.EstaActivo;

//            // 3. Guardar los cambios
//            await _context.SaveChangesAsync();

//            return true;
//        }

//        public async Task<ImportResultDto> ImportarProductosAsync(Stream fileStream, string fileType)
//        {
//            var resultado = new ImportResultDto();

//            // Paso 1: Obtener mapeos (identificadores de texto a IDs numéricos)
//            var mapeos = await ObtenerMapeosAsync(); // Asumimos un método que obtiene TiposDetalle, Categorías y Proveedores

//            // Paso 2: Lectura y Conversión del Archivo (Usando un lector de CSV)
//            // El lector debe convertir las filas del CSV/Excel a ProductoImportRowDto
//            var filasImportar = LeerArchivo(fileStream, fileType);
//            resultado.TotalRegistrosProcesados = filasImportar.Count;

//            using (var transaction = await _context.Database.BeginTransactionAsync())
//            {
//                try
//                {
//                    foreach (var filaDto in filasImportar)
//                    {
//                        // Paso 3: Validación de Negocio (Asumimos un método privado de validación)
//                        if (ValidarFila(filaDto, mapeos, out string error))
//                        {
//                            // Paso 4: Construcción y Guardado de Entidades
//                            var nuevoProducto = ConstruirEntidad(filaDto, mapeos);
//                            _context.Productos.Add(nuevoProducto);
//                            await _context.SaveChangesAsync(); // Guardar para obtener el ID

//                            // Paso 5: Mapeo de Detalles (producto_detalle)
//                            MapearYGuardarDetalles(nuevoProducto.Id, filaDto, mapeos);

//                            resultado.RegistrosExitosos++;
//                        }
//                        else
//                        {
//                            resultado.MensajesError.Add($"Fila {resultado.TotalRegistrosProcesados + 1}: {error}");
//                        }
//                    }

//                    await transaction.CommitAsync();
//                    resultado.Exito = true;
//                }
//                catch (Exception ex)
//                {
//                    await transaction.RollbackAsync(); // Revertir todo si falla
//                    resultado.MensajesError.Add($"Error crítico de base de datos: {ex.Message}");
//                    resultado.Exito = false;
//                }
//            }
//            return resultado;
//        }

//        private List<ProductoImportRowDto> LeerArchivo(Stream fileStream, string fileType)
//        {
//            var filasImportar = new List<ProductoImportRowDto>();

//            // NOTA: Formato CSV, donde las columnas están separadas por comas.
//            // Si se espera Excel (.xlsx), se debe usar una biblioteca como EPPlus o NPOI.

//            // Usamos StreamReader para leer el contenido del archivo subido
//            using (var reader = new StreamReader(fileStream))
//            {
//                // 1. Omitir el encabezado (la primera línea contiene los nombres de las columnas)
//                reader.ReadLine();

//                string? linea;

//                // 2. Leer línea por línea
//                while ((linea = reader.ReadLine()) != null)
//                {
//                    if (string.IsNullOrWhiteSpace(linea)) continue; // Saltar líneas vacías

//                    // 3. Dividir la línea en campos (asumiendo CSV delimitado por coma)
//                    var campos = linea.Split(';');

//                    // Verificar la longitud mínima requerida
//                    const int MIN_CAMPOS_REQUERIDOS = 16;

//                    if (campos.Length < MIN_CAMPOS_REQUERIDOS)
//                    {
//                        // Si no tiene suficientes campos, la fila está incompleta y no se puede mapear.
//                        // En un entorno de producción, esto debería registrarse en el ImportResultDto.
//                        throw new Exception($"Fila incompleta: Se esperaban {MIN_CAMPOS_REQUERIDOS} columnas, se encontraron {campos.Length}. los campos son: {campos}");
//                    }

//                    // 4. Mapear los campos al DTO (CRÍTICO: El orden de los campos debe coincidir con el DTO/Plantilla)
//                    try
//                    {
//                        // Convertir el texto de REQUIERE_RECETA a un entero (1 o 0)
//                        int requiereRecetaValue = campos[5].Trim().ToUpper() == "SÍ" ? 1 : 0;
//                        // NOTA: Usamos .Trim().ToUpper() para ser robustos ante espacios y variaciones.

//                        var dto = new ProductoImportRowDto
//                        {
//                            NOMBRE = campos[0],
//                            SKU = campos[1],
//                            PRECIO = decimal.Parse(campos[2], CultureInfo.InvariantCulture), // Usar InvariantCulture para manejar el punto decimal
//                            PRECIO_ANTERIOR = decimal.Parse(campos[3], CultureInfo.InvariantCulture),
//                            IMAGEN_URL = campos[4],
//                            REQUIERE_RECETA = requiereRecetaValue,
//                            CATEGORIA_IDENTIFICADOR = campos[6],
//                            PROVEEDOR_IDENTIFICADOR = campos[7],
//                            IDIOMA = campos[8],
//                            DESCRIPTION = campos[9],
//                            PRODUCT_SPECIFICATIONS = campos[10],
//                            INGREDIENTS = campos[11],
//                            NUTRITION_FACTS = campos[12],
//                            WARNINGS = campos[13],
//                            SHIPPING_SPECIFICATIONS = campos[14],
//                            FROM_THE_BRAND = campos[15]
//                        };
//                        filasImportar.Add(dto);
//                    }
//                    catch (FormatException ex)
//                    {
//                        // Si falla la conversión de decimal o int, se registra el error y se salta la fila
//                        // Nota: La lógica de reporte de errores debe ser implementada en ImportarProductosAsync
//                        // Por ahora, solo lanzaremos la excepción para depuración.
//                        throw new Exception($"Error de formato en la línea: {linea}. Mensaje: {ex.Message}");
//                    }
//                }
//            }

//            return filasImportar;
//        }

//        private async Task<ImportMapeos> ObtenerMapeosAsync()
//        {
//            var mapeos = new ImportMapeos();

//            // 1. Mapeo de Tipos de Detalle (IdentificadorUnico)
//            mapeos.TiposDetalle = await _context.TipoDetalles
//                .ToDictionaryAsync(
//                    td => td.IdentificadorUnico, // Clave: Ej. "INGREDIENTS"
//                    td => td.Id);                // Valor: Ej. 3

//            // 2. Mapeo de Categorías (Nombre)
//            mapeos.Categorias = await _context.Categorias
//                .ToDictionaryAsync(
//                    c => c.Nombre,               // Clave: Ej. "ANALGESICOS"
//                    c => c.Id);                  // Valor: Ej. 1

//            // 3. Mapeo de Proveedores (Nombre)
//            mapeos.Proveedores = await _context.Proveedores
//                .ToDictionaryAsync(
//                    p => p.Nombre,               // Clave: Ej. "LAB RAMOS"
//                    p => p.Id);                  // Valor: Ej. 5

//            // Carga de SKUs existentes
//            mapeos.SkusExistentes = await _context.Productos
//                .Select(p => p.Sku)
//                .Where(sku => sku != null) // Ignorar SKUs nulos si se permiten
//                .ToHashSetAsync();

//            return mapeos;
//        }

//        /// <summary>
//        /// Valida la sintaxis de una fila de importación y comprueba que todas las claves foráneas existan en la BD.
//        /// </summary>
//        /// <param name="filaDto">El DTO que representa la fila CSV/Excel.</param>
//        /// <param name="mapeos">Los diccionarios con las claves válidas de la BD.</param>
//        /// <param name="errorMensaje">Devuelve el mensaje de error si la validación falla.</param>
//        /// <returns>True si la fila es válida, False en caso contrario.</returns>
//        private bool ValidarFila(ProductoImportRowDto filaDto, ImportMapeos mapeos, out string errorMensaje)
//        {
//            // Usaremos una lista para acumular todos los errores y devolver un mensaje detallado
//            var errores = new List<string>();

//            // 1. VALIDACIÓN DE REQUERIDOS Y LONGITUDES MÍNIMAS (Validación del DTO)

//            // Podemos usar el validador estándar de .NET para los atributos [Required]
//            var validationContext = new ValidationContext(filaDto, serviceProvider: null, items: null);
//            var validationResults = new List<ValidationResult>();

//            if (!Validator.TryValidateObject(filaDto, validationContext, validationResults, validateAllProperties: true))
//            {
//                foreach (var result in validationResults)
//                {
//                    errores.Add(result.ErrorMessage);
//                }
//            }

//            // 2. VALIDACIÓN DE REGLAS DE NEGOCIO Y CLAVES FORÁNEAS

//            // Validar Categoría
//            if (!mapeos.Categorias.ContainsKey(filaDto.CATEGORIA_IDENTIFICADOR))
//            {
//                errores.Add($"Categoría '{filaDto.CATEGORIA_IDENTIFICADOR}' no existe en el catálogo.");
//            }

//            // Validar Proveedor
//            if (!mapeos.Proveedores.ContainsKey(filaDto.PROVEEDOR_IDENTIFICADOR))
//            {
//                errores.Add($"Proveedor '{filaDto.PROVEEDOR_IDENTIFICADOR}' no existe en el catálogo.");
//            }

//            // Validar SKU (Debe tener un valor para que la unicidad funcione)
//            if (string.IsNullOrWhiteSpace(filaDto.SKU))
//            {
//                errores.Add("El campo SKU es obligatorio y no puede estar vacío.");
//            }

//            // Validar IDIOMA (Debe ser un código válido de 2 letras)
//            if (filaDto.IDIOMA?.Length != 2)
//            {
//                errores.Add("El código de idioma debe ser un código ISO de 2 letras (Ej: ES, EN).");
//            }

//            // UNICIDAD DEL SKU
//            if (!string.IsNullOrWhiteSpace(filaDto.SKU) &&
//                mapeos.SkusExistentes.Contains(filaDto.SKU))
//            {
//                errores.Add($"El SKU '{filaDto.SKU}' ya está registrado y debe ser único.");
//            }


//            // 3. VALIDACIÓN DE DETALLES (Asegurarse de que los identificadores existan si se provee valor)
//            // Se itera sobre las columnas de detalle que tienen texto (se asume que si hay texto, debe haber un tipo de detalle)

//            // Lista de identificadores de detalle en la plantilla (usando reflexión o lista manual):
//            var identificadoresDetalle = new List<string> {
//                "DESCRIPTION", "PRODUCT_SPECIFICATIONS", "INGREDIENTS",
//                "NUTRITION_FACTS", "WARNINGS", "SHIPPING_SPECIFICATIONS", "FROM_THE_BRAND"
//            };

//            // Uso de reflexión para verificar dinámicamente si el texto existe
//            var tipoDetalle = typeof(ProductoImportRowDto);

//            foreach (var identificador in identificadoresDetalle)
//            {
//                // Nota: Esto requiere que los nombres de las propiedades coincidan exactamente con la columna SQL (Ej: 'INGREDIENTS')
//                var valorPropiedad = tipoDetalle.GetProperty(identificador)?.GetValue(filaDto) as string;

//                // Si se envió algún valor de detalle y la clave del tipo de detalle no existe, es un error.
//                if (!string.IsNullOrWhiteSpace(valorPropiedad) &&
//                    !mapeos.TiposDetalle.ContainsKey(identificador))
//                {
//                    // Si el administrador llenó la columna 'WARNINGS' pero la clave 'WARNINGS' no existe en tipo_detalles, fallamos.
//                    errores.Add($"El tipo de detalle '{identificador}' no está catalogado en la base de datos.");
//                }
//            }

//            // 4. RETORNO FINAL
//            if (errores.Any())
//            {
//                errorMensaje = string.Join(" | ", errores);
//                return false;
//            }

//            errorMensaje = string.Empty;
//            return true;
//        }

//        /// <summary>
//        /// Construye la entidad Producto principal y sus detalles a partir del DTO de la fila importada.
//        /// </summary>
//        /// <param name="filaDto">La fila de datos ya validada del CSV.</param>
//        /// <param name="mapeos">Los diccionarios de IDs resueltos (Categorías, Proveedores, Tipos de Detalle).</param>
//        /// <returns>La entidad Producto lista para ser insertada.</returns>
//        private Producto ConstruirEntidad(ProductoImportRowDto filaDto, ImportMapeos mapeos)
//        {
//            // 1. RESOLUCIÓN DE CLAVES FORÁNEAS (Asignación de IDs numéricos)

//            // Obtenemos el ID de la Categoría y el Proveedor, usando TryGetValue para seguridad
//            // NOTA: La validación previa (ValidarFila) ya aseguró que estas claves existen.
//            mapeos.Categorias.TryGetValue(filaDto.CATEGORIA_IDENTIFICADOR, out int categoriaId);
//            mapeos.Proveedores.TryGetValue(filaDto.PROVEEDOR_IDENTIFICADOR, out int proveedorId);

//            // 2. CONSTRUCCIÓN DE LA ENTIDAD BASE (Producto)
//            var nuevoProducto = new Producto
//            {
//                Nombre = filaDto.NOMBRE,
//                Sku = filaDto.SKU,
//                Precio = filaDto.PRECIO,
//                PrecioAnterior = filaDto.PRECIO_ANTERIOR,
//                RequiereReceta = filaDto.REQUIERE_RECETA == 1, // Mapeo de int (0/1) a bool
//                EstaActivo = true,           // El producto se crea como activo por defecto

//                // Asignación de IDs numéricos
//                CategoriaId = categoriaId,
//                ProveedorId = proveedorId,

//                // Valores por defecto o no utilizados en el DTO
//                TipoProducto = "fisico", // Asumimos que la carga masiva es solo para productos físicos
//                PuntosParaCanje = 0,     // Por defecto en 0

//                // Inicialización de la colección de detalles (CRÍTICO)
//                Detalles = new List<ProductoDetalle>()
//            };

//            // 3. CONSTRUCCIÓN Y ASIGNACIÓN DE DETALLES (producto_detalle)

//            // Definimos los nombres de las columnas que contienen los detalles a mapear
//            var detalleColumnas = new Dictionary<string, string>
//            {
//                // Clave del DTO de Fila -> Identificador Único de Tipo Detalle
//                { filaDto.DESCRIPTION ?? "",            "MAIN_DESCRIPTION" },
//                { filaDto.PRODUCT_SPECIFICATIONS ?? "", "PRODUCT_SPECIFICATIONS" },
//                { filaDto.INGREDIENTS ?? "",            "INGREDIENTS" },
//                { filaDto.NUTRITION_FACTS ?? "",        "NUTRITION_FACTS" },
//                { filaDto.WARNINGS ?? "",               "WARNINGS" },
//                { filaDto.SHIPPING_SPECIFICATIONS ?? "","SHIPPING_SPECIFICATIONS" },
//                { filaDto.FROM_THE_BRAND ?? "",         "FROM_THE_BRAND" }
//            };

//            // Obtenemos el código de idioma estandarizado
//            string idiomaCode = filaDto.IDIOMA.ToUpper();

//            foreach (var par in detalleColumnas)
//            {
//                string? valorDetalle = par.Key;
//                string identificadorTipo = par.Value;

//                // Solo creamos un registro si el valor de la celda no está vacío
//                if (!string.IsNullOrWhiteSpace(valorDetalle))
//                {
//                    // Resolvemos el ID de TipoDetalle
//                    if (mapeos.TiposDetalle.TryGetValue(identificadorTipo, out int tipoDetalleId))
//                    {
//                        nuevoProducto.Detalles.Add(new ProductoDetalle
//                        {
//                            TipoDetalleId = tipoDetalleId,
//                            valor_detalle = valorDetalle,
//                            MediaUrl = null, // La plantilla actual no soporta URL por detalle
//                            Idioma = idiomaCode
//                        });
//                    }
//                    // Si el Tipo Detalle no se resuelve, la fila debería haber fallado en ValidarFila.
//                }
//            }

//            return nuevoProducto;
//        }

//        /// <summary>
//        /// Mapea y guarda todos los detalles de un producto (producto_detalle) que existen en la fila importada.
//        /// </summary>
//        /// <param name="productoId">El ID del producto principal recién creado.</param>
//        /// <param name="filaDto">La fila de datos ya validada del CSV.</param>
//        /// <param name="mapeos">Los diccionarios de IDs resueltos.</param>
//        private void MapearYGuardarDetalles(int productoId, ProductoImportRowDto filaDto, ImportMapeos mapeos)
//        {
//            // 1. OBTENER EL CÓDIGO DE IDIOMA ESTANDARIZADO
//            string idiomaCode = filaDto.IDIOMA.ToUpper();

//            // 2. DEFINICIÓN DE LOS DETALLES A MAPEAR

//            // Usamos un diccionario que mapea el nombre de la columna (en DTO) al IdentificadorUnico (en la BD)
//            // El orden es importante para la lógica de Mapeo
//            var detallesImportables = new Dictionary<string, string>
//            {
//                { nameof(filaDto.DESCRIPTION), "MAIN_DESCRIPTION" },
//                { nameof(filaDto.PRODUCT_SPECIFICATIONS), "PRODUCT_SPECIFICATIONS" },
//                { nameof(filaDto.INGREDIENTS), "INGREDIENTS" },
//                { nameof(filaDto.NUTRITION_FACTS), "NUTRITION_FACTS" },
//                { nameof(filaDto.WARNINGS), "WARNINGS" },
//                { nameof(filaDto.SHIPPING_SPECIFICATIONS), "SHIPPING_SPECIFICATIONS" },
//                { nameof(filaDto.FROM_THE_BRAND), "FROM_THE_BRAND" }
//            };

//            // 3. OBTENER VALORES DINÁMICAMENTE (usando reflexión, ya que los nombres de las columnas son fijos)
//            var tipoFila = typeof(ProductoImportRowDto);
//            var nuevosDetalles = new List<ProductoDetalle>();

//            foreach (var par in detallesImportables)
//            {
//                string nombrePropiedad = par.Key; // Ej: "DESCRIPTION"
//                string identificadorTipo = par.Value; // Ej: "MAIN_DESCRIPTION"

//                // Usamos GetProperty para obtener el valor de la celda de la fila
//                var valorPropiedad = tipoFila.GetProperty(nombrePropiedad)?.GetValue(filaDto) as string;

//                // Solo creamos un detalle si el campo no está vacío/nulo y el tipo de detalle está mapeado
//                if (!string.IsNullOrWhiteSpace(valorPropiedad) &&
//                    mapeos.TiposDetalle.TryGetValue(identificadorTipo, out int tipoDetalleId))
//                {
//                    nuevosDetalles.Add(new ProductoDetalle
//                    {
//                        producto_id = productoId, // Asigna el ID del producto recién creado
//                        TipoDetalleId = tipoDetalleId,
//                        valor_detalle = valorPropiedad,
//                        MediaUrl = null, // Asume que no hay columna separada para URL por detalle
//                        Idioma = idiomaCode
//                    });
//                }
//            }

//            // 4. GUARDAR DETALLES EN LA BASE DE DATOS
//            if (nuevosDetalles.Any())
//            {
//                // Añadimos todos los detalles a la colección del contexto
//                _context.ProductoDetalles.AddRange(nuevosDetalles);
//            }
//        }

//        private class ImportMapeos
//        {
//            // Resuelve IdentificadorUnico (string) a ID (int)
//            public Dictionary<string, int> TiposDetalle { get; set; } = new Dictionary<string, int>();

//            // Resuelve Nombre (string) a ID (int)
//            public Dictionary<string, int> Categorias { get; set; } = new Dictionary<string, int>();

//            // Resuelve Nombre (string) a ID (int)
//            public Dictionary<string, int> Proveedores { get; set; } = new Dictionary<string, int>();
//            public HashSet<string> SkusExistentes { get; set; } = new HashSet<string>();
//        }

//        public async Task<bool> AddImagenesGaleriaAsync(int productoId, ICollection<IFormFile> archivos)
//        {
//            var producto = await _context.Productos.FindAsync(productoId);
//            if (producto == null) return false;

//            var nuevasImagenes = new List<ProductoImagen>();
//            int ordenInicial = producto.ImagenesGaleria.Count > 0 ? producto.ImagenesGaleria.Max(i => i.Orden) + 1 : 1;

//            foreach (var archivo in archivos)
//            {
//                // 1. Guardar en el disco del VPS y obtener la URL pública
//                string urlPublica = await GuardarImagenEnVPSAsync(archivo);

//                // 2. Crear la entidad y asignarla al producto
//                nuevasImagenes.Add(new ProductoImagen
//                {
//                    ProductoId = productoId,
//                    UrlImagen = urlPublica,
//                    Orden = ordenInicial++
//                });
//            }

//            _context.ProductoImagenes.AddRange(nuevasImagenes);
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        private async Task<string> GuardarImagenEnVPSAsync(IFormFile imagen)
//        {
//            // 1. Definir la ruta física en el servidor (ej: wwwroot/images/productos)
//            // Ruta para Local
//            //var directorioBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "productos");

//            // Ruta para producción
//            var directorioBase = "/var/lib/farma-data/images/productos";

//            // Asegurar que el directorio exista
//            if (!Directory.Exists(directorioBase))
//            {
//                Directory.CreateDirectory(directorioBase);
//            }

//            // 2. Crear un nombre de archivo único y seguro
//            var extension = Path.GetExtension(imagen.FileName);
//            var nombreArchivoUnico = $"{Guid.NewGuid()}{extension}";
//            var rutaCompleta = Path.Combine(directorioBase, nombreArchivoUnico);

//            // 3. Escribir el archivo en el disco
//            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
//            {
//                await imagen.CopyToAsync(stream);
//            }

//            // 4. Devolver la URL pública que el frontend usará
//            // La URL pública es relativa a wwwroot. Asumimos que la base del sitio es el dominio.
//            var urlPublica = $"/images/productos/{nombreArchivoUnico}";
//            return urlPublica;
//        }

//        public async Task<IEnumerable<ProveedorDto>> GetProveedoresAsync()
//        {
//            // Carga todos los proveedores
//            var proveedores = await _context.Proveedores
//                .OrderBy(p => p.Nombre)
//                .ToListAsync();

//            // Mapeo a DTO
//            return proveedores.Select(p => new ProveedorDto
//            {
//                Id = p.Id,
//                Nombre = p.Nombre,
//                LogoUrl = p.LogoUrl
//            }).ToList();
//        }

//        public async Task<bool> DeleteImagenGaleriaAsync(int imagenId)
//        {
//            // 1. Buscar el registro de la imagen en la BD
//            var imagenAEliminar = await _context.ProductoImagenes
//                .FirstOrDefaultAsync(img => img.Id == imagenId);

//            if (imagenAEliminar == null)
//            {
//                return false; // El registro no existe
//            }

//            // 2. ELIMINACIÓN FÍSICA DEL ARCHIVO (Hard Delete del Disco del VPS)
//            try
//            {
//                // Ruta para Local
//                //var directorioBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"); // Asumimos wwwroot como base

//                // La ruta donde realmente están los archivos guardados:
//                // Ruta para producción
//                var rutaFisicaBase = "/var/lib/farma-data/images";

//                // La URL almacenada en la BD es como '/images/productos/GUID.jpg'.
//                // Debemos eliminar el prefijo /images/ y /productos/ para obtener solo el GUID.jpg
//                var nombreArchivo = Path.GetFileName(imagenAEliminar.UrlImagen);

//                // Construye la ruta física completa
//                var rutaFisicaCompleta = Path.Combine(rutaFisicaBase, "productos", nombreArchivo);

//                // -----------------------------------------------------------------

//                if (File.Exists(rutaFisicaCompleta))
//                {
//                    File.Delete(rutaFisicaCompleta);
//                }
//                // Nota: Si el archivo no existe, la lógica continúa para limpiar la BD.
//            }
//            catch (Exception ex)
//            {
//                // Registrar el error de E/S. No lanzamos la excepción para que al menos se limpie la BD.
//                // _logger.LogError(ex, $"Fallo al eliminar físicamente el archivo: {imagenAEliminar.UrlImagen}");
//            }

//            // 3. ELIMINACIÓN FÍSICA DEL REGISTRO DE LA BD
//            _context.ProductoImagenes.Remove(imagenAEliminar);
//            await _context.SaveChangesAsync();

//            return true;
//        }

//        public async Task<string> UploadFileAsync(IFormFile file)
//        {
//            // Lógica sencilla: solo llama a la función de I/O
//            return await GuardarImagenEnVPSAsync(file);
//        }

//        public async Task<PagedResultDto<ProductoVencimientoDto>> GetInventarioPaginadoAsync(int page, int pageSize, string? searchTerm)
//        {
//            var query = _context.Productos.AsQueryable();

//            // 1. APLICAR FILTRO DE BÚSQUEDA (CRÍTICO)
//            if (!string.IsNullOrWhiteSpace(searchTerm))
//            {
//                // Convertimos a minúsculas o mayúsculas para la búsqueda sin distinción de mayúsculas/minúsculas
//                var term = searchTerm.ToLower();

//                query = query.Where(p =>
//                    p.Nombre.ToLower().Contains(term) || // Buscar por Nombre del producto
//                    p.Sku.ToLower().Contains(term)       // Buscar por SKU
//                );
//            }

//            // 1. Obtener el conteo total de registros
//            var totalCount = await query.CountAsync();
//            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

//            // 2. Aplicar Paginación
//            var productos = await query
//                .Include(p => p.InventarioSucursals) // Necesario para obtener el Stock Total
//                .OrderBy(p => p.Id) // Siempre ordenar para paginar correctamente
//                .Skip((page - 1) * pageSize) // Saltar registros anteriores
//                .Take(pageSize) // Tomar solo los registros de esta página
//                .ToListAsync();

//            // 3. Mapeo al DTO (similar al Dashboard, pero sin filtro de fecha)
//            var items = productos.Select(p =>
//            {
//                // Calculamos métricas necesarias
//                var stockTotal = p.InventarioSucursals.Sum(inv => inv.Stock);

//                // Obtener el valor de stock mínimo. Tomaremos el StockMinimo del primer registro encontrado 
//                // o 0 si no hay inventario (Regla de Negocio: Idealmente, cada sucursal tendría el mismo valor de seguridad).
//                var stockMinimo = p.InventarioSucursals.FirstOrDefault()?.StockMinimo ?? 0;

//                return new ProductoVencimientoDto
//                {
//                    Id = p.Id,
//                    NombreProducto = p.Nombre,
//                    StockActual = stockTotal,
//                    Sku = p.Sku ?? "",
//                    FechaVencimiento = p.FechaVencimiento,
//                    DiasParaExpirar = (p.FechaVencimiento.HasValue) ? (p.FechaVencimiento.Value.Date - DateTime.UtcNow.Date).Days : 999,

//                    // Mapeo de la Alerta
//                    StockMinimo = stockMinimo,
//                    AlertaStockBajo = stockTotal <= stockMinimo // Lógica de la alerta de seguridad
//                };
//            }).ToList();

//            // 4. (Devolver el resultado paginado) se mantiene igual
//            return new PagedResultDto<ProductoVencimientoDto>
//            {
//                Items = items,
//                TotalPages = totalPages,    
//                TotalCount = totalCount,
//                PageSize = pageSize,
//                CurrentPage = page
//            };
//        }

//        public async Task<bool> SetImagenPrincipalAsync(int productoId, int imagenId)
//        {
//            // 1. Iniciar Transacción
//            await using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                // 2. Anular todas las imágenes principales existentes para este producto
//                await _context.ProductoImagenes
//                    .Where(img => img.ProductoId == productoId && img.EsPrincipal == true)
//                    .ExecuteUpdateAsync(updates => updates
//                        .SetProperty(img => img.EsPrincipal, false));

//                // 3. Establecer la nueva imagen como principal
//                var resultado = await _context.ProductoImagenes
//                    .Where(img => img.Id == imagenId && img.ProductoId == productoId)
//                    .ExecuteUpdateAsync(updates => updates
//                        .SetProperty(img => img.EsPrincipal, true));

//                // 4. Confirmar la transacción
//                await transaction.CommitAsync();

//                return resultado > 0;
//            }
//            catch (Exception)
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<bool> CreateOpinionAsync(int productoId, int userId, OpinionCreateDto dto)
//        {
//            // 1. Validar que el producto exista (FK check)
//            if (!await _context.Productos.AnyAsync(p => p.Id == productoId))
//            {
//                throw new KeyNotFoundException($"Producto con ID {productoId} no encontrado.");
//            }

//            // 2. Opcional: Impedir que un usuario deje múltiples reviews
//            // if (await _context.OpinionesProductos.AnyAsync(o => o.ProductoId == productoId && o.UsuarioId == userId))
//            // {
//            //     throw new InvalidOperationException("Ya has dejado una opinión para este producto.");
//            // }

//            // 3. Mapeo a la Entidad
//            var nuevaOpinion = new OpinionesProducto
//            {
//                producto_id = productoId,
//                UsuarioId = userId, // ID del usuario autenticado
//                Calificacion = dto.Calificacion,
//                Comentario = dto.Comentario,
//                FechaOpinion = DateTime.UtcNow
//            };

//            // 4. Guardar en la Base de Datos
//            _context.OpinionesProductos.Add(nuevaOpinion);
//            await _context.SaveChangesAsync();

//            return true;
//        }

//        public async Task<IEnumerable<OpinionDto>> GetOpinionesAdminAsync()
//        {
//            // Carga todas las opiniones e incluye el Usuario para el nombre
//            var opiniones = await _context.OpinionesProductos
//                .Include(op => op.Usuario)
//                .Include(op => op.Producto)
//                .OrderByDescending(op => op.FechaOpinion)
//                .ToListAsync();

//            // Mapeo a DTO
//            return opiniones.Select(op => new OpinionDto
//            {
//                Calificacion = op.Calificacion,
//                Comentario = op.Comentario,
//                FechaOpinion = op.FechaOpinion,
//                UsuarioNombre = op.Usuario?.NombreCompleto ?? "Usuario Anónimo",
//                ProductoNombre = op.Producto?.Nombre
//            }).ToList();
//        }

//        public async Task<IEnumerable<OpinionDto>> GetOpinionesByProductoIdAsync(int productoId)
//        {
//            // 1. Carga las opiniones filtradas por ProductoId e incluye el Usuario para el nombre
//            var opiniones = await _context.OpinionesProductos
//                .Where(op => op.producto_id == productoId)
//                .Include(op => op.Usuario)
//                .Include(op => op.Producto)
//                .OrderByDescending(op => op.FechaOpinion) // Las más recientes primero
//                .ToListAsync();

//            // 2. Mapeo a DTO
//            return opiniones.Select(op => new OpinionDto
//            {
//                Calificacion = op.Calificacion,
//                Comentario = op.Comentario,
//                FechaOpinion = op.FechaOpinion,
//                UsuarioNombre = op.Usuario?.NombreCompleto ?? "Usuario Anónimo",
//                ProductoNombre = op.Producto?.Nombre
//            }).ToList();
//        }
//    }
//}
