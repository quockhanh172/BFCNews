$(document).ready(() =>{
    $("#btnChangePassword").click(()=>{
        $("#changePasswordModal").modal('show');
    })
    $("#changePasswordModal").on("click", "#ChangePassword", () => {
        alertHide();
        var formData = new FormData($("#formChangePassword")[0]);
        $.ajax({
            url: '/User/ChangePassword',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    $("#msgalert").text("Đã đổi password thành công");
                    $(".alert").css("background", "#28a745");
                    $(".alert").css('display', 'block');
                    $('#changePasswordModal').modal('hide')
                } else {
                    var errorHtml = "<ul>";
                    for (var i = 0; i < response.errors.length; i++) {
                        errorHtml += "<li>" + response.errors[i] + "</li>";
                    }
                    errorHtml += "</ul>";
                    $("#msgalert").html(errorHtml);
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            },
            error: function () {
                $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                $(".alert").css("background", "#dc3545");
                $(".alert").css('display', 'block');
            }
        })
    })
})