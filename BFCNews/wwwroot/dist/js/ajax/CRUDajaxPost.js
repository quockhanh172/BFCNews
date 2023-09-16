//lấy Id youtube
function getId(url) {
    var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
    var match = url.match(regExp);

    if (match && match[2].length == 11) {
        return match[2];
    } else {
        return 'error';
    }
}
// thay oembed thành iframe
function replaceOEmbedWithIframe(width) {
    $('figure.media oembed').each(function () {
        var oembedUrl = $(this).attr('url');
        console.log(oembedUrl);
        var urlvideo ='//www.youtube.com/embed/'+getId(oembedUrl);
        var iframeElement = $('<iframe></iframe>');

        // Cấu hình thuộc tính của thẻ <iframe>
        iframeElement.attr('src', urlvideo);
        iframeElement.attr('width', width); // Thay đổi kích thước theo nhu cầu
        iframeElement.attr('height', '580');
        iframeElement.attr('frameborder', '0');
        iframeElement.attr('allowfullscreen', 'true');

        // Thay thế thẻ <oembed> bằng thẻ <iframe>
        $(this).replaceWith(iframeElement);
    });
}

//function pagination


function dateTimeToDate(date) {
    var dateObj = new Date(date);
    var day = dateObj.getDate();
    var month = dateObj.getMonth() + 1; // Lấy tháng (0-11, nên cộng thêm 1)
    var year = dateObj.getFullYear();

    // Tạo một chuỗi để hiển thị ngày tháng năm
    var formattedDate = day + '/' + month + '/' + year;
    return formattedDate;
}

