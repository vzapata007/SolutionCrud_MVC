// Define a base model for contacts
const Modelo_base = {
    idContacto: 0,
    nombre: "",
    telefono: "",
    fechaNacimiento: ""
};

// Function to show the modal with contact details
function mostrarModal(modelo) {
    // Populate modal fields with contact data
    $("#txtIdContacto").val(modelo.idContacto);
    $("#txtNombre").val(modelo.nombre);
    $("#txtTelefono").val(modelo.telefono);
    $("#txtFechaNacimiento").val(modelo.fechaNacimiento);

    // Show the modal
    $('.modal').modal('show');

    // Use setTimeout to ensure input focus and select happens after modal is fully shown
    setTimeout(() => $("#txtNombre").focus().select(), 100);
}

// Click handler for "Nuevo" button to show modal with empty base model
$("#btnNuevo").click(() => {
    mostrarModal(Modelo_base);
});

// Click handler for "Guardar" button to save or update a contact
$("#btnGuardar").click(() => {
    // Create a new contact model with updated values from modal inputs
    const NuevoModelo = {
        ...Modelo_base,
        idContacto: $("#txtIdContacto").val(),
        nombre: $("#txtNombre").val(),
        telefono: $("#txtTelefono").val(),
        fechaNacimiento: $("#txtFechaNacimiento").val()
    };

    // Determine the URL and HTTP method based on whether it's a new or existing contact
    const url = NuevoModelo.idContacto === "0" ? "Home/Insertar" : "Home/Actualizar";
    const method = NuevoModelo.idContacto === "0" ? "POST" : "PUT";

    // Send a fetch request to save or update the contact
    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(NuevoModelo)
    })
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        })
        .then(dataJson => {
            // Display success message and update contact list
            const mensaje = NuevoModelo.idContacto === "0" ? "registrado" : "editado";
            alert(mensaje);
            $('.modal').modal('hide'); // Hide the modal after saving/updating
            // You may optionally update the UI here if needed
        })
        .catch(error => console.error('Error:', error));
});

// Function to fetch and display the list of contacts
function listaContactos() {
    fetch("Home/Lista")
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        })
        .then(dataJson => {
            mostrarContactos(dataJson); // Display contacts in the table
        })
        .catch(error => console.error('Error:', error));
}

// Function to display contacts in the table
function mostrarContactos(dataJson) {
    const tbody = $("#tbContacto tbody");
    tbody.empty(); // Clear existing rows

    // Iterate over fetched contacts and append rows to the table
    dataJson.forEach(item => {
        const tr = $("<tr>");
        tr.append(
            $("<td>").text(item.nombre),
            $("<td>").text(item.telefono),
            $("<td>").text(item.fechaNacimiento),
            $("<td>").append(
                $("<button>").addClass("btn btn-primary btn-sm me-2 btn-editar").data("modelo", item).text("Editar"),
                $("<button>").addClass("btn btn-danger btn-sm btn-eliminar").data("id", item.idContacto).text("Eliminar")
            )
        );
        tbody.append(tr); // Append row to the table body
    });

    attachEventListeners(); // Attach event listeners to newly rendered buttons
}

// Function to attach event listeners to edit and delete buttons in the contact table
function attachEventListeners() {
    // Edit button click handler
    $("#tbContacto tbody").on("click", ".btn-editar", function () {
        const contacto = $(this).data("modelo");
        mostrarModal(contacto); // Show modal with selected contact details
    });

    // Delete button click handler
    $("#tbContacto tbody").on("click", ".btn-eliminar", function () {
        const idcontacto = $(this).data("id");
        const resultado = window.confirm("¿Desea eliminar el contacto?");

        if (resultado) {
            // Send a fetch request to delete the contact
            fetch(`Home/Eliminar?id=${idcontacto}`, {
                method: "DELETE"
            })
                .then(response => {
                    if (!response.ok) throw new Error('Network response was not ok');
                    return response.json();
                })
                .then(dataJson => {
                    listaContactos(); // Refresh the contact list after deletion
                })
                .catch(error => console.error('Error:', error));
        }
    });
}

// Initial call to load contacts when the document is ready
$(document).ready(() => {
    listaContactos();
});
