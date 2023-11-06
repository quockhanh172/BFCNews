
$(document).ready(function () {
        $('#modalAdd').on("shown.bs.modal", () => {
            $("#Department").focus();
        });
        var urlParams = new URLSearchParams(window.location.search);
        var activeLink = urlParams.get('activelink');
        var department = urlParams.get('department');
        if (activeLink != null||department!=null) {
            var titleText = ""; // Chuỗi nội dung tiêu đề mặc định

            // Kiểm tra giá trị của activeLink và department và thay đổi nội dung tiêu đề
            if (activeLink != null) {
                titleText += activeLink;
            }

            if (department != null) {
                titleText += department;
            }
            var upperCaseText = titleText.toUpperCase();
            // Đặt nội dung của phần tử có id là "title-web"
            $("#title-web").text(upperCaseText);
            $("#title-web").css("font-weight", "bold");
        }

    $("#search-btn").on("input", function () {
        var text = $("#search-btn").val();
        $(".dropdown-men").hide();
        $(".dropdown-men").empty();
        $.ajax({
            url: '/Management/Search',
            type: "POST",
            data: { 'text': text },
            success: function (response) {
                if (Array.isArray(response.posts) && response.posts.length > 0) {
                    $(".dropdown-men").show();
                    response.posts.forEach(post => {
                        var html = '<a class="dropdown-item" href="/management/detail?id=' + post.id + '">' + post.title + '</a>';
                        $(".dropdown-men").append(html);
                    });
                }
                else {
                    $(".dropdown-men").hide();
                    $(".dropdown-men").empty();
                }
            },
            error:function(){
                $("#msgalert").text("Website gặp lỗi xin quay lại sau");
                $(".alert").css("background", "#dc3545");
                $(".alert").css('display', 'block');
            }
        })
    });
});

