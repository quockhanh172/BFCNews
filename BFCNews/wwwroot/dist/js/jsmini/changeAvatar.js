$(document).ready(function () {
    $('#Change-Avarta').click(function () {
        $('#avatarModal').modal('show'); // Hiển thị modal khi nhấp vào nút
    });
    $('#newAvatar').on('change', function (event) {
        var selectedFile = event.target.files[0];
        if (selectedFile) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#newAvatarPreview').attr('src', e.target.result); // Hiển thị ảnh đã chọn
                $('#newAvatarPreview').show(); // Hiển thị phần tử <img>
            };
            reader.readAsDataURL(selectedFile);
        } else {
            $('#newAvatarPreview').hide(); // Ẩn phần tử <img> nếu không có tệp nào được chọn
        }
    });
    $("#avatarModal").on("click", "#Change-Avatar", function (event) {
        var formData = new FormData($("#FormChangeAvatar")[0]);
        /*formData.append('userId', UserId);*/
        $.ajax({
            url: '/User/ChangeAvatar',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function(response) {
                if (response.message == "success") {
                    $("#msgalert").text("Đã thay đổi avatar thành công");
                    $(".alert").css("background", "#28a745");
                    $(".alert").css('display', 'block');
                    $('#modal-Add-Account').modal('hide')
                    reload1sAvartar();
                }
                if (response.message == "failed") {
                    $("#msgalert").text("Bạn đã không thay dổi gì");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            },
            error: function(response) {
                $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                $(".alert").css("background", "#dc3545");
                $(".alert").css('display', 'block');
            }
        })
    })
});
reload1sAvartar = () => {
    setTimeout(function () {
        location.reload();
    }, 500);
}
$(document).ready(function () {
    $('#img-Edit').on('click', function (event) {
        if ($(event.target).is('#img-Edit')) {
            $('#imgToast').toast('show'); // Hiển thị toast khi nhấp vào ảnh
        }
    });

    // Ẩn toast khi nhấp bên ngoài toast
    $(document).on('click', function (event) {
        if (!$(event.target).closest('.toast').length && !$(event.target).is('#img-Edit')) {
            $('#imgToast').toast('hide');
        }
    });
});