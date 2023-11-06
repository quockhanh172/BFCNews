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
        alertHide();
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
        alertHide();
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
    //Edit post
    $("#tbPost").on("click", ".btn-warning", function () {
        alertHide();
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
                if (e.status == 403) {
                    window.location.href = "/Error/PermissionDenied";
                } else {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            }
        })
    });

    //edit post
    $("#EditPostModal").on("click","#editPost",function () {
        alertHide();
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
                }
                if (response.messager == "failed") {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            },
            error: function (e) {
                if (e.status == 403) {
                    window.location.href = "/Error/PermissionDenied";
                } else {
                    $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                    $(".alert").css("background", "#dc3545");
                    $(".alert").css('display', 'block');
                }
            }

        })
    })

    //pagination
    var numPages = $("#tbPost").data("post-total");

    $("#pagination-post").append('<li class="paginate_button page-item previous" id="tbAccount_previous"><a data-dt-idx="0" tabindex = "0" class= "page-link"> Previous</a></li >');
    for (var i = 1; i <= numPages; i++) {
        if (i == 1) {
            $('#pagination-post').append('<li class="paginate_button page-item item active"><a href="#" aria-controls="tbAccount" data-dt-idx="'+i+'" tabindex="0" class="page-link">'+i+'</a></li>');
        }
        else {
            $('#pagination-post').append('<li class="paginate_button page-item item"><a href="#" aria-controls="tbAccount" data-dt-idx="' + i + '" tabindex="0" class="page-link">' + i + '</a></li>');
        }
    }
    $("#pagination-post").append('<li class="paginate_button page-item next" id="tbAccount_next" ><a href="#" aria-controls="tbAccount" data - dt - idx="11" tabindex ="0" class="page-link"> Next</a ></li >');
    // Tổng số trang
    var currentpage = $(".active").find("a").first().text();
    var currentPage = parseInt(currentpage);// Trang hiện tại
    var previousButton = $("#tbAccount_previous");
    var nextButton = $("#tbAccount_next");
    // Hàm cập nhật trang
    if (currentpage === "1") {
        previousButton.prop("disabled", true);
        previousButton.addClass("disabled")
    }
    function updatePagination() {
        $('#pagination-post li.page-item.item').hide();
        $('#pagination-post li.page-item.ellipsis').remove();

        // Hiển thị 2 trang trước và 2 trang sau của trang hiện tại
        for (var i = currentPage - 2; i <= currentPage + 2; i++) {
            if (i >= 1 && i <= numPages) {
                $('#pagination-post li.page-item.item').eq(i - 1).show();
            }
        }

        // Hiển thị trang 1 và trang 10 ở giữa là 3 chấm
        if (currentPage >= 5) {
            $('#pagination-post li.page-item.item').eq(0).show();
            $('#pagination-post li.page-item.item').eq(numPages - 1).show();
        }

        // Thêm dấu ba chấm khi cần thiết
        if (currentPage > 3) {
            $('#pagination-post li.page-item.item').eq(1).before('<li class="paginate_button page-item ellipsis"><span class="page-link">...</span></li>');
        }
        if (currentPage < numPages - 2) {
            $('#pagination-post li.page-item.item').eq(currentPage + 2).after('<li class="paginate_button page-item ellipsis"><span class="page-link">...</span></li>');
        }
    }

    //Sự kiện khi ấn page
    $("#pagination-post").on("click", ".page-item.item", function (e) {
        e.preventDefault();
        var activepage = $(".active");
        activepage.removeClass("active");
        $(this).addClass("active");
        currentPage = parseInt($(this).text());
        updatePagination();
        disablePrevious($(this).find("a").first().text(), previousButton);
        disableNext($(this).find("a").first().text(), nextButton, numPages);
        var category = $("#title-web").text();
        var page = $(this).find("a").first().text();
        $.ajax({
            url: '/Management/Pagination',
            type: "POST",
            data: {'page':page,'category':category},
            success: function (response) {
                if (response.message == "success") {
                    var posts = response.posts;
                    $("#tbPost tbody").empty();
                    // Lặp qua danh sách posts và thêm chúng vào bảng
                    for (var i = 0; i < posts.length; i++) {
                        var post = posts[i];
                        var category = "";
                        if (post.department != null) {
                            category += '<td style="width: 15%;">' + post.department.name + '</td>';
                        }
                        if (post.category != null) {
                            category += '<td style="width: 15%;">' + post.category.name + '</td>';
                        }
                        var date = new Date(post.publishedDate);
                        var formattedDate = moment(date).format('DD/MM/YYYY');
                        var newRow = '<tr data-post-id="' + post.id + '">' +
                            '<td style="text-align:left;width: 20%;"><a href="#" target="_blank">' + post.title + '</a></td>' +
                            '<td style="width: 20%;">' + post.user.fullName + '</td>' + category +
                            '<td style="width: 20%;">' + formattedDate + '</td>' +
                            '<td style="text-align:left;width: 30%;">';
                        

                        if (post.files != null) {
                            for (var j = 0; j < post.files.length; j++) {
                                var item = post.files[j];
                                newRow += '<a href="../../OfficeUploads/' + item.image + '">' +
                                    '<i class="fas fa-file"></i>' + item.image + '</a><br />';
                            }
                        }

                        newRow += '</td>' +
                            '<td style="width: 20%">' +
                            '<button class="btn btn-warning"><i class="fa fa-edit" aria-hidden="true"></i></button>' +
                            '<button class="btn btn-danger"><i class="fa fa-trash" aria-hidden="true"></i></button>' +
                            '</td>' +
                            '</tr>';

                        // Thêm hàng mới vào bảng
                        $("#tbPost tbody").append(newRow);
                    }
                }
            }, error: function () {
                $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                $(".alert").css("background", "#dc3545");
                $(".alert").css('display', 'block');
            }
        })
    });

    // Sự kiện khi nhấn nút "Next"
    $('#pagination-post').on('click','#tbAccount_next', function (e) {
        e.preventDefault();
        var activepage = $(".active");
        var currentPageNumber = activepage.find('a').first().text();
        if (currentPageNumber < numPages) {
            // Find the next page number
            var nextPageNumber = parseInt(currentPageNumber) + 1;
            var targetPageItem = $(".page-item.item").filter(function () {
                return $(this).find('a').text() === nextPageNumber.toString();
            });
            targetPageItem.click();
        }
        if (currentPage < numPages) {
            currentPage++;
            updatePagination();
        }
    });

    // Sự kiện khi nhấn nút "Previous"
    $('#pagination-post').on('click','#tbAccount_previous', function (e) {
        e.preventDefault();
        var activepage = $(".active");
        var currentPageNumber = activepage.find('a').first().text();

        if (currentPageNumber > 1) {
            // Find the previous page number
            var previousPageNumber = parseInt(currentPageNumber) - 1;
            // Trigger a click on the corresponding page item
            var targetPageItem = $(".page-item.item").filter(function () {
                return $(this).find('a').text() === previousPageNumber.toString();
            });
            targetPageItem.click();
        }
        if (currentPage > 1) {
            currentPage--;
            updatePagination();
        }
    });

    // Mặc định hiển thị trang đầu tiên
    updatePagination();


    //getTotal page
});


function disablePrevious(a, previousbutton) {
    if (a === "1") {
        previousbutton.prop("disabled", true);
        previousbutton.addClass("disabled");
    }
    else {
        previousbutton.prop("disabled", false);
        previousbutton.removeClass("disabled");
    }
}
function disableNext(a, nextButton, length) {
    if (a === length.toString()) {
        nextButton.prop("disabled", true);
        nextButton.addClass("disabled");
    }
    else {
        nextButton.prop("disabled", false);
        nextButton.removeClass("disabled");
    }
}