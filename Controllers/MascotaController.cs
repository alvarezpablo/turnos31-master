using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.ViewModels;
using Turnos31.Filters;

namespace Turnos31.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class MascotaController : Controller
    {
        private readonly VeterinariaContext _context;

        public MascotaController(VeterinariaContext context) => _context = context;

        // GET: Mascota
        public async Task<IActionResult> Index(int? idDueno)
        {
            try
            {
                // Cargar dueños para el filtro de forma segura
                var duenos = new List<object>();
                try
                {
                    var duenosTemp = await _context.Duenos
                        .Where(d => d.Nombre != null && d.Nombre.Trim() != "")
                        .Select(d => new {
                            d.IdDueno,
                            Nombre = (d.Nombre ?? "Sin nombre") + " " + (d.Apellido ?? "")
                        })
                        .OrderBy(d => d.Nombre)
                        .ToListAsync();

                    duenos = duenosTemp.Cast<object>().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar dueños: {ex.Message}");
                    duenos = new List<object>();
                }

                ViewBag.IdDueno = new SelectList(duenos, "IdDueno", "Nombre", idDueno);

                // Consulta básica de mascotas sin filtros restrictivos
                var mascotasQuery = _context.Mascotas.AsQueryable();

                if (idDueno.HasValue)
                {
                    mascotasQuery = mascotasQuery.Where(m => m.IdDueno == idDueno.Value);
                }

                // Obtener todas las mascotas sin filtrar por nombre
                var mascotasBasicas = await mascotasQuery
                    .Select(m => new
                    {
                        m.IdMascota,
                        m.Nombre,
                        m.Color,
                        m.Sexo,
                        m.FechaNacimiento,
                        m.IdEspecie,
                        m.IdRaza,
                        m.IdDueno
                    })
                    .OrderBy(m => m.Nombre ?? "ZZZ") // Ordenar poniendo los NULL al final
                    .ToListAsync();

                // Crear lista de mascotas con datos seguros
                var mascotasList = new List<Mascota>();

                foreach (var mascotaBasica in mascotasBasicas)
                {
                    var mascota = new Mascota
                    {
                        IdMascota = mascotaBasica.IdMascota,
                        Nombre = mascotaBasica.Nombre ?? "Sin nombre",
                        Color = mascotaBasica.Color ?? "No especificado",
                        Sexo = mascotaBasica.Sexo ?? "N",
                        FechaNacimiento = mascotaBasica.FechaNacimiento,
                        IdEspecie = mascotaBasica.IdEspecie,
                        IdRaza = mascotaBasica.IdRaza,
                        IdDueno = mascotaBasica.IdDueno
                    };

                    // Cargar especie de forma segura
                    try
                    {
                        var especie = await _context.Especies
                            .Where(e => e.IdEspecie == mascotaBasica.IdEspecie)
                            .FirstOrDefaultAsync();
                        mascota.Especie = especie ?? new Especie { Nombre = "Sin especie" };
                    }
                    catch
                    {
                        mascota.Especie = new Especie { Nombre = "Sin especie" };
                    }

                    // Cargar raza de forma segura
                    try
                    {
                        var raza = await _context.Razas
                            .Where(r => r.IdRaza == mascotaBasica.IdRaza)
                            .FirstOrDefaultAsync();
                        mascota.Raza = raza ?? new Raza { Nombre = "Sin raza" };
                    }
                    catch
                    {
                        mascota.Raza = new Raza { Nombre = "Sin raza" };
                    }

                    // Cargar dueño de forma segura
                    try
                    {
                        var dueno = await _context.Duenos
                            .Where(d => d.IdDueno == mascotaBasica.IdDueno)
                            .FirstOrDefaultAsync();
                        mascota.Dueno = dueno ?? new Dueno { Nombre = "Sin dueño", Apellido = "" };
                    }
                    catch
                    {
                        mascota.Dueno = new Dueno { Nombre = "Sin dueño", Apellido = "" };
                    }

                    mascotasList.Add(mascota);
                }

                return View(mascotasList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error crítico en Index Mascota: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                ViewBag.IdDueno = new SelectList(new List<object>(), "IdDueno", "Nombre");
                ViewBag.ErrorMessage = $"Error al cargar las mascotas: {ex.Message}";
                return View(new List<Mascota>());
            }
        }

        // GET: Mascota/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Obtener mascota básica primero
                var mascotaBasica = await _context.Mascotas
                    .Where(m => m.IdMascota == id)
                    .Select(m => new
                    {
                        m.IdMascota,
                        m.Nombre,
                        m.Color,
                        m.Sexo,
                        m.FechaNacimiento,
                        m.IdEspecie,
                        m.IdRaza,
                        m.IdDueno,
                        m.Peso,
                        m.Tamaño,
                        m.NumeroMicrochip,
                        m.Pelaje,
                        m.Alergia,
                        m.Observaciones
                    })
                    .FirstOrDefaultAsync();

                if (mascotaBasica == null)
                {
                    return NotFound();
                }

                // Crear objeto mascota con datos seguros
                var mascota = new Mascota
                {
                    IdMascota = mascotaBasica.IdMascota,
                    Nombre = mascotaBasica.Nombre ?? "Sin nombre",
                    Color = mascotaBasica.Color ?? "No especificado",
                    Sexo = mascotaBasica.Sexo ?? "N",
                    FechaNacimiento = mascotaBasica.FechaNacimiento,
                    IdEspecie = mascotaBasica.IdEspecie,
                    IdRaza = mascotaBasica.IdRaza,
                    IdDueno = mascotaBasica.IdDueno,
                    Peso = mascotaBasica.Peso,
                    Tamaño = mascotaBasica.Tamaño,
                    NumeroMicrochip = mascotaBasica.NumeroMicrochip,
                    Pelaje = mascotaBasica.Pelaje,
                    Alergia = mascotaBasica.Alergia,
                    Observaciones = mascotaBasica.Observaciones
                };

                // Cargar entidades relacionadas de forma segura
                try
                {
                    var especie = await _context.Especies
                        .Where(e => e.IdEspecie == mascotaBasica.IdEspecie)
                        .FirstOrDefaultAsync();
                    mascota.Especie = especie ?? new Especie { Nombre = "Sin especie" };
                }
                catch
                {
                    mascota.Especie = new Especie { Nombre = "Sin especie" };
                }

                try
                {
                    var raza = await _context.Razas
                        .Where(r => r.IdRaza == mascotaBasica.IdRaza)
                        .FirstOrDefaultAsync();
                    mascota.Raza = raza ?? new Raza { Nombre = "Sin raza" };
                }
                catch
                {
                    mascota.Raza = new Raza { Nombre = "Sin raza" };
                }

                try
                {
                    var dueno = await _context.Duenos
                        .Where(d => d.IdDueno == mascotaBasica.IdDueno)
                        .FirstOrDefaultAsync();
                    mascota.Dueno = dueno ?? new Dueno { Nombre = "Sin dueño", Apellido = "" };
                }
                catch
                {
                    mascota.Dueno = new Dueno { Nombre = "Sin dueño", Apellido = "" };
                }

                // Cargar consultas de forma segura a través de Agenda
                try
                {
                    var consultas = await _context.Consultas
                        .Include(c => c.Agenda)
                        .Where(c => c.Agenda != null && c.Agenda.IdMascota == mascotaBasica.IdMascota)
                        .ToListAsync();
                    mascota.Consultas = consultas ?? new List<Consulta>();
                }
                catch
                {
                    mascota.Consultas = new List<Consulta>();
                }

                return View(mascota);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Details: {ex.Message}");
                return NotFound();
            }
        }

        // GET: Mascota/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var duenos = await _context.Duenos
                    .Select(d => new SelectListItem
                    {
                        Value = d.IdDueno.ToString(),
                        Text = (d.Nombre ?? "Sin nombre") + " " + (d.Apellido ?? "")
                    })
                    .ToListAsync();

                var especies = await _context.Especies
                    .Where(e => e.Activo)
                    .Select(e => new SelectListItem
                    {
                        Value = e.IdEspecie.ToString(),
                        Text = e.Nombre
                    })
                    .ToListAsync();

                var razas = await _context.Razas
                    .Where(r => r.Activo)
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRaza.ToString(),
                        Text = r.Nombre
                    })
                    .ToListAsync();

                var vm = new MascotaCreateViewModel
                {
                    Duenos = duenos,
                    Especies = especies,
                    Razas = razas
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Create Mascota: {ex.Message}");

                var vm = new MascotaCreateViewModel
                {
                    Duenos = new List<SelectListItem>(),
                    Especies = new List<SelectListItem>(),
                    Razas = new List<SelectListItem>()
                };

                ViewBag.ErrorMessage = $"Error al cargar los datos: {ex.Message}";
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MascotaCreateViewModel vm)
        {
            try
            {
                // Crear una nueva instancia de Mascota con valores por defecto
                var mascota = new Mascota
                {
                    Nombre = vm.Mascota.Nombre?.Trim() ?? "",
                    Color = string.IsNullOrWhiteSpace(vm.Mascota.Color) ? "No especificado" : vm.Mascota.Color.Trim(),
                    Sexo = vm.Mascota.Sexo?.Trim() ?? "",
                    IdEspecie = vm.Mascota.IdEspecie,
                    IdRaza = vm.Mascota.IdRaza,
                    IdDueno = vm.Mascota.IdDueno,
                    IdCliente = vm.Mascota.IdDueno, // Asignar el mismo valor para compatibilidad con hosting
                    Activo = true, // Campo requerido por la base de datos del hosting
                    FechaNacimiento = vm.Mascota.FechaNacimiento,
                    Peso = vm.Mascota.Peso,
                    Tamaño = vm.Mascota.Tamaño?.Trim(),
                    NumeroMicrochip = vm.Mascota.NumeroMicrochip?.Trim(),
                    Pelaje = vm.Mascota.Pelaje?.Trim(),
                    Alergia = vm.Mascota.Alergia?.Trim(),
                    Observaciones = vm.Mascota.Observaciones?.Trim()
                };

                // Validar campos requeridos manualmente
                if (string.IsNullOrWhiteSpace(mascota.Nombre))
                {
                    ModelState.AddModelError("Mascota.Nombre", "El nombre es requerido");
                }

                if (mascota.IdEspecie <= 0)
                {
                    ModelState.AddModelError("Mascota.IdEspecie", "Debe seleccionar una especie");
                }

                if (mascota.IdRaza <= 0)
                {
                    ModelState.AddModelError("Mascota.IdRaza", "Debe seleccionar una raza");
                }

                if (mascota.IdDueno <= 0)
                {
                    ModelState.AddModelError("Mascota.IdDueno", "Debe seleccionar un dueño");
                }

                if (string.IsNullOrWhiteSpace(mascota.Sexo) || (mascota.Sexo != "M" && mascota.Sexo != "H"))
                {
                    ModelState.AddModelError("Mascota.Sexo", "Debe seleccionar el sexo (M o H)");
                }

                if (ModelState.IsValid)
                {
                    // Log antes de guardar
                    Console.WriteLine("=== ANTES DE GUARDAR ===");
                    Console.WriteLine($"Color: '{mascota.Color}' (Length: {mascota.Color?.Length ?? 0})");
                    Console.WriteLine($"Nombre: '{mascota.Nombre}'");
                    Console.WriteLine($"Sexo: '{mascota.Sexo}'");
                    Console.WriteLine($"IdEspecie: {mascota.IdEspecie}");
                    Console.WriteLine($"IdRaza: {mascota.IdRaza}");
                    Console.WriteLine($"IdDueno: {mascota.IdDueno}");
                    Console.WriteLine($"IdCliente: {mascota.IdCliente}");
                    Console.WriteLine($"Activo: {mascota.Activo}");

                    _context.Mascotas.Add(mascota);
                    await _context.SaveChangesAsync();

                    // Si es una petición AJAX, retornar JSON
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, id = mascota.IdMascota, nombre = mascota.Nombre });
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear mascota: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                ViewBag.ErrorMessage = $"Error al guardar la mascota: {ex.Message}. Inner: {ex.InnerException?.Message}";
            }

            // Si es una petición AJAX y hay errores, retornar la vista parcial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", vm);
            }

            // Si hay error, recargar combos
            try
            {
                vm.Duenos = await _context.Duenos
                    .Select(d => new SelectListItem
                    {
                        Value = d.IdDueno.ToString(),
                        Text = (d.Nombre ?? "Sin nombre") + " " + (d.Apellido ?? "")
                    })
                    .ToListAsync();

                vm.Especies = await _context.Especies
                    .Where(e => e.Activo)
                    .Select(e => new SelectListItem
                    {
                        Value = e.IdEspecie.ToString(),
                        Text = e.Nombre
                    })
                    .ToListAsync();

                vm.Razas = await _context.Razas
                    .Where(r => r.Activo)
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRaza.ToString(),
                        Text = r.Nombre
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recargar combos: {ex.Message}");
                vm.Duenos = new List<SelectListItem>();
                vm.Especies = new List<SelectListItem>();
                vm.Razas = new List<SelectListItem>();
                ViewBag.ErrorMessage = $"Error al cargar los datos: {ex.Message}";
            }

            return View(vm);
        }

        // GET: Mascota/CreateModal - Para cargar en modal
        public async Task<IActionResult> CreateModal()
        {
            try
            {
                var duenos = await _context.Duenos
                    .Where(d => d.Activo)
                    .Select(d => new SelectListItem
                    {
                        Value = d.IdDueno.ToString(),
                        Text = (d.Nombre ?? "Sin nombre") + " " + (d.Apellido ?? "")
                    })
                    .ToListAsync();

                var especies = await _context.Especies
                    .Where(e => e.Activo)
                    .Select(e => new SelectListItem
                    {
                        Value = e.IdEspecie.ToString(),
                        Text = e.Nombre
                    })
                    .ToListAsync();

                var razas = await _context.Razas
                    .Where(r => r.Activo)
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRaza.ToString(),
                        Text = r.Nombre
                    })
                    .ToListAsync();

                var vm = new MascotaCreateViewModel
                {
                    Mascota = new Mascota { Activo = true },
                    Duenos = duenos,
                    Especies = especies,
                    Razas = razas
                };

                return PartialView("_CreatePartial", vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateModal Mascota: {ex.Message}");
                return PartialView("_CreatePartial", new MascotaCreateViewModel
                {
                    Mascota = new Mascota { Activo = true },
                    Duenos = new List<SelectListItem>(),
                    Especies = new List<SelectListItem>(),
                    Razas = new List<SelectListItem>()
                });
            }
        }

        // GET: Mascota/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Obtener mascota de forma segura
                var mascotaData = await _context.Mascotas
                    .Where(m => m.IdMascota == id)
                    .Select(m => new
                    {
                        m.IdMascota,
                        m.Nombre,
                        m.Color,
                        m.Sexo,
                        m.FechaNacimiento,
                        m.IdEspecie,
                        m.IdRaza,
                        m.IdDueno,
                        m.Peso,
                        m.Tamaño,
                        m.NumeroMicrochip,
                        m.Pelaje,
                        m.Alergia,
                        m.Observaciones
                    })
                    .FirstOrDefaultAsync();

                if (mascotaData == null)
                {
                    return NotFound();
                }

                // Crear objeto mascota con valores seguros
                var mascota = new Mascota
                {
                    IdMascota = mascotaData.IdMascota,
                    Nombre = mascotaData.Nombre ?? "",
                    Color = mascotaData.Color ?? "No especificado",
                    Sexo = mascotaData.Sexo ?? "M",
                    FechaNacimiento = mascotaData.FechaNacimiento,
                    IdEspecie = mascotaData.IdEspecie,
                    IdRaza = mascotaData.IdRaza,
                    IdDueno = mascotaData.IdDueno,
                    Peso = mascotaData.Peso,
                    Tamaño = mascotaData.Tamaño ?? "",
                    NumeroMicrochip = mascotaData.NumeroMicrochip ?? "",
                    Pelaje = mascotaData.Pelaje ?? "",
                    Alergia = mascotaData.Alergia ?? "",
                    Observaciones = mascotaData.Observaciones ?? ""
                };

                // Cargar listas para el ViewModel
                var duenos = new List<SelectListItem>();
                var especies = new List<SelectListItem>();
                var razas = new List<SelectListItem>();

                try
                {
                    duenos = await _context.Duenos
                        .Select(d => new SelectListItem
                        {
                            Value = d.IdDueno.ToString(),
                            Text = (d.Nombre ?? "Sin nombre") + " " + (d.Apellido ?? ""),
                            Selected = d.IdDueno == mascota.IdDueno
                        })
                        .ToListAsync();
                }
                catch
                {
                    duenos = new List<SelectListItem>();
                }

                try
                {
                    especies = await _context.Especies
                        .Where(e => e.Activo)
                        .Select(e => new SelectListItem
                        {
                            Value = e.IdEspecie.ToString(),
                            Text = e.Nombre ?? "Sin nombre",
                            Selected = e.IdEspecie == mascota.IdEspecie
                        })
                        .ToListAsync();
                }
                catch
                {
                    especies = new List<SelectListItem>();
                }

                try
                {
                    razas = await _context.Razas
                        .Where(r => r.Activo)
                        .Select(r => new SelectListItem
                        {
                            Value = r.IdRaza.ToString(),
                            Text = r.Nombre ?? "Sin nombre",
                            Selected = r.IdRaza == mascota.IdRaza
                        })
                        .ToListAsync();
                }
                catch
                {
                    razas = new List<SelectListItem>();
                }

                // Crear ViewModel para edición
                var viewModel = new MascotaCreateViewModel
                {
                    Mascota = mascota,
                    Duenos = duenos,
                    Especies = especies,
                    Razas = razas
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Edit GET: {ex.Message}");
                return NotFound();
            }
        }

        // POST: Mascota/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MascotaCreateViewModel vm)
        {
            if (id != vm.Mascota.IdMascota)
            {
                return NotFound();
            }

            try
            {
                // Asegurar que los campos tengan valores por defecto si están vacíos
                if (string.IsNullOrWhiteSpace(vm.Mascota.Color))
                {
                    vm.Mascota.Color = "No especificado";
                }

                // Validar campos requeridos manualmente
                if (string.IsNullOrWhiteSpace(vm.Mascota.Nombre))
                {
                    ModelState.AddModelError("Mascota.Nombre", "El nombre es requerido");
                }

                if (vm.Mascota.IdEspecie <= 0)
                {
                    ModelState.AddModelError("Mascota.IdEspecie", "Debe seleccionar una especie");
                }

                if (vm.Mascota.IdRaza <= 0)
                {
                    ModelState.AddModelError("Mascota.IdRaza", "Debe seleccionar una raza");
                }

                if (vm.Mascota.IdDueno <= 0)
                {
                    ModelState.AddModelError("Mascota.IdDueno", "Debe seleccionar un dueño");
                }

                if (string.IsNullOrWhiteSpace(vm.Mascota.Sexo) || (vm.Mascota.Sexo != "M" && vm.Mascota.Sexo != "H"))
                {
                    ModelState.AddModelError("Mascota.Sexo", "Debe seleccionar el sexo (M o H)");
                }

                if (ModelState.IsValid)
                {
                    // Crear objeto actualizado con todos los campos necesarios
                    var mascotaActualizada = new Mascota
                    {
                        IdMascota = vm.Mascota.IdMascota,
                        Nombre = vm.Mascota.Nombre?.Trim() ?? "",
                        Color = string.IsNullOrWhiteSpace(vm.Mascota.Color) ? "No especificado" : vm.Mascota.Color.Trim(),
                        Sexo = vm.Mascota.Sexo?.Trim() ?? "",
                        IdEspecie = vm.Mascota.IdEspecie,
                        IdRaza = vm.Mascota.IdRaza,
                        IdDueno = vm.Mascota.IdDueno,
                        IdCliente = vm.Mascota.IdDueno, // Asignar el mismo valor para compatibilidad con hosting
                        Activo = true, // Mantener activo
                        FechaNacimiento = vm.Mascota.FechaNacimiento,
                        Peso = vm.Mascota.Peso,
                        Tamaño = vm.Mascota.Tamaño?.Trim(),
                        NumeroMicrochip = vm.Mascota.NumeroMicrochip?.Trim(),
                        Pelaje = vm.Mascota.Pelaje?.Trim(),
                        Alergia = vm.Mascota.Alergia?.Trim(),
                        Observaciones = vm.Mascota.Observaciones?.Trim()
                    };

                    // Log antes de actualizar
                    Console.WriteLine("=== ANTES DE ACTUALIZAR ===");
                    Console.WriteLine($"IdMascota: {mascotaActualizada.IdMascota}");
                    Console.WriteLine($"Color: '{mascotaActualizada.Color}'");
                    Console.WriteLine($"IdCliente: {mascotaActualizada.IdCliente}");
                    Console.WriteLine($"Activo: {mascotaActualizada.Activo}");

                    _context.Update(mascotaActualizada);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MascotaExists(vm.Mascota.IdMascota))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al editar mascota: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                ViewBag.ErrorMessage = $"Error al actualizar la mascota: {ex.Message}";
            }

            // Si hay error, recargar combos del ViewModel
            try
            {
                vm.Duenos = await _context.Duenos
                    .Select(d => new SelectListItem
                    {
                        Value = d.IdDueno.ToString(),
                        Text = (d.Nombre ?? "Sin nombre") + " " + (d.Apellido ?? ""),
                        Selected = d.IdDueno == vm.Mascota.IdDueno
                    })
                    .ToListAsync();

                vm.Especies = await _context.Especies
                    .Where(e => e.Activo)
                    .Select(e => new SelectListItem
                    {
                        Value = e.IdEspecie.ToString(),
                        Text = e.Nombre ?? "Sin nombre",
                        Selected = e.IdEspecie == vm.Mascota.IdEspecie
                    })
                    .ToListAsync();

                vm.Razas = await _context.Razas
                    .Where(r => r.Activo)
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRaza.ToString(),
                        Text = r.Nombre ?? "Sin nombre",
                        Selected = r.IdRaza == vm.Mascota.IdRaza
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recargar combos: {ex.Message}");
                vm.Duenos = new List<SelectListItem>();
                vm.Especies = new List<SelectListItem>();
                vm.Razas = new List<SelectListItem>();
                ViewBag.ErrorMessage = $"Error al cargar los datos: {ex.Message}";
            }

            return View(vm);
        }

        // GET: Mascota/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Obtener mascota básica primero
                var mascotaBasica = await _context.Mascotas
                    .Where(m => m.IdMascota == id)
                    .Select(m => new
                    {
                        m.IdMascota,
                        m.Nombre,
                        m.Color,
                        m.Sexo,
                        m.FechaNacimiento,
                        m.IdEspecie,
                        m.IdRaza,
                        m.IdDueno
                    })
                    .FirstOrDefaultAsync();

                if (mascotaBasica == null)
                {
                    return NotFound();
                }

                // Crear objeto mascota con datos seguros
                var mascota = new Mascota
                {
                    IdMascota = mascotaBasica.IdMascota,
                    Nombre = mascotaBasica.Nombre ?? "Sin nombre",
                    Color = mascotaBasica.Color ?? "No especificado",
                    Sexo = mascotaBasica.Sexo ?? "N",
                    FechaNacimiento = mascotaBasica.FechaNacimiento,
                    IdEspecie = mascotaBasica.IdEspecie,
                    IdRaza = mascotaBasica.IdRaza,
                    IdDueno = mascotaBasica.IdDueno
                };

                // Cargar entidades relacionadas de forma segura
                try
                {
                    var especie = await _context.Especies
                        .Where(e => e.IdEspecie == mascotaBasica.IdEspecie)
                        .FirstOrDefaultAsync();
                    mascota.Especie = especie ?? new Especie { Nombre = "Sin especie" };
                }
                catch
                {
                    mascota.Especie = new Especie { Nombre = "Sin especie" };
                }

                try
                {
                    var raza = await _context.Razas
                        .Where(r => r.IdRaza == mascotaBasica.IdRaza)
                        .FirstOrDefaultAsync();
                    mascota.Raza = raza ?? new Raza { Nombre = "Sin raza" };
                }
                catch
                {
                    mascota.Raza = new Raza { Nombre = "Sin raza" };
                }

                try
                {
                    var dueno = await _context.Duenos
                        .Where(d => d.IdDueno == mascotaBasica.IdDueno)
                        .FirstOrDefaultAsync();
                    mascota.Dueno = dueno ?? new Dueno { Nombre = "Sin dueño", Apellido = "" };
                }
                catch
                {
                    mascota.Dueno = new Dueno { Nombre = "Sin dueño", Apellido = "" };
                }

                return View(mascota);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Delete: {ex.Message}");
                return NotFound();
            }
        }

        // POST: Mascota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascotas.Any(e => e.IdMascota == id);
        }

        // Método para obtener razas por especie (AJAX)
        [HttpGet]
        public async Task<JsonResult> GetRazasByEspecie(int idEspecie)
        {
            try
            {
                var razas = await _context.Razas
                    .Where(r => r.IdEspecie == idEspecie && r.Activo)
                    .Select(r => new { value = r.IdRaza, text = r.Nombre })
                    .ToListAsync();

                return Json(razas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener razas: {ex.Message}");
                return Json(new List<object>());
            }
        }
    }
}