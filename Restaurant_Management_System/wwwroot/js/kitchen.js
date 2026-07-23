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