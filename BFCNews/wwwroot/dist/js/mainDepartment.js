$(document).ready(function () {
    const activePage = window.location.pathname;
    const navLinks = $(".nav-link");
    navLinks.each((element,value) => {
        var a = $(value);
        if (activePage == "/") {
            a.removeClass("active")
        } else {
            if (a.attr('href').indexOf(activePage) !== -1) {
                a.addClass('active');
                var i = 0;
                for (i = activePage.toString().length; i > 0; i--) {
                    if (activePage.charAt(i) === "/") {
                        break;
                    }
                }
                $("#title-web").text(activePage.slice(i + 1)).css("font-weight","bold");
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

