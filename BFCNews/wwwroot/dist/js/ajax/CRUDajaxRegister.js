$(document).ready(()=> {
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
$(document).ready(() => {
    $("#ClaimAdmin").show();
    $('#modal-Add-Account').on('change', "#Role", function () {
        var selectedValue = $(this).val();
        if (selectedValue === "User") {
            $("#Claim").show();
            $("#inputPosition").show();
            $("#inputDepartment").show();
        }
        if (selectedValue === "SuperAdmin"|| selectedValue === "Admin") {
            $('#Claim').hide();
            $("#inputPosition").hide();
            $("#inputDepartment").hide();
        }
    });
    $("#modal-Add-Account").on("click", "#btnAdd", function () {
        alertHide();
        var name = $("#userName").val().toString();
        var email = $("#email").val().toString();
        var role = $("#Role").val().toString();
        var avatar = $("#avatar").val().toString();
        if (name.length != 0 && email.length != 0 && role.length != 0 && avatar.length != 0) {
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
                        $('#modal-Add-Account').modal('hide')
                        reload1s();
                    }
                    if (response.messager == "available") {
                        $("#msgalert").text("Username đã tồn tại");
                        $(".alert").css("background", "#FF0000");
                        $(".alert").css('display', 'block');
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
        }
        else {
            $("#msgalert").text("vui lòng điền đầy đủ thông tin");
            $(".alert").css("background", "#FF0000");
            $(".alert").css('display', 'block');
        }
    });
}) 

$(document).ready(() => {
    $('#tbAccount').on('click', '.btn-primary', function () {
        alertHide();
        var rowAccountLock = $(this).closest('tr');
        var userName = rowAccountLock.find('td:eq(1)').text();
        if (userName != "") {
            $.ajax({
                url: '/User/LockDownAccount',
                type: 'Post',
                data: { username: userName },
                success: function (response) {
                    if (response.messager == "Success") {
                        reload1s();
                    }
                    if (response.messager == "Error") {
                        $("#msgalert").text("Lỗi");
                        $(".alert").css("background", "#FF0000");
                        $(".alert").css('display', 'block');
                    }
                }
            })
        }
    });

    $('#tbAccount').on('click', '.btn-danger', function () {
        alertHide();
       var rowAccountLock = $(this).closest('tr');
       var userName = rowAccountLock.find('td:eq(1)').text();
       if (userName != "") {
           $.ajax({
               url: '/User/LockDownAccount',
               type: 'Post',
               data: { username: userName },
               success: function (response) {
                   if (response.messager == "Success") {
                       reload1s();
                   }
                   if (response.messager == "Error") {
                       $("#msgalert").text("Lỗi");
                       $(".alert").css("background", "#FF0000");
                       $(".alert").css('display', 'block');
                   }
                   if (response.messager == "failed") {
                       $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                       $(".alert").css("background", "#dc3545");
                       $(".alert").css('display', 'block');
                   }
               },error: function (xhr, status, error) {
                   // Handle errors here
                   var relativePath = '/Error/PermissionDenied';
                   window.location.href = relativePath;
               }
           })
       }
    });
})

$(document).ready( function () {
    $("#adminEditUser").click(function () {
        $("#confirmModalEdit").modal("show");
    })
    $("#confirmModalEdit").on("click", "#confirmBtnEdit", function () {
        alertHide();
        var id = $("#dataEdit").data("itemid");
        var formData = new FormData($("#formEditUser")[0]); 
        formData.append("id", id);
        $.ajax({
            url: '/User/AdminEditUser',
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
                    $("#msgalert").text("Username hoặc email đã tồn tại");
                    $(".alert").css("background", "#FF0000");
                    $(".alert").css('display', 'block');
                }
                if (response.messager == "failed") {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            },
            error: function (e) {
                if (e.status = 403) {
                    window.location.href = "/Error/PermissionDenied";
                } else {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            }

        })
    })
})