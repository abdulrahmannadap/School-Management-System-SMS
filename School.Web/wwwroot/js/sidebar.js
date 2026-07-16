// Sidebar navigation: desktop icon-rail collapse, mobile overlay drawer,
// single-open-submenu accordion, active-link detection, tooltips, keyboard nav.
// Vanilla JS only — no jQuery. Relies on Bootstrap 5.3's Collapse/Tooltip APIs.
(function () {
    'use strict';

    var STORAGE_COLLAPSED = 'sms.sidebar.collapsed';
    var STORAGE_OPEN_SUBMENU = 'sms.sidebar.openSubmenu';
    var MOBILE_BREAKPOINT = 991;

    var sidebar = document.getElementById('sidebar');
    var overlay = document.getElementById('sidebarOverlay');
    if (!sidebar) return;

    var sidebarNav = sidebar.querySelector('.sidebar-nav');
    var collapseToggleBtn = document.getElementById('sidebarCollapseToggle');
    var mobileToggleBtn = document.getElementById('sidebarMobileToggle');

    // ── Desktop collapse (icon-rail) ────────────────────────────────────
    function isCollapsed() {
        return document.body.classList.contains('sidebar-collapsed');
    }

    function setCollapsed(collapsed, persist) {
        document.body.classList.toggle('sidebar-collapsed', collapsed);
        if (persist !== false) localStorage.setItem(STORAGE_COLLAPSED, collapsed ? '1' : '0');
        updateTooltipsEnabled();
        if (collapseToggleBtn) collapseToggleBtn.setAttribute('aria-pressed', String(collapsed));
    }

    function restoreCollapsedState() {
        // Applied without a transition so there's no animated "jump" on first paint.
        document.body.classList.add('no-transition');
        setCollapsed(localStorage.getItem(STORAGE_COLLAPSED) === '1', false);
        requestAnimationFrame(function () {
            requestAnimationFrame(function () { document.body.classList.remove('no-transition'); });
        });
    }

    function initDesktopCollapse() {
        if (!collapseToggleBtn) return;
        collapseToggleBtn.addEventListener('click', function () {
            setCollapsed(!isCollapsed(), true);
        });
    }

    // ── Mobile overlay drawer ───────────────────────────────────────────
    function openMobileDrawer() {
        sidebar.classList.add('open');
        if (overlay) overlay.classList.add('open');
    }

    function closeMobileDrawer() {
        sidebar.classList.remove('open');
        if (overlay) overlay.classList.remove('open');
    }

    function initMobileDrawer() {
        if (mobileToggleBtn) {
            mobileToggleBtn.addEventListener('click', function () {
                sidebar.classList.contains('open') ? closeMobileDrawer() : openMobileDrawer();
            });
        }
        if (overlay) overlay.addEventListener('click', closeMobileDrawer);

        window.addEventListener('resize', function () {
            if (window.innerWidth > MOBILE_BREAKPOINT) closeMobileDrawer();
        });
    }

    // ── Accordion: only one submenu open at a time ──────────────────────
    function initAccordion() {
        var subNavs = sidebarNav.querySelectorAll('.sub-nav');
        subNavs.forEach(function (subNav) {
            subNav.addEventListener('show.bs.collapse', function (evt) {
                if (evt.target !== subNav) return; // ignore bubbled events from nested elements
                subNavs.forEach(function (other) {
                    if (other !== subNav && other.classList.contains('show')) {
                        bootstrap.Collapse.getOrCreateInstance(other, { toggle: false }).hide();
                    }
                });
            });
            subNav.addEventListener('shown.bs.collapse', function (evt) {
                if (evt.target !== subNav) return;
                localStorage.setItem(STORAGE_OPEN_SUBMENU, subNav.id);
            });
        });
    }

    // ── Active link detection + auto-expand parent ──────────────────────
    function markActiveAndExpand() {
        var currentPath = window.location.pathname.toLowerCase();
        var matched = false;

        sidebarNav.querySelectorAll('.nav-link').forEach(function (link) {
            var href = link.getAttribute('href');
            if (!href || href === '#') return;

            var linkPath;
            try { linkPath = new URL(href, window.location.origin).pathname.toLowerCase(); }
            catch (e) { return; }

            var isActive = currentPath === linkPath || (linkPath.length > 1 && currentPath.startsWith(linkPath + '/'));
            if (!isActive) return;

            matched = true;
            link.classList.add('active');
            link.setAttribute('aria-current', 'page');

            var parentSubNav = link.closest('.sub-nav');
            if (parentSubNav) {
                expandSubNav(parentSubNav);
                var trigger = document.querySelector('[data-bs-toggle="collapse"][href="#' + parentSubNav.id + '"]');
                if (trigger) trigger.classList.add('active-parent');
            }
        });

        if (!matched) {
            var storedId = localStorage.getItem(STORAGE_OPEN_SUBMENU);
            if (storedId) {
                var stored = document.getElementById(storedId);
                if (stored && stored.classList.contains('sub-nav')) expandSubNav(stored);
            }
        }
    }

    function expandSubNav(subNav) {
        // Direct class toggle (not the Bootstrap Collapse API) so this doesn't
        // animate or fire show.bs.collapse during initial page load.
        subNav.classList.add('show');
        var trigger = document.querySelector('[data-bs-toggle="collapse"][href="#' + subNav.id + '"]');
        if (trigger) trigger.setAttribute('aria-expanded', 'true');
    }

    // ── Tooltips (collapsed rail only) ──────────────────────────────────
    var tooltipInstances = [];

    function initTooltips() {
        sidebarNav.querySelectorAll('.nav-link').forEach(function (link) {
            var label = link.querySelector('.nav-label');
            var text = label ? label.textContent.trim() : '';
            if (!text) return;

            var tooltip = new bootstrap.Tooltip(link, {
                title: text,
                placement: 'right',
                trigger: 'hover focus'
            });
            tooltipInstances.push(tooltip);
        });
        updateTooltipsEnabled();
    }

    function updateTooltipsEnabled() {
        var enabled = isCollapsed();
        tooltipInstances.forEach(function (t) {
            enabled ? t.enable() : t.disable();
        });
    }

    // ── Keyboard navigation ──────────────────────────────────────────────
    function initKeyboardNav() {
        sidebarNav.addEventListener('keydown', function (evt) {
            if (evt.key !== 'ArrowDown' && evt.key !== 'ArrowUp' && evt.key !== 'Escape') return;

            var links = Array.prototype.filter.call(
                sidebarNav.querySelectorAll('.nav-link'),
                function (el) { return el.offsetParent !== null; }
            );

            if (evt.key === 'Escape') {
                var openSubNav = evt.target.closest('.sidebar-nav')
                    ? sidebarNav.querySelector('.sub-nav.show')
                    : null;
                if (openSubNav) {
                    bootstrap.Collapse.getOrCreateInstance(openSubNav).hide();
                    var trigger = document.querySelector('[data-bs-toggle="collapse"][href="#' + openSubNav.id + '"]');
                    if (trigger) trigger.focus();
                } else if (window.innerWidth <= MOBILE_BREAKPOINT) {
                    closeMobileDrawer();
                }
                return;
            }

            var currentIndex = links.indexOf(document.activeElement);
            if (currentIndex === -1) return;
            evt.preventDefault();

            var nextIndex = evt.key === 'ArrowDown' ? currentIndex + 1 : currentIndex - 1;
            if (nextIndex < 0) nextIndex = links.length - 1;
            if (nextIndex >= links.length) nextIndex = 0;
            links[nextIndex].focus();
        });
    }

    // ── Init ─────────────────────────────────────────────────────────────
    restoreCollapsedState();
    initDesktopCollapse();
    initMobileDrawer();
    initAccordion();
    markActiveAndExpand();
    initTooltips();
    initKeyboardNav();
})();