$(document).ready(function () {
    //Uploads Posts
   /* $('#uploadModal').modal({
        focus: false
    });*/
    const selectedFileNames = $('#selected-file-names');
    $('#uploadModal').on('change', "#OfficeFile", function () {
        if (this.files.length > 0) {
            const fileNames = [];
            for (let i = 0; i < this.files.length; i++) {
                const fileName = this.files[i].name;
                fileNames.push(`<i class="fas fa-file"></i> ${fileName}`); // Thêm biểu tượng trước tên tệp
            }
            selectedFileNames.html(fileNames.join('<br>')); // Sử dụng <br> để xuống dòng giữa các tệp
        } else {
            selectedFileNames.text('No files selected');
        }
    });

    $('#uploadModal').on('click', '#addPost', function () {
        $(".alert").css('display', 'none');
        var formData = new FormData($("#addPostForm")[0]);
        var ckeditorData = editor.getData();
        formData.append("Content", ckeditorData);
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
                    $('#uploadModal').modal('hide')
                    reload1s();
                }
                if (response.messager == "accessDenied") {
                    console.log(response.messager);
                    window.location.href = '/Error/AccessDenied';
                }
                if (response.messager == "available") {
                    $("#msgalert").text("Tiêu đề đã tồn tại");
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
    });

    $("#deleteConfirmedButton").click(function () {
        $(".alert").css('display', 'none');
        if (postIdToDelete, rowDelete) {
            $.ajax({
                url: '/Management/Delete',
                type: "POST",
                data: { "id": postIdToDelete },
                success: (response) => {
                    if (response.messager === "success") {
                        $("#deleteModal").modal('hide');
                        $("#msgalert").text("Đã xóa thành công");
                        $(".alert").css("background", "#28a745");
                        $(".alert").css('display', 'block');
                        rowDelete.remove();
                    }
                    else {
                        $("#deleteModal").modal('hide');
                        $("#msgalert").text("Đã xóa không thành công");
                        $(".alert").css("background", "##dc3545");
                        $(".alert").css('display', 'block');
                    }
                }, error: (e) => {
                    if (e.status = 403) {
                        window.location.href = "/Error/PermissionDenied";
                    } else {
                        $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                        $(".alert").css("background", "#dc3545");
                        $(".alert").css('display', 'block');
                    }    
                }
            })
            // Thực hiện ajax request ở đây

            // Đóng modal
            $("#deleteModal").modal("hide");
        }
    });

    // Xử lý sự kiện khi nút "delete" trên hàng bị nhấp
    $("#tbPost").on("click", ".btn-danger", function () {
        rowDelete = $(this).closest("tr");
        postIdToDelete = $(this).closest("tr").data("post-id");
        // Mở modal xác nhận xóa
        $("#deleteModal").modal("show");
    });

    //click vào link
    $("#tbPost").on("click", "a[target='_blank']", function (e) {
        e.preventDefault();    
        var modalFooter = document.getElementById('filesDetailFooter');
        modalFooter.innerHTML = '';
        var maxWidth = $("#PostDetailModal").width();
        var postId = $(this).closest("tr").data("post-id");
        $.ajax({
            url: '/Management/PostDetail',
            type: "POST",
            data: { "id": postId },
            success: (response) => {
                if (response.messager = "success") {
                    var date = dateTimeToDate(response.date);
                    $("#datePlaceholder").text(date);
                    $("#modalDetailContent").html(response.content); 
                    $("#myModalLabel").text(response.title);
                    $("#PostDetailModal").modal("show");
                    if (response.files != null && response.files.length > 0) {
                        var modalFooter = document.getElementById('filesDetailFooter');
                        // Lặp qua danh sách tệp và tạo phần tử cho mỗi tệp
                        response.files.forEach(function (file) {
                            // Tạo một phần tử <i> và đặt class và aria-hidden
                            var icon = document.createElement('i');
                            icon.classList.add('fa', 'fa-file');
                            icon.setAttribute('aria-hidden', 'true');

                            // Tạo một phần tử <a> và đặt thuộc tính href và nội dung
                            var fileLink = document.createElement('a');
                            fileLink.href = "../../OfficeUploads/"+file; // Sử dụng đường dẫn từ danh sách tệp
                            fileLink.textContent = file+ "    "; // Sử dụng tên tệp từ danh sách tệp

                           // Thêm phần tử cha vào phần footer của modal
                            modalFooter.appendChild(icon);
                            modalFooter.appendChild(fileLink);
                        });
                    }
                    replaceOEmbedWithIframe(maxWidth);
                } else {

                }
            }, error: function () {
                $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                $(".alert").css("background", "#dc3545");
                $(".alert").css('display', 'block');
            }
        });
    });

    //Edit Post
    const selectedFileNamesEdit = $('#selected-file-names-edit');
    $('#EditPostModal').on('change', "#OfficeFileEdit", function () {
        if (this.files.length > 0) {
            const fileNames = [];
            for (let i = 0; i < this.files.length; i++) {
                const fileName = this.files[i].name;
                fileNames.push(`<i class="fas fa-file"></i> ${fileName}`); // Thêm biểu tượng trước tên tệp
            }
            selectedFileNamesEdit.html(fileNames.join('<br>')); // Sử dụng <br> để xuống dòng giữa các tệp
        } else {
            selectedFileNamesEdit.text('No files selected');
        }
    });
    $("#tbPost").on("click", ".btn-warning", function () {
        var EditId = $(this).closest("tr").data("post-id");
        console.log(EditId);
        $.ajax({
            url: '/Management/GetPostEdit',
            type: "POST",
            data: { "id": EditId },
            success: (response) => {
                if (response.messager == "success") {
                    $("#EditTitle").val(response.title);
                    $("#EditSummary").val(response.summary);
                    $("#id-edit-post").val(EditId);
                    EditPostEditor.setData(response.content);
                    $("#EditPostModal").modal('show');
                }
                else {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            }, error: (e) => {
                if (e.status = 403) {
                    window.location.href = "/Error/PermissionDenied";
                } else {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            }
        })
    });


    $("#EditPostModal").on("click","#editPost",function () {
        $(".alert").css('display', 'none');
        var EditPostEditorData = EditPostEditor.getData();
        var formData = new FormData($("#editPostForm")[0]);
        formData.append("Content", EditPostEditorData);
        $.ajax({
            url: '/Management/Edit',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.messager == "success") {
                    $("#msgalert").text("Đã thêm thành công");
                    $(".alert").css("background", "#28a745");
                    $(".alert").css('display', 'block');
                    $('#uploadModal').modal('hide')
                    reload1s();
                }
                if (response.messager == "accessDenied") {
                    console.log(response.messager);
                    window.location.href = '/Error/AccessDenied';
                }
                if (response.messager == "available") {
                    $("#msgalert").text("Tiêu đề đã tồn tại");
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

});