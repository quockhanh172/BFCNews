const queryString = window.location.pathname;
var sidebar = $("#sidebarAdmin li");
function getPath() {
    var n = "";
    for (var i = queryString.length - 1; i >= 0; i--) {
        if (queryString[i] === "/") {
            var a = queryString.slice(i + 1, queryString.length);
            n += a;
            break;
        }
    }
    return n;
}

window.onload = (event) => {
    for (var i = 0; i < sidebar.length; i++) {
        if ($(sidebar[i]).children().data("id")===(getPath())) {
           $(sidebar[i]).children("a").addClass("active");
        }
    }
    for (var i = queryString.length-1; i >=0 ; i--) {
        if (queryString[i] === "/") {
           var a= queryString.slice(i+1, queryString.length);
            $("#destinationPage").html(a);
            $("#dPage").html(a);
            break;
        }
    }
};

    $(document).ready(function () {
        $("#addDepartment").click(() => {
            $('#modalAdd').modal("show");
            $('#modalAdd').on("shown.bs.modal",() => {
                $("#Department").focus();
            });
        })
    });
