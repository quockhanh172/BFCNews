$(document).ready(function () {
    $('#avatar').on('change', function (e) {
        var file = e.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#avatarDisplay').addClass("img-thumbnail");
            $('#avatarDisplay').attr('src', e.target.result);
        }

        reader.readAsDataURL(file);
    });
});

//Add account
$(document).ready(function () {
    $("#modal-Add-Account").on("click", "#btnAdd", () => {
        var name = $("#userName").val().toString();
        var email = $("#email").val().toString();
        var role = $("#Role").val().toString();
        var avatar = $("#Role").val().toString();
        if (name != null && email != null && role != null && avatar != null) {
            var formdata = new FormData($("#form-Add-Account")[0]);
            $.ajax({
                url: '/User/Add',
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
                    if (response == "available") {
                        $("#msgalert").text("Tên phòng ban đã tồn tại");
                        $(".alert").css("background", "#FF0000");
                        $(".alert").css('display', 'block');
                    }
                    $('#modal-Add-Department').modal('hide')
                }

            })
        }
    });
}) 