// Theme toggle button behavior. theme-init.js (loaded early in <head>) has
// already applied the persisted theme before paint; this just wires the
// button click once the DOM (and the button) exists.
(function () {
    'use strict';

    var toggleBtn = document.getElementById('themeToggle');
    if (!toggleBtn) return;

    function currentTheme() {
        return document.documentElement.getAttribute('data-bs-theme') === 'dark' ? 'dark' : 'light';
    }

    function applyTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        localStorage.setItem('sms.theme', theme);
        toggleBtn.setAttribute('aria-pressed', String(theme === 'dark'));
    }

    toggleBtn.setAttribute('aria-pressed', String(currentTheme() === 'dark'));

    toggleBtn.addEventListener('click', function () {
        applyTheme(currentTheme() === 'dark' ? 'light' : 'dark');
    });
})();
