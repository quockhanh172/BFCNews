$(document).ready(function () {
    $('#uploadModal').on('click','#addPost',function () {
        var formData = new FormData($("#addPostForm")[0]);
        var content = $('#summernote').summernote('code');
        formData.append('content', content);

        // Lấy danh sách tệp hình ảnh từ nội dung HTML
        var imageUrls = [];
        $(content).find('img').each(function () {
            var imageUrl = $(this).attr('src');
            console.log(imageUrl+"-----------")
            imageUrls.push(imageUrl);
        });
        imageUrls.forEach(function (imageUrl) {
            var imageName = imageUrl.substring(imageUrl.lastIndexOf('/') + 1);
            fetch(imageUrl)
                .then(response => response.blob())
                .then(blob => {
                    // Thêm Blob vào FormData với tên imageName
                    formData.append('images[]', blob, imageName);
                })
                .catch(error => {
                    console.error('Lỗi khi tải hình ảnh:', error);
                });
        });
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