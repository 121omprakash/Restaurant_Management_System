// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const themeToggleBtn = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');
    const themeText = document.getElementById('themeText');

    if (themeToggleBtn) {
        // MATCHING FALLBACK: Use "day" (light) as default to match your layout
        const currentTheme = localStorage.getItem('theme') || 'day';

        // Sync attributes and classes on initial load
        if (currentTheme === 'dark' || currentTheme === 'night') {
            document.documentElement.setAttribute('data-theme', 'dark');
            document.documentElement.classList.add('dark-theme');
            updateToggleUI('dark');
        } else {
            document.documentElement.setAttribute('data-theme', 'light');
            document.documentElement.classList.remove('dark-theme');
            updateToggleUI('light');
        }

        themeToggleBtn.addEventListener('click', () => {
            const isCurrentlyDark = document.documentElement.getAttribute('data-theme') === 'dark';
            let targetTheme = isCurrentlyDark ? 'light' : 'dark';

            // Apply updates dynamically on click
            document.documentElement.setAttribute('data-theme', targetTheme);

            if (targetTheme === 'dark') {
                document.documentElement.classList.add('dark-theme');
                localStorage.setItem('theme', 'dark');
            } else {
                document.documentElement.classList.remove('dark-theme');
                localStorage.setItem('theme', 'day');
            }

            updateToggleUI(targetTheme);
        });
    }

    function updateToggleUI(theme) {
        if (!themeIcon || !themeText) return;

        if (theme === 'dark') {
            themeIcon.className = 'fa-solid fa-moon';
            themeText.innerText = 'Night Mode';
        } else {
            themeIcon.className = 'fa-solid fa-sun';
            themeText.innerText = 'Day Mode';
        }
    }
});