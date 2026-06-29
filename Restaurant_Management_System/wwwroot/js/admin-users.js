// wwwroot/js/admin-users.js

document.addEventListener("DOMContentLoaded", function () {

    // 1. Live Table Search Bar Filter Logic
    const userSearch = document.getElementById("userSearch");
    if (userSearch) {
        userSearch.addEventListener("keyup", function () {
            let filterValue = this.value.toLowerCase();
            let tableRows = document.querySelectorAll("#userTableBody tr");
            tableRows.forEach(row => {
                let rowText = row.textContent.toLowerCase();
                row.style.display = rowText.includes(filterValue) ? "" : "none";
            });
        });
    }

    // 2. Interactive Edit Modal Parameters Assignment Listener
    const editUserModal = document.getElementById('editUserModal');
    if (editUserModal) {
        editUserModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;

            document.getElementById('edit_Id').value = button.getAttribute('data-id');
            document.getElementById('edit_UserId').value = button.getAttribute('data-userid');
            document.getElementById('edit_Name').value = button.getAttribute('data-name');
            document.getElementById('edit_Role').value = button.getAttribute('data-role');
        });
    }

    // 3. Delete Target Routing Confirmation Pop-up Event Mapping
    const deleteConfirmModal = document.getElementById('deleteConfirmModal');
    if (deleteConfirmModal) {
        deleteConfirmModal.addEventListener('show.bs.modal', function (event) {
            const button = event.relatedTarget;
            const targetId = button.getAttribute('data-id');
            const targetName = button.getAttribute('data-name');
            const deleteUrl = button.getAttribute('data-url'); // Dynamically reads the URL from the button

            document.getElementById('deleteModalMessage').textContent = `Are you sure you want to permanently remove access settings for ${targetName}?`;
            document.getElementById('deleteModalForm').action = deleteUrl;
        });
    }
});


//Filter role 
const roleOptions = document.querySelectorAll(".filter-role-opt");
roleOptions.forEach(option => {
    option.addEventListener("click", function (e) {
        e.preventDefault();
        let targetRole = this.getAttribute("data-role").toLowerCase();
        let tableRows = document.querySelectorAll("#userTableBody tr");

        // Update the Dropdown button text to show the active filter selection status
        document.getElementById("roleFilterDropdown").innerHTML = `<i class="fa-solid fa-filter me-2 text-muted"></i> Role: ${this.textContent}`;

        tableRows.forEach(row => {
            // Looks at the Role Designation column cell text
            let roleCellText = row.querySelector("td:nth-child(3)").textContent.toLowerCase().trim();

            if (targetRole === "all" || roleCellText === targetRole) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });
    });
});