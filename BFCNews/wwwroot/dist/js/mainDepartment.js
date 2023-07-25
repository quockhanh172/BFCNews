$(document).ready(function () {
    const activePage = window.location.pathname;
    console.log(activePage);
    const navLinks = $(".nav-link");
    navLinks.each((element,value) => {
        var a = $(value);
        if (activePage == "/") {
            a.removeClass("active")
        } else {
            if (a.attr('href').indexOf(activePage) !== -1) {
                a.addClass('active');
            }
            else {
                a.removeClass('active');
            }
        }     
    });
});


$(document).ready(function () {
        $('#modalAdd').on("shown.bs.modal", () => {
            $("#Department").focus();
        });
});

