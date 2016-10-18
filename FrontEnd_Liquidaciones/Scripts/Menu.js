$(document).ready(function () {
    $('.menu-trigger').click(function () {
        $(".side-menu-wrapper").toggleClass("mnu-open-all");
        $(".frame-content").toggleClass("with-menu");
        setTimeout(function () {
            $('.fixed-table-body').find('table').each(function () {
                $(this).bootstrapTable('resetView');
            });
        }, 300);
    });



    var classeAtiva = 'ativo';
    var $wrapper = $('.wrapper');

    /*$("#menu_principal").hover(function () {
        hideMenu();
    }, function () {
        $("#boton_menu").click();
        if ($("#menu_principal").hasClass("mnu-open-all")) {
            $wrapper.addClass(classeAtiva);
        } else {
            $wrapper.removeClass(classeAtiva);
        }
    });

    $(".menu-trigger").hover(function () {
        $("#boton_menu").click();
        if ($("#menu_principal").hasClass("mnu-open-all")) {
            $wrapper.addClass(classeAtiva);
        } else {
            $wrapper.removeClass(classeAtiva);
        }
    }, function () {
        hideMenu();
    });*/

    $("#boton_menu").click(function () {
        if ($("#menu_principal").hasClass("mnu-open-all")) {
            $wrapper.addClass(classeAtiva);
        } else {
            $wrapper.removeClass(classeAtiva);
        }
    });
});


