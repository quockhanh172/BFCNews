$(document).ready(function () {
    const activePage = window.location.pathname;

    const navLinks = $(".nav-link");

    navLinks.each((element,value) => {
        var a = $(value);
        if (a.attr('href').indexOf(activePage) !== -1) {
            a.addClass('active');
        }
    });
});


$(document).ready(function () {
        $('#modalAdd').on("shown.bs.modal", () => {
            $("#Department").focus();
        });
});

