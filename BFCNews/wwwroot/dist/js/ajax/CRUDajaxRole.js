var rowEditRole;
$(document).ready(function () {
    $("#modal-Add-Role").on("click", "#btnAdd", () => {
        alertHide();
        var name = $("#Role").val().toString();
        if (name != null) {
            var formdata = new FormData($("#form-Add-Role")[0]);
            $.ajax({
                url: '/Role/Add',
                type: "POST",
                data: formdata,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.messager == "success") {
                        $("#msgalert").text("Đã thêm thành công");
                        $(".alert").css("background", "#28a745");
                        $(".alert").css('display', 'block');
                        reload();
                    }
                    if (response.messager == "available") {
                        $("#msgalert").text("Tên Role ban đã tồn tại");
                        $(".alert").css("background", "#FF0000");
                        $(".alert").css('display', 'block');
                    }
                    if (response.messager == "failed") {
                        $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                        $(".alert").css("background", "#dc3545");
                        $(".alert").css('display', 'block');
                    }
                }
                , error: function (xhr, status, error) {
                    // Handle errors here
                    var relativePath = 'Error/PermissionDenied';
                    window.location.pathname = relativePath;
                }

            })
        }
    });
}) 

//Role Edit
$(document).ready(function () {
    $("#tbRole").on('click', '.btn-warning', function () {
        $("#modal-Edit-Role").modal('show');
        rowEditRole = $(this).closest('tr');
        $("#role-edit-input").val(rowEditRole.find("td:eq(1)").text());
    });
});

$(document).ready(function () {
    alertHide();
    $("#modal-Edit-Role").on('click', '#btn-Edit-Role', () => {
        var id = rowEditRole.find("td:eq(0)").text();
        var formData = new FormData($("#form-Edit-Role")[0]);
        formData.append("id", id);
        console.log(formData);
        $.ajax({
            url: 'Role/Edit',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: (response) => {
                if (response.messager == "success") {
                    rowEditRole.find("td:eq(1)").text(response.role.name);
                    $("#msgalert").text("Đã sửa thành công");
                    $(".alert").css("background", "#28a745");
                    $(".alert").css('display', 'block');
                    $("#modal-Edit-Role").modal('hide');
                    reload();

                }
                if (response.messager == "donothing") {
                    $("#msgalert").text("Bạn đã không thay đổi gì.");
                    $(".alert").css("background", "#ffc107");
                    $(".alert").css('display', 'block');
                }
                if (response.messager == "failed") {
                    $("#msgalert").text("Đường mạng không ổn định hãy quay lại sau.");
                    $(".alert").css("background", "#FF0000");
                    $(".alert").css('display', 'block');
                }
            }
        })
    })
})

//selectpicker
