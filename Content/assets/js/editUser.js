
const editBtn = document.getElementById("editBtn");

const formContainer = document.getElementById("formContainer");
const closeForm = document.querySelector(".close");


editBtn.addEventListener("click", function (userId) {

    formContainer.style.display = "block";

});

closeForm.addEventListener("click", () => {
  formContainer.style.display = "none";
});


