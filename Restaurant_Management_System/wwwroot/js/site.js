// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const themeToggleBtn = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');
    const themeText = document.getElementById('themeText');

    if (themeToggleBtn) {
        // Fallback to dark theme if no preference is saved
        const currentTheme = localStorage.getItem('theme') || 'dark';
        document.documentElement.setAttribute('data-theme', currentTheme);
        updateToggleUI(currentTheme);

        themeToggleBtn.addEventListener('click', () => {
            let targetTheme = 'dark';
            if (document.documentElement.getAttribute('data-theme') === 'dark') {
                targetTheme = 'light';
            }
            document.documentElement.setAttribute('data-theme', targetTheme);
            localStorage.setItem('theme', targetTheme);
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