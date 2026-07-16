// Full-page loading overlay: shown while a normal in-app navigation (link
// click or form submit) is in flight, hidden once the new page has painted.
// This is a classic full-page-reload MVC app, not an SPA, so "showing" just
// means displaying the overlay until the browser replaces the document.
(function () {
    'use strict';

    var loader = document.getElementById('pageLoader');
    if (!loader) return;

    function show() { loader.classList.add('show'); }
    function hide() { loader.classList.remove('show'); }

    document.addEventListener('click', function (evt) {
        var link = evt.target.closest('a[href]');
        if (!link) return;

        var href = link.getAttribute('href');
        var isSameDocSection = href.startsWith('#');
        var opensNewTab = link.target === '_blank';
        var isDownload = link.hasAttribute('download');
        var isExternal = link.origin && link.origin !== window.location.origin;

        if (!isSameDocSection && !opensNewTab && !isDownload && !isExternal) show();
    });

    document.addEventListener('submit', function (evt) {
        if (evt.target instanceof HTMLFormElement && evt.target.target !== '_blank') show();
    });

    // Hide on normal load and on back/forward-cache restores.
    window.addEventListener('pageshow', hide);
})();
