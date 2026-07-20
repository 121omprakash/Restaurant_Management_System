document.addEventListener("DOMContentLoaded", function () {

    const addIngredientBtn = document.getElementById("addIngredientBtn");
    const ingredientBody = document.getElementById("ingredientBody");

    if (!addIngredientBtn || !ingredientBody) {
        return;
    }

    // Add Ingredient Row
    addIngredientBtn.addEventListener("click", function () {

        const firstRow = ingredientBody.querySelector("tr");

        if (!firstRow) {
            return;
        }

        // Clone the first row
        const newRow = firstRow.cloneNode(true);

        // Clear all input fields
        newRow.querySelectorAll("input").forEach(input => {
            input.value = "";
        });

        // Reset all dropdowns
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
            }
            else {
                alert("At least one ingredient is required.");
            }
        }

    });

});

// Add Recipe Step
const addStepBtn = document.getElementById("addStepBtn");
const stepsContainer = document.getElementById("stepsContainer");

if (addStepBtn && stepsContainer) {

    addStepBtn.addEventListener("click", function () {

        const stepCount = stepsContainer.querySelectorAll(".step").length + 1;

        const firstStep = stepsContainer.querySelector(".step");

        const newStep = firstStep.cloneNode(true);

        // Update label
        newStep.querySelector("label").textContent = "Step " + stepCount;

        // Clear textarea
        newStep.querySelector("textarea").value = "";

        stepsContainer.appendChild(newStep);

    });

    // Remove Step
    stepsContainer.addEventListener("click", function (e) {

        if (e.target.classList.contains("removeStep")) {

            const steps = stepsContainer.querySelectorAll(".step");

            if (steps.length > 1) {

                e.target.closest(".step").remove();

                // Renumber steps
                const remainingSteps = stepsContainer.querySelectorAll(".step");

                remainingSteps.forEach((step, index) => {
                    step.querySelector("label").textContent = "Step " + (index + 1);
                });

            }
            else {
                alert("At least one step is required.");
            }
        }

    });

}