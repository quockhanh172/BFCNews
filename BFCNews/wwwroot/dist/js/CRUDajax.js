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
                        $("#alertarea").append('<div class="alert alert-success alert-dismissible fade show" role="alert" id="alert"><strong>Đã tên Phòng ban Thành Công</strong><button type = "button" id="btnclosealeart" class= "close"><span aria-hidden="true">&times;</span></button></div>');
                        autoHide();
                    }
                    if (response == "available") {
                        $("#alertarea").append('<div class="alert alert-danger alert-dismissible fade show" role="alert" id="alert"><strong>Đã tên Phòng ban đã tồn tại</strong><button type = "button" id="btnclosealeart" class= "close"><span aria-hidden="true">&times;</span></button></div>');
                        autoHide();
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
        $('.alert').alert('close');
    }, 2000);
}
//close aleart
$(document).ready(function () {  
   
    $("#alertarea").on("click", "#btnclosealeart", () => {
        $("#alert").alert("close");
    })
}
)
//Delete Department
$(document).ready(function () {
    $("#tbDepartment").on('click', '.btn-danger', function () {
        var row = $(this).closest('tr');
        $("#modal-delete-department-lb").text("" + row.find("td:eq(1)").text());
        $("#deleteModal").modal('show');
        var id = row.find("td:eq(0)").text();
        console.log(id);
        $("#modal-btn-department-yes").click(() => {
            $.ajax({
                url: '/Department/Delete',
                type: "POST",
                data: { "id": id },
                success: (response) => {
                    if (response.messager === "success") {
                        $("#content-modal-delete-department").text("Đã xóa thành công");
                        row.remove();
                    }
                    else {
                        $("#content-modal-delete-department").text("Đã xóa không thành công");
                    }
                }
            })
        })
    });
}
)
//close modal delete
$(document).ready(function () {
    $("#modal-btn-department-no").click(() => {
        $("#deleteModal").modal("hide");
    });
}
)