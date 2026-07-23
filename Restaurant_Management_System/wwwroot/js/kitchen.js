document.addEventListener("DOMContentLoaded", function () {

    // ==========================
    // Ingredient Section
    // ==========================
    const addIngredientBtn = document.getElementById("addIngredientBtn");
    const ingredientBody = document.getElementById("ingredientBody");

    if (addIngredientBtn && ingredientBody) {

        // Add Ingredient Row
        addIngredientBtn.addEventListener("click", function () {

            const firstRow = ingredientBody.querySelector("tr");

            if (!firstRow) return;

            const newRow = firstRow.cloneNode(true);

            // Clear input values
            newRow.querySelectorAll("input").forEach(input => {
                input.value = "";
            });

            // Reset dropdowns
            newRow.querySelectorAll("select").forEach(select => {
                select.selectedIndex = 0;
            });

            ingredientBody.appendChild(newRow);

        });

        // Remove Ingredient Row
        ingredientBody.addEventListener("click", function (e) {

            if (e.target.classList.contains("removeIngredient")) {

                const rows = ingredientBody.querySelectorAll("tr");

                if (rows.length > 1) {
                    e.target.closest("tr").remove();
                } else {
                    alert("At least one ingredient is required.");
                }
            }

        });

    }

    // ==========================
    // Recipe Steps Section
    // ==========================
    const addStepBtn = document.getElementById("addStepBtn");
    const removeStepBtn = document.getElementById("removeStepBtn");
    const stepsContainer = document.getElementById("stepsContainer");
    const recipeTextArea = document.querySelector("textarea[name='RecipeSteps']");

    if (addStepBtn && stepsContainer) {

        addStepBtn.addEventListener("click", function () {
            stepsContainer.style.display = "block";
            addStepBtn.style.display = "none";
        });

    }

    if (removeStepBtn && stepsContainer && recipeTextArea) {

        removeStepBtn.addEventListener("click", function () {
            recipeTextArea.value = "";
            stepsContainer.style.display = "none";
            addStepBtn.style.display = "inline-block";
        });

    }

});

function searchRecipes() {
    const search = document.querySelector(".search-box").value.toLowerCase();
    const rows = document.querySelectorAll(".recipe-table tbody tr");

    rows.forEach(row => {
        const rowText = row.innerText.toLowerCase();

        if (rowText.includes(search)) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}


function filterRecipes() {

    const selectedCategory = document.getElementById("categoryFilter").value;
    const rows = document.querySelectorAll(".recipe-table tbody tr");

    rows.forEach(function (row) {

        const category = row.cells[2].innerText.trim();

        if (selectedCategory === "All" || category === selectedCategory) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }

    });

}

function showOrders(type) {

    let orders = [];

    if (type === "pending")
        orders = pendingOrders;
    else if (type === "preparing")
        orders = preparingOrders;
    else if (type === "completed")
        orders = completedOrders;
    else
        orders = delayedOrders;

    const body = document.getElementById("orderTableBody");
    body.innerHTML = "";

    orders.forEach(order => {

        let statusClass = "";

        if (order.status === "Pending")
            statusClass = "pending";
        else if (order.status === "Preparing")
            statusClass = "preparing";
        else if (order.status === "Completed")
            statusClass = "completed";
        else
            statusClass = "delayed";

        body.innerHTML += `
            <tr>
                <td>${order.id}</td>
                <td>${order.item}</td>
                <td><span class="status ${statusClass}">${order.status}</span></td>
                <td>${order.date}</td>
                    <td>
                       
                    <div class="action-buttons">

                        <button class="action-btn view-btn">
                            <i class="fa-solid fa-eye"></i>
                            View
                        </button>

                        ${order.status === "Pending"
                ? `<button class="action-btn start-btn">
                                    <i class="fa-solid fa-play"></i>
                                    Start Preparing
                               </button>`

                : order.status === "Preparing"

                    ? `<button class="action-btn complete-btn">
                                    <i class="fa-solid fa-check"></i>
                                    Complete
                               </button>`

                    : ""
            }

                    </div>
                </td>
            </tr>`;
    });

}

window.onload = function () {
    showOrders("pending");


}
function searchOrders() {
    const search = document.querySelector(".search-box").value.toLowerCase();
    const rows = document.querySelectorAll(".order-table tbody tr");

    rows.forEach(row => {
        const text = row.innerText.toLowerCase();

        if (text.includes(search)) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}
