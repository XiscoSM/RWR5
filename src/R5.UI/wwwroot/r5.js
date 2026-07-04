// Utilidades mínimas de la UI R5.
window.r5 = {
    // Devuelve el foco a un campo y selecciona su contenido: clave para encadenar
    // lecturas del escáner de código de barras (emulación de teclado) sin tocar la pantalla.
    seleccionar: function (id) {
        const el = document.getElementById(id);
        if (el) { el.focus(); el.select(); }
    },

    // Pitidos de confirmación/rechazo (WebAudio, sin ficheros): en tablet con escáner
    // el operario no mira la pantalla en cada lectura, el sonido es la confirmación.
    _audio: null,
    sonido: function (tipo) {
        try {
            if (!this._audio) this._audio = new (window.AudioContext || window.webkitAudioContext)();
            const ctx = this._audio;
            if (ctx.state === "suspended") ctx.resume();
            const tono = (frec, inicio, dur) => {
                const osc = ctx.createOscillator();
                const gan = ctx.createGain();
                osc.type = "square";
                osc.frequency.value = frec;
                gan.gain.setValueAtTime(0.15, ctx.currentTime + inicio);
                gan.gain.exponentialRampToValueAtTime(0.001, ctx.currentTime + inicio + dur);
                osc.connect(gan).connect(ctx.destination);
                osc.start(ctx.currentTime + inicio);
                osc.stop(ctx.currentTime + inicio + dur);
            };
            if (tipo === "exito") {
                tono(1245, 0, 0.09); // un pitido agudo corto: lectura aceptada
            } else if (tipo === "error") {
                tono(220, 0, 0.18);  // dos pitidos graves: rechazo, levanta la vista
                tono(220, 0.24, 0.18);
            }
        } catch { /* sin audio no se rompe nada */ }
    }
};
