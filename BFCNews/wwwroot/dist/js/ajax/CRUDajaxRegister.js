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
$(document).ready( ()=> {
    $("#modal-Add-Account").on("click", "#btnAdd", function()  {
        console.log("aaaaaa");
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
                    }
                    if (response.messager == "available") {
                        $("#msgalert").text("Username đã tồn tại");
                        $(".alert").css("background", "#FF0000");
                        $(".alert").css('display', 'block');
                        autoHide();
                    }
                }

            })
        }
        else {
            $("#msgalert").text("vui lòng điền đầy đủ thông tin");
            $(".alert").css("background", "#FF0000");
            $(".alert").css('display', 'block');
            autoHide();
        }
    });
}) 

$(document).ready(()=> {
   $('#tbl-Account').on('click', '.btn-danger', function() {
       var rowAccountLock = $(this).closest('tr');
       var userName = rowAccountLock.find('td:eq(1)').text();
       console.log(userName);
    });
})