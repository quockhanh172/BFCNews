$(document).ready(function () {
    $("#btnAdd").click(function(){
        var name = $("#Department").val().toString();
        if (name != null) {
            $.ajax({
                url: '/Department/Add',
                type: "POST",
                data: { "name": name },
                success: function (response) {
                    console.log(response)
                    if (response.messager =="success") {
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

autoHide = () => {
    setTimeout(function () {
        $('.alert').alert('close');
    }, 1000);
}

$(document).ready(function () {  
   
    $("#alertarea").on("click", "#btnclosealeart", () => {
        $("#alert").alert("close");
    })
})