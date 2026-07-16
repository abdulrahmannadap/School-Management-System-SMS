// Sets the light/dark theme attribute before any CSS paints, avoiding a flash
// of the wrong theme. Must be loaded as a plain, non-deferred, non-async
// <script> as the very first thing in <head> (parser-blocking by design) so
// it runs before the browser starts rendering. No inline script needed --
// an external file loaded this way blocks the same way.
(function () {
    'use strict';
    var stored = localStorage.getItem('sms.theme');
    var theme = stored === 'dark' ? 'dark' : 'light';
    document.documentElement.setAttribute('data-bs-theme', theme);
})();
