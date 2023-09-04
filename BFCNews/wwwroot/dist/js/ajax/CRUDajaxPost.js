$(document).ready(function () {
    $('#uploadModal').on('click','#addPost',function () {
        var formData = new FormData($("#addPostForm")[0]);
        formData.append('content', $('#summernote').val());
        $.ajax({
            url: '/Management/Add',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.messager == "success") {
                    $("#msgalert").text("Đã thêm thành công");
                    $(".alert").css("background", "#28a745");
                    $(".alert").css('display', 'block');
                    $('#modal-Add-Account').modal('hide')
                    reload1s();
                }
                if (response.messager == "available") {
                    $("#msgalert").text("Username đã tồn tại");
                    $(".alert").css("background", "#FF0000");
                    $(".alert").css('display', 'block');
                    autoHide();
                }
                if (response.messager == "failed") {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
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
    });
});