$(document).ready(function () {




    document.getElementById("menu-toggle").addEventListener("click", function () {
        document.getElementById("sidebar").classList.toggle("active");
    });



    $('.dropdown-toggle').on('click', function (e) {
        var $submenu = $(this).siblings('.collapse');
        var href = $(this).attr('href');
        if (href && href !== "#" && href !== "" && href !== "") {
            e.preventDefault();
           // window.location.replace(href);
            // Si la URL es relativa, conviértela en absoluta usando window.location.origin
            if (!href.startsWith('http') && !href.startsWith('https')) {
                href = window.location.origin + '/' + href; // Concatena la URL base
            }

            // Redirigir usando replace para evitar que la ruta anterior quede en el historial
            window.location.replace(href);
        }
        if ($submenu.length) {
            e.preventDefault(); // Solo previene el comportamiento por defecto si hay un submenú
            // Oculta otros submenús en el mismo nivel
            $(this).parent().siblings().find('.collapse').slideUp();
            // Muestra/oculta el submenú actual
            $submenu.slideToggle();
        }
       
    });


    // Ocultar el mene al hacer clic fuera de boton
    document.addEventListener("click", function (event) {
        var sidebar = document.getElementById("sidebar");
        var toggleButton = document.getElementById("menu-toggle");

        // Verifica si el clic se realiza fuera del mene y del boton
        if (!sidebar.contains(event.target) && !toggleButton.contains(event.target)) {
            sidebar.classList.remove("active"); // Oculta el menÃº
        }
    });








});//fin del ready