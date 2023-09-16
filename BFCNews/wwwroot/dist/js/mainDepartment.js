
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
});

