// Utilidades mínimas de la UI R5.
window.r5 = {
    // Devuelve el foco a un campo y selecciona su contenido: clave para encadenar
    // lecturas del escáner de código de barras (emulación de teclado) sin tocar la pantalla.
    seleccionar: function (id) {
        const el = document.getElementById(id);
        if (el) { el.focus(); el.select(); }
    }
};
