$(document).ready(function () {
    $("#modal-Add-Position").on("click", "#btnAdd", () => {
        var name = $("#Position").val().toString();
        if (name != null) {
            $.ajax({
                url: '/Position/Add',
                type: "POST",
                data: { "name": name },
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
