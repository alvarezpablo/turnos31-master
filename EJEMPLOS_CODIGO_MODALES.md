# 💻 Ejemplos de Código - Modales Ficha de Ingreso

## 📋 Índice
1. [Ejemplos de Controllers](#ejemplos-de-controllers)
2. [Ejemplos de Views](#ejemplos-de-views)
3. [Ejemplos de JavaScript](#ejemplos-de-javascript)
4. [Casos de Uso Específicos](#casos-de-uso-específicos)
5. [Extensiones y Mejoras](#extensiones-y-mejoras)

---

## 🎯 Ejemplos de Controllers

### **DuenoController.cs - Implementación Completa**

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class DuenoController : Controller
    {
        private readonly VeterinariaContext _context;

        public DuenoController(VeterinariaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Carga el formulario de creación de dueño para modal
        /// GET: /Dueno/CreateModal
        /// </summary>
        /// <returns>Vista parcial con formulario de dueño</returns>
        public IActionResult CreateModal()
        {
            var dueno = new Dueno 
            { 
                Activo = true 
            };
            return PartialView("_CreatePartial", dueno);
        }

        /// <summary>
        /// Procesa la creación de dueño vía AJAX desde modal
        /// POST: /Dueno/CreateAjaxModal
        /// </summary>
        /// <param name="dueno">Datos del dueño a crear</param>
        /// <returns>JSON con resultado de la operación</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjaxModal(
            [Bind("Nombre,Apellido,Direccion,Rut,Telefono,Email,Activo")] Dueno dueno)
        {
            try
            {
                // Verificar disponibilidad del contexto de base de datos
                if (_context == null)
                {
                    return Json(new { 
                        success = false, 
                        error = "Error de configuración: contexto de base de datos no disponible" 
                    });
                }

                // Validación de campos requeridos
                var validationErrors = ValidarDueno(dueno);
                if (validationErrors.Any())
                {
                    return Json(new { 
                        success = false, 
                        error = string.Join(", ", validationErrors) 
                    });
                }

                // Configurar valores por defecto
                dueno.IdDueno = 0; // Entity Framework generará el ID automáticamente
                dueno.Activo = true;
                
                // Guardar en base de datos
                _context.Add(dueno);
                await _context.SaveChangesAsync();
                
                // Retornar respuesta exitosa
                return Json(new { 
                    success = true, 
                    id = dueno.IdDueno, 
                    nombre = $"{dueno.Nombre} {dueno.Apellido}",
                    message = "Dueño creado exitosamente"
                });
            }
            catch (DbUpdateException dbEx)
            {
                // Error específico de base de datos
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return Json(new { 
                    success = false, 
                    error = $"Error de base de datos: {innerMessage}" 
                });
            }
            catch (InvalidOperationException ioEx)
            {
                // Error de configuración
                return Json(new { 
                    success = false, 
                    error = $"Error de configuración: {ioEx.Message}" 
                });
            }
            catch (Exception ex)
            {
                // Error general
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return Json(new { 
                    success = false, 
                    error = $"Error inesperado: {innerMessage}" 
                });
            }
        }

        /// <summary>
        /// Valida los datos del dueño
        /// </summary>
        /// <param name="dueno">Dueño a validar</param>
        /// <returns>Lista de errores de validación</returns>
        private List<string> ValidarDueno(Dueno dueno)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(dueno.Nombre))
                errores.Add("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(dueno.Apellido))
                errores.Add("El apellido es requerido");

            if (string.IsNullOrWhiteSpace(dueno.Telefono))
                errores.Add("El teléfono es requerido");

            if (string.IsNullOrWhiteSpace(dueno.Email))
                errores.Add("El email es requerido");
            else if (!IsValidEmail(dueno.Email))
                errores.Add("El formato del email no es válido");

            return errores;
        }

        /// <summary>
        /// Valida formato de email
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>True si es válido</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
```

### **FichaIngresoController.cs - Métodos de Apoyo**

```csharp
/// <summary>
/// Obtiene lista actualizada de dueños para dropdown
/// GET: /FichaIngreso/GetDuenos
/// </summary>
/// <returns>JSON con lista de dueños activos</returns>
public async Task<IActionResult> GetDuenos()
{
    try
    {
        var duenos = await _context.Duenos
            .Where(d => d.Activo)
            .OrderBy(d => d.Nombre)
            .ThenBy(d => d.Apellido)
            .Select(d => new {
                value = d.IdDueno,
                text = $"{d.Nombre} {d.Apellido}"
            })
            .ToListAsync();

        return Json(new { 
            success = true, 
            data = duenos,
            count = duenos.Count
        });
    }
    catch (Exception ex)
    {
        return Json(new { 
            success = false, 
            error = $"Error al obtener dueños: {ex.Message}" 
        });
    }
}

/// <summary>
/// Obtiene lista actualizada de mascotas para dropdown
/// GET: /FichaIngreso/GetMascotas
/// </summary>
/// <returns>JSON con lista de mascotas activas</returns>
public async Task<IActionResult> GetMascotas()
{
    try
    {
        var mascotas = await _context.Mascotas
            .Include(m => m.Dueno)
            .Include(m => m.Raza)
            .Where(m => m.Activo)
            .OrderBy(m => m.Nombre)
            .Select(m => new {
                value = m.IdMascota,
                text = $"{m.Nombre} ({m.Dueno.Nombre} {m.Dueno.Apellido})"
            })
            .ToListAsync();

        return Json(new { 
            success = true, 
            data = mascotas,
            count = mascotas.Count
        });
    }
    catch (Exception ex)
    {
        return Json(new { 
            success = false, 
            error = $"Error al obtener mascotas: {ex.Message}" 
        });
    }
}
```

---

## 🎨 Ejemplos de Views

### **_CreatePartial.cshtml - Formulario Modal Dueño**

```html
@model Turnos31.Models.Dueno

<form asp-action="CreateAjaxModal" method="post" class="needs-validation" novalidate>
    @Html.AntiForgeryToken()
    
    <!-- Resumen de errores de validación -->
    <div asp-validation-summary="ModelOnly" class="alert alert-danger" role="alert"></div>
    
    <!-- Campos principales en dos columnas -->
    <div class="row">
        <div class="col-md-6">
            <div class="mb-3">
                <label asp-for="Nombre" class="form-label">
                    <i class="bi bi-person me-1"></i>Nombre *
                </label>
                <input asp-for="Nombre" 
                       class="form-control" 
                       placeholder="Ingrese el nombre" 
                       required 
                       maxlength="100"
                       autocomplete="given-name" />
                <span asp-validation-for="Nombre" class="text-danger"></span>
                <div class="invalid-feedback">
                    Por favor ingrese un nombre válido.
                </div>
            </div>
        </div>
        
        <div class="col-md-6">
            <div class="mb-3">
                <label asp-for="Apellido" class="form-label">
                    <i class="bi bi-person me-1"></i>Apellido *
                </label>
                <input asp-for="Apellido" 
                       class="form-control" 
                       placeholder="Ingrese el apellido" 
                       required 
                       maxlength="100"
                       autocomplete="family-name" />
                <span asp-validation-for="Apellido" class="text-danger"></span>
                <div class="invalid-feedback">
                    Por favor ingrese un apellido válido.
                </div>
            </div>
        </div>
    </div>

    <!-- Segunda fila: RUT y Teléfono -->
    <div class="row">
        <div class="col-md-6">
            <div class="mb-3">
                <label asp-for="Rut" class="form-label">
                    <i class="bi bi-card-text me-1"></i>RUT
                </label>
                <input asp-for="Rut" 
                       class="form-control" 
                       placeholder="12.345.678-9"
                       maxlength="20"
                       pattern="[0-9]{1,2}\.[0-9]{3}\.[0-9]{3}-[0-9kK]{1}" />
                <span asp-validation-for="Rut" class="text-danger"></span>
                <div class="form-text">Formato: 12.345.678-9 (opcional)</div>
            </div>
        </div>
        
        <div class="col-md-6">
            <div class="mb-3">
                <label asp-for="Telefono" class="form-label">
                    <i class="bi bi-telephone me-1"></i>Teléfono *
                </label>
                <input asp-for="Telefono" 
                       class="form-control" 
                       placeholder="+56 9 1234 5678" 
                       required 
                       maxlength="20"
                       type="tel"
                       autocomplete="tel" />
                <span asp-validation-for="Telefono" class="text-danger"></span>
                <div class="invalid-feedback">
                    Por favor ingrese un teléfono válido.
                </div>
            </div>
        </div>
    </div>

    <!-- Email (ancho completo) -->
    <div class="mb-3">
        <label asp-for="Email" class="form-label">
            <i class="bi bi-envelope me-1"></i>Correo Electrónico *
        </label>
        <input asp-for="Email" 
               type="email" 
               class="form-control" 
               placeholder="correo@ejemplo.com" 
               required 
               maxlength="100"
               autocomplete="email" />
        <span asp-validation-for="Email" class="text-danger"></span>
        <div class="invalid-feedback">
            Por favor ingrese un email válido.
        </div>
    </div>

    <!-- Dirección (opcional) -->
    <div class="mb-3">
        <label asp-for="Direccion" class="form-label">
            <i class="bi bi-geo-alt me-1"></i>Dirección
        </label>
        <textarea asp-for="Direccion" 
                  class="form-control" 
                  rows="2" 
                  placeholder="Ingrese la dirección completa (opcional)"
                  maxlength="200"></textarea>
        <span asp-validation-for="Direccion" class="text-danger"></span>
        <div class="form-text">Campo opcional</div>
    </div>

    <!-- Campos ocultos -->
    <input type="hidden" asp-for="Activo" value="true" />
    <input type="hidden" name="Activo" value="true" />

    <!-- Botones de acción -->
    <div class="d-flex justify-content-end gap-2 mt-4">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
            <i class="bi bi-x-circle me-2"></i>Cancelar
        </button>
        <button type="submit" class="btn btn-primary">
            <i class="bi bi-check-circle me-2"></i>Guardar Dueño
        </button>
    </div>
</form>

<!-- Script para validación Bootstrap -->
<script>
(function() {
    'use strict';
    
    // Validación de Bootstrap
    var form = document.querySelector('.needs-validation');
    if (form) {
        form.addEventListener('submit', function(event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    }
    
    // Formateo automático de RUT
    var rutInput = document.querySelector('input[name="Rut"]');
    if (rutInput) {
        rutInput.addEventListener('input', function(e) {
            var value = e.target.value.replace(/[^0-9kK]/g, '');
            if (value.length > 1) {
                value = value.slice(0, -1).replace(/\B(?=(\d{3})+(?!\d))/g, '.') + '-' + value.slice(-1);
            }
            e.target.value = value;
        });
    }
})();
</script>
```

### **Create.cshtml - Dropdown Inteligente**

```html
<!-- Dropdown de Dueños con opción "Agregar" -->
<div class="mb-3">
    <label for="selectDueno" class="form-label">
        <i class="bi bi-person me-1"></i>Dueño *
    </label>
    <select id="selectDueno" name="IdDueno" class="form-select" required>
        <option value="">Seleccione un dueño</option>
        @if (ViewBag.Duenos != null)
        {
            @foreach (var dueno in ViewBag.Duenos as SelectList)
            {
                <option value="@dueno.Value" 
                        @(dueno.Selected ? "selected" : "")>
                    @dueno.Text
                </option>
            }
        }
        <option value="0" class="text-primary fw-bold">
            <i class="bi bi-plus-circle"></i> Agregar dueño
        </option>
    </select>
    <span asp-validation-for="IdDueno" class="text-danger"></span>
    <div class="form-text">
        Si el dueño no existe, seleccione "Agregar dueño" para crearlo.
    </div>
</div>

<!-- Dropdown de Mascotas con opción "Agregar" -->
<div class="mb-3">
    <label for="selectMascota" class="form-label">
        <i class="bi bi-heart me-1"></i>Mascota *
    </label>
    <select id="selectMascota" name="IdMascota" class="form-select" required>
        <option value="">Seleccione una mascota</option>
        @if (ViewBag.Mascotas != null)
        {
            @foreach (var mascota in ViewBag.Mascotas as SelectList)
            {
                <option value="@mascota.Value" 
                        @(mascota.Selected ? "selected" : "")>
                    @mascota.Text
                </option>
            }
        }
        <option value="0" class="text-success fw-bold">
            <i class="bi bi-plus-circle"></i> Agregar mascota
        </option>
    </select>
    <span asp-validation-for="IdMascota" class="text-danger"></span>
    <div class="form-text">
        Si la mascota no existe, seleccione "Agregar mascota" para crearla.
    </div>
</div>
```

---

## ⚡ Ejemplos de JavaScript

### **Funciones Principales - Manejo de Modales**

```javascript
/**
 * Clase para manejar modales de Ficha de Ingreso
 */
class FichaIngresoModales {
    constructor() {
        this.initializeEventListeners();
        this.configureToastr();
    }

    /**
     * Inicializa los event listeners principales
     */
    initializeEventListeners() {
        // Detectar cambios en dropdown de dueños
        $('#selectDueno').on('change', (e) => {
            if ($(e.target).val() === '0') {
                this.abrirModalDueno();
                $(e.target).val(''); // Resetear selección
            }
        });

        // Detectar cambios en dropdown de mascotas
        $('#selectMascota').on('change', (e) => {
            if ($(e.target).val() === '0') {
                this.abrirModalMascota();
                $(e.target).val(''); // Resetear selección
            }
        });

        // Limpiar modales al cerrar
        $('#modalAgregarDueno').on('hidden.bs.modal', () => {
            this.limpiarModal('#contenidoModalDueno');
        });

        $('#modalAgregarMascota').on('hidden.bs.modal', () => {
            this.limpiarModal('#contenidoModalMascota');
        });
    }

    /**
     * Configura Toastr para notificaciones
     */
    configureToastr() {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
    }

    /**
     * Abre modal de agregar dueño
     */
    abrirModalDueno() {
        console.log('Abriendo modal de dueño...');

        // Mostrar modal con loading
        $('#modalAgregarDueno').modal('show');
        this.mostrarLoading('#contenidoModalDueno');

        // Cargar formulario vía AJAX
        $.get('/Dueno/CreateModal')
            .done((data) => {
                $('#contenidoModalDueno').html(data);
                this.configurarFormularioDueno();
                this.configurarValidacionBootstrap('#contenidoModalDueno form');
            })
            .fail((xhr, status, error) => {
                console.error('Error al cargar formulario de dueño:', error);
                $('#contenidoModalDueno').html(
                    '<div class="alert alert-danger">' +
                    '<i class="bi bi-exclamation-triangle me-2"></i>' +
                    'Error al cargar el formulario. Por favor intente nuevamente.' +
                    '</div>'
                );
            });
    }

    /**
     * Configura el formulario de dueño
     */
    configurarFormularioDueno() {
        const form = $('#contenidoModalDueno form');

        // Remover eventos anteriores para evitar duplicados
        form.off('submit').on('submit', (e) => {
            e.preventDefault();
            this.procesarFormularioDueno(form);
        });
    }

    /**
     * Procesa el envío del formulario de dueño
     */
    procesarFormularioDueno(form) {
        const formData = form.serialize();
        const actionUrl = form.attr('action');
        const submitBtn = form.find('button[type="submit"]');

        // Logging para debugging
        console.log('=== PROCESANDO DUEÑO ===');
        console.log('URL:', actionUrl);
        console.log('Data:', formData);

        // Validación del lado cliente
        if (!form[0].checkValidity()) {
            form.addClass('was-validated');
            return;
        }

        // Deshabilitar botón y mostrar loading
        this.setButtonLoading(submitBtn, 'Guardando...');

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: formData,
            timeout: 30000,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            success: (response) => {
                this.handleDuenoSuccess(response, submitBtn);
            },
            error: (xhr, status, error) => {
                this.handleDuenoError(xhr, status, error, submitBtn);
            }
        });
    }

    /**
     * Maneja respuesta exitosa del dueño
     */
    handleDuenoSuccess(response, submitBtn) {
        console.log('Respuesta dueño:', response);

        // Restaurar botón
        this.resetButton(submitBtn, '<i class="bi bi-check-circle me-2"></i>Guardar Dueño');

        if (response && response.success) {
            // Éxito - cerrar modal y actualizar
            $('#modalAgregarDueno').modal('hide');
            this.actualizarComboDuenos();
            toastr.success(response.message || 'Dueño agregado exitosamente');
        } else if (typeof response === 'string') {
            // Errores de validación - mostrar formulario con errores
            $('#contenidoModalDueno').html(response);
            this.configurarFormularioDueno();
            this.configurarValidacionBootstrap('#contenidoModalDueno form');
        } else if (response && response.error) {
            // Error específico del servidor
            toastr.error(response.error);
        } else {
            // Respuesta inesperada
            toastr.error('Respuesta inesperada del servidor');
        }
    }

    /**
     * Maneja errores del dueño
     */
    handleDuenoError(xhr, status, error, submitBtn) {
        console.error('=== ERROR DUEÑO ===');
        console.error('Status:', status);
        console.error('Error:', error);
        console.error('Response:', xhr.responseText);

        // Restaurar botón
        this.resetButton(submitBtn, '<i class="bi bi-check-circle me-2"></i>Guardar Dueño');

        // Mostrar error específico
        let errorMessage = 'Error al guardar el dueño';

        if (xhr.status === 400) {
            errorMessage = 'Datos inválidos. Verifique la información ingresada.';
        } else if (xhr.status === 500) {
            errorMessage = 'Error interno del servidor. Contacte al administrador.';
        } else if (status === 'timeout') {
            errorMessage = 'La operación tardó demasiado. Intente nuevamente.';
        }

        toastr.error(`${errorMessage} (${xhr.status})`);
    }

    /**
     * Actualiza el dropdown de dueños
     */
    actualizarComboDuenos() {
        console.log('Actualizando combo de dueños...');

        $.get('/FichaIngreso/GetDuenos')
            .done((response) => {
                if (response.success && response.data) {
                    const select = $('#selectDueno');
                    const valorActual = select.val();

                    // Limpiar opciones excepto la primera
                    select.find('option').not(':first').remove();

                    // Agregar dueños
                    response.data.forEach((dueno) => {
                        select.append(`<option value="${dueno.value}">${dueno.text}</option>`);
                    });

                    // Agregar opción "Agregar dueño"
                    select.append('<option value="0" class="text-primary fw-bold">Agregar dueño</option>');

                    console.log(`Combo actualizado con ${response.data.length} dueños`);
                } else {
                    console.error('Error en respuesta de dueños:', response);
                }
            })
            .fail((xhr, status, error) => {
                console.error('Error al actualizar dueños:', error);
                toastr.error('Error al actualizar la lista de dueños');
            });
    }

    /**
     * Configura validación de Bootstrap
     */
    configurarValidacionBootstrap(selector) {
        const form = document.querySelector(selector);
        if (form) {
            form.addEventListener('submit', function(event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        }
    }

    /**
     * Muestra loading en un contenedor
     */
    mostrarLoading(selector) {
        $(selector).html(`
            <div class="text-center py-4">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Cargando...</span>
                </div>
                <div class="mt-2">Cargando formulario...</div>
            </div>
        `);
    }

    /**
     * Limpia el contenido de un modal
     */
    limpiarModal(selector) {
        $(selector).empty();
    }

    /**
     * Configura botón en estado loading
     */
    setButtonLoading(button, text) {
        button.prop('disabled', true)
              .html(`<span class="spinner-border spinner-border-sm me-2" role="status"></span>${text}`);
    }

    /**
     * Restaura botón a estado normal
     */
    resetButton(button, html) {
        button.prop('disabled', false).html(html);
    }
}

// Inicializar cuando el documento esté listo
$(document).ready(function() {
    window.fichaIngresoModales = new FichaIngresoModales();
});
```
