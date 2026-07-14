// Keep track of the currently selected filters globally within the page scope
let currentRoleFilter = 'all';
let currentStatusFilter = 'all';

// Initialization: Runs all internal interface bindings once the browser DOM tree finishes structural parsing
document.addEventListener("DOMContentLoaded", function () {

    // ======= 1. SIDEBAR ACTIVE HIGHLIGHT MANAGEMENT =======
    const currentPath = window.location.pathname.toLowerCase().replace(/\/$/, "");
    const menuItems = document.querySelectorAll("#adminSidebarMenu li");

    menuItems.forEach(item => {
        const linkElement = item.querySelector("a");
        if (linkElement) {
            const hrefTarget = linkElement.getAttribute("href").toLowerCase().replace(/\/$/, "");
            const dataMenu = item.getAttribute("data-menu") ? item.getAttribute("data-menu").toLowerCase() : "";

            // If path matches directly, or current path includes the link target action path
            if (currentPath === hrefTarget ||
                (hrefTarget !== "" && currentPath.includes(hrefTarget)) ||
                (currentPath.includes("admin/employees") && dataMenu === "employees") ||
                (currentPath.includes("admin/settings") && dataMenu === "settings")) {

                item.classList.add("active");
                linkElement.classList.add("active");
            } else {
                item.classList.remove("active");
                linkElement.classList.remove("active");
            }
        }
    });

    // ======= 2. SIDEBAR COLLAPSE TOGGLE =======
    const toggleBtn = document.getElementById("sidebarToggle");
    const sidebar = document.querySelector(".sidebar");
    if (toggleBtn && sidebar) {
        toggleBtn.addEventListener("click", function () {
            sidebar.classList.toggle("collapsed");
        });
    }

    // ======= 3. DROPDOWN STATUS & ROLE FILTERS =======
    document.querySelectorAll('.filter-role-opt').forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();

            const selection = this.getAttribute('data-role');
            const dropdownBtn = document.getElementById('roleFilterDropdown');
            if (!dropdownBtn) return;

            if (selection === 'status-active') {
                currentStatusFilter = 'active';
                dropdownBtn.innerHTML = `<i class="fa-solid fa-filter me-2"></i> Status: Active`;
            } else if (selection === 'status-inactive') {
                currentStatusFilter = 'inactive';
                dropdownBtn.innerHTML = `<i class="fa-solid fa-filter me-2"></i> Status: Inactive`;
            } else if (selection === 'all') {
                currentRoleFilter = 'all';
                currentStatusFilter = 'all';
                dropdownBtn.innerHTML = `<i class="fa-solid fa-filter me-2"></i> All Employees`;
            } else {
                currentRoleFilter = selection.toLowerCase();
                dropdownBtn.innerHTML = `<i class="fa-solid fa-filter me-2"></i> Role: ${selection}`;
            }

            filterTable();
        });
    });

    // ======= 4. SEARCH BAR LOGIC =======
    const userSearch = document.getElementById("userSearch");
    if (userSearch) {
        userSearch.addEventListener('keyup', function () {
            filterTable();
        });
    }

    // ======= 5. EDIT USER MODAL BINDINGS =======
    const editUserModal = document.getElementById('editUserModal');
    if (editUserModal) {
        editUserModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;

            document.getElementById('edit_Id').value = button.getAttribute('data-id');
            document.getElementById('edit_UserId').value = button.getAttribute('data-userid');
            document.getElementById('edit_Name').value = button.getAttribute('data-name');
            document.getElementById('edit_Role').value = button.getAttribute('data-role');

            const isActive = button.getAttribute('data-active') === "true";
            document.getElementById('edit_IsActive').checked = isActive;
        });
    }

    // ======= 6. DELETE/DEACTIVATE CONFIRMATION MODAL =======
    const deleteConfirmModal = document.getElementById('deleteConfirmModal');
    if (deleteConfirmModal) {
        deleteConfirmModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const id = button.getAttribute('data-id');
            const name = button.getAttribute('data-name');
            const isActive = button.getAttribute('data-active') === 'true';

            const modalTitle = this.querySelector('.modal-body h6');
            const modalMessage = document.getElementById('deleteModalMessage');
            const submitBtn = this.querySelector('button[type="submit"]');
            const hiddenIdInput = document.getElementById('delete_Id');

            hiddenIdInput.value = id;

            if (isActive) {
                modalTitle.textContent = "Disable Profile Access?";
                modalMessage.textContent = `Are you sure you want to suspend systemic terminal access rights configurations for ${name}?`;
                submitBtn.textContent = "Yes, Deactivate";
                submitBtn.className = "btn btn-sm btn-danger fw-bold px-3 py-2 rounded-3 flex-grow-1";
            } else {
                modalTitle.textContent = "Restore Profile Access?";
                modalMessage.textContent = `Are you sure you want to reactivate and restore systemic terminal access rights for ${name}?`;
                submitBtn.textContent = "Yes, Activate";
                submitBtn.className = "btn btn-sm btn-success fw-bold px-3 py-2 rounded-3 flex-grow-1";
            }
        });
    }
});

// ======= 7. CORE TABLE FILTER EXECUTION ENGINE =======
function filterTable() {
    const userSearch = document.getElementById('userSearch');
    const searchQuery = userSearch ? userSearch.value.toLowerCase() : '';
    const rows = document.querySelectorAll('#userTableBody tr');

    rows.forEach(row => {
        const nameCell = row.querySelector('td:nth-child(1) .fw-semibold');
        const idCell = row.querySelector('td:nth-child(2) .emp-id-badge');
        const roleCell = row.querySelector('td:nth-child(3) .fw-semibold');
        const badgeCell = row.querySelector('td:nth-child(4) .badge');

        if (!nameCell || !idCell || !roleCell || !badgeCell) return;

        const nameText = nameCell.textContent.toLowerCase();
        const idText = idCell.textContent.toLowerCase();
        const roleText = roleCell.textContent.toLowerCase().trim();
        const statusBadge = badgeCell.textContent.toLowerCase();

        const isRowActive = statusBadge.includes('active') && !statusBadge.includes('deactivated');

        const matchesSearch = nameText.includes(searchQuery) || idText.includes(searchQuery);
        const matchesRole = (currentRoleFilter === 'all') || (roleText === currentRoleFilter);

        let matchesStatus = true;
        if (currentStatusFilter === 'active') matchesStatus = isRowActive;
        if (currentStatusFilter === 'inactive') matchesStatus = !isRowActive;

        if (matchesSearch && matchesRole && matchesStatus) {
            row.style.setProperty('display', '', 'important');
        } else {
            row.style.setProperty('display', 'none', 'important');
        }
    });
}