using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.Filters;

namespace Turnos31.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class MenuController : Controller
    {
        private readonly VeterinariaContext _context;

        public MenuController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus
                .Include(m => m.MenuPadre)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
            return View(menus);
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.MenuPadre)
                .Include(m => m.MenuRoles)
                    .ThenInclude(mr => mr.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            ViewBag.MenuPadreId = new SelectList(_context.Menus, "Id", "Nombre");
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return View();
        }

        // POST: Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Url,Icono,MenuPadreId")] Menu menu, int[] roles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();

                if (roles != null)
                {
                    foreach (var rolId in roles)
                    {
                        var menuRol = new MenuRol
                        {
                            MenuId = menu.Id,
                            RolId = rolId
                        };
                        _context.MenuRoles.Add(menuRol);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.MenuPadreId = new SelectList(_context.Menus, "Id", "Nombre", menu.MenuPadreId);
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return View(menu);
        }

        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.MenuRoles)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            ViewBag.MenuPadreId = new SelectList(_context.Menus.Where(m => m.Id != id), "Id", "Nombre", menu.MenuPadreId);
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            ViewBag.SelectedRoles = menu.MenuRoles.Select(mr => mr.RolId).ToList();
            return View(menu);
        }

        // POST: Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Url,Icono,MenuPadreId")] Menu menu, int[] roles)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar el menú
                    _context.Update(menu);

                    // Eliminar roles existentes
                    var menuRoles = await _context.MenuRoles.Where(mr => mr.MenuId == id).ToListAsync();
                    _context.MenuRoles.RemoveRange(menuRoles);

                    // Agregar nuevos roles
                    if (roles != null)
                    {
                        foreach (var rolId in roles)
                        {
                            var menuRol = new MenuRol
                            {
                                MenuId = menu.Id,
                                RolId = rolId
                            };
                            _context.MenuRoles.Add(menuRol);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
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
            ViewBag.MenuPadreId = new SelectList(_context.Menus.Where(m => m.Id != id), "Id", "Nombre", menu.MenuPadreId);
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            ViewBag.SelectedRoles = roles?.ToList() ?? new List<int>();
            return View(menu);
        }

        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.MenuPadre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus
                .Include(m => m.MenuRoles)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menu != null)
            {
                _context.MenuRoles.RemoveRange(menu.MenuRoles);
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetMenu()
        {
            var usuarioRol = HttpContext.Session.GetString("UsuarioRol");
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            
            if (string.IsNullOrEmpty(usuarioRol))
            {
                return Json(new { success = false, message = "Usuario no autenticado" });
            }

            try
            {
                // Debug: Imprimir el valor exacto del rol
                Console.WriteLine($"Rol en sesión: '{usuarioRol}'");

                // Obtener el rol del usuario
                var rol = await _context.Roles
                    .FirstOrDefaultAsync(r => r.NombreRol.Trim().ToLower() == usuarioRol.Trim().ToLower());

                if (rol == null)
                {
                    // Debug: Listar todos los roles disponibles
                    var rolesDisponibles = await _context.Roles.Select(r => new { r.IdRol, r.NombreRol }).ToListAsync();
                    return Json(new { 
                        success = false, 
                        message = $"Rol no encontrado: '{usuarioRol}'",
                        debug = new {
                            rolEnSesion = usuarioRol,
                            rolesDisponibles = rolesDisponibles
                        }
                    });
                }

                // Debug: Verificar todos los roles y menús
                var todosLosRoles = await _context.Roles.ToListAsync();
                var todosLosMenus = await _context.Menus.ToListAsync();

                // Consulta directa a MenuRoles con información detallada
                var menuRolesDetalle = await _context.MenuRoles
                    .Include(mr => mr.Menu)
                    .Include(mr => mr.Rol)
                    .Select(mr => new {
                        mr.Id,
                        mr.MenuId,
                        mr.RolId,
                        MenuNombre = mr.Menu.Nombre,
                        RolNombre = mr.Rol.NombreRol
                    })
                    .ToListAsync();

                // Debug: Imprimir información detallada de MenuRoles
                Console.WriteLine($"Total de registros en MenuRoles: {menuRolesDetalle.Count}");
                foreach (var mr in menuRolesDetalle)
                {
                    Console.WriteLine($"MenuRoles - Id: {mr.Id}, MenuId: {mr.MenuId}, RolId: {mr.RolId}, Menu: {mr.MenuNombre}, Rol: {mr.RolNombre}");
                }

                // Obtener los menús asignados al rol del usuario
                var menuRoles = await _context.MenuRoles
                    .Where(mr => mr.RolId == rol.IdRol)
                    .Select(mr => mr.MenuId)
                    .ToListAsync();

                // Debug: Imprimir información de los menús asignados
                Console.WriteLine($"Menús asignados al rol {rol.IdRol}: {string.Join(", ", menuRoles)}");

                if (menuRoles.Count == 0)
                {
                    return Json(new { 
                        success = false, 
                        message = $"No hay menús asignados al rol: {usuarioRol}",
                        debug = new {
                            usuarioRol = usuarioRol,
                            rolId = rol.IdRol,
                            rolNombre = rol.NombreRol,
                            todosLosRoles = todosLosRoles.Select(r => new { r.IdRol, r.NombreRol }),
                            todosLosMenus = todosLosMenus.Select(m => new { m.Id, m.Nombre }),
                            menuRolesDetalle = menuRolesDetalle
                        }
                    });
                }

                // Obtener todos los menús y sus roles en una sola consulta
                var menus = await _context.Menus
                    .Where(m => menuRoles.Contains(m.Id))
                    .OrderBy(m => m.Nombre)
                    .ToListAsync();

                if (menus.Count == 0)
                {
                    return Json(new { success = false, message = "No se encontraron menús en la base de datos" });
                }

                // Construir la jerarquía de menús
                var menuItems = menus
                    .Where(m => m.MenuPadreId == null)
                    .Select(m => new
                    {
                        id = m.Id,
                        nombre = m.Nombre,
                        url = m.Url,
                        icono = m.Icono,
                        submenus = menus
                            .Where(sm => sm.MenuPadreId == m.Id)
                            .OrderBy(sm => sm.Nombre)
                            .Select(sm => new
                            {
                                id = sm.Id,
                                nombre = sm.Nombre,
                                url = sm.Url,
                                icono = sm.Icono
                            })
                            .ToList()
                    })
                    .ToList();

                // Agregar información de depuración
                var debugInfo = new
                {
                    usuarioId = usuarioId,
                    usuarioRol = usuarioRol,
                    rolId = rol.IdRol,
                    rolNombre = rol.NombreRol,
                    menuRolesCount = menuRoles.Count,
                    menusCount = menus.Count,
                    menuItemsCount = menuItems.Count,
                    menuRoles = menuRoles,
                    menuItems = menuItems,
                    todosLosRoles = todosLosRoles.Select(r => new { r.IdRol, r.NombreRol }),
                    todosLosMenus = todosLosMenus.Select(m => new { m.Id, m.Nombre }),
                    menuRolesDetalle = menuRolesDetalle
                };

                return Json(new { 
                    success = true, 
                    menus = menuItems,
                    debug = debugInfo
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // Página de diagnóstico - REMOVER EN PRODUCCIÓN
        public IActionResult Debug()
        {
            return View();
        }

        // Método temporal para depuración - REMOVER EN PRODUCCIÓN
        public async Task<IActionResult> DebugMenus()
        {
            var usuarioRol = HttpContext.Session.GetString("UsuarioRol");
            var usuarioId = HttpContext.Session.GetString("UsuarioId");

            var result = new
            {
                UsuarioId = usuarioId,
                UsuarioRol = usuarioRol,
                Roles = await _context.Roles.Select(r => new { r.IdRol, r.NombreRol }).ToListAsync(),
                TodosLosMenus = await _context.Menus.Select(m => new { m.Id, m.Nombre, m.Url, m.MenuPadreId }).ToListAsync(),
                MenuRoles = await _context.MenuRoles
                    .Include(mr => mr.Menu)
                    .Include(mr => mr.Rol)
                    .Select(mr => new {
                        mr.Id,
                        MenuId = mr.MenuId,
                        MenuNombre = mr.Menu.Nombre,
                        RolId = mr.RolId,
                        RolNombre = mr.Rol.NombreRol
                    })
                    .ToListAsync(),
                EspeciesYRazas = await _context.Menus
                    .Where(m => m.Nombre == "Especies" || m.Nombre == "Razas")
                    .Select(m => new { m.Id, m.Nombre, m.Url, m.MenuPadreId })
                    .ToListAsync()
            };

            return Json(result);
        }
    }
}