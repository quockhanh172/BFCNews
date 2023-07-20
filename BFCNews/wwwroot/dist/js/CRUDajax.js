$(document).ready(function () {
    $("#btnAdd").click(function(){
        var name = $("#Department").val().toString();
        if (name != null) {
            $.ajax({
                url: '/Department/Add',
                type: "POST",
                data: { "name": name },
                success: function (response) {
                    if (response === "success") {
                        $("#alertarea").append('<div class="alert alert-success alert-dismissible fade show" role="alert" id="alert"><strong>Đã Thêm Deparment Thành Công</strong><button type = "button" id="btnclosealeart" class= "close"><span aria-hidden="true">&times;</span></button></div>');
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
    }, 2000);
}

$(document).ready(function () {  
   
    $("#alertarea").on("click", "#btnclosealeart", () => {
        $("#alert").alert("close");
    })
})