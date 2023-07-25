var rowDelete;
var rowEdit;
$(document).ready(function () {
    $("#btnAdd").click(function(){
        var name = $("#Department").val().toString();
        if (name != null) {
            $.ajax({
                url: '/Department/Add',
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
                }

            })
        }
        $('#modalAdd').modal('hide')
    });
}) 
//autohide alert
autoHide = () => {
    setTimeout(function () {
        $('#alert1').alert('close');
    }, 800);
}

reload = () => {
    setTimeout(function () {
        location.reload();
    }, 900);
}

$("#alert1").on("click", "#btn-close-alert", () => {
    $(".alert").css('display', 'none');
})
//Edit Department
$(document).ready(function () {
    $("#tbDepartment").on('click', '.btn-warning', function () {
        $("#modal-Edit-Department").modal('show');
        rowEdit = $(this).closest('tr');
        $("#department-edit-input").val(rowEdit.find("td:eq(1)").text());
    });
});

$(document).ready(function () {
    $("#modal-Edit-Department").on('click', '#btn-Edit-Department', () => {
        var id = rowEdit.find("td:eq(0)").text();
        var formData = new FormData($("#form-Edit-Department")[0]);
        console.log(id);
        formData.append("id", id);
        console.log(formData);
        $.ajax({
            url: 'Department/Edit',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: (response) => {
                alert("success");
            }
        })
    })
})
//Delete Department

$(document).ready(function () {
    $("#tbDepartment").on('click', '.btn-danger', function () {
        $("#deleteModal").modal('show');
        rowDelete = $(this).closest('tr');
        console.log(rowDelete);
        $("#modal-delete-department-lb").text("" + rowDelete.find("td:eq(1)").text());

    });
});
//close modal delete
$(document).ready(function () {
    $("#modal-btn-department-no").click(() => {
        $("#deleteModal").modal("hide");
    });
}
)

$("#deleteModal").on('click', '#modal-btn-department-yes', () => {
    var id = rowDelete.find("td:eq(0)").text();
    $.ajax({
        url: '/Department/Delete',
        type: "POST",
        data: { "id": id },
        success: (response) => {
            if (response.messager === "success") {
                $("#deleteModal").modal('hide');
                $("#msgalert").text("Đã xóa thành công");
                $(".alert").css("background", "#28a745");
                $(".alert").css('display', 'block');
                rowDelete.remove();
            }
            else {
                $("#content-modal-delete-department").text("Đã xóa không thành công");
            }
        }
    })
})